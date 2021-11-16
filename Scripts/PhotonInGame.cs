using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using COTUONG;
public class PhotonInGame : Photon.MonoBehaviour
{
    public int waitTime = 300;
    public GameObject PanelWait;
    public UILabel TimeWaitLeft;

    string cloneroomID = "";
    int cloneBet = 0;
    int timeThing = 0;
    int timeMax = 0;

    
    void OpenWaitTime()
    {
        StartCoroutine(UltilityGame.instance.TweenPanel(true, PanelWait));
        waitTime = 300;
        InvokeRepeating("WaitTimeDown", 1, 1);
    }
    void WaitTimeDown()
    {
        waitTime--;
        TimeWaitLeft.text = GameController.instance.CountTiming(waitTime);
        if (waitTime == 1)
        {
            CancelInvoke("WaitTimeDown");
            OnLeave();
        }
        
    }
    public virtual void OnJoinedLobby()
    {
        Debug.Log("Lobby Again");
    }
    public virtual void OnPhotonJoinRoomFailed()
    {
        Debug.Log("FAIL");
        SceneManager.LoadScene("NewRoomSelect");
    }
    public virtual void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
        SceneManager.LoadScene("NewRoomSelect");
    }
    void OnLeave()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("NewRoomSelect");
    }
    public void OnJoinedRoom()
    {
        GameController.instance.RoomStatus.text = "Mã Bàn : " + PhotonNetwork.room.CustomProperties["roomid"].ToString();

        if ((bool)PhotonNetwork.room.CustomProperties["ismatchmaking"] && PhotonNetwork.isMasterClient)
        {
            OpenWaitTime();
        }
    }
    public void OnClickMaBan()
    {
        GUIUtility.systemCopyBuffer = PhotonNetwork.room.CustomProperties["roomid"].ToString();
        NGUIMessage.instance.CALLALERT("Đã copy mã bàn !!");
    }
    public void OnPhotonPlayerConnected(PhotonPlayer New)
    {
        if (PhotonNetwork.isMasterClient && (bool)PhotonNetwork.room.CustomProperties["ismatchmaking"])
        {
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            h.Add("addplayer", 2);

            PhotonNetwork.player.SetCustomProperties(h);
            CancelInvoke("WaitTimeDown");
            StartCoroutine(UltilityGame.instance.TweenPanel(false, PanelWait));
            StartToCreateRoom();
        }
        if (PhotonNetwork.isMasterClient && !(bool)PhotonNetwork.room.CustomProperties["ismatchmaking"] && !GameController.instance.GameStarted)
        {
            //DA CO 2 NGUOI , CREATE ROOM INFO
            if(GameController.instance.RoomInfoIG)
            {
                PhotonNetwork.Destroy(GameController.instance.RoomInfoIG.gameObject);
                GameController.instance.CreateRoomName();
            }else
                GameController.instance.CreateRoomName();

        }
        if (GameController.instance.GameStarted && PhotonNetwork.isMasterClient)//RE CONNECT
        {
            string[] testStr = new string[2];
            testStr = New.NickName.Split('-');
            StartCoroutine(GameController.instance.UpdateAllPlayer());
            StartCoroutine(GameController.instance.CheckIsPlayerRejoin((testStr[0]),GameController.instance.MatchTime));
            StartCoroutine(GameController.instance.AddChessIfStarted(testStr[0]));
            StartCoroutine(GameController.instance.ADDSTEPFOROTHER(testStr[0]));
        }
    }
    public virtual void OnPhotonPlayerDisconnected(PhotonPlayer _Name)
    {//ONLY MASTER
        if (PhotonNetwork.isMasterClient)
        {
            string[] testStr = new string[3];
            testStr = _Name.NickName.Split('-');
            if (!GameController.instance.GameStarted)//NEU GAME CHUA START , DELETE PLAYER NGAY LAP TUC
                GameController.instance.DeletePlayer(testStr[0]);
            else
            {
                GameController.instance.WaitReconnect(testStr[0]);
            }
        }
    }
    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        SceneManager.LoadScene("NewSelectRoom");
    }
    void StartToCreateRoom()
    {
        char[] cloneID = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM".ToCharArray();
        string cloneRoom = "";
        for (int i = 0; i < 6; i++)
        {
            cloneRoom = cloneRoom + cloneID[UnityEngine.Random.Range(0, cloneID.Length)];
        }
        cloneRoom = RoomSelect.instance.CurrentRoom.RoomID + "-" + cloneRoom;
        photonView.RPC("GoToNewRoom", PhotonTargets.All, cloneRoom, RoomSelect.instance.CurrentRoom.BetCoins, RoomSelect.instance.CurrentRoom.TimeThink, RoomSelect.instance.CurrentRoom.TimeMax);
    }
    

    [PunRPC]
    void GoToNewRoom(string _roomID, int _bet, int _timeThink, int _timeMax)
    {
        Debug.Log("recieve new room ");
        cloneroomID = _roomID;
        cloneBet = _bet;
        timeThing = _timeThink;
        timeMax = _timeMax;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinLobby();

        if(PhotonNetwork.isMasterClient)
            InvokeRepeating("CreateRoom", 1, 1);
        else
            InvokeRepeating("CreateRoom", 1.5f, 1.5f);
    }

    void CreateRoom()
    {
        if (PhotonNetwork.insideLobby)
        {
            CancelInvoke("CreateRoom");

            ExitGames.Client.Photon.Hashtable customPropreties = new ExitGames.Client.Photon.Hashtable();
            customPropreties["ismatchmaking"] = false;
            customPropreties["roomid"] = cloneroomID;
            customPropreties["bet"] = cloneBet;
            customPropreties["timethink"] = timeThing;
            customPropreties["timemax"] = timeMax;
            RoomOptions roomOptions = new RoomOptions() { CustomRoomProperties = customPropreties, IsVisible = true, IsOpen = true, MaxPlayers = 2, CleanupCacheOnLeave = false };

            roomOptions.CustomRoomPropertiesForLobby = new string[]
            {
                "ismatchmaking",
                "roomid",
                "bet",
                "timethink",
                "timemax"
            };
            PhotonNetwork.JoinOrCreateRoom(cloneroomID, roomOptions, TypedLobby.Default);
            Debug.Log("Create or Start Room");
        }
    }
}

