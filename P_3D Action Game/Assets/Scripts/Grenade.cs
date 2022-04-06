using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rigid;

    void Start()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);
        
        // 속도
        rigid.velocity = Vector3.zero;
        // 회전 속도
        rigid.angularVelocity = Vector3.zero;

        meshObj.SetActive(false);
        effectObj.SetActive(true);
    }
}
