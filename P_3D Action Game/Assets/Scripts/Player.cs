using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;
    bool wDown;
    Vector3 moveVec;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 키보드입력에 따라 0 ~ 1로 변환 left right up down
        hAxis = Input.GetAxisRaw("Horizontal"); 
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Run");
        // x y z
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        // transform
        transform.position += moveVec * (wDown ? 1.3f : 1f) * speed * Time.deltaTime;

        // animator
        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", wDown);
    }
}
