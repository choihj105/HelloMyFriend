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



        // 피격범위 레이캐스트
        // SphereCastAll: 구체모양의 레이캐스팅 (모든 오브젝트)
        RaycastHit[] rayHits =
           Physics.SphereCastAll(transform.position,
                                15, Vector3.up, 0f,
                                LayerMask.GetMask("Enemy"));
        foreach(RaycastHit hitObj in rayHits) {
            hitObj.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }

        Destroy(gameObject, 5);

    }
}
