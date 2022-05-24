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

            //�̵�
            Move();
        }
    }

    void GetInput()
    {
        axis = Input.GetAxisRaw("Horizontal");
    }

    void Move()
    {
        RB.velocity = new Vector2( speed * axis, RB.velocity.y);

        if (axis != 0)
        {
            AN.SetBool("Walk", true);
            PV.RPC("FlipXRPC", RpcTarget.AllBuffered, axis); // �����ӽ� flipX�� ����ȭ ���ֱ� ���� AllBuffered
        }
        else AN.SetBool("Walk", false);
    }

    [PunRPC]
    void FlipXRPC(float axis) => SR.flipX = axis == -1;


    // ���� ����ȭ
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }
}