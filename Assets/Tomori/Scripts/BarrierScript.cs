using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    bool isBarrier = false;
    [SerializeField] GameObject barrier;

    [SerializeField] int barrierHP = 150;//���̃o���AHP
    [SerializeField] float energyPool = 10000;//���̃G�l���M�[�v�[����
    public float playerHP = 500;//���̃v���C���[HP

    //���L3�̃_���[�W�͕ʂ̕ʂ̐l��������_���[�W�ɉ����ĕύX
    /*[SerializeField] int bulletAtack = 200;//���̎��e����̃_���[�W
    [SerializeField] int energyAtack = 300;//���̃G�l���M�[����̃_���[�W
    [SerializeField] int missileAtack = 400;//���̃~�T�C������̃_���[�W
    [SerializeField] int contactAtack = 300;//���̐ڐG�U���̃_���[�W*/
    [SerializeField] int contactBonus = 100;//�ڐG�U���̃{�[�i�X�|�C���g�i�d�l���ɏ�����Ă������߁j

    [SerializeField] Transform playerTR;
    [SerializeField] float sizeMultiplier = 1.2f;

    GameObject currentBarrier;

    [SerializeField,Header("�v���C���[�̈ʒu")] Vector3 detectionCenter;  // ���o�̒��S�_�i�v���C���[�̈ʒu)�j
    [SerializeField,Header("���o�͈͂̔��a")] float detectionRadius;
    [SerializeField,Header("���̃��C���[")] LayerMask bulletLayer;

    Collider[] bullets;

    public enum AttackType
    {
        Bullet,
        Energy,
        Missile,
        Contact
    }

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
        float diameter = currentBarrier.transform.localScale.x;
        float cost = diameter * barrierHP;

        if (bullets.Length > 0)
        {
            if (!isBarrier && energyPool >= cost)
            {
                energyPool -= cost;
                isBarrier = true;
                currentBarrier.SetActive(true);
            }
        }
        else
        {
            if (isBarrier)
            {
                isBarrier = false;
                currentBarrier.SetActive(false);
            }
        }
    }


    //�v���C���[�̑傫���ɍ��킹���o���A�𐶐�
    void CreateBarrier()
    {
        Renderer[] renderers = playerTR.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0) return;

        Bounds combinedBounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(renderers[i].bounds);
        }

        float maxSize = Mathf.Max(combinedBounds.size.x, combinedBounds.size.y, combinedBounds.size.z);

        currentBarrier = Instantiate(barrier, playerTR.position, Quaternion.identity, playerTR);
        currentBarrier.transform.localScale = Vector3.one * maxSize * sizeMultiplier;

        detectionRadius = maxSize * sizeMultiplier * 1.1f;//�o���A�̑傫���ɍ��킹�Č��o�͈͂̔��a���傫��
    }


    public void ReceiveDamage(AttackType attackType, int damageAmount)
    {
        switch (attackType)
        {
            case AttackType.Bullet:
                if (isBarrier)
                {
                    if (barrierHP <= damageAmount)
                        playerHP += barrierHP - damageAmount;
                    // else �� �o���A�Ŋ��S�h��
                }
                else
                {
                    playerHP -= damageAmount;
                }
                break;

            case AttackType.Energy:
                if (!isBarrier)
                {
                    playerHP -= damageAmount;
                }
                break;

            case AttackType.Missile:
                if (isBarrier)
                {
                    if (barrierHP <= damageAmount)
                        playerHP += barrierHP - damageAmount;
                }
                else
                {
                    playerHP -= damageAmount;
                }
                break;

            case AttackType.Contact:
                int totalContactDamage = damageAmount + contactBonus;
                if (isBarrier)
                {
                    if (barrierHP <= totalContactDamage)
                        playerHP += barrierHP - totalContactDamage;
                }
                else
                {
                    playerHP -= totalContactDamage;
                }
                break;
        }
    }

    //�_���[�W���v�Z���邽�߂�trigger���g���iplayer��isTrigger��true�j
    /*private void OnTriggerEnter(Collider other)
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

        //�ڐG�U�������m���邽�߂̃^�O�i�ꍇ�ɂ���Ă͏��� or �ύX�j
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
    }*/
}

