using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateRoom : MonoBehaviour {

	//NEW
	int betCoin, timeThink, timeMax;
	public UIInput inputMoney;
	bool canCreateRoom = false;
	public UILabel checkimg;
	float CurrentMinBet = 0;
	void Start () {
		timeThink = 30;
		timeMax = 300;
		betCoin = 2000;
		CurrentMinBet = 2000;
		canCreateRoom = false;
	}
	
	public void OnTimeThink1() { timeThink = 30; timeMax = 300; CurrentMinBet = 2000; inputMoney.value = "2000"; OnSubmitMoney(); }
	public void OnTimeThink2() { timeThink = 60; timeMax = 600; CurrentMinBet = 3000; inputMoney.value = "3000"; OnSubmitMoney(); }
	public void OnTimeThink3() { timeThink = 90; timeMax = 900; CurrentMinBet = 5000; inputMoney.value = "5000"; OnSubmitMoney(); }
	public void OnTimeThink4() { timeThink = 120; timeMax = 1200; CurrentMinBet = 10000; inputMoney.value = "10000"; OnSubmitMoney(); }
	public void OnTimeThink5() { timeThink = 180; timeMax = 1800; CurrentMinBet = 20000; inputMoney.value = "20000"; OnSubmitMoney(); }

	public void OnSubmitMoney()
    {
        if (int.Parse(inputMoney.value) >= CurrentMinBet)
        {
			if (int.Parse(inputMoney.value) * 10 > UserConfig.Data.UserCoins)
			{
				StartCoroutine (NGUIMessage.instance.CALLALERT("Không đủ tiền cược vui lòng nhập lại thấp hơn "));
				canCreateRoom = false;
				checkimg.text = "";
			}
			else
			{
				canCreateRoom = true;
				int clone = int.Parse(inputMoney.value);
				checkimg.text = clone.ToString("C0");
				betCoin = int.Parse(inputMoney.value);
			}
        }
        else
        {
			canCreateRoom = false;
			StartCoroutine( NGUIMessage.instance.CALLALERT("Tiền cược của phòng hiện tại phải lớn hơn " + CurrentMinBet.ToString()));
		}
		
    }

	void OnCreateRoom()
    {
		if (canCreateRoom)
		{
			char[] cloneID = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM".ToCharArray();
			string cloneRoom = "";
			for (int i = 0; i < 6; i++)
			{
				cloneRoom = cloneRoom + cloneID[UnityEngine.Random.Range(0, cloneID.Length)];
			}

			RoomSelect.instance.CurrentRoom = new NewRoomInfo()
			{
				RoomID = cloneRoom,
				BetCoins = betCoin,
				TimeThink = timeThink,
				TimeMax = timeMax
			};

			ExitGames.Client.Photon.Hashtable customPropreties = new ExitGames.Client.Photon.Hashtable();
			customPropreties["ismatchmaking"] = false;
			customPropreties["roomid"] = cloneRoom;
			customPropreties["bet"] = betCoin;
			customPropreties["timemax"] = timeMax;
			customPropreties["timethink"] = timeThink;

			customPropreties["addplayer"] = 1;
			RoomOptions roomOptions = new RoomOptions() { CustomRoomProperties = customPropreties, IsVisible = false, IsOpen = true, MaxPlayers = 2, CleanupCacheOnLeave = false };

			roomOptions.CustomRoomPropertiesForLobby = new string[]
			{
				 "ismatchmaking",
				 "roomid",
				 "bet",
				 "timemax",
				 "timethink",
				 "addplayer"
			};
			PhotonNetwork.JoinOrCreateRoom(cloneRoom, roomOptions, TypedLobby.Default);
			SceneManager.LoadScene("InGameChess");
		}
		else
		{
			StartCoroutine( NGUIMessage.instance.CALLALERT("Không tạo được phòng vui lòng thử lại "));
		}
	}
	
	void CloseCreateRoom(){
		StartCoroutine(UltilityGame.instance.TweenPanel(false,this.gameObject));
	}
}
