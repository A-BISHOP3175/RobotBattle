using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    public CoreScript coreScript;
    public BarrierManager barrierManager;

    //�_���[�W���v�Z���邽�߂�trigger���g���iplayer��isTrigger��true�j
    private void OnCollisionEnter(Collision collision)
    {
        int damage = 0;

        //���e���킩�甭�˂���鋅�̃^�O
        if (collision.gameObject.tag == "Bullet")
        {
            damage = collision.gameObject.GetComponent<BulletScript>().GetDamage();
            
                damage = damage - barrierManager.barrierHP;
                damage = damage < 0 ? 0 : damage;
                coreScript.Damage(damage);
           

            collision.gameObject.SetActive(false);
        }

        //�G�l���M�[���킩�甭�˂���鋅�̃^�O
        if (collision.gameObject.tag == "Energy")
        {
            damage = collision.gameObject.GetComponent<EnergyBulletScript>().EnergyDamege();

            
                coreScript.Damage(0);
           

            collision.gameObject.SetActive(false);
        }

        //�~�T�C�����킩�甭�˂����~�T�C���̃^�O
        if (collision.gameObject.tag == "Missile")
        {
            damage = collision.gameObject.GetComponent<MissileBulletSc>().GetDamage();

            
                damage = damage - barrierManager.barrierHP;
                damage = damage < 0 ? 0 : damage;
                coreScript.Damage(damage);
           

            collision.gameObject.SetActive(false);
        }
    }
}

