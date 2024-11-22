using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    #region 접속 상태
    public string Status() => PhotonNetwork.NetworkClientState.ToString();
    #endregion

    #region 서버 접속/종료
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon 서버 접속 완료");
        JoinLobby();
    }

    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료");
        PhotonNetwork.NickName = "멋쟁이 " + Random.Range(0, 1000).ToString("0000");
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Photon 서버 접속 종료");
    }



    #endregion

    #region 방

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(Random.Range(10, 1000).ToString(), new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room Creation Failed: " + message);
        PhotonNetwork.CreateRoom(Random.Range(10, 1000).ToString(), new RoomOptions { MaxPlayers = 4 });
    }

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public void JoinRoom(string RoomName) => PhotonNetwork.JoinRoom(RoomName);

    

    #endregion

}
