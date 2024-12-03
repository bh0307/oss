using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MapManager : MonoBehaviour
{
    // 게임 맵의 8x8 크기 배열 (각 위치에 배치된 아이템 정보)
    public int[,] map = new int[8, 8];
    
    // 각 맵 위치에 해당하는 월드 좌표 (Vector3 배열)
    public Vector3[,] map_pos = new Vector3[8, 8];
    
    // 그린존을 표시하는 배열 (true: 그린존, false: 그린존 아님)
    public bool[,] isGreenZone = new bool[8, 8];
    
    // 맵에 생성할 오브젝트의 갯수 (아이템 개수)
    public int count = 10;

    // MapManager 클래스의 싱글톤 인스턴스
    public static MapManager MM;

    // PhotonView 컴포넌트 참조 (네트워크 통신을 위해 사용)
    PhotonView PV;

    // Awake 함수: 스크립트가 처음 시작될 때 호출 (초기화)
    void Awake()
    {
        // MapManager 싱글톤 인스턴스 설정
        MM = this;

        // PhotonView 컴포넌트 가져오기 (네트워크 통신을 위해 필요)
        PV = GetComponent<PhotonView>();
    }

    // Start 함수: 게임 시작 시 한 번 호출
    void Start()
    {
        // 그린존 영역 설정
        SetGreenZone();

        // 만약 현재 플레이어가 마스터 클라이언트라면 아이템을 생성
        if (PhotonNetwork.IsMasterClient)
        {
            // 설정된 count 수만큼 아이템을 생성
            for (int i = 0; i < count; i++)
            {
                Spawn();
            }
        }
    }

    // SetMapPos 함수: 맵의 각 위치에 해당하는 월드 좌표를 설정
    public void SetMapPos()
    {
        // 8x8 맵에 대해 각 위치에 대한 월드 좌표 계산
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                // (i, j)에 해당하는 월드 좌표 계산 (X: 2*j, Z: -2*i, Y: 0)
                map_pos[i, j] = new Vector3(2 * j, 0, -2 * i);
            }
        }
    }

    // SetGreenZone 함수: 그린존 영역을 설정
    public void SetGreenZone()
    {
        // 8x8 맵에 대해 그린존 영역을 설정
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                // (1,1)부터 (6,6)까지 범위는 그린존, 나머지는 그린존 아님
                if ((1 < i && i < 6) && (1 < j && j < 6))
                {
                    isGreenZone[i, j] = true;  // 그린존
                }
                else
                {
                    isGreenZone[i, j] = false; // 그린존 아님
                }
            }
        }
    }

    // SetMapItem 함수: 네트워크에서 아이템을 설정하는 RPC 함수
    [PunRPC]
    public void SetMapItem(int posX, int posY, int item)
    {
        // 해당 위치에 아이템을 설정
        map[posX, posY] = item;
        // 콘솔에 위치 출력
        Debug.Log(posX + " " + posY);
    }

    // RPC_SetMapItem 함수: 네트워크 상의 모든 클라이언트에서 SetMapItem 호출
    public void RPC_SetMapItem(int posX, int posY, int item)
    {
        // 모든 클라이언트에서 SetMapItem을 호출하는 RPC
        PV.RPC("SetMapItem", RpcTarget.All, posX, posY, item);
        // 콘솔에 위치 출력
        Debug.Log(posX + " " + posY);
    }

    // Spawn 함수: 아이템을 랜덤한 위치에 생성하는 함수
    private void Spawn()
    {
        int posX;
        int posY;
        int selection;

        // 아이템을 배치할 수 있는 빈 위치를 찾을 때까지 반복
        while (true)
        {
            // 랜덤한 맵 좌표 선택 (0~7 범위)
            posX = Random.Range(0, 8);
            posY = Random.Range(0, 8);

            // 해당 위치가 비어 있고, 그린존이 아니면 아이템을 배치
            if (map[posX, posY] == 0 && !isGreenZone[posX, posY])
            {
                // 1부터 6까지 랜덤한 아이템 선택
                selection = Random.Range(1, 7);

                // 네트워크를 통해 모든 클라이언트에 아이템 배치 정보 전달
                PV.RPC("SetMapItem", RpcTarget.All, posX, posY, selection);
                break; // 아이템을 생성했으므로 반복문 종료
            }
        }
    }
}
