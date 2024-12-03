using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateItem : MonoBehaviour
{
    public GameObject[] prefabs;            // 찍어낼 게임 오브젝트를 저장할 배열
                                            // 배열로 만든 이유는 다양한 종류의 게임 오브젝트를 랜덤으로 생성하기 위해서입니다

    public int[,] map = new int[8, 8];      // 게임 맵을 나타내는 8x8 배열
                                            // 각 위치에 배치된 아이템의 정보를 저장합니다
    public int count = 10;                  // 생성할 아이템의 개수

    // Start 함수는 객체가 활성화되면 처음으로 호출됩니다
    void Start()
    {
        for (int i = 0; i < count; i++)     // 지정된 개수(count) 만큼 아이템을 생성합니다
        {
            Spawn();  // 아이템을 생성하는 Spawn 함수 호출
        }
    }

    // Spawn 함수는 아이템을 맵에 랜덤하게 배치하는 함수입니다
    private void Spawn()
    {
        int posX;  // 아이템의 X 좌표
        int posY;  // 아이템의 Y 좌표
        int selection;  // 선택된 아이템의 인덱스
        GameObject selectedPrefab;  // 선택된 아이템의 프리팹

        while(true)  // 아이템 배치가 유효한 위치를 찾을 때까지 반복
        {
            // 랜덤하게 X, Y 좌표를 선택 (0부터 7까지의 값)
            posX = Random.Range(0, 8);
            posY = Random.Range(0, 8);

            // 맵의 중앙 부분(1,1)부터 (6,6)까지는 아이템을 생성하지 않음
            if (1 < posX && posX < 6 && 1 < posY && posY < 6)
            {
                continue;  // 중앙 부분은 제외하고 다시 랜덤한 좌표를 찾음
            }

            // 선택된 위치에 이미 아이템이 배치되어 있지 않다면
            if (map[posX, posY] == 0)           // map 배열의 값이 0이면 해당 위치에는 아이템이 없다는 의미
            {
                // 아이템을 랜덤하게 선택
                selection = Random.Range(1, prefabs.Length + 1);  // prefabs 배열의 길이를 기준으로 랜덤 선택
                selectedPrefab = prefabs[selection - 1];  // 선택된 아이템의 프리팹을 선택

                // 맵 배열에 해당 위치에 생성된 아이템 정보 저장
                map[posX, posY] = selection;    

                break;  // 유효한 위치에 아이템을 배치했으면 반복문을 종료
            }
        }

        // 아이템을 맵 상에 배치할 좌표 계산 (맵에서 각 위치는 2 단위 크기로 공간을 차지)
        Vector3 spawnPos = new Vector3(posX * 2, 0, posY * 2);

        // 실제 아이템을 생성 (현재는 주석 처리됨)
        //GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);

        Debug.Log(posX + " " + posY);  // 아이템이 생성된 위치를 콘솔에 출력
    }
}
