using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.SceneManagement;

public class NetworkManager :Photon.MonoBehaviour {//NETWORK BAN CA 
	/// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
	public bool AutoConnect = true;
	public static int myPlayer; 
	public static bool isReverse;

	public byte Version = 1;
	public GameObject Player1, Player2, Player3, Player4;

	/// <summary>if we don't want to connect in Start(), we have to "remember" if we called ConnectUsingSettings()</summary>
	private bool ConnectInUpdate = true;
	public void Awake()
	{
		isReverse = false;
		ConnectInUpdate = true;
		//PhotonNetwork.autoCleanUpPlayerObjects = false;
	}
	public virtual void Start()
	{
		PhotonNetwork.playerName = UserConfig.Data.UserID;
		Invoke("checkGoBack",8);
	}

	void checkGoBack() {
		if (PhotonNetwork.connectionStateDetailed.ToString () == "JoinedLobby") {
			PhotonNetwork.Disconnect ();
			SceneManager.LoadScene("MainMenu_new");
		}
	}
	public virtual void OnDisconnectedFromPhoton(){
		//NGUIMessage.instance.BackToLogin ();
	}
	public virtual void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer){
		Debug.Log (otherPlayer);
		//if(PhotonNetwork.isMasterClient)
			//GameController.instance.XuLiDC (otherPlayer.NickName);
	}
	public virtual void Update()
	{
		if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
		{
			Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
			ConnectInUpdate = false;
			PhotonNetwork.ConnectUsingSettings(NGUIMessage.instance.PS_BanCa);
		}
	}
	
	// to react to events "connected" and (expected) error "failed to join random room", we implement some methods. PhotonNetworkingMessage lists all available methods!
	
	public virtual void OnConnectedToMaster()
	{
		if (PhotonNetwork.networkingPeer.AvailableRegions != null) Debug.LogWarning("List of available regions counts " + PhotonNetwork.networkingPeer.AvailableRegions.Count + ". First: " + PhotonNetwork.networkingPeer.AvailableRegions[0] + " \t Current Region: " + PhotonNetwork.networkingPeer.CloudRegion);
		Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
		//PhotonNetwork.JoinRandomRoom();
	}
	
	public virtual void OnPhotonRandomJoinFailed()
	{
		Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
		//PhotonNetwork.CreateRoom("COOPMART1", new RoomOptions() { maxPlayers = 4 }, null);
		PhotonNetwork.CreateRoom("santhu"+ System.Guid.NewGuid().ToString(), new RoomOptions() { maxPlayers = 4 }, null);
	}

	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("Cause: " + cause);
		PhotonNetwork.Disconnect ();
		SceneManager.LoadScene("MainMenu_new");
	}
	public virtual void OnMasterClientSwitched(PhotonPlayer newMster){
		fireBullet.instance.CheckWhenBecomeNewMaster ();
	}
	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
		StartCoroutine(SpawnMyPlayer ());
	}

	IEnumerator SpawnMyPlayer(){
		yield return new WaitForSeconds (1.0f);
		bool joined = false;
		while (joined == false) {
			int a = UnityEngine.Random.Range (1, 5);
			//int a = 3;
			if(a==1){
				if(!GameObject.Find("Player1Parent(Clone)")){ // if not exits player 1
					GameObject myP = PhotonNetwork.Instantiate(Player1.name,Player1.transform.position, 
					                          Player1.transform.rotation,0);
					//GameObject.Find("Player1").GetComponent<fireBullet> ().PlayerID = UserConfig.Data.UserID;
					GameObject.Find("Player1").GetComponent<fireBullet>().enabled = true;
					GameObject.Find("Player1/tangSung").GetComponent<UIButtonMessage>().enabled = true;
					GameObject.Find("Player1/giamSung").GetComponent<UIButtonMessage>().enabled = true;
					joined = true;
					myPlayer = 1;
				}
			}
			if(a==2){
				if(!GameObject.Find("Player2Parent(Clone)")){ // if not exits player 2
					GameObject myP = PhotonNetwork.Instantiate(Player2.name,Player2.transform.position, 
					                          Player2.transform.rotation,0);
					//GameObject.Find("Player2").GetComponent<fireBullet> ().PlayerID = UserConfig.Data.UserID;
					GameObject.Find("Player2").GetComponent<fireBullet>().enabled = true;
					GameObject.Find("Player2/tangSung").GetComponent<UIButtonMessage>().enabled = true;
					GameObject.Find("Player2/giamSung").GetComponent<UIButtonMessage>().enabled = true;
					joined = true;
					myPlayer = 2;
				}
			}
			if(a==3){
				if(!GameObject.Find("Player3Parent(Clone)")){ // if not exits player 3
					GameObject myP = PhotonNetwork.Instantiate(Player3.name,Player3.transform.position, 
					                          Player3.transform.rotation,0);
					//GameObject.Find("Player3").GetComponent<fireBullet> ().PlayerID = UserConfig.Data.UserID;
					GameObject.Find("Player3").GetComponent<fireBullet>().enabled = true;
					GameObject.Find("Player3/tangSung").GetComponent<UIButtonMessage>().enabled = true;
					GameObject.Find("Player3/giamSung").GetComponent<UIButtonMessage>().enabled = true;
					joined = true;
					myPlayer = 3;
					//isReverse = true;
					//checkIsReverse ();
				}
			}
			if(a==4){
				if(!GameObject.Find("Player4Parent(Clone)")){ // if not exits player 4
					GameObject myP = PhotonNetwork.Instantiate(Player4.name,Player4.transform.position, 
					                          Player4.transform.rotation,0);
					//GameObject.Find("Player4").GetComponent<fireBullet> ().PlayerID = UserConfig.Data.UserID;
					GameObject.Find("Player4").GetComponent<fireBullet>().enabled = true;
					GameObject.Find("Player4/tangSung").GetComponent<UIButtonMessage>().enabled = true;
					GameObject.Find("Player4/giamSung").GetComponent<UIButtonMessage>().enabled = true;
					joined = true;
					myPlayer = 4;
					//isReverse = true;
					//checkIsReverse ();
				}
			}
		}

	}
	void checkIsReverse (){
		if (NetworkManager.isReverse) {
			Camera.main.transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
			//GameObject temp = GameObject.Find("Anchor -BotRight");
			//temp.transform.position = new Vector3(-3.2f,1.3f,0);
			GameObject.Find("Popup - Pause").transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);

			GameObject tempJoin = GameObject.Find("Popup - JoinRoom");
			tempJoin.transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
			tempJoin.transform.position = new Vector3(5.5f,0,0);

			GameObject tempChat = GameObject.Find("Panel - Chat");
			tempChat.transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
			tempChat.transform.localPosition = new Vector3(1300,0,0);

			GameObject.Find("textTimer").transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
			GameObject.Find("Timer").transform.position= new Vector3(0,-2.0f,0);
//			GameObject.Find("TimerBg").transform.position = new Vector3 (1,0,0);
		}
	}
	public void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby(). Use a GUI to show existing rooms available in PhotonNetwork.GetRoomList().");
		//SceneManager.LoadScene("MainMenu_new");
		PhotonNetwork.JoinRandomRoom();
	}
}
