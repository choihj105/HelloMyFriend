using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;
    bool rDown;
    bool jDown;

    bool isJump;
    bool isDodge;

    Vector3 moveVec;
    Vector3 dodgeVec;
    Rigidbody rigid;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
    }


    void GetInput()
    {
        // 키보드입력에 따라 0 ~ 1로 변환 left right up down
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        jDown = Input.GetButtonDown("Jump");
    }

    void Move()
    {
        // x y z
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        // 회피 시 업데이트 안되게
        if (isDodge)
            moveVec = dodgeVec;
      

        // transform
        transform.position += moveVec * (rDown ? 1.3f : 1f) * speed * Time.deltaTime;

        // animator
        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }

    void Turn()
    {
        // Rotation
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if(jDown && moveVec == Vector3.zero && !isJump && !isDodge) {
            // 물리엔진에 힘을 준다. 여기선 즉발적인 Impulse
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");

            isJump = true;
        }
    }

    void Dodge()
    {
        // 이동하면서 점프할때, 
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f); // 시간지연 라이브러리 기능
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }



        // 충돌 시 이벤트 함수로 착지 구현
        void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor"){
            isJump = false;
            anim.SetBool("isJump", false);
        }
    }
}
