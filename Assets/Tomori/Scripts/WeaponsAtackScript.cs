using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsAtackScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Bullet�Ȃǂ̍U�����鋅���������߂̏����i�K�v�ɉ����ĕʃX�N���v�g�Ɉړ����Ăق����j
        if(other.gameObject.tag == "Ballier" || other.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
        }
    }
}
