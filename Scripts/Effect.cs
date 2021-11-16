using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using COTUONG;
public class Effect : MonoBehaviour {

	public GameController gameCoTuongManager;
	public Shader OverNGUI;
	public GameObject objLeaveGame,objSurrender,objDrawRequest,objDrawRecive,objDrawRecive2,objPlayerInfo;

	public GameResultCoin objResult;

	public GameObject PlayGround;

	Vector3 OriginalChatPanel,OriginalEmoPanel;
	
    //public GameObject objTemp;
	//public GameObject Join1, Join2;
	public UILabel[] QuickChatText;
	public GameObject ShowChessPlace;

	
	void Start(){
		StartCoroutine (GetPos());
		QuichChatSet ();
	}

	
	void QuichChatSet(){
		QuickChatText [0].text = "Nhanh đi ba";
		QuickChatText [1].text = "Ăn hành đi ";
		QuickChatText [2].text = "Chơi mạnh lên nào";
		QuickChatText [3].text = "Chơi tiếp không em ?";
		QuickChatText [4].text = "Anh quá mạnh !!";
		QuickChatText [5].text = "Ha ha ! Chấp quân luôn nè";
		QuickChatText [6].text = "Đánh vậy thua sao ?";
		QuickChatText [7].text = "Vui lên nào ";
		QuickChatText [8].text = "Sấp mặt chưa em ?";
		QuickChatText [9].text = "Đùa tí thôi , thắng mấy hồi !!";
	}

	IEnumerator GetPos(){
		yield return new WaitForSeconds (1);
		OriginalChatPanel = ChatPanel.transform.localPosition;
		OriginalEmoPanel = EmotionPanel.transform.localPosition;
	}
	int CurrentMusic = 0;

	public IEnumerator PlayRandomMusic(){
		int randomS = 0;
		while(CurrentMusic == randomS){
			randomS = Random.Range (0,LoadPrefab.instance.GameMusic.Length);
		}
		CurrentMusic = randomS;
		for (int i = 0; i < LoadPrefab.instance.GameMusic.Length; i++)
			LoadPrefab.instance.GameMusic [i].SetActive (false);
		LoadPrefab.instance.GameMusic [randomS].SetActive (true);
		yield return new WaitForSeconds (LoadPrefab.instance.GameMusic[randomS].GetComponent<AudioSource>().clip.length);
		StartCoroutine (PlayRandomMusic());
	}

	void CallNap(){
		//NGUIMessage.instance.OpenNapthePanel ();
		//NGUIMessage.instance.onClickCancelBigMessage ();
	}
	public void OnRestart(){
		Board.instance.NewGame(RoomSelect.CoLoan);
		//NGUIMessage.instance.CALLBIGMESSAGE ("On Ready","Bạn không đủ tiền cược , bấm Xác nhận để vào cửa hàng !!!",this.gameObject,"CallOpenNap");
	}
	/*
	public IEnumerator EffectStartGame(){
		GameObject Sp3 = GameObject.Instantiate (LoadPrefab.instance.Sprite3,this.transform);
		Sp3.transform.localScale = new Vector3 (1,1,1);
		Destroy (Sp3,1);
		yield return new WaitForSeconds (1f);
		GameObject Sp2 = GameObject.Instantiate (LoadPrefab.instance.Sprite2,this.transform);
		Sp2.transform.localScale = new Vector3 (1,1,1);
		Destroy (Sp2,1);
		yield return new WaitForSeconds (1f);
		GameObject Sp1 = GameObject.Instantiate (LoadPrefab.instance.Sprite1,this.transform);
		Sp1.transform.localScale = new Vector3 (1,1,1);
		Destroy (Sp1,1);
		yield return new WaitForSeconds (1f);
		GameObject ST = GameObject.Instantiate (LoadPrefab.instance.SpriteStart,this.transform);
		ST.transform.localScale = new Vector3 (1,1,1);
		ST.transform.Find ("Pl1/Pl1Name").GetComponent<UILabel>().text = GameController.instance.Players[0].LbName.text;
		ST.transform.Find ("Pl2/Pl2Name").GetComponent<UILabel>().text = GameController.instance.Players[1].LbName.text;
		Destroy (ST,2);
	}
	*/
	void OnExit(){
        if (!GameController.instance.GameStarted)
        {
			GameController.instance.LeaveRoom();
        }
        else {
			StartCoroutine(UltilityGame.instance.TweenPanel(true, objLeaveGame));
			objLeaveGame.transform.Find("Main/NOTICE").GetComponent<UILabel>().text = "Bạn có muốn thoát giữa trận và chấp nhận thua cuộc !!?";
		}
	}
	void OnCloseExit(){
		StartCoroutine(UltilityGame.instance.TweenPanel(false, objLeaveGame));
	}
	public void OnLeave(){
		OnCloseExit();
		StartCoroutine (BeforeLeave());
	}
	IEnumerator BeforeLeave(){
		if (GameController.instance.GameStarted) {
			if (GameController.instance.myOrder == 0)
				GameController.instance.ClaimWin (1,true, "Thoát Game ", 0,"",0,"");
			if (GameController.instance.myOrder == 1)
				GameController.instance.ClaimWin (0, true, "Thoát Game", 0, "", 0, "");
		}
		yield return new WaitForSeconds (0.2f);
		GameController.instance.LeaveRoom();
	}

	void OnSurrender(){
		StartCoroutine(UltilityGame.instance.TweenPanel(true, objSurrender));
	}
	void OnCloseSurrender(){
		StartCoroutine(UltilityGame.instance.TweenPanel(false, objSurrender));
	}
	void OnOKSurrender(){
		OnCloseSurrender ();
		NGUIMessage.instance.CALLLOADING (true);
		if (GameController.instance.MyPlayer.TimeLeft >= 3) {
			if (GameController.instance.myOrder == 0)
				GameController.instance.ClaimWin (1, true, "Đầu Hàng ", 0, "", 0, "");
			if (GameController.instance.myOrder == 1)
				GameController.instance.ClaimWin (0, true, "Đầu Hàng ", 0, "", 0, "");
		}
	}
	void OnDraw(){
		GameController.instance.NumDraw++;
		if (GameController.instance.NumDraw > 3) {
			NGUIMessage.instance.CALLSMALLMESSAGE ("Cầu Hòa","Đã hết lượt xin hoà của ván đấu !!");
		} else {
			StartCoroutine(UltilityGame.instance.TweenPanel(true, objDrawRequest));
			objDrawRequest.transform.Find ("Main/NOTICE").GetComponent<UILabel> ().text = "Bạn có muốn cầu hoà đối thủ không ?";
		}
	}
	void OnCloseDrawRequest(){
		StartCoroutine(UltilityGame.instance.TweenPanel(false, objDrawRequest));
	}
	void OnDrawRequest(){
		GameController.instance.ShowDraw (GameController.instance.myOrder);	
		OnCloseDrawRequest ();
	}
	void OnOkDraw1(){
		OnCloseRecieved();
		StartCoroutine(UltilityGame.instance.TweenPanel(true, objDrawRecive2));
		objDrawRecive2.transform.Find ("Main/NOTICE").GetComponent<UILabel> ().text = "Bạn có chắc muốn hoà ván đấu ???";
	}
	void OnOkDraw2(){
		StartCoroutine(UltilityGame.instance.TweenPanel(false, objDrawRecive2));
		NGUIMessage.instance.CALLLOADING (true);
		StartCoroutine (GameController.instance.ClaimDraw ("Cả 2 bên đồng ý hòa"));
	}
	void OnCloseDraw2(){
		StartCoroutine(UltilityGame.instance.TweenPanel(false, objDrawRecive2));
	}
	void OnCloseRecieved(){
		StartCoroutine(UltilityGame.instance.TweenPanel(false, objDrawRecive));
	}



	public GameObject BtnEmotion,ChatPanel,ChatPanelBlack,EmotionPanel,EmotionPanelBlack;
	bool ChatPanelShow = false,CanClick = true,EmoPanelShow = false,CanClickEmo = true;
	public IEnumerator onClickChat(){
		Debug.Log("VUI");
		if (CanClick == true) {
			if (ChatPanelShow == false ) {
				ChatPanelShow = true;
				ChatPanelBlack.SetActive (true);
				CanClick = false;
				iTween.MoveTo (ChatPanel, iTween.Hash ("x", ChatPanel.transform.localPosition.x - 620, "time", 0.5f, "isLocal", true));
				yield return new WaitForSeconds (0.6f);
				CanClick = true;
			} else {
				ChatPanelShow = false;
				ChatPanel.transform.localPosition = OriginalChatPanel;
				ChatPanelBlack.SetActive (false);
			}
		}
	}
	public IEnumerator onClickEmo(){
		if (CanClickEmo == true) {
			if (EmoPanelShow == false ) {
				EmoPanelShow = true;
				EmotionPanelBlack.SetActive (true);
				CanClickEmo = false;
				iTween.MoveTo (EmotionPanel, iTween.Hash ("x", EmotionPanel.transform.localPosition.x - 620, "time", 0.5f, "isLocal", true));
				yield return new WaitForSeconds (0.6f);
				CanClickEmo = true;
			} else {
				EmoPanelShow = false;
				EmotionPanel.transform.localPosition = OriginalEmoPanel;
				EmotionPanelBlack.SetActive (false);
			}
		}
	}

	void OnCloseResult()
    {
		StartCoroutine(UltilityGame.instance.TweenPanel(false,objResult.gameObject));
    }
	void OnCloseDraw(){
		//StartCoroutine(UltilityGame.instance.TweenPanel(false, draw));
	}

    //Effect select quan
    public GameObject objEffectSelectChess,objWarning;
    public void onEffectSelectChess(Transform tfParent)
    {
        objEffectSelectChess.SetActive(true);
        objEffectSelectChess.transform.position = tfParent.position + new Vector3(0,0,-1);  //new Vector2(0, 0);
    }
    public void offEffectSelectChess()
    {
        objEffectSelectChess.SetActive(false);
    }

	#region ADMINZONE
	private const string Rsource = "GamePlay/ChinaChess/ChessSet/NewChess/";

	void OnCheatBtn(){
		OnCloseCheatBtn ();
		if(GameController.instance.GameStarted){
			Debug.Log ("SHOW CO ");
			string path = "";
			string _teamchess = "";
			string _typechess = "";
			//SHOW RED
			for(int i = 0 ; i < Board.instance.listChessRed.Count ; i++){
				if (!Board.instance.mNode [Board.instance.listChessRed [i].y, Board.instance.listChessRed [i].x].chess.show) {//NEU CO VI TRI CHUA HIEN QUAN
					if (Board.instance.mNode [Board.instance.listChessRed [i].y, Board.instance.listChessRed [i].x].chess.teamRedShow) {
						_teamchess = "1";
					} else
						_teamchess = "2";
					
					_typechess = Board.instance.mNode [Board.instance.listChessRed [i].y, Board.instance.listChessRed [i].x].chess.typeChessShow.ToString ();

					path = Rsource + _teamchess + _typechess;

					GameObject Chess = GameObject.Instantiate (LoadPrefab.instance.ShowChessPref,ShowChessPlace.transform);
					Chess.transform.localScale = new Vector3 (1,1,1);
					UI2DSprite Tex = Chess.GetComponent<UI2DSprite> ();
					Tex.height = 65;
					Tex.width = 65;
					Tex.depth = 10;
					Tex.sprite2D = Resources.Load(path, typeof(Sprite)) as Sprite;
					Chess.transform.position = Board.instance.mNode [Board.instance.listChessRed [i].y, Board.instance.listChessRed [i].x].transform.position;
				}
			}
			//SHOW BLUE
			for(int i = 0 ; i < Board.instance.listChessBlue.Count ; i++){
				if (!Board.instance.mNode [Board.instance.listChessBlue [i].y, Board.instance.listChessBlue [i].x].chess.show) {//NEU CO VI TRI CHUA HIEN QUAN
					if (Board.instance.mNode [Board.instance.listChessBlue [i].y, Board.instance.listChessBlue [i].x].chess.teamRedShow) {
						_teamchess = "1";
					} else
						_teamchess = "2";

					_typechess = Board.instance.mNode [Board.instance.listChessBlue [i].y, Board.instance.listChessBlue [i].x].chess.typeChessShow.ToString ();

					path = Rsource + _teamchess + _typechess;

					GameObject Chess = GameObject.Instantiate (LoadPrefab.instance.ShowChessPref,ShowChessPlace.transform);
					Chess.transform.localScale = new Vector3 (1,1,1);
					UI2DSprite Tex = Chess.GetComponent<UI2DSprite> ();
					Tex.height = 65;
					Tex.width = 65;
					Tex.depth = 10;
					Tex.sprite2D = Resources.Load(path, typeof(Sprite)) as Sprite;
					Chess.transform.position = Board.instance.mNode [Board.instance.listChessBlue [i].y, Board.instance.listChessBlue [i].x].transform.position;
				}
			}
		}
	}
	void OnCloseCheatBtn(){
		List<Transform> cloneList = new List<Transform>();
		foreach (Transform A in ShowChessPlace.transform) {
			cloneList.Add (A);
		}
		for(int i = 0 ; i < cloneList.Count ; i++){
			Destroy (cloneList[i].gameObject);
		}
	}
	#endregion

	
}
