using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsAtackScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Bullet�Ȃǂ̍U�����鋅���������߂̏����i�K�v�ɉ����ĕʃX�N���v�g�Ɉړ����Ăق����j
        if(other.gameObject.tag == "Ballier" || other.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
        }
    }
}
