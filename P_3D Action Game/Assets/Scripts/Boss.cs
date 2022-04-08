using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;

    Vector3 lookVec; // 플레이어 움직임 예측
    Vector3 tauntVec; // 찍어내리는 벡터
    bool isLook; // 플레이어 바라보는 플래그

    void Start()
    {
        isLook = true;
    }

    
    void Update()
    {
        if (isLook) {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(Target.position + lookVec);
        }
    }
}
