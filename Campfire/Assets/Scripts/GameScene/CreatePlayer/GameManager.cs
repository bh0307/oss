using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text txt;  // 현재 턴과 플레이어의 턴 정보를 표시할 UI 텍스트
    public int curTurn = 0;  // 현재 턴을 나타내는 변수 (0부터 시작)
    public int playerCount;  // 참가한 플레이어 수

    public GameObject mine;  // 현재 플레이어의 게임 오브젝트
    public PlayerController myController;  // 현재 플레이어의 컨트롤러
    public List<PlayerController> PlayerControllerList = new List<PlayerController>();  // 모든 플레이어의 컨트롤러 리스트

    public static GameManager GM;  // 싱글톤 인스턴스
    PhotonView PV;  // 네트워크 동기화 및 RPC 호출을 위한 PhotonView

    void Awake()
    {
        GM = this;  // 싱글톤 인스턴스 설정
        PV = GetComponent<PhotonView>();  // PhotonView 컴포넌트 초기화
    }

    void Update()
    {
        // 현재 턴과 내 턴 번호를 화면에 표시
        txt.text = "curTurn : " + curTurn.ToString() + " myTurn:" + myController.myTurn.ToString();
    }

    // 플레이어 수를 설정하는 RPC 메서드
    [PunRPC]
    public void SetPlayerCount(int p)
    {
        playerCount = p;  // 네트워크를 통해 받은 플레이어 수를 설정
    }

    // 플레이어 리스트를 설정하는 메서드 (동기화 불가하므로 추후 stream 이용 권장)
    public void SetValue(List<PlayerController> pl)
    {
        PlayerControllerList = pl;  // 플레이어 컨트롤러 리스트 설정
    }

    // 새로운 턴을 시작하는 RPC 메서드
    [PunRPC]
    public void NewTurnStart()
    {
        curTurn = (curTurn + 1) % playerCount;  // 턴을 다음 플레이어로 넘김 (순환 구조)
        UiManager.UM.LImitTime = 20f;  // 턴 시간 제한 초기화 (20초로 설정)
        
        // 인벤토리의 아이템 슬롯 버튼을 비활성화
        Inventory.IM.itemSlot[0].interactable = false;  
        Inventory.IM.itemSlot[1].interactable = false;  

        HeartManager.HM.HeartMove();  // 하트 관리 (회복 또는 감소)
        myController.CheckMyTurn();  // 현재 플레이어의 턴 여부 확인
    }

    // 모든 클라이언트에서 새로운 턴을 시작하도록 동기화하는 RPC 호출
    [PunRPC]
    public void RPC_NewTurnStart()
    {
        PV.RPC("NewTurnStart", RpcTarget.All);  // 모든 클라이언트에 NewTurnStart 호출
    }

    // 현재 턴을 반환하는 메서드
    public int GetCurTurn()
    {
        return curTurn;
    }

    // 플레이어가 이동 목표를 설정할 때 호출되는 메서드
    public void SetTarget(int i)
    {
        myController.SetTarget(i);  // `PlayerController`의 `SetTarget` 메서드 호출
    }

    // 플레이어가 실제 이동을 수행하도록 호출하는 메서드 (비동기 이동)
    public void Move()
    {
        _ = myController.Move();  // `PlayerController`의 `Move` 메서드 호출
    }
}
