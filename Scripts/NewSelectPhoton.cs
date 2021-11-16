using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using UnityEngine.SceneManagement;

public class NewSelectPhoton : Photon.MonoBehaviour
{
	public bool AutoConnect = true;

	public static bool ConnectInUpdate = true;

	public virtual void OnJoinedLobby()
	{
		RoomSelect.instance.SetDefaultRoom();
		RoomSelect.instance.CallUpdateRoom();
		NGUIMessage.instance.CALLLOADING(false);
		RoomSelect.instance.CheckReconnect();
	}

	public virtual void Update()
	{
		if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
		{
			PhotonNetwork.ConnectUsingSettings(NGUIMessage.instance.PS_CUOP);
			Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings()" + "Connected : " + PhotonNetwork.connected);
			ConnectInUpdate = false;
		}
	}
	public virtual void OnPhotonJoinRoomFailed()
	{
		NGUIMessage.instance.CALLLOADING(false);
		StartCoroutine( NGUIMessage.instance.CALLALERT("Phòng không tòn tại hoặc đã đầy !"));
	}

	public virtual void OnConnectedToMaster()
	{
		if (PhotonNetwork.networkingPeer.AvailableRegions != null) Debug.LogWarning("List of available regions counts " + PhotonNetwork.networkingPeer.AvailableRegions.Count + ". First: " + PhotonNetwork.networkingPeer.AvailableRegions[0] + " \t Current Region: " + PhotonNetwork.networkingPeer.CloudRegion);
		Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinLobby();
	}

	public virtual void OnPhotonRandomJoinFailed()
	{
		Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
	}

	// the following methods are implemented to give you some context. re-implement them as needed.

	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		//Debug.Log("BACK TO MENU");
	}
	public virtual void OnMasterClientSwitched(PhotonPlayer newMster)
	{

	}
	public void OnJoinedRoom()
	{
		
	}
}

