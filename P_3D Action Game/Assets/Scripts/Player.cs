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
        // Ű�����Է¿� ���� 0 ~ 1�� ��ȯ left right up down
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        jDown = Input.GetButtonDown("Jump");
    }

    void Move()
    {
        // x y z
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        // ȸ�� �� ������Ʈ �ȵǰ�
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
            // ���������� ���� �ش�. ���⼱ ������� Impulse
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");

            isJump = true;
        }
    }

    void Dodge()
    {
        // �̵��ϸ鼭 �����Ҷ�, 
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f); // �ð����� ���̺귯�� ���
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }



        // �浹 �� �̺�Ʈ �Լ��� ���� ����
        void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor"){
            isJump = false;
            anim.SetBool("isJump", false);
        }
    }
}
