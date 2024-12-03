using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MapManager : MonoBehaviour
{
    public int[,] map = new int[8, 8];      // 게임 맵에 배치된 아이템에 대한 정보를 저장하는 8x8 배열
    public Vector3[,] map_pos = new Vector3[8, 8];  // 각 맵 위치에 대한 월드 좌표를 저장하는 8x8 배열
    public bool[,] isGreenZone = new bool[8, 8];     // 각 위치가 GreenZone인지 아닌지를 저장하는 8x8 배열
    public int count = 10;                  // 아이템을 생성할 횟수 (기본 10개)

    public static MapManager MM;            // MapManager 싱글톤 객체
    PhotonView PV;                          // PhotonView 컴포넌트

    // Awake: MapManager의 인스턴스를 싱글톤으로 설정하고, PhotonView 초기화
    void Awake()
    {
        MM = this;  // 싱글톤 객체로 설정
        PV = GetComponent<PhotonView>();  // PhotonView 컴포넌트 초기화
    }

    // Start: 맵 초기화 및 아이템 생성
    void Start()
    {
        SetGreenZone();  // GreenZone 설정
        if (PhotonNetwork.IsMasterClient)  // 이 객체가 MasterClient인 경우에만 아이템을 생성
        {
            for (int i = 0; i < count; i++)     // count 수만큼 아이템을 생성
            {
                Spawn();  // 아이템 생성
            }
        }
    }

    // SetMapPos: 맵의 각 위치에 월드 좌표를 설정
    public void SetMapPos()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                // 각 맵 위치에 해당하는 월드 좌표 설정 (2 단위로 간격)
                map_pos[i, j] = new Vector3(2 * j, 0, -2 * i);
            }
        }
    }

    // SetGreenZone: 맵 내에 GreenZone을 설정 (1~6의 범위 내에 있는 곳을 GreenZone으로 설정)
    public void SetGreenZone()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                // 1 < i < 6 && 1 < j < 6이면 GreenZone 설정
                if ((1 < i && i < 6) && (1 < j && j < 6))
                {
                    isGreenZone[i, j] = true;  // GreenZone
                }
                else
                {
                    isGreenZone[i, j] = false; // GreenZone 아님
                }
            }
        }
    }

    // SetMapItem: 특정 위치에 아이템을 설정 (Photon RPC)
    [PunRPC]
    public void SetMapItem(int posX, int posY, int item)
    {
        map[posX, posY] = item;  // 해당 위치에 아이템 번호 저장
        Debug.Log(posX + " " + posY);  // 디버깅 로그
    }

    // RPC_SetMapItem: Photon RPC를 통해 SetMapItem을 호출
    public void RPC_SetMapItem(int posX, int posY, int item)
    {
        PV.RPC("SetMapItem", RpcTarget.All, posX, posY, item);  // 모든 클라이언트에 SetMapItem 호출
        Debug.Log(posX + " " + posY);  // 디버깅 로그
    }

    // Spawn: 맵에 아이템을 랜덤하게 생성하는 함수
    private void Spawn()
    {
        int posX;
        int posY;
        int selection;

        while (true)
        {
            posX = Random.Range(0, 8);  // 0부터 7까지 랜덤 X 좌표 선택
            posY = Random.Range(0, 8);  // 0부터 7까지 랜덤 Y 좌표 선택

            // 해당 위치가 비어 있고 GreenZone이 아닌 경우에만 아이템을 생성
            if (map[posX, posY] == 0 && !isGreenZone[posX, posY])   
            {
                selection = Random.Range(1, 7);  // 아이템 종류를 1부터 6까지 랜덤 선택
                PV.RPC("SetMapItem", RpcTarget.All, posX, posY, selection);  // 모든 클라이언트에 아이템 위치 정보 전송
                break;  // 아이템을 한 번 생성하면 루프 종료
            }
        }
    }
}
