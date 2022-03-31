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

    // IEnumerator : 열거형 함수 클래스
    IEnumerator Swing()
    {
        // 중요한 개념인 코루틴
        // 기존 : Use() 메인루틴 -> Swing() 서브루틴 -> Use() 메인루틴 (교차실행)
        // 코루틴 : Use() 메인루틴 + Swing() 코루틴 Co - (동시실행)

        // yield : 결과를 전달하는 키워드, 여러 개 사용해 시간차 로직 구현가능
        
        //1
        yield return new WaitForSeconds(0.1f); // 0.1 초 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        //2
        yield return new WaitForSeconds(0.3f); // 0.3 초 대기
        meleeArea.enabled = false;

        //3
        yield return new WaitForSeconds(0.3f); // 0.3 초 대기
        trailEffect.enabled = false;


    }

}
