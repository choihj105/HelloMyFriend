using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;

    Vector3 moveVec;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Ű�����Է¿� ���� 0 ~ 1�� ��ȯ left right up down
        hAxis = Input.GetAxisRaw("Horizontal"); 
        vAxis = Input.GetAxisRaw("Vertical");

        // x y z
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        // transform
        transform.position += moveVec * speed * Time.deltaTime;

    }
}
