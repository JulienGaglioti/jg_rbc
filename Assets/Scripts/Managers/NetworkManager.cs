using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    #region Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.LogError("Connected to Server. Looking for random room.");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"Join failed because of {message}. Creating a new one.");
        PhotonNetwork.CreateRoom(null);
    }

    #endregion
}
