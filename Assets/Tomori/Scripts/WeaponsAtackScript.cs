using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsAtackScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Bullet�Ȃǂ̍U�����鋅���������߂̏����i�K�v�ɉ����ĕʃX�N���v�g�Ɉړ����Ăق����j
        if (collision.gameObject.tag == "Barrier" || collision.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
        }
    }
}
