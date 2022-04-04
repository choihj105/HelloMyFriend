using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    public int hasGrenades;
    public Camera followCamera;

    public int ammo;
    public int health;
    public int coin;
    
    public int maxAmmo;
    public int maxHealth;
    public int maxCoin;
    public int maxHasGrenades;



    float hAxis;
    float vAxis;
    
    bool rDown;
    bool jDown;
    bool fDown;
    bool reDown;
    bool iDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;

    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isReload;
    bool isFireReady = true;
    

    Vector3 moveVec;
    Vector3 dodgeVec;
    Rigidbody rigid;

    Animator anim;

    GameObject nearObject;
    Weapon equipWeapon;
    int equipWeaponIndex = -1;
    float fireDelay;

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
        Attack();
        Reload();
        Dodge();
        Swap();
        Interation();
        
        
    }


    void GetInput()
    {
        // 키보드입력에 따라 0 ~ 1로 변환 left right up down
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetButton("Run");
        jDown = Input.GetButtonDown("Jump");
        reDown = Input.GetButtonDown("Reload");
        fDown = Input.GetButton("Fire1");
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    void Move()
    {
        // x y z
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        // 회피 시 업데이트 안되게
        if (isDodge)
            moveVec = dodgeVec;
        
        // 스왑 및 공격 시 못움직이게
        if (isSwap || !isFireReady)
            moveVec = Vector3.zero;

        // transform
        transform.position += moveVec * (rDown ? 1.3f : 1f) * speed * Time.deltaTime;

        // animator
        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }

    void Turn()
    {
        // #1. 키보드에 의한 회전
        transform.LookAt(transform.position + moveVec);

        // #2. 마우스에 의한 회전

        // Ray란?, ScreenPointToRay() : 스크린에서 월드로 Ray를 쏘는 함수
        if(fDown)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            // out 키워드는, 반환값을 주어진 변수에 저장하는 키워드
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point;
                nextVec.y = 0;
                transform.LookAt(nextVec);
            }
        }
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

    void Attack()
    {
        // 공격할 조건만 플레이어에 두고, 공격로직은 무기에 위임한다.
        if (equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(fDown && isFireReady && !isDodge && !isSwap) {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
    }

    void Reload()
    {
        if (equipWeapon == null)
            return;
        if (equipWeapon.type == Weapon.Type.Melee)
            return;
        if (ammo == 0 || equipWeapon.curAmmo == equipWeapon.maxAmmo)
            return;

        if(reDown && !isJump && !isDodge && !isSwap && isFireReady)
        {
            anim.SetTrigger("doReload");
            isReload = true;

            // Invoke("ReloadOut", 2f); -> 애니메이션 기능에 ReloadOut() 기능 구현했습니다.
        }
    }

    public void ReloadOut()
    {
        int reAmmo = equipWeapon.maxAmmo - equipWeapon.curAmmo;
        reAmmo = ammo < reAmmo ? ammo : reAmmo;

        equipWeapon.curAmmo += reAmmo;
        ammo -= reAmmo;
        isReload = false;
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
    
    void Swap()
    {
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;

        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        // 1, 2 ,3 버튼 누를때
        if((sDown1 || sDown2 || sDown3) && !isJump && !isDodge) {
            if(equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);
            
            anim.SetTrigger("doSwap");
            

            // 스왑 중
            isSwap = true;
            Invoke("SwapOut", 0.4f);

        }
    }
    void SwapOut()
    {
        isSwap = false;
    }

    void Interation()
    {
        if(iDown && nearObject != null && !isJump && !isDodge) {
            if(nearObject.tag == "Weapon") {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }
    void FixedUpdate()
    {
        FreezeRotation();
    }

    // 충돌 시 이벤트 함수로 착지 구현
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor"){
            isJump = false;
            anim.SetBool("isJump", false);
        }
    }

    // 아이템 획득
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item") {
            Item item = other.GetComponent<Item>();
            switch (item.type){
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
                case Item.Type.Grenade:
                    if (hasGrenades == maxHasGrenades)
                        return;
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    break;
            }
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
    }
}
