using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    bool isBarrier = false;
    [SerializeField] GameObject barrier;

    public float barrierHP = 100;//���̃o���AHP
    public float playerHP = 500;//���̃v���C���[HP
    [SerializeField] float bulletAtack = 200;//���̎��e�̃_���[�W

    public Transform player;
    public float sizeMultiplier = 2.0f;

    private GameObject currentBarrier;

    void Start()
    {
        CreateBarrier();
        currentBarrier.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isBarrier = true;
            SetBarrier();
        }
    }


    //�o���A��true�ɂ���
    void SetBarrier()
    {
        if (isBarrier)
        {
            currentBarrier.SetActive(true);
        }
    }


    //�v���C���[�̑傫���ɍ��킹���o���A�𐶐�
    void CreateBarrier()
    {
        Renderer playerRenderer = player.GetComponentInChildren<Renderer>();

        Bounds bounds = playerRenderer.bounds;
        float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);

        // �o���A����
        currentBarrier = Instantiate(barrier, player.position, Quaternion.identity, player);

        // �o���A�̃T�C�Y�����i���̃T�C�Y����ɃX�P�[�����O�j
        currentBarrier.transform.localScale = Vector3.one * maxSize * sizeMultiplier;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //���e���킩�甭�˂���鋅�̃^�O
        if (collision.gameObject.tag == "Bullet")
        {
            if (isBarrier)
            {
                if (barrierHP < bulletAtack)
                {
                    playerHP = playerHP + (barrierHP - bulletAtack);
                }
                else if (barrierHP > bulletAtack)
                {
                    //playerHP = playerHP;�i�������Ȃ��j
                }
            }
            else
            {
                playerHP = playerHP - bulletAtack;
            }
        }

        //�G�l���M�[���킩�甭�˂���鋅�̃^�O
        if(collision.gameObject.tag == "EnergyBullet")
        {

        }
    }
}

