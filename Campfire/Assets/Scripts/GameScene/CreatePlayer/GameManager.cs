using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text txt;  // 현재 턴과 내 턴을 표시할 UI 텍스트
    public int curTurn = 0;  // 현재 턴 (0부터 시작)
    public int playerCount;  // 게임에 참여하는 플레이어 수

    public GameObject mine;  // 플레이어의 캐릭터
    public PlayerController myController;  // 내 캐릭터의 PlayerController
    public List<PlayerController> PlayerControllerList = new List<PlayerController>();  // 게임에 참여하는 모든 플레이어의 리스트

    public static GameManager GM;  // GameManager의 싱글톤 객체
    PhotonView PV;  // PhotonView 컴포넌트

    // Awake: GameManager의 인스턴스를 싱글톤으로 설정하고, PhotonView 초기화
    void Awake()
    {
        GM = this;  // 싱글톤 객체로 설정
        PV = GetComponent<PhotonView>();  // PhotonView 컴포넌트 초기화
    }

    // Update: UI에 현재 턴과 내 턴을 갱신
    void Update()
    {
        txt.text = "curTurn : " + curTurn.ToString() + " myTurn:" + myController.myTurn.ToString();
    }

    // SetPlayerCount: 플레이어 수를 설정하는 RPC 함수
    [PunRPC]
    public void SetPlayerCount(int p)
    {
        playerCount = p;  // 플레이어 수 설정
    }

    // SetValue: PlayerController 리스트를 설정 (동기화는 불가능하므로 나중에 스트림 사용 예정)
    public void SetValue(List<PlayerController> pl)
    {
        PlayerControllerList = pl;  // 게임에 참여하는 플레이어들의 리스트를 설정
    }

    // NewTurnStart: 새로운 턴을 시작하는 함수 (RPC로 호출됨)
    [PunRPC]
    public void NewTurnStart()
    {
        curTurn = (curTurn + 1) % playerCount;  // 턴을 다음 플레이어로 변경, 플레이어 수에 맞춰 순환
        UiManager.UM.LImitTime = 20f;  // 시간 제한을 10초로 초기화
        Inventory.IM.itemSlot[0].interactable = false;  // 아이템 슬롯 버튼 비활성화
        Inventory.IM.itemSlot[1].interactable = false;  // 아이템 슬롯 버튼 비활성화
        HeartManager.HM.HeartMove();  // 심장(체력) 상태를 갱신
        myController.CheckMyTurn();  // 내 턴 여부 확인 후 처리
    }

    // RPC_NewTurnStart: 모든 클라이언트에서 NewTurnStart를 호출하는 RPC 함수
    [PunRPC]
    public void RPC_NewTurnStart()
    {
        PV.RPC("NewTurnStart", RpcTarget.All);  // 모든 클라이언트에서 NewTurnStart를 호출
    }

    // GetCurTurn: 현재 턴을 반환하는 함수
    public int GetCurTurn()
    {
        return curTurn;  // 현재 턴 반환
    }

    // SetTarget: 내 캐릭터의 목표 위치를 설정하는 함수
    public void SetTarget(int i)
    {
        myController.SetTarget(i);  // 목표 위치를 설정
    }

    // Move: 내 캐릭터를 이동시키는 함수
    public void Move()
    {
        _ = myController.Move();  // 캐릭터를 이동
    }
}
