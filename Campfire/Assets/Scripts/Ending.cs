using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

// 게임의 엔딩 처리를 관리하는 클래스
public class Ending : MonoBehaviour
{
    // 특정 위치에서 필요한 아이템의 목록
    ItemType[] EndingItem = { ItemType.Hammer, ItemType.Pickax, ItemType.Shovels, ItemType.Sickle };
    PhotonView PV;  // Photon 네트워크 동기화를 위한 PhotonView 객체

    void Start()
    {
        // PhotonView 컴포넌트 가져오기
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        // 필요 시 업데이트 코드 추가 가능 (현재는 비어 있음)
    }

    // 특정 좌표(posX, posY)에서 엔딩 조건을 확인하는 메서드
    public void CheckEnding(int posX, int posY)
    {
        Inventory inventory = Inventory.IM;  // 인벤토리 싱글톤 객체 참조
        if (inventory == null) return;  // 인벤토리가 없으면 종료

        ItemType requiredItem = GetRequiredItem(posX, posY);  // 해당 좌표에 필요한 아이템 가져오기
        if (requiredItem == ItemType.NULL) return;  // 필요 아이템이 없으면 종료

        // 플레이어가 필요한 아이템을 보유하고 있는지 확인
        bool hasItem = inventory.itemArr[0] == requiredItem || inventory.itemArr[1] == requiredItem;
        if (hasItem)
        {
            // 모든 클라이언트에서 게임 종료를 실행하도록 RPC 호출
            PV.RPC("EndGame", RpcTarget.All);
        }
        else
        {
            // 필요한 아이템이 없을 경우 경고 메시지 출력
            UiManager.UM.SetNotice($"{requiredItem} 아이템이 필요합니다!");
        }
    }

    // 특정 좌표에서 필요한 아이템을 반환하는 메서드
    ItemType GetRequiredItem(int x, int y)
    {
        if (x == 0 && y == 0) return ItemType.Hammer;  // 좌상단: 망치
        if (x == 7 && y == 0) return ItemType.Pickax;  // 우상단: 곡괭이
        if (x == 0 && y == 7) return ItemType.Sickle;  // 좌하단: 낫
        if (x == 7 && y == 7) return ItemType.Shovels; // 우하단: 삽
        return ItemType.NULL;  // 필요한 아이템이 없는 경우
    }

    // 게임 종료 처리를 위한 RPC 메서드
    [PunRPC]
    void EndGame()
    {
        // 게임 종료 메시지 출력
        UiManager.UM.SetNotice("게임 종료! 로비로 돌아갑니다.");
        // 로비 씬으로 이동
        SceneManager.LoadScene("LobbyScene");
    }
}
