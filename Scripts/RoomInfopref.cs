using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomInfopref : MonoBehaviour
{
	public NewRoomInfo roomInfo = new NewRoomInfo();
	public UILabel lbTiencuoc, lbTime;

	public GameObject Pleft, Pright;
	public UISlider slider;
	public float PlayerInChannel = 0;
	float maxPlayerInChannel = 100;
	public int int_bet = 0;
	void Start()
	{
		OnLoadDefault();
	}
	void OnLoadDefault()
	{
		int_bet = roomInfo.BetCoins;
		lbTiencuoc.text = "";
		lbTime.text = "";
		PlayerInChannel = 0;
		slider.gameObject.SetActive(false);
		Pleft.SetActive(false);
		Pright.SetActive(false);
	}
	public void mSetRoom()
	{
		OnLoadDefault();
		lbTiencuoc.text = "Cược :" + (roomInfo.BetCoins / 1000).ToString() + "K";
		lbTime.text = roomInfo.TimeThink + "s\n" + (roomInfo.TimeMax / 60).ToString() + ":00'";
	}
	public void SetValueChannel()
    {
		slider.value = PlayerInChannel / maxPlayerInChannel;
    }

	void OnClickJoinRoom()
	{
		RoomSelect.instance.MatchMaking(this.roomInfo.RoomID, this.roomInfo.BetCoins, this.roomInfo.TimeThink, this.roomInfo.TimeMax);
	}
}

