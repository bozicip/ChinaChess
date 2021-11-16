using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using COTUONG;
public class PlayerManager : MonoBehaviour {

    [SerializeField]
	public GameObject UI;
	public UITexture WrongStep;
	public UILabel TiSo;
	public GameObject objChieu;
	public bool isPlayerDisconnected = false;
	public int TimeLeft,PlayerOder,AFKTime,ThinkTime;
	public string PlayerID, PlayerName;
	public int PlayerAvatarIndex;
	public float PlayerCoin;
	public UI2DSprite AvatarDF;
	public UILabel LbName,LbCoins;

	public UILabel LbTimeLeft;
	public UI2DSprite ReconnectSprite;
	public UI2DSprite DelayConnect;
	public GameObject TuongShow;


	private const string linkChessSet = "GamePlay/ChinaChess/ChessSet/NewChess/";
	public List<string> EatedChess;
	public List<bool> EatedChessBool;
	public List<bool> EatedChessRedTeam;
	public GameObject EatedChessPlace,EatedPref;
	public GameObject BtnNap;
    private bool isTeamRed; 

	public bool isReady = false;
	public bool isCheckRematch = false;
	public bool isEndTimeThink = false;

	public float TienAnQuan = 0;
	public UILabel lbSide;
	public GameObject EndTimeCount,AlertWrongMove;
	void Start(){
		TuongShow.GetComponent<TweenAlpha> ().enabled = false;
		isCheckRematch = false;
	}

	public void RESET(){
		AlertWrongMove.SetActive(false);
		isPlayerDisconnected = false;
		this.ReconnectSprite.gameObject.SetActive (false);
		this.TuongShow.GetComponent<TweenAlpha> ().value = 1;
		this.TuongShow.GetComponent<TweenAlpha> ().enabled = false;
		TienAnQuan = 0;
		DeleteAllCloneChess ();
	}

	void CallNap(){
		//NGUIMessage.instance.OpenNapthePanel ();
		//NGUIMessage.instance.onClickCancelBigMessage ();
	}

	public void StopAFK(){
		this.ReconnectSprite.gameObject.SetActive (false);
		CancelInvoke ();
	}

	public void Timedown(){//ALL PLAYER DO BUT ONLY MASTERCLIENT COUNT
		InvokeRepeating ("TimeCoundown",0.1f,1);
		ThinkTime = GameController.instance.RoomInfoIG.RoomTimePerTurn;
		InvokeRepeating ("TimeThinkCountdown",0.1f,1);
		TuongShow.GetComponent<TweenAlpha> ().enabled = true;
		this.ReconnectSprite.gameObject.SetActive (true);
	}
	void TimeCoundown(){
		if (PhotonNetwork.isMasterClient) {
			if (!this.isEndTimeThink)
			{
				TimeLeft--;
				GameController.instance.RoomInfoIG.GetComponent<PhotonView>().RPC("TD", PhotonTargets.All, this.PlayerOder, TimeLeft,RoomSelect.HeSoThoiGian);
			}
			else
			{
				TimeLeft -= RoomSelect.HeSoThoiGian;
				GameController.instance.RoomInfoIG.GetComponent<PhotonView>().RPC("TD", PhotonTargets.All, this.PlayerOder, TimeLeft, RoomSelect.HeSoThoiGian);
			}
			
		}
	}
	void TimeThinkCountdown(){
		if (PhotonNetwork.isMasterClient) {
			ThinkTime--;
			GameController.instance.RoomInfoIG.GetComponent<PhotonView>().RPC ("TT", PhotonTargets.All, this.PlayerOder, ThinkTime);
		}
	}

	public void StopTime(){//END TURN
		this.CancelInvoke();
		this.isEndTimeThink = false;
		this.EndTimeCount.SetActive(false);
		this.TuongShow.GetComponent<TweenAlpha> ().value = 1;
		this.TuongShow.GetComponent<TweenAlpha> ().enabled = false;
		this.ReconnectSprite.gameObject.SetActive (false);
		GameController.instance.objEffect.objSurrender.SetActive (false);
		GameController.instance.objEffect.objLeaveGame.SetActive (false);
	}
	public void DCShow(bool _isDC){//SHOULD BE CLIENT
		this.DelayConnect.gameObject.SetActive (_isDC);
	}

	// ================================ EAT CHESS =============================================
	public void CreateChessEated(int _Oder ,string _NameChess,bool _IsShow, bool _IsTeamRedShowEnd){
			string path3 = "";
			if (GameController.instance.myOrder == _Oder) {//NGUOI AN LUON LUON THAY
				if (GameController.instance.RoomInfoIG.RoomType != RoomSelect.CoLoan) {
					if (_Oder == 0)
						path3 = linkChessSet + "2" + _NameChess;
					else
						path3 = linkChessSet + "1" + _NameChess;
				} else {
					if(_IsTeamRedShowEnd)
						path3 = linkChessSet + "1" + _NameChess;
					else
						path3 = linkChessSet + "2" + _NameChess;
				}
				GameObject Chess31 = GameObject.Instantiate (EatedPref, EatedChessPlace.transform);
				Chess31.transform.localScale = new Vector3 (1, 1, 1);
				UI2DSprite Tex1 = Chess31.GetComponent<UI2DSprite> ();
				Tex1.sprite2D = Resources.Load (path3, typeof(Sprite)) as Sprite;
				//HIEU UNG QUAN UP
				if(!_IsShow){
					Chess31.transform.Find("Exten").GetComponent<UI2DSprite>().sprite2D = Resources.Load (linkChessSet + "saovang", typeof(Sprite)) as Sprite;
				}
				SortChess (_Oder);
			} else {//NGUOI BI AN QUAN VA KHAN GIA
				if (!_IsShow) {
					if (GameController.instance.RoomInfoIG.RoomType != RoomSelect.CoLoan) {
						if (_Oder == 0) {
							path3 = linkChessSet + "2Up";
						} else
							path3 = linkChessSet + "1Up";
					} else {
						path3 = linkChessSet + "napden";
					}
					GameObject Chess32 = GameObject.Instantiate (EatedPref, EatedChessPlace.transform);
					Chess32.transform.localScale = new Vector3 (1, 1, 1);
					UI2DSprite Tex32 = Chess32.GetComponent<UI2DSprite> ();
					Tex32.sprite2D = Resources.Load (path3, typeof(Sprite)) as Sprite;
					SortChess (_Oder);
				} else {
					if(_Oder == 0)
						path3 = linkChessSet + "2" + _NameChess;
					else
						path3 = linkChessSet + "1" + _NameChess;
					
					GameObject Chess33 = GameObject.Instantiate (EatedPref,EatedChessPlace.transform);
					Chess33.transform.localScale = new Vector3 (1,1,1);
					UI2DSprite Tex1 = Chess33.GetComponent<UI2DSprite> ();
					Tex1.sprite2D = Resources.Load(path3, typeof(Sprite)) as Sprite;
					SortChess (_Oder);
				}
		}
	}

	public void SortChess(int _Oder){
		List<Transform> cloneList = new List<Transform>();
		foreach (Transform A in EatedChessPlace.transform) {
			cloneList.Add (A);
		}
		//if (_Oder == 1) {
			for (int i = 0; i < cloneList.Count; i++) {
				cloneList [i].transform.localPosition = new Vector3 (i * 40, 0, 0);
				if (i >= 5) {
					cloneList [i].transform.localPosition = new Vector3 ((i - 5) * 40, -40, 0);
				}
				if (i >= 10)
					cloneList [i].transform.localPosition = new Vector3 ((i - 10) * 40, -80, 0);
				if (i >= 15)
					cloneList[i].transform.localPosition = new Vector3((i - 15) * 40, -120, 0);
		}
		TienAnQuan = GetTienAnQuan();
	}
	public void DeleteFinalChess(int _Oder){
		EatedChessBool.RemoveAt (EatedChessBool.Count -1);
		EatedChess.RemoveAt (EatedChess.Count -1);
		EatedChessRedTeam.RemoveAt (EatedChessRedTeam.Count -1);
		List<Transform> cloneList = new List<Transform>();
		foreach (Transform A in EatedChessPlace.transform) {
			cloneList.Add (A);
		}
		Destroy (cloneList [cloneList.Count - 1].gameObject);
	}
	public void DeleteAllCloneChess(){
		List<Transform> cloneList = new List<Transform>();
		foreach (Transform A in EatedChessPlace.transform) {
			cloneList.Add (A);
		}
		for(int i = 0 ; i < cloneList.Count ; i++){
			Destroy (cloneList[i].gameObject);
		}
	}
	// ----------------------------------------------------------------------------------------
	public GameObject EmotionPrefab,QuickChatPrefab;
	bool CanCreateEmotion = true,CanCreateQuickChat = true;
	public IEnumerator EmotionShow(int _sprite){
		if (CanCreateEmotion == true) {
			CanCreateEmotion = false;
			GameController.instance.PlayEmotion ();
			GameObject CloneEmotion = GameObject.Instantiate (EmotionPrefab, this.transform);
			CloneEmotion.transform.localScale = new Vector3 (1,1,1);
			CloneEmotion.transform.localPosition = new Vector3 (0,0,0);
			CloneEmotion.GetComponent<UI2DSprite>().sprite2D = Resources.Load("GamePlay/EmotionChat/" + _sprite.ToString(), typeof(Sprite)) as Sprite;
			CloneEmotion.GetComponent<UI2DSprite> ().depth = 10;
			if (this.PlayerOder == 0) {
				iTween.MoveTo (CloneEmotion,iTween.Hash("position",new Vector3 ( 200 ,100,0),"time",1f,"isLocal",true));
			}else
				iTween.MoveTo (CloneEmotion,iTween.Hash("position",new Vector3 ( -200 ,100,0),"time",1f,"isLocal",true));

			iTween.ScaleFrom (CloneEmotion,iTween.Hash("x",0,"y",0,"time",1));
			yield return new WaitForSeconds (4);
			Destroy (CloneEmotion);
			CanCreateEmotion = true;
		}
	}

	public IEnumerator QuickChatShow(string _Text){
		if (CanCreateQuickChat == true) {
			CanCreateQuickChat = false;
			//SOUND
			GameController.instance.PlayChat();
			GameObject CloneQuick = GameObject.Instantiate (QuickChatPrefab,this.transform);
			CloneQuick.transform.localScale = new Vector3 (1,1,1);
			if (this.PlayerOder == 0)
				CloneQuick.transform.localPosition = new Vector3 (120, 100, 0);
			else {
				CloneQuick.GetComponent<UI2DSprite> ().flip = UIBasicSprite.Flip.Horizontally;
				CloneQuick.GetComponent<UI2DSprite> ().pivot = UIWidget.Pivot.BottomRight;
				CloneQuick.transform.localPosition = new Vector3 (-120, 100, 0);
				//revrse
			}
			CloneQuick.transform.Find ("Chat").GetComponent<UILabel>().text = _Text;

			iTween.PunchScale (CloneQuick,iTween.Hash("x",1.4f,"y",1.4f,"time",0.5f));
			yield return new WaitForSeconds (4f);
			Destroy (CloneQuick);
			CanCreateQuickChat = true;
		}
	}

	public float GetTienAnQuan()
    {
		float  _TienAnQuan = 0;
        for (int i = 0; i < EatedChess.Count; i++)
        {
			if (EatedChess[i] == "Xe") { _TienAnQuan += GameController.instance.RoomInfoIG.RoomCoin * 0.75f; }
			if (EatedChess[i] == "Phao") { _TienAnQuan += GameController.instance.RoomInfoIG.RoomCoin * 0.3f; }
			if (EatedChess[i] == "Ma") { _TienAnQuan += GameController.instance.RoomInfoIG.RoomCoin * 0.3f; }
			if (EatedChess[i] == "Sy") { _TienAnQuan += GameController.instance.RoomInfoIG.RoomCoin * 0.2f; }
			if (EatedChess[i] == "Tinh") { _TienAnQuan += GameController.instance.RoomInfoIG.RoomCoin * 0.2f; }
			if (EatedChess[i] == "Chot") { _TienAnQuan += GameController.instance.RoomInfoIG.RoomCoin * 0.1f; }
		}
		return _TienAnQuan;
    }
	public void OnClickNap(){
		//NGUIMessage.instance.OpenNapthePanel ();
	}
}
