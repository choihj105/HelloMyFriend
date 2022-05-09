using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;
    public SoundControl soundControl;

    public int maxAmmo;
    public int curAmmo;

    public void Use()
    {
        if(type == Type.Melee) {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        
        else if(type == Type.Range && curAmmo > 0) {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }

    // IEnumerator : ������ �Լ� Ŭ����
    IEnumerator Swing()
    {
        // �߿��� ������ �ڷ�ƾ
        // ���� : Use() ���η�ƾ -> Swing() �����ƾ -> Use() ���η�ƾ (��������)
        // �ڷ�ƾ : Use() ���η�ƾ + Swing() �ڷ�ƾ Co - (���ý���)

        // yield : ����� �����ϴ� Ű����, ���� �� ����� �ð��� ���� ��������

        soundControl.swingSound.Play();
        //1
        yield return new WaitForSeconds(0.2f); // 0.2 �� ���
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        //2
        yield return new WaitForSeconds(0.3f); // 0.3 �� ���
        meleeArea.enabled = false;

        //3
        yield return new WaitForSeconds(0.3f); // 0.3 �� ���
        trailEffect.enabled = false;


    }

    IEnumerator Shot()
    {
        soundControl.shotSound.Play();
        // 1. �Ѿ� �߻� , Instantiate() �Լ��� �Ѿ� �ν��Ͻ�ȭ �ϱ�
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;         // velocity �ӷ��ֱ�
        yield return null;
        // 2. ź�� ����
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody CaseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        CaseRigid.AddForce(caseVec, ForceMode.Impulse);
        
        CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse); // ź�� ȸ��
    }

}
