using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomManage : MonoBehaviourPunCallbacks
{
    string mapType;
    [SerializeField]
    TextMeshProUGUI OccupancyRateText_ForCafe;
 

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;//other player will load the same scene
      
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UI Callback Methods
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

   
    public void OnEnterRoomBtnClicked_Cafe()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_CAFE;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType} };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties,0);
    }
    #endregion

    #region Photon Callback Methods
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        CreateAndJoinRoom();

    }

    public override void OnCreatedRoom()
    {
        Debug.Log("A room is created with name: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()//only local player join will be called, other player join will not be called 
    {
        Debug.Log("The local player: "+PhotonNetwork.NickName + "Joined to"+PhotonNetwork.CurrentRoom.Name + " Player count: "+PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MultiplayerVRConstants.MAP_TYPE_KEY))
        {
            object mapType;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.MAP_TYPE_KEY, out mapType))
            {

                Debug.Log((string)mapType);
                if((string) mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_CAFE)
                {
                    PhotonNetwork.LoadLevel("World_Cafe");
                } 
            }
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined the lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)//when new player join the room
    {
        Debug.Log(newPlayer.NickName+"Joined with "+"Player Count:" + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {//if someone create, join or change propety of a room
        if (roomList.Count ==0)
        {
            OccupancyRateText_ForCafe.text = 0 + "/" + 20; 
        }
        foreach(RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            if (room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_CAFE))
            {
                OccupancyRateText_ForCafe.text = room.PlayerCount + "/" + 20;
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    #endregion

    #region Private Methods
    void CreateAndJoinRoom()
    {
        string randomRoomName = "Room_" + mapType + Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        string[] roomPropsInLobby = {MultiplayerVRConstants.MAP_TYPE_KEY };//there are 2 different maps
        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { {MultiplayerVRConstants.MAP_TYPE_KEY, mapType} };
        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;
        PhotonNetwork.CreateRoom(randomRoomName,roomOptions);
    }
    #endregion
}
