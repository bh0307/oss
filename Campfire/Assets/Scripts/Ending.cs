using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    ItemType[] EndingItem = { ItemType.Hammer, ItemType.Pickax, ItemType.Shovels, ItemType.Sickle };
    PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        // 필요시 업데이트 코드 추가 가능
    }

    public void CheckEnding(int posX, int posY)
    {
        Inventory inventory = Inventory.IM;
        if (inventory == null) return;

        ItemType requiredItem = GetRequiredItem(posX, posY);
        if (requiredItem == ItemType.NULL) return;

        bool hasItem = inventory.itemArr[0] == requiredItem || inventory.itemArr[1] == requiredItem;
        if (hasItem)
        {
            PV.RPC("EndGame", RpcTarget.All);
        }
        else
        {
            UiManager.UM.SetNotice($"{requiredItem} 아이템이 필요합니다!");
        }
    }

    ItemType GetRequiredItem(int x, int y)
    {
        if (x == 0 && y == 0) return ItemType.Hammer;
        if (x == 7 && y == 0) return ItemType.Pickax;
        if (x == 0 && y == 7) return ItemType.Sickle;
        if (x == 7 && y == 7) return ItemType.Shovels;
        return ItemType.NULL;
    }

    [PunRPC]
    void EndGame()
    {
        UiManager.UM.SetNotice("게임 종료! 로비로 돌아갑니다.");
        SceneManager.LoadScene("LobbyScene");
    }
}
