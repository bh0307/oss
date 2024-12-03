using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region 변수

    [Header("Network")]
    [SerializeField] private NetworkManager networkManager;  // 네트워크 매니저 참조

    [Header("Panel")]
    public GameObject lobbyPanel;  // Lobby 화면을 표시하는 Panel
    public GameObject matchingPanel;  // 매칭 화면을 표시하는 Panel

    [Header("Button")]
    public Button[] roomBtn;  // 방 버튼 배열
    public Button prev, next, back;  // 이전, 다음, 뒤로 가기 버튼

    [Header("ETC")]
    public Text StatusText;  // 네트워크 상태를 보여줄 텍스트
    int cur = 0, maxpage;  // 현재 페이지, 최대 페이지
    private List<RoomInfo> current_roomList = new List<RoomInfo>();  // 현재 방 리스트

    #endregion

    #region start,update

    // 게임 시작 시 호출되는 함수
    void Start()
    {
        // networkManager.Connect();  // 네트워크 연결을 시도 (현재는 비활성화)
        matchingPanel.SetActive(false);  // 매칭 패널 비활성화
    }

    // 매 프레임 호출되는 함수
    void Update()
    {
        StatusText.text = networkManager.Status();  // 네트워크 상태 텍스트 업데이트
    }

    #endregion

    #region panel

    // 패널 전환 함수 (로비와 매칭 화면을 전환)
    public void PanelChange()
    {
        lobbyPanel.SetActive(!lobbyPanel.activeSelf);  // 로비 패널의 활성/비활성화 상태 변경
        matchingPanel.SetActive(!lobbyPanel.activeSelf);  // 매칭 패널은 로비 패널과 항상 반대 상태로 설정
    }

    #endregion

    #region RoomList Update

    // 방 리스트 업데이트 함수 (Photon 네트워크에서 방 리스트가 변경될 때 호출됨)
    public override void OnRoomListUpdate(List<RoomInfo> changed_roomList)  
    {
        Debug.Log("RoomListUpdate");
        CurRoomListUpdate(changed_roomList);  // 변경된 방 리스트를 현재 방 리스트에 반영
        ButtonUpdate(current_roomList);  // 방 리스트를 바탕으로 버튼 업데이트
    }

    // 변경된 방 리스트를 바탕으로 현재 방 리스트를 업데이트하는 함수
    public void CurRoomListUpdate(List<RoomInfo> changed_roomList)
    {
        // 변경된 방 리스트를 순차적으로 처리
        for (int i = 0; i < changed_roomList.Count; i++)
        {
            if (changed_roomList[i].RemovedFromList)
                current_roomList.RemoveAt(current_roomList.IndexOf(changed_roomList[i]));   // 방이 삭제되었으면 리스트에서 제거
            else
            {
                if (current_roomList.Contains(changed_roomList[i]))
                    current_roomList[current_roomList.IndexOf(changed_roomList[i])] = changed_roomList[i]; // 변경된 방 정보 업데이트
                else
                    current_roomList.Add(changed_roomList[i]); // 새로 생성된 방 추가
            }
        }

        // 방 정보 출력 (디버그용)
        for (int i = 0; i < current_roomList.Count; i++)
            print("Debug : " + i.ToString() + " " + current_roomList[i].Name + " " + current_roomList[i].PlayerCount.ToString());

        maxpage = current_roomList.Count / 4;  // 페이지 수 계산 (한 페이지에 최대 4개 방)

        // 페이지 번호가 최대 페이지를 넘지 않도록 조정
        if (cur > maxpage)
        {
            cur = maxpage;
        }
    }

    // 방 리스트를 바탕으로 방 버튼들을 업데이트하는 함수
    public void ButtonUpdate(List<RoomInfo> current_roomList)
    {
        for (int i = 0; i < roomBtn.Length; i++)
        {
            // 현재 페이지에 해당하는 방이 없으면 버튼을 비활성화하고 텍스트를 비워둠
            if (cur * 4 + i >= current_roomList.Count)
            {
                roomBtn[i].interactable = false;  // 버튼 비활성화
                roomBtn[i].transform.GetChild(0).GetComponent<Text>().text = "";  // 방 이름 비우기
                roomBtn[i].transform.GetChild(1).GetComponent<Text>().text = "";  // 플레이어 수 비우기
            }
            else
            {
                // 해당 방이 있으면 버튼을 활성화하고 정보를 표시
                roomBtn[i].interactable = true;
                roomBtn[i].transform.GetChild(0).GetComponent<Text>().text = current_roomList[cur * 4 + i].Name;  // 방 이름 표시
                roomBtn[i].transform.GetChild(1).GetComponent<Text>().text = current_roomList[cur * 4 + i].PlayerCount.ToString() + "/4";  // 플레이어 수 표시
            }
        }
    }

    // 다음 페이지로 이동하는 함수
    public void Next()
    {
        if (cur < maxpage)
        {
            cur++;  // 페이지 번호 증가
            ButtonUpdate(current_roomList);  // 버튼 업데이트
            Debug.Log(cur + " " + maxpage);
        }
    }

    // 이전 페이지로 이동하는 함수
    public void Prev()
    {
        if (cur > 0)
        {
            cur--;  // 페이지 번호 감소
            ButtonUpdate(current_roomList);  // 버튼 업데이트
        }
    }
    #endregion

    #region JoinRoom

    // 방에 입장하는 함수 (버튼 클릭 시 호출됨)
    public void JoinRoom(int i)
    {
        string RoomName = roomBtn[i].transform.GetChild(0).GetComponent<Text>().text;  // 선택한 방 이름 가져오기
        networkManager.JoinRoom(RoomName);  // 네트워크 매니저를 통해 해당 방에 입장
    }

    // 방에 성공적으로 입장한 후 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log("방 접속 완료");
        SceneManager.LoadScene("RoomScene");  // 방에 입장한 후 RoomScene으로 이동
    }
    #endregion
}
