using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D RB;
    public Animator AN;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Text NicknameText;
    public Image HealthImage;

    public int speed;
    
    float axis;
    bool jDown;

    bool isGround;
    Vector3 curPos;

    void Awake()
    {
        NicknameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        NicknameText.color = PV.IsMine ? Color.green : Color.red;
    }

    void Update()
    {
        if (PV.IsMine)
        {
            GetInput();

            //이동
            Move();

            //점프
            Jump();
        }
    }

    void GetInput()
    {
        axis = Input.GetAxisRaw("Horizontal");
        jDown = Input.GetButtonDown("Jump");
    }

    void Move()
    {
        RB.velocity = new Vector2( speed * axis, RB.velocity.y);
 
        if (axis != 0)
        {
            PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis); // 재접속시 flipX를 동기화 해주기 위해 AllBuffered
        }
        AN.SetBool("walk", axis != 0);

    }

    [PunRPC]
    void FlipXRPC(float axis) => SR.flipX = axis == -1;

    void Jump()
    {
        if(jDown && isGround)
        {
            isGround = false;
            AN.SetBool("jump", true);
            PV.RPC("JumpRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    void JumpRPC()
    {
        RB.velocity = Vector2.zero;
        RB.AddForce(Vector2.up * 700);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            AN.SetBool("jump", false);
        }
    }

    // 변수 동기화
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }
}
