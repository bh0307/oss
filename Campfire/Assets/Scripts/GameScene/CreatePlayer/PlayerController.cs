using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using static InitManager;
using static GameManager;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    public int curPosX;  // 현재 플레이어의 X 좌표
    public int curPosY;  // 현재 플레이어의 Y 좌표
    public GameObject targetPlanePrefab;  // 이동 목표 타일 프리팹
    public GameObject targetPlane;  // 생성된 이동 목표 타일 오브젝트

    public Vector2 targetPos;  // 이동할 목표 위치의 2D 좌표
    public int targetPosX;  // 목표 위치의 X 좌표
    public int targetPosY;  // 목표 위치의 Y 좌표
    public int myTurn;  // 플레이어의 턴 번호
    private bool isMyTurnSelected = false;  // 내 턴이 설정되었는지 여부
    PhotonView PV;  // 네트워크 동기화를 위한 PhotonView
    public static PlayerController playerController;  // 싱글톤 패턴 객체
    public Animator anim;  // 애니메이션 컨트롤러

    void Awake()
    {
        PV = GetComponent<PhotonView>();  // PhotonView 초기화
        playerController = GetComponent<PlayerController>();  // 싱글톤 인스턴스 설정
        IM.PlayerControllerList.Add(this);  // InitManager에 플레이어 컨트롤러 등록
        anim = GetComponent<Animator>();  // Animator 컴포넌트 초기화
    }

    // 목표 타일 초기화
    public void InitTargetPlane()
    {
        targetPlane = Instantiate(targetPlanePrefab, transform);  // 타일 생성
        targetPlane.SetActive(false);  // 초기에는 비활성화
    }

    // 목표 타일을 현재 위치로 설정하고 표시
    public void Bulddeok()
    {
        targetPlane.SetActive(true);  // 목표 타일 활성화
        targetPosX = curPosX;  // 목표 X 좌표를 현재 X로 설정
        targetPosY = curPosY;  // 목표 Y 좌표를 현재 Y로 설정
        targetPlane.transform.position = MapManager.MM.map_pos[targetPosX, targetPosY];  // 타일 위치 설정
    }

    // 현재 위치와 목표 위치 사이의 거리 계산 (맨해튼 거리)
    public int DistanceCheck()
    {
        return Mathf.Abs(curPosX - targetPosX) + Mathf.Abs(curPosY - targetPosY);
    }

    // 목표 좌표를 설정하는 메서드 (i 값에 따라 방향 결정)
    public void SetTarget(int i)
    {
        if (i == 0) targetPosX--;  // 위로 이동
        if (i == 1) targetPosX++;  // 아래로 이동
        if (i == 2) targetPosY--;  // 왼쪽으로 이동
        if (i == 3) targetPosY++;  // 오른쪽으로 이동

        // 이동 범위를 벗어나거나 거리 초과 시 좌표 복구
        if (DistanceCheck() > 4 || targetPosX < 0 || targetPosX > 7 || targetPosY < 0 || targetPosY > 7)
        {
            if (i == 0) targetPosX++;
            if (i == 1) targetPosX--;
            if (i == 2) targetPosY++;
            if (i == 3) targetPosY--;
        }
        else
        {
            // 범위를 벗어나지 않으면 목표 타일 위치 업데이트
            targetPlane.transform.position = MapManager.MM.map_pos[targetPosX, targetPosY];
        }
    }

    // 비동기로 플레이어 이동
    public async Task Move()
    {
        targetPlane.SetActive(false);  // 목표 타일 비활성화
        anim.SetInteger("state", 1);  // 이동 애니메이션 시작

        GameManager.GM.mine.transform.LookAt(MapManager.MM.map_pos[targetPosX, targetPosY]);  // 목표로 회전

        // X 축 이동
        while (true)
        {
            if (transform.position == MapManager.MM.map_pos[targetPosX, curPosY])
            {
                curPosX = targetPosX;
                break;  // 목표 X 좌표에 도달하면 탈출
            }
            transform.position = Vector3.MoveTowards(transform.position, MapManager.MM.map_pos[targetPosX, curPosY], 5f * Time.deltaTime);
            await Task.Yield();  // 비동기 작업 대기
        }

        // Y 축 이동
        while (true)
        {
            if (transform.position == MapManager.MM.map_pos[targetPosX, targetPosY])
            {
                curPosY = targetPosY;
                break;  // 목표 Y 좌표에 도달하면 탈출
            }
            transform.position = Vector3.MoveTowards(transform.position, MapManager.MM.map_pos[targetPosX, targetPosY], 5f * Time.deltaTime);
            await Task.Yield();
        }

        Inventory.IM.GetItem(curPosX, curPosY);  // 아이템 획득 시도
        anim.SetInteger("state", 0);  // 대기 상태로 전환
    }

    // 플레이어 턴 번호 설정 (한 번만 설정)
    [PunRPC]
    void SetTurn(int turnNumber)
    {
        if (!isMyTurnSelected)
        {
            myTurn = turnNumber;
            isMyTurnSelected = true;
        }
    }

    // 현재 턴이 내 턴인지 확인
    [PunRPC]
    public void CheckMyTurn()
    {
        if (!PV.IsMine) return;  // 내 객체가 아니면 리턴

        if (myTurn == GM.GetCurTurn())  // 현재 턴이 내 턴이면
        {
            UiManager.UM.MyTurnStart();  // 내 턴 UI 표시
        }
        else
        {
            UiManager.UM.OthersTurn();  // 상대 턴 UI 표시
        }
    }
}
