using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public static HeartManager HM;  // HeartManager 클래스의 싱글톤 객체
    public int heart = 1;  // 현재 보유한 하트 수 (초기값 1)
    public Text txt;  // 하트 수를 표시할 텍스트 UI

    void Awake()
    {
        HM = this;  // 싱글톤 객체 초기화
    }

    void Update()
    {
        txt.text = "하트 : " + heart;  // 하트 수를 UI 텍스트로 업데이트
    }

    // 하트의 이동 (증가 또는 감소) 함수
    public void HeartMove()
    {
        // 플레이어의 현재 위치가 'GreenZone'인지 체크
        if (MapManager.MM.isGreenZone[GameManager.GM.myController.curPosX, GameManager.GM.myController.curPosY])
        {
            // GreenZone에 있으면 하트가 4 미만일 경우 1 증가
            if (heart < 4)
                heart += 1;
        }
        else
        {
            // GreenZone이 아니면 하트가 0보다 크면 1 감소
            if (heart > 0)
                heart -= 1;
        }
    }
}
