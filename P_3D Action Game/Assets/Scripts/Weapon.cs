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

    public void Use()
    {
        if(type == Type.Melee) {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }

    // IEnumerator : ������ �Լ� Ŭ����
    IEnumerator Swing()
    {
        // �߿��� ������ �ڷ�ƾ
        // ���� : Use() ���η�ƾ -> Swing() �����ƾ -> Use() ���η�ƾ (��������)
        // �ڷ�ƾ : Use() ���η�ƾ + Swing() �ڷ�ƾ Co - (���ý���)

        // yield : ����� �����ϴ� Ű����, ���� �� ����� �ð��� ���� ��������
        
        //1
        yield return new WaitForSeconds(0.1f); // 0.1 �� ���
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        //2
        yield return new WaitForSeconds(0.3f); // 0.3 �� ���
        meleeArea.enabled = false;

        //3
        yield return new WaitForSeconds(0.3f); // 0.3 �� ���
        trailEffect.enabled = false;


    }

}
