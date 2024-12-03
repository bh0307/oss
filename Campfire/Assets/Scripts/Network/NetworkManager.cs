using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// Photon 네트워크 관리를 담당하는 클래스
public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region 접속 상태
    // 현재 네트워크 상태를 문자열로 반환하는 메서드
    public string Status() => PhotonNetwork.NetworkClientState.ToString();
    #endregion

    #region 서버 접속/종료
    // Photon 서버에 접속하는 메서드 (설정값을 사용)
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    // 마스터 서버에 접속되었을 때 호출되는 콜백 메서드
    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon 서버 접속 완료");  // 서버 접속 성공 메시지 출력
        JoinLobby();  // 로비에 접속 시도
    }

    // 로비에 접속하는 메서드
    public void JoinLobby() => PhotonNetwork.JoinLobby();

    // 로비에 접속되었을 때 호출되는 콜백 메서드
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료");  // 로비 접속 성공 메시지 출력
        PhotonNetwork.NickName = "멋쟁이 " + Random.Range(0, 1000).ToString("0000");  // 랜덤 닉네임 설정
        PhotonNetwork.AutomaticallySyncScene = true;  // 모든 클라이언트가 씬을 자동으로 동기화
    }

    // 서버 연결을 끊는 메서드
    public void Disconnect() => PhotonNetwork.Disconnect();

    // 서버와의 연결이 끊어졌을 때 호출되는 콜백 메서드
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Photon 서버 접속 종료");  // 서버 접속 종료 메시지 출력
    }
    #endregion

    #region 방
    // 새로운 방을 생성하는 메서드 (최대 4명 입장 가능)
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(Random.Range(10, 1000).ToString(), new RoomOptions { MaxPlayers = 4 });
    }

    // 방 생성에 실패했을 때 호출되는 콜백 메서드
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room Creation Failed: " + message);  // 실패 메시지 출력
        // 방 생성 실패 시 새로운 방을 다시 생성 시도
        PhotonNetwork.CreateRoom(Random.Range(10, 1000).ToString(), new RoomOptions { MaxPlayers = 4 });
    }

    // 현재 방에서 나가는 메서드
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    // 특정 이름의 방에 접속하는 메서드
    public void JoinRoom(string RoomName) => PhotonNetwork.JoinRoom(RoomName);
    #endregion
}
