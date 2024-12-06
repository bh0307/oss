using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public ItemType[] itemArr = new ItemType[2];    // 내 인벤토리 (2개의 아이템 슬롯)
    public Button[] itemSlot = new Button[2];       // 아이템 슬롯(버튼) UI
    public static Inventory IM;  // Inventory 클래스의 싱글톤 인스턴스

    PhotonView PV;  // PhotonView 컴포넌트

    // Awake 함수는 객체가 활성화될 때 최초로 호출되는 함수
    void Awake()
    {
        IM = this;  // Inventory의 인스턴스를 설정
        PV = GetComponent<PhotonView>();  // PhotonView 컴포넌트를 가져옴
        
        // 아이템 배열을 초기화 (NULL로 설정)
        for (int i = 0; i < itemArr.Length; i++)
        {
            itemArr[i] = ItemType.NULL;
        }
    }

    // 아이템을 획득하는 함수 (현재 위치에서 아이템을 얻음)
    public void GetItem(int curPosX, int curPosY)
    {
        for (int i = 0; i < itemArr.Length; i++)
        {
            // 인벤토리가 비어있는 슬롯에 아이템을 추가
            if (itemArr[i] == ItemType.NULL)
            {
                // 아이템을 슬롯에 설정하고, 맵에서 해당 아이템을 제거
                SetItemSlot(i, (ItemType)MapManager.MM.map[curPosX, curPosY]);
                MapManager.MM.RPC_SetMapItem(curPosX, curPosY, 0); // 다른 플레이어와 동기화

                Debug.Log("획득아이템 : " + (int)itemArr[i]);

                // 특정 위치에서 종료 조건을 체크
                if ((curPosX == 0 && curPosY == 0) || (curPosX == 7 && curPosY == 0) || (curPosX == 0 && curPosY == 7) || (curPosX == 7 && curPosY == 7))
                    EndingCheck(curPosX, curPosY);
                else if (itemArr[i] == ItemType.NULL)
                    UiManager.UM.SetNotice("이곳엔 아이템이 없다.");
                else
                    UiManager.UM.SetNotice("아이템을 얻었다.");
                
                break;
            }
        }
    }

    // 게임 종료를 처리하는 RPC 함수
    [PunRPC]
    void End()
    {
        SceneManager.LoadScene(0);  // 게임을 종료하고 첫 번째 씬(메인 메뉴)으로 로드
    }

    // 종료 조건을 체크하는 함수 (특정 위치에서 필요한 아이템을 갖고 있는지 확인)
    public void EndingCheck(int curPosX, int curPosY)
    {
        if (curPosX == 0 && curPosY == 0)
        {
            if (itemArr[0] == ItemType.Hammer || itemArr[1] == ItemType.Hammer)
            {
                PV.RPC("End", RpcTarget.All);  // 망치가 있으면 게임 종료
            }
            else
            {
                UiManager.UM.SetNotice("망치를 가져와야할것 같다.");
            }
        }
        if (curPosX == 7 && curPosY == 0)
        {
            if (itemArr[0] == ItemType.Pickax || itemArr[1] == ItemType.Pickax)
            {
                PV.RPC("End", RpcTarget.All);  // 곡괭이가 있으면 게임 종료
            }
            else
            {
                UiManager.UM.SetNotice("곡괭이를 가져와야할것 같다.");
            }
        }
        if (curPosX == 0 && curPosY == 7)
        {
            if (itemArr[0] == ItemType.Sickle || itemArr[1] == ItemType.Sickle)
            {
                PV.RPC("End", RpcTarget.All);  // 낫이 있으면 게임 종료
            }
            else
            {
                UiManager.UM.SetNotice("낫을 가져와야할것 같다.");
            }
        }
        if (curPosX == 7 && curPosY == 7)
        {
            if (itemArr[0] == ItemType.Shovels || itemArr[1] == ItemType.Shovels)
            {
                PV.RPC("End", RpcTarget.All);  // 삽이 있으면 게임 종료
            }
            else
            {
                UiManager.UM.SetNotice("삽을 가져와야할것 같다.");
            }
        }
    }

    // 아이템 조합을 시도하는 함수
    public void MixItem()
    {
        ItemType itemA = itemArr[0];  // 첫 번째 아이템
        ItemType itemB = itemArr[1];  // 두 번째 아이템

        // 아이템을 정렬하여 두 개의 아이템을 비교하기 쉬운 형태로 만듦
        if ((int)itemA > (int)itemB)
        {
            ItemType temp = itemA;
            itemA = itemB;
            itemB = temp;
        }

        int result = ((int)itemA * 10 + (int)itemB);  // 아이템 조합 결과 계산

        // 조합 가능한 아이템에 대해 처리 (조합 불가능한 경우 메시지 출력)
        switch (result)
        {
            case 13:
            case 14:
            case 25:
            case 26:
                break;
            default:
                UiManager.UM.SetNotice("조합할 수 없다.");  // 조합 불가능 메시지
                Debug.Log("조합할 수 없습니다.");
                return;
        }

        // 조합 완료
        Debug.Log("조합 완료");
        UiManager.UM.SetNotice("아이템을 하나 완성했다.");
        SetItemSlot(0, (ItemType)result);  // 새로운 아이템을 첫 번째 슬롯에 설정
        SetItemSlot(1, ItemType.NULL);  // 두 번째 슬롯은 비움
    }

    // 아이템을 버리기 위한 버튼 활성화 함수
    public void ThrowItemButton()
    {
        // 현재 위치가 맵에서 아이템을 버릴 수 있는 곳인지 확인
        if (MapManager.MM.map[GameManager.GM.myController.curPosX, GameManager.GM.myController.curPosX] == 0)
        {
            // 인벤토리가 비어 있으면 메시지 출력
            if (itemArr[0] == ItemType.NULL && itemArr[1] == ItemType.NULL)
            {
                UiManager.UM.SetNotice("버릴 아이템이 없다.");
                return;
            }

            Debug.Log("버릴 아이템을 선택하세요");
            UiManager.UM.SetNotice("오른쪽에서 버릴 아이템을 선택해야한다.");

            // UI에서 아이템 선택 가능하도록 버튼 활성화
            itemSlot[0].interactable = true;
            itemSlot[1].interactable = true;
        }
        else
        {
            UiManager.UM.SetNotice("이미 뭔가 버려져있다.");
            Debug.Log("이미 뭔가 버려져있다.");
        }
    }

    // 아이템을 버리는 함수 (선택된 슬롯의 아이템을 버림)
    public void ThrowItem(int i)
    {
        itemSlot[0].interactable = false;  // 아이템 슬롯 비활성화
        itemSlot[1].interactable = false;
        // 맵에서 아이템을 제거하고, 해당 슬롯을 비움
        MapManager.MM.RPC_SetMapItem(GameManager.GM.myController.curPosX, GameManager.GM.myController.curPosY, (int)itemArr[i]);
        SetItemSlot(i, ItemType.NULL);  // 해당 슬롯을 비움
        UiManager.UM.MyTurnAction();  // 턴 종료
    }

    // 인벤토리 슬롯을 설정하는 함수 (아이템 설정 및 UI 업데이트)
    public void SetItemSlot(int i, ItemType item)
    {
        itemArr[i] = item;  // 인벤토리 배열을 업데이트
        itemSlot[i].GetComponent<Image>().sprite = ItemContainor.IC.itemImg[(int)item];  // UI 이미지 업데이트
    }
}
