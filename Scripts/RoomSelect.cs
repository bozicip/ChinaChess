using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Net;

public class NewRoomInfo
{
	public string RoomID;
	public int TimeThink;
	public int TimeMax;
	public int BetCoins;
	public void SetRoom(string _IDroom, int _timeThink, int _timeMax, int _betCoins)
	{
		RoomID = _IDroom;
		TimeThink = _timeThink;
		TimeMax = _timeMax;
		BetCoins = _betCoins;
	}
}
public class RoomSelect : Photon.MonoBehaviour
{
	public static RoomSelect instance;
	public NewSelectPhoton PNetwork;
	//public UILabel StatusConnect;
	public NewRoomInfo CurrentRoom, CURRENTCHANNEL;

	public static bool EnableAdmin = false;
	public static int AvatarSelec = 0;
	public UI2DSprite mainAvatar;
	public static float timeTweenPanel = 0.3f;
	public static int HeSoThoiGian = 2;

	//public UIPopupList SelectPanel;
	//public UILabel LB_LuatHienThi;
	public AudioSource Music;
	public UILabel pName, pCoin;
	public GameObject ExchangePanel;
	public RoomInfopref[] listChannel;

	// Use this for initialization

	public GameObject PanelReconnect;
	public GameObject CreateRoomPanel;
	public RoomInfopref[] RoomList;
	public avatarInfo[] listAvatar;
	public const string TruyenThong = "Truyền Thống";
	public const string CoUp = "Cờ Úp";
	public const string CoLoan = "Cờ Loạn";
	public const string MoPhong = "Mô Phỏng";

	public GameObject PanelAvatarChange;

	public UILabel _JSON;
	public bool isLoadCompleted = false;
	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
	public void GetJson(string _json)
    {
		_JSON.text = _json;
		JSON gameData = new JSON();
		gameData.serialized = _json;

		//parse JSON
		UserConfig.Data.UserID = gameData.ToString("playerid");
		UserConfig.Data.UserName = gameData.ToString("playername");
		UserConfig.Data.UserCoins = gameData.ToInt("playercoin");
		GetUserDATA(UserConfig.Data.UserID, UserConfig.Data.UserName);
	}
	//======================MAIN AVATAR=====================================
	void OnOpenAvatarChange()
    {
		StartCoroutine(UltilityGame.instance.TweenPanel(true,PanelAvatarChange));
    }
	void OnCloseAvatarChange()
    {
		StartCoroutine(UltilityGame.instance.TweenPanel(false, PanelAvatarChange));
	}
	public void OnChangeAvatar()
    {
		PlayerPrefs.SetInt("CoUpAvatar",AvatarSelec);
		mainAvatar.sprite2D = LoadPrefab.instance.SpriteAvatars[AvatarSelec];
		OnCloseAvatarChange();
	}
	void DefaultAvatar()
    {
		AvatarSelec = PlayerPrefs.GetInt("CoUpAvatar", 0);
		mainAvatar.sprite2D = LoadPrefab.instance.SpriteAvatars[AvatarSelec];
		for (int i = 0; i < listAvatar.Length; i++)
        {
			listAvatar[i].index = i;
			listAvatar[i].OnChoosen.gameObject.SetActive(false);
        }
		listAvatar[AvatarSelec].OnChoosen.gameObject.SetActive(true);
	}
	public GameObject PanelCreateName;
	public UIInput nameInput;
	string cloneName;
	void GetStartedAndroid()
    {
		cloneName = PlayerPrefs.GetString("PLAYERNAME",UserConfig.Data.UserName);
		//GetUserDATA(UserConfig.DeviceID,cloneName);
        
    }
	public void OnSubmitName()
    {
		if (nameInput.value != "")
		{
			cloneName = nameInput.value;
			cloneName = cloneName.Replace(" ", "");
			PlayerPrefs.SetString("PLAYERNAME", cloneName);
			StartCoroutine(UltilityGame.instance.TweenPanel(false, PanelCreateName));
			GetUserDATA(SystemInfo.deviceUniqueIdentifier.Substring(0, 6), cloneName);
        }
        else
        {
			NGUIMessage.instance.CALLALERT("Vui lòng nhập tên người chơi ");
        }
	}
	//====================================================================
	void Start()
	{
		if (!PhotonNetwork.insideLobby)
			PhotonNetwork.JoinLobby();
		isLoadCompleted = false;
		
		NGUIMessage.instance.CALLLOADING(true);
		FakeRoom();
		DefaultAvatar();
#if UNITY_WEBGL
		Invoke("TryToGetJsonPlayer", 1);//FOR WEBGL
#endif
#if UNITY_ANDROID
		GetStartedAndroid();
#endif
		/*
#if UNITY_EDITOR
		COIN_DTV = 99999;
		NAME_DTV = "LUMPRO";
		ID_DTV = "LUMEO123";
		GetUserDATA(ID_DTV, NAME_DTV);
#endif
		*/
	}
	void TryToGetJsonPlayer()
    {
		Application.ExternalCall("SendJson","Hello ? ");
    }
	void GetUserDATA(string _DTV_ID,string _DTV_NAME)
    {
		pName.text = UserConfig.Data.UserName;
		pCoin.text = UserConfig.Data.UserCoins.ToString("C0");
		/*
		string cloneName = _DTV_NAME.Replace(" ", "");
		StartCoroutine(COTUONG.ReqAPI.Instance.GetDataCoUp(_DTV_ID, cloneName, _GETUSERIDHERE => {
			UserConfig.Data_CoTuong = _GETUSERIDHERE;
			//SHOW INFO AFTER LOAD

			
			PhotonNetwork.playerName = UserConfig.Data_CoTuong.UserID + "-" + UserConfig.Data_CoTuong.UserName;
			isLoadCompleted = true;
		}));
		*/
	}
	//AFTER LOBBY
	public void CheckReconnect()
    {
        if (UserConfig.Data.UserIsPlaying == "1")
        {
			StartCoroutine(UltilityGame.instance.TweenPanel(true, PanelReconnect));
		}
    }
	void OnCloseReconnect()
    {
		StartCoroutine(UltilityGame.instance.TweenPanel(false, PanelReconnect));
	}
	void OnClickReconnect()
    {
		OnCloseReconnect();
		PhotonNetwork.JoinRoom(UserConfig.Data.UserRoomPlaying);
    }
	void OnClickChoiNhanh()
    {
		if (PhotonNetwork.insideLobby) {
			bool canJoin = false;
			RoomInfo[] rooms = PhotonNetwork.GetRoomList();
            for (int i = 0; i < rooms.Length; i ++)
            {
				if((int)rooms[i].CustomProperties["bet"]*10 < UserConfig.Data.UserCoins)
                {
					canJoin = true;
					PhotonNetwork.JoinRoom(rooms[i].name);
					break;
                }
            }
			if (!canJoin)//AUTO SEARCH ROOM
			{
				bool joining = false;
				for (int i = listChannel.Length; i > 0;i--)
                {
					if(listChannel[i-1].int_bet * 10 < (int)UserConfig.Data.UserCoins)
                    {
						MatchMaking(listChannel[i-1].roomInfo.RoomID, listChannel[i-1].roomInfo.BetCoins, listChannel[i-1].roomInfo.TimeThink, listChannel[i-1].roomInfo.TimeMax);
						joining = true;
						break;
					}
                }
				if(!joining)
					StartCoroutine(NGUIMessage.instance.CALLALERT("Không đủ tiền cược vào bất cứ phòng nào!\n Vui lòng đổi thêm tiền "));
			}
		}
	}

	void FakeRoom()
	{
		listChannel[0].roomInfo.SetRoom("CU1", 30, 300, 2000);
		listChannel[1].roomInfo.SetRoom("CU2", 30, 300, 3000);
		listChannel[2].roomInfo.SetRoom("CU3", 30, 300, 5000);
		listChannel[3].roomInfo.SetRoom("CU4", 60, 600, 5000);
		listChannel[4].roomInfo.SetRoom("CU6", 30, 300, 10000);
		listChannel[5].roomInfo.SetRoom("CU7", 60, 600, 10000);
		listChannel[6].roomInfo.SetRoom("CU8", 30, 300, 20000);
		listChannel[7].roomInfo.SetRoom("CU9", 60, 600, 20000);
		listChannel[8].roomInfo.SetRoom("CU10", 90, 900, 20000);

		listChannel[9].roomInfo.SetRoom("CU11", 60, 600, 30000);
		listChannel[10].roomInfo.SetRoom("CU11", 90, 900, 30000);
		listChannel[11].roomInfo.SetRoom("CU12", 60, 600, 50000);
		listChannel[12].roomInfo.SetRoom("CU13", 90, 900, 50000);
		listChannel[13].roomInfo.SetRoom("CU14", 120, 1200, 50000);
		listChannel[14].roomInfo.SetRoom("CU15", 150, 1800, 100000);
	}

	public void SetDefaultRoom()
	{
		for (int i = 0; i < listChannel.Length; i++)
		{
			listChannel[i].mSetRoom();
		}
	}
	public void CallUpdateRoom()
    {
		CancelInvoke("UPDATEROOMLIST");
		InvokeRepeating("UPDATEROOMLIST", 1, 2);
	}
	void UPDATEROOMLIST()
    {
		SetDefaultRoom();
		RoomInfo[] rooms = PhotonNetwork.GetRoomList();
		if(rooms.Length > 0)
        {
			for (int i = 0; i < rooms.Length; i++)
				for(int j = 0; j < listChannel.Length; j++) {
					if (rooms[i].CustomProperties["roomid"].ToString() == listChannel[j].roomInfo.RoomID)
					{
						if((int)rooms[i].CustomProperties["addplayer"] == 1)
							listChannel[j].Pleft.SetActive(true);
						if ((int)rooms[i].CustomProperties["addplayer"] == 2)
						{
							listChannel[j].Pleft.SetActive(true);
							listChannel[j].Pright.SetActive(true);
						}
					}
					if(rooms[i].CustomProperties["roomid"].ToString() != listChannel[j].roomInfo.RoomID)
                    {
						string[] splt = rooms[i].CustomProperties["roomid"].ToString().Split('-');
						if(splt[0] == listChannel[j].roomInfo.RoomID)
                        {
							listChannel[j].slider.gameObject.SetActive(true);
							listChannel[j].PlayerInChannel+=2f;
							listChannel[j].SetValueChannel();
                        }
					}
				}
        }
    }
	public void MatchMaking(string _ID, int _Bet, int _ThinkT, int _MaxTime)
	{
		if (UserConfig.Data.UserCoins >= _Bet * 10)
		{
			CurrentRoom = new NewRoomInfo()
			{
				RoomID = _ID,
				BetCoins = _Bet,
				TimeThink = _ThinkT,
				TimeMax = _MaxTime
			};

			CURRENTCHANNEL = CurrentRoom;

			ExitGames.Client.Photon.Hashtable customPropreties = new ExitGames.Client.Photon.Hashtable();
			customPropreties["ismatchmaking"] = true;
			customPropreties["roomid"] = _ID;
			customPropreties["bet"] = _Bet;
			customPropreties["addplayer"] = 1;
			RoomOptions roomOptions = new RoomOptions() { CustomRoomProperties = customPropreties, IsVisible = true, IsOpen = true, MaxPlayers = 2, CleanupCacheOnLeave = false };

			roomOptions.CustomRoomPropertiesForLobby = new string[]
			{
				 "ismatchmaking",
				 "roomid",
				 "bet",
				 "addplayer"
			};
			SceneManager.LoadScene("InGameChess");
			PhotonNetwork.JoinOrCreateRoom(_ID, roomOptions, TypedLobby.Default);
		}
		else
		{
			StartCoroutine( NGUIMessage.instance.CALLALERT("Không đủ tiền vào bàn này "));
		}
	}
	void Update()
	{
		//StatusConnect.text = PhotonNetwork.connectedAndReady ? "Connected! (" + PhotonNetwork.CloudRegion + ")" : PhotonNetwork.connecting ? "Connecting..." : "Finding network...";
		//StatusConnect.color = PhotonNetwork.connectedAndReady ? Color.green : Color.yellow;
	}
	public GameObject ExitPanel;
	void onClickBack()
	{
		StartCoroutine(UltilityGame.instance.TweenPanel(true, ExitPanel));
	}

	void OnOKExit()
    {
		PhotonNetwork.Disconnect();
		SceneManager.LoadScene("MainMenu_new");
	}
	void OnCloseExit()
    {
		StartCoroutine(UltilityGame.instance.TweenPanel(false, ExitPanel));
	}
	//BUTTON HERE
	void OnExchange()
	{
		StartCoroutine(UltilityGame.instance.TweenPanel(true, ExchangePanel));
	}
	void OnCloseExchange()
	{
		StartCoroutine(UltilityGame.instance.TweenPanel(false, ExchangePanel));
	}

	void OnTaoBan()
	{
		StartCoroutine(UltilityGame.instance.TweenPanel(true, CreateRoomPanel));
	}
	void OCloseTaoBan()
    {
		StartCoroutine(UltilityGame.instance.TweenPanel(false, CreateRoomPanel));
	}
	public UIInput inPutRoom;
	public void OnSubitRoom()
    {
		if (PhotonNetwork.insideLobby)
			PhotonNetwork.JoinRoom(inPutRoom.value);
    }
	//UPDATE PROFILE
}

