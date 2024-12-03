using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public GameObject[] prefabs; // 생성할 아이템들을 저장하는 배열
                                 // 이 배열에 여러 아이템의 프리팹을 추가하여 다양한 아이템을 생성할 수 있음
    private BoxCollider area;    // 아이템을 생성할 영역을 정의하는 BoxCollider 컴포넌트
    public int count = 11;        // 생성할 아이템의 수

    private List<GameObject> gameObject = new List<GameObject>(); // 생성된 아이템들을 저장할 리스트

    // 게임 시작 시 호출되는 함수
    void Start()
    {
        area = GetComponent<BoxCollider>();  // 해당 오브젝트의 BoxCollider 컴포넌트를 가져옴

        // 지정된 수(count)만큼 아이템을 생성
        for (int i = 0; i < count; ++i)
        {
            Spawn();  // 아이템을 생성하고 위치를 설정하는 함수 호출
        }

        area.enabled = false;  // BoxCollider를 비활성화하여 충돌 영역을 사용하지 않음
    }

    // 랜덤한 위치를 반환하는 함수
    private Vector3 GetRandomPosition()
    {
        Vector3 basePosition = transform.position;  // 현재 오브젝트의 위치를 기준으로 삼음
        Vector3 size = area.size;  // BoxCollider의 크기를 가져옴

        // BoxCollider 내에서 랜덤한 위치를 계산하여 반환
        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);
        float posZ = basePosition.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = new Vector3(posX, posY, posZ);  // 최종 랜덤 위치 계산

        return spawnPos;  // 랜덤 위치 반환
    }

    // 아이템을 생성하는 함수
    private void Spawn()
    {
        int selection = Random.Range(0, prefabs.Length);  // 아이템 프리팹 배열에서 랜덤으로 선택

        GameObject selectedPrefab = prefabs[selection];  // 선택된 아이템 프리팹

        Vector3 spawnPos = GetRandomPosition();  // 랜덤 위치 얻기

        // 선택된 아이템 프리팹을 해당 위치에 생성
        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        gameObject.Add(instance);  // 생성된 아이템을 리스트에 추가
    }
}
