using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPrefab : MonoBehaviour {
	public static LoadPrefab instance;
	void Awake(){
		if (instance == null)
			instance = this;
		else
			Destroy(this);
		AutoCloseMusicCoTuong();
	}
	//MENU
	//INGAME
	public GameObject RoomNamepref;
	public GameObject ShowChessPref;
	//public GameObject PlayerItempf;
	public GameObject[] GameMusic;
	public GameObject[] EatEffect;

	public Sprite[] SpriteAvatars;
	public AudioClip SoundTime,SoundEndTime,SoundJoin,SoundOut,SoundSit,SoundMove,SoundEat,SoundWin,SoundLose,SoundStart,SoundChieu,SoundChieuHet,SoundChat, ChonQuan,SaiQuan;
	public AudioClip SoundCloseMainTime,SoundStartTurn;
	public void AutoCloseMusicCoTuong()
    {
		for(int i  = 0; i < GameMusic.Length; i++)
        {
			GameMusic[i].SetActive(false);
        }
    }
}
