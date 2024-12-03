using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using static InitManager;
using static GameManager;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    public int curPosX;  // 현재 플레이어의 X 좌표
    public int curPosY;  // 현재 플레이어의 Y 좌표
    public GameObject targetPlanePrefab;  // 타겟 위치 표시를 위한 평면 프리팹
    public GameObject targetPlane;  // 타겟 위치 표시 객체

    public Vector2 targetPos;  // 타겟 위치
    public int targetPosX;  // 타겟 X 좌표
    public int targetPosY;  // 타겟 Y 좌표
    public int myTurn;  // 현재 플레이어의 턴 번호
    private bool isMyTurnSelected = false;  // 내 턴이 선택되었는지 확인하는 변수
    PhotonView PV;  // PhotonView 객체
    public static PlayerController playerController;  // PlayerController 싱글톤 객체
    public Animator anim;  // 플레이어 애니메이터

    void Awake()
    {
        PV = GetComponent<PhotonView>();  // PhotonView 컴포넌트 초기화
        playerController = GetComponent<PlayerController>();  // 싱글톤 객체 초기화
        IM.PlayerControllerList.Add(this);  // InitManager에 플레이어 컨트롤러 추가
        anim = GetComponent<Animator>();  // Animator 컴포넌트 초기화
    }

    // 타겟 평면을 초기화하는 함수
    public void InitTargetPlane()
    {
        targetPlane = Instantiate(targetPlanePrefab, transform);  // 타겟 평면을 생성
        targetPlane.SetActive(false);  // 처음에는 비활성화
    }

    // 'Bulddeok' 액션을 시작하는 함수
    public void Bulddeok()
    {
        targetPlane.SetActive(true);  // 타겟 평면을 활성화
        targetPosX = curPosX;  // 타겟 X 좌표 초기화
        targetPosY = curPosY;  // 타겟 Y 좌표 초기화
        targetPlane.transform.position = MapManager.MM.map_pos[targetPosX, targetPosY];  // 타겟 평면 위치 설정
    }

    // 현재 위치와 목표 위치 간의 거리를 계산하는 함수
    public int DistanceCheck()
    {
        return Mathf.Abs(curPosX - targetPosX) + Mathf.Abs(curPosY - targetPosY);  // X, Y 거리 차이의 합
    }

    // 목표 위치를 설정하는 함수
    public void SetTarget(int i)
    {
        // 타겟 위치 변경 (0: 왼쪽, 1: 오른쪽, 2: 아래, 3: 위)
        if(i == 0) targetPosX--;
        if(i == 1) targetPosX++;
        if(i == 2) targetPosY--;
        if(i == 3) targetPosY++;

        // 이동 거리가 4 이상이거나, 맵 범위를 벗어나면 원위치
        if(DistanceCheck() > 4 || targetPosX < 0 || targetPosX > 7 || targetPosY < 0 || targetPosY > 7)
        {
            if(i == 0) targetPosX++;
            if(i == 1) targetPosX--;
            if(i == 2) targetPosY++;
            if(i == 3) targetPosY--;
        }
        else
        {
            targetPlane.transform.position = MapManager.MM.map_pos[targetPosX, targetPosY];  // 타겟 평면 위치 갱신
        }
    }

    // 비동기로 이동하는 함수
    public async Task Move()
    {
        targetPlane.SetActive(false);  // 타겟 평면 비활성화
        anim.SetInteger("state", 1);  // 애니메이션 상태 변경

        // 타겟 위치로 플레이어를 이동
        GameManager.GM.mine.transform.LookAt(MapManager.MM.map_pos[targetPosX, targetPosY]);  // 목표를 향해 회전
        while(true)
        {
            if(transform.position == MapManager.MM.map_pos[targetPosX, curPosY])  // X 좌표에 도달하면
            {
                Debug.Log("while 1 break");
                curPosX = targetPosX;  // X 좌표 업데이트
                break;
            }
                
            transform.position = Vector3.MoveTowards(transform.position, MapManager.MM.map_pos[targetPosX, curPosY], 5f * Time.deltaTime);  // X 방향으로 이동
            await Task.Yield();  // 비동기적으로 계속 대기
        }

        GameManager.GM.mine.transform.LookAt(MapManager.MM.map_pos[targetPosX, targetPosY]);  // 목표를 향해 회전
        while(true)
        {
            if(transform.position == MapManager.MM.map_pos[targetPosX, targetPosY])  // Y 좌표에 도달하면
            {
                Debug.Log("while 2 break");
                curPosY = targetPosY;  // Y 좌표 업데이트
                break;
            }
                
            transform.position = Vector3.MoveTowards(transform.position, MapManager.MM.map_pos[targetPosX, targetPosY], 5f * Time.deltaTime);  // Y 방향으로 이동
            await Task.Yield();  // 비동기적으로 계속 대기
        }

        Inventory.IM.GetItem(curPosX, curPosY);  // 아이템 확인
        anim.SetInteger("state", 0);  // 애니메이션 상태 초기화
    }
    
    // Photon RPC를 통해 턴 설정
    [PunRPC]
    void SetTurn(int turnNumber)
    {
        if(isMyTurnSelected == false)
        {
            myTurn = turnNumber;  // 턴 번호 설정
            isMyTurnSelected = true;  // 턴이 설정되었음을 표시
        }
    }

    // 내 턴을 확인하는 함수
    [PunRPC]
    public void CheckMyTurn()
    {
        if(!PV.IsMine)  // PhotonView가 내 객체가 아니면 리턴
            return;
        
        Debug.Log(GM.GetCurTurn() + " 현재 턴");
        Debug.Log(gameObject.GetComponent<PlayerController>().myTurn + " 나의 턴");

        if(myTurn == GM.GetCurTurn())  // 현재 턴이 내 턴인지 확인
        {
            Debug.Log("myTurn!");
            UiManager.UM.MyTurnStart();  // 내 턴 시작
        }
        else
        {
            Debug.Log("not my Turn");
            UiManager.UM.OthersTurn();  // 다른 사람의 턴
        }
    }
}
