using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    // HeartManager 클래스의 싱글톤 인스턴스
    public static HeartManager HM;
    
    // 플레이어의 하트 수 (기본값 1)
    public int heart = 1;

    // 하트 수를 화면에 표시할 UI Text 컴포넌트
    public Text txt;

    // Awake 함수는 스크립트가 시작될 때 한 번 호출
    void Awake()
    {
        // 싱글톤 인스턴스에 현재 객체를 할당
        HM = this;
    }

    // 매 프레임마다 호출되는 Update 함수
    void Update()
    {
        // 하트 수를 화면에 출력 (예: "하트 : 3")
        txt.text = "하트 : " + heart;
    }

    // 하트를 증가 또는 감소시키는 함수
    public void HeartMove()
    {
        // 현재 위치가 그린존이라면
        if (MapManager.MM.isGreenZone[GameManager.GM.myController.curPosX, GameManager.GM.myController.curPosY])
        {
            // 하트가 4 미만이면 하트를 증가시킴
            if (heart < 4)
                heart += 1;
        }
        else
        {
            // 그린존이 아니면 하트가 0보다 크다면 하트를 감소시킴
            if (heart > 0)
                heart -= 1;
        }
    }
}
