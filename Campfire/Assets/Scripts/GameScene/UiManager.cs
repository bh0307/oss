using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    GameObject myTurnStartPanel;  // 내 턴 시작 시 표시될 패널
    [SerializeField]
    GameObject myTurnMovePanel;  // 내 턴 이동 시 표시될 패널
    [SerializeField]
    GameObject myTurnActionPanel;  // 내 턴 행동 시 표시될 패널
    [SerializeField]
    GameObject othersTurnPanel;  // 다른 플레이어 턴 시 표시될 패널

    public static UiManager UM;  // UiManager의 싱글톤 객체
    public float LImitTime;  // 제한 시간
    public Text text_Timer;  // 타이머를 표시할 텍스트
    public GameObject notice_obj;  // 공지사항 텍스트를 표시할 오브젝트
    public Text notice;  // 공지사항 텍스트
    PhotonView PV;  // PhotonView 컴포넌트 (Photon 네트워크 관련)

    void Awake()
    {
        UM = this;  // 싱글톤 초기화
        PV = GetComponent<PhotonView>();  // PhotonView 컴포넌트 가져오기
    }

    // 매 프레임 호출되는 함수
    void Update()
    {
        LImitTime -= Time.deltaTime;  // 제한 시간 감소
        text_Timer.text = "남은 시간 : " + (int)LImitTime;  // 남은 시간을 텍스트로 표시

        // 제한 시간이 0 이하일 때 새로운 턴을 시작하는 RPC 호출
        if (LImitTime <= 0)
        {
            GameManager.GM.RPC_NewTurnStart();  // 새로운 턴 시작
            LImitTime = 10f;  // 제한 시간 초기화
        }
    }

    // 내 턴이 시작될 때 호출되는 함수
    public void MyTurnStart()
    {
        myTurnStartPanel.SetActive(true);  // 내 턴 시작 패널 활성화
        myTurnMovePanel.SetActive(false);  // 내 턴 이동 패널 비활성화
        myTurnActionPanel.SetActive(false);  // 내 턴 행동 패널 비활성화
        othersTurnPanel.SetActive(false);  // 다른 턴 패널 비활성화
        notice_obj.SetActive(false);  // 공지사항 비활성화
    }

    // 내 턴에서 이동할 때 호출되는 함수
    public void MyTurnMove()
    {
        myTurnStartPanel.SetActive(false);  // 내 턴 시작 패널 비활성화
        myTurnMovePanel.SetActive(true);  // 내 턴 이동 패널 활성화
        myTurnActionPanel.SetActive(false);  // 내 턴 행동 패널 비활성화
        othersTurnPanel.SetActive(false);  // 다른 턴 패널 비활성화
        GameManager.GM.myController.Bulddeok();  // 이동 작업 (게임 로직에 맞는 메서드 호출)
        notice_obj.SetActive(false);  // 공지사항 비활성화
    }

    // 내 턴에서 행동할 때 호출되는 함수
    public void MyTurnAction()
    {
        myTurnStartPanel.SetActive(false);  // 내 턴 시작 패널 비활성화
        myTurnMovePanel.SetActive(false);  // 내 턴 이동 패널 비활성화
        myTurnActionPanel.SetActive(true);  // 내 턴 행동 패널 활성화
        othersTurnPanel.SetActive(false);  // 다른 턴 패널 비활성화
        notice_obj.SetActive(false);  // 공지사항 비활성화
    }

    // 다른 플레이어 턴일 때 호출되는 함수
    public void OthersTurn()
    {
        myTurnStartPanel.SetActive(false);  // 내 턴 시작 패널 비활성화
        myTurnMovePanel.SetActive(false);  // 내 턴 이동 패널 비활성화
        myTurnActionPanel.SetActive(false);  // 내 턴 행동 패널 비활성화
        notice_obj.SetActive(false);  // 공지사항 비활성화
        notice_obj.SetActive(true);  // 공지사항 활성화
        SetNotice("다른 플레이어의 턴입니다.");  // 공지사항 텍스트 설정
    }

    // 공지사항을 일정 시간 동안 표시하는 코루틴
    IEnumerator Notice()
    {
        notice_obj.SetActive(false);  // 공지사항 비활성화
        notice_obj.SetActive(true);  // 공지사항 활성화
        yield return new WaitForSeconds(3f);  // 3초 대기
        notice_obj.SetActive(false);  // 공지사항 비활성화
    }

    // 공지사항 텍스트 설정 함수
    public void SetNotice(string txt)
    {
        notice_obj.SetActive(false);  // 공지사항 비활성화
        notice_obj.SetActive(true);  // 공지사항 활성화
        notice.text = txt;  // 공지사항 텍스트 변경
    }
}
