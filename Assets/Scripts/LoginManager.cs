using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class LoginManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField PlayerName_InputField;
    #region UNITY Methods 
    // Start is called before the first frame update
    
    #endregion


    #region UI Callback Methods

    public void ConnectToPhotonServer()
    {
        if (PlayerName_InputField != null)
        {
            PhotonNetwork.NickName = PlayerName_InputField.text;
            PhotonNetwork.ConnectUsingSettings();
        } 
        
    }

    #endregion

    #region Photon Callback Methods
    public override void OnConnected() //First method that will be called when conencted 
    {
        Debug.Log("OnConnect is called, the server is available");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the master server with player name " +PhotonNetwork.NickName);
        PhotonNetwork.LoadLevel("HomeScene");
    }
    #endregion
}
