using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    [SerializeField] GameObject barrier;

    [SerializeField] int barrierHP = 15;//�o���AHP

    [SerializeField] Transform playerTR;
    [SerializeField] float sizeMultiplier = 2f;

    [SerializeField, Header("�v���C���[�̈ʒu")] Vector3 detectionCenter;  // ���o�̒��S�_�i�v���C���[�̈ʒu)�j
    [SerializeField, Header("���o�͈͂̔��a")] float detectionRadius;
    [SerializeField, Header("���̃��C���[")] LayerMask bulletLayer;

    [SerializeField] EnergyScript energyScript;

    public CoreScript coreScript;

    Collider[] bullets;

    GameObject currentBarrier;

    float barrierDuration = 2.0f; // �o���A���ێ�����b��
    float barrierTimer = 0f;

    bool isBarrier = false;

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

        // �o���AON���̓^�C�}�[����
        if (isBarrier)
        {
            barrierTimer -= Time.deltaTime;
            if (barrierTimer <= 0f)
            {
                isBarrier = false;
                currentBarrier.SetActive(false);
            }
        }
    }


    //�o���A��true�ɂ���
    void SetBarrier()
    {
        float diameter = currentBarrier.transform.localScale.x;
        float cost = diameter * barrierHP;

        if (bullets.Length > 0)
        {
            if (!isBarrier && energyScript.UseEnergy(cost + 50))
            {
                isBarrier = true;
                currentBarrier.SetActive(true);
                barrierTimer = barrierDuration; // �o���A����莞�Ԉێ�
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


    /*public int ReceiveDamage(AttackType attackType, int damageAmount)
    {
        int actualDamage = 0;

        switch (attackType)
        {
            case AttackType.Bullet:
                if (isBarrier)
                {
                    if (barrierHP <= damageAmount)
                    {
                        actualDamage = damageAmount - barrierHP;
                        playerHP -= actualDamage;
                    }
                    // else �� �o���A�Ŋ��S�h��i�_���[�W0�j
                }
                else
                {
                    actualDamage = damageAmount;
                    playerHP -= actualDamage;
                }
                break;

            case AttackType.Energy:
                if (!isBarrier)
                {
                    actualDamage = damageAmount;
                    playerHP -= actualDamage;
                }
                break;

            case AttackType.Missile:
                if (isBarrier)
                {
                    if (barrierHP <= damageAmount)
                    {
                        actualDamage = damageAmount - barrierHP;
                        playerHP -= actualDamage;
                    }
                }
                else
                {
                    actualDamage = damageAmount;
                    playerHP -= actualDamage;
                }
                break;

            case AttackType.Contact:
                int totalContactDamage = damageAmount + contactBonus;
                if (isBarrier)
                {
                    if (barrierHP <= totalContactDamage)
                    {
                        actualDamage = totalContactDamage - barrierHP;
                        playerHP -= actualDamage;
                    }
                }
                else
                {
                    actualDamage = totalContactDamage;
                    playerHP -= actualDamage;
                }
                break;
        }

        return actualDamage;
    }*/


    //�_���[�W���v�Z���邽�߂�trigger���g���iplayer��isTrigger��true�j
    private void OnCollisionEnter(Collision collision)
    {
        int damage = 0;

        //���e���킩�甭�˂���鋅�̃^�O
        if (collision.gameObject.tag == "Bullet")
        {
            damage = collision.gameObject.GetComponent<BulletScript>().GetDamage();
            if (isBarrier)
            {
                damage = damage - barrierHP;
                damage = damage < 0 ? 0 : damage;
                coreScript.Damage(damage);
            }
            else
            {
                coreScript.Damage(damage);
            }

            collision.gameObject.SetActive(false);
        }

        //�G�l���M�[���킩�甭�˂���鋅�̃^�O
        if (collision.gameObject.tag == "Energy")
        {
            damage = collision.gameObject.GetComponent<EnergyBulletScript>().EnergyDamege();

            if (isBarrier)
            {
                coreScript.Damage(0);
            }
            else
            {
                coreScript.Damage(damage);
            }

            collision.gameObject.SetActive(false);
        }

        //�~�T�C�����킩�甭�˂����~�T�C���̃^�O
        if (collision.gameObject.tag == "Missile")
        {
            damage = collision.gameObject.GetComponent<MissileBulletSc>().GetDamage();

            if (isBarrier)
            {
                damage = damage - barrierHP;
                damage = damage < 0 ? 0 : damage;
                coreScript.Damage(damage);
            }
            else
            {
                coreScript.Damage(damage);
            }

            collision.gameObject.SetActive(false);
        }
    }
}

