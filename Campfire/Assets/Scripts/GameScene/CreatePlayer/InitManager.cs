using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;
using static GameManager;
using static MapManager;

public class InitManager : MonoBehaviour
{
    private int posX, posY;  // 플레이어의 초기 위치 (X, Y 좌표)
    private Vector3 newPos;  // 실제 월드 공간에서의 위치
    private int playerCount;  // 게임에 참여한 플레이어의 수
    public List<PlayerController> PlayerControllerList = new List<PlayerController>();  // 모든 플레이어의 컨트롤러 리스트

    public static InitManager IM;  // InitManager 클래스의 인스턴스
    PhotonView PV;  // PhotonView 컴포넌트

    // Awake 함수는 객체가 활성화될 때 최초로 호출되는 함수
    void Awake()
    {
        IM = this;  // InitManager의 인스턴스를 설정
        PV = GetComponent<PhotonView>();  // PhotonView 컴포넌트를 가져옴
    }

    // 게임 시작 시 호출되는 함수
    void Start()
    {
        MM.SetMapPos();  // 맵 위치 설정
        MM.SetGreenZone();  // 녹색 영역 설정 (특정 지역을 설정하는 함수일 가능성)
        RandomPos();  // 랜덤으로 플레이어의 초기 위치를 설정
        CreatePlayer();  // 플레이어 오브젝트 생성
        StartCoroutine(SetPlayersInfo());  // 모든 플레이어의 정보를 설정하는 코루틴 시작
    }

    // 랜덤한 위치를 선정하여 플레이어의 초기 위치를 설정하는 함수
    void RandomPos()
    {
        while(true)
        {
            posX = Random.Range(2, 6);  // X 좌표를 2에서 5 사이에서 랜덤으로 선택
            posY = Random.Range(2, 6);  // Y 좌표를 2에서 5 사이에서 랜덤으로 선택
            if(posX == 2 || posX == 5 || posY == 2 || posY == 5)  // X, Y 좌표가 2 또는 5일 때만 선택
            {
                newPos = MM.map_pos[posX, posY];  // 맵의 해당 좌표에서 새로운 위치를 얻음
                break;  // 랜덤 위치가 조건에 맞으면 루프 종료
            }
        }
    }

    // 플레이어 오브젝트를 생성하고 초기 설정을 하는 함수
    void CreatePlayer()
    {
        // PhotonNetwork를 사용하여 플레이어 오브젝트를 생성
        GM.mine = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PlayerObjWithAnim"), newPos, Quaternion.identity);
        GM.myController = GM.mine.GetComponent<PlayerController>();  // 생성된 플레이어 오브젝트에서 PlayerController 컴포넌트를 가져옴
        GM.myController.curPosX = posX;  // 플레이어의 현재 X 좌표 설정
        GM.myController.curPosY = posY;  // 플레이어의 현재 Y 좌표 설정
        GM.myController.InitTargetPlane();  // 타겟 평면 초기화
    }

    // 플레이어들의 순서를 설정하는 코루틴
    IEnumerator SetPlayersInfo()
    {
        yield return new WaitForSeconds(1f);  // 1초 기다림 (네트워크 동기화 대기)
        
        // 마스터 클라이언트에서만 실행되는 코드
        if(PhotonNetwork.IsMasterClient)
        {
            playerCount = PlayerControllerList.Count;  // 참여한 플레이어 수

            // 모든 플레이어에 대해 순서 설정
            for(int i = 0; i < playerCount; i++)
            {
                // 각 플레이어의 순서를 설정 (RPC를 통해 모든 클라이언트에 전달)
                PlayerControllerList[i].GetComponent<PhotonView>().RPC("SetTurn", RpcTarget.All, i);
            }

            // 모든 클라이언트에게 플레이어 수를 전달
            GM.GetComponent<PhotonView>().RPC("SetPlayerCount", RpcTarget.All, playerCount);
        }

        // 각 플레이어의 순서에 따라 게임의 상태를 설정
        GM.SetValue(PlayerControllerList);  

        yield return new WaitForSeconds(1f);  // 1초 기다림 (게임 진행 전에 여유를 둠)
        
        // 새로운 턴이 시작됨
        GM.NewTurnStart();
    }
}
