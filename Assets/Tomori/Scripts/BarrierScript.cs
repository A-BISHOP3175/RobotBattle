using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    bool isBarrier = false;
    [SerializeField] GameObject barrier;

    [SerializeField] float barrierHP = 100;//���̃o���AHP
    public float playerHP = 500;//���̃v���C���[HP

    //���L3�̃_���[�W�͕ʂ̕ʂ̐l��������_���[�W�ɉ����ĕύX
    [SerializeField] float bulletAtack = 200;//���̎��e����̃_���[�W
    [SerializeField] float energyAtack = 300;//���̃G�l���M�[����̃_���[�W
    [SerializeField] float missileAtack = 400;//���̃~�T�C������̃_���[�W
    [SerializeField] float contactAtack = 300;//���̐ڐG�U���̃_���[�W
    [SerializeField] float contactBonus = 100;//�ڐG�U���̃{�[�i�X�|�C���g�i�d�l���ɏ�����Ă������߁j

    [SerializeField] Transform playerTR;
    [SerializeField] float sizeMultiplier = 1.2f;

    GameObject currentBarrier;

    [SerializeField,Header("�v���C���[�̈ʒu")] Vector3 detectionCenter;  // ���o�̒��S�_�i�v���C���[�̈ʒu)�j
    [SerializeField,Header("�Ϗo�͈͂̔��a")] float detectionRadius;
    [SerializeField,Header("���̃��C���[")] LayerMask bulletLayer;

    Collider[] bullets;

    void Start()
    {
        CreateBarrier();//�o���A�𐶐�
        currentBarrier.SetActive(false);//�o���A��false
    }


    void Update()
    {
        detectionCenter = playerTR.localPosition;

        // �w�肵���͈͓��̂��ׂĂ̓G�R���C�_�[�����o
        bullets = Physics.OverlapSphere(detectionCenter, detectionRadius, bulletLayer);

        SetBarrier();
    }


    //�o���A��true�ɂ���
    void SetBarrier()
    {
        if(bullets.Length > 0)
        {
            isBarrier = true;
            currentBarrier.SetActive(true);
        }
        else if(bullets.Length < 0)
        {
            isBarrier = false;
            currentBarrier.SetActive(false);
        }
    }


    //�v���C���[�̑傫���ɍ��킹���o���A�𐶐�
    void CreateBarrier()
    {
        Renderer playerRenderer = playerTR.GetComponentInChildren<Renderer>();

        Bounds bounds = playerRenderer.bounds;
        float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);//��ԑ傫���Ƃ����maxSize�ɐݒ�

        // �o���A����
        currentBarrier = Instantiate(barrier, playerTR.position, Quaternion.identity, playerTR);

        // �o���A�̃T�C�Y�����i���̃T�C�Y����ɃX�P�[�����O�j
        currentBarrier.transform.localScale = Vector3.one * maxSize * sizeMultiplier;
    }

    
    //�_���[�W���v�Z���邽�߂�trigger���g���iplayer��isTrigger��true�j
    private void OnTriggerEnter(Collider other)
    {
        //���e���킩�甭�˂���鋅�̃^�O
        if (other.gameObject.tag == "Bullet")
        {
            if (isBarrier)
            {
                if (barrierHP <= bulletAtack)
                {
                    playerHP = playerHP + (barrierHP - bulletAtack);
                }
                else if (barrierHP >= bulletAtack)
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
        if (other.gameObject.tag == "EnergyBullet")
        {
            if (isBarrier)
            {
                //playerHP = playerHP�i�������Ȃ��j;
            }
            else
            {
                playerHP = playerHP - energyAtack;
            }
        }

        //�~�T�C�����킩�甭�˂����~�T�C���̃^�O
        if(other.gameObject.tag == "Missile")
        {
            if (isBarrier)
            {
                if (barrierHP <= missileAtack)
                {
                    playerHP = playerHP + (barrierHP - missileAtack);
                }
                else if (barrierHP >= missileAtack)
                {
                    //playerHP = playerHP;�i�������Ȃ��j
                }
            }
            else
            {
                playerHP = playerHP - missileAtack;
            }
        }

        //�ڐG�U�������m���邽�߂̃^�O�i�ꍇ�ɂ���Ă͏����j
        if(other.gameObject.tag == "Contact")
        {
            if (isBarrier)
            {
                if (barrierHP <= contactAtack + contactBonus)
                {
                    playerHP = playerHP + (barrierHP - (contactAtack + contactBonus));
                }
                else if (barrierHP >= contactAtack + contactBonus)
                {
                    //playerHP = playerHP;�i�������Ȃ��j
                }
            }
            else
            {
                playerHP = playerHP - missileAtack;
            }
        }
    }
}

