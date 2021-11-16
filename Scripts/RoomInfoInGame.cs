using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using COTUONG;
public class RoomInfoInGame : Photon.MonoBehaviour
{
	public GameObject ReviewButton;
	public UILabel LbRoomTime, LbRoomCoin, LbRoomName, LBLuat;
	public int RoomTime, RoomTimePerTurn, RoomCoin;
	public string RoomID;
	public bool IsTournament, IsPublicRoom;
	public string RoomType = "Cờ Úp";
	public string MatchID;
	public List<Node> ListChess;
	public GameObject BtnPlay, BtnPause;
	public bool MovingChess = false;
	public UILabel Stepcount;

	public string IDPlayerRed = "", IDPlayerBlue = "";
	public float CurrentScoreRed, CurrentScoreBlue;
	public int TeamStartTurn;
	public bool ISTEAMREDPLAY;

	public List<string> ListID = new List<string>();
	//CHAP PLACE

	public bool isNewPlayerCome = true;
	public string PlayerLoseLastMatch = "";
	void Awake()
	{
		ListID = new List<string>();
	}
	void Start()
	{
		if(PhotonNetwork.isMasterClient)
			InvokeRepeating("waitPlayer",2,1);
		Stepcount = GameObject.FindGameObjectWithTag("MoveLabel").GetComponent<UILabel>();
		this.transform.parent = GameController.instance.objEffect.transform;
		this.transform.localScale = new Vector3(1, 1, 1);
	}
	void waitPlayer()
    {
		if (PhotonNetwork.isMasterClient && PhotonNetwork.playerList.Length == 2)
		{
			CancelInvoke("waitPlayer");
			int side = UnityEngine.Random.Range(0, 2);
			photonView.RPC("ChooseSide", PhotonTargets.All, UserConfig.Data.UserID, side);
			photonView.RPC("CreateBoard", PhotonTargets.All);
			photonView.RPC("CallStart", PhotonTargets.All);
		}
	}
	public void UpdateStepMove(int _Step)
	{
		Stepcount.text = (_Step / 2).ToString();
		if (_Step >= 100)
			Stepcount.color = Color.yellow;
		if (_Step >= 200)
			Stepcount.color = Color.green;
	}
	public void CheckTiso(string _IdRedOne, string _IdBlueOne, float _RedScore, float _BlueScore)
	{
		string cloneLastMatch = IDPlayerRed + "." + IDPlayerBlue;
		string cloneCurrentMatch = _IdRedOne + "." + _IdBlueOne;

		if (cloneLastMatch != cloneCurrentMatch)
		{
			CurrentScoreRed = 0;
			CurrentScoreBlue = 0;
			TeamStartTurn = 0;
			IDPlayerRed = _IdRedOne;
			IDPlayerBlue = _IdBlueOne;
		}
		else
		{
			CurrentScoreRed += _RedScore;
			CurrentScoreBlue += _BlueScore;
		}
		ShowTiso();
	}
	public void ShowTiso()
	{
		GameController.instance.Players[0].TiSo.text = CurrentScoreRed.ToString();
		GameController.instance.Players[1].TiSo.text = CurrentScoreBlue.ToString();

		if (CurrentScoreRed > CurrentScoreBlue)
		{
			GameController.instance.Players[0].TiSo.color = Color.green;
			GameController.instance.Players[1].TiSo.color = Color.white;
		}
		if (CurrentScoreRed == CurrentScoreBlue)
		{
			GameController.instance.Players[0].TiSo.color = Color.white;
			GameController.instance.Players[1].TiSo.color = Color.white;
		}
		if (CurrentScoreRed < CurrentScoreBlue)
		{
			GameController.instance.Players[0].TiSo.color = Color.white;
			GameController.instance.Players[1].TiSo.color = Color.green;
		}
	}
	public void ClearList()
	{
		ListChess.Clear();
	}
	public void AddChess(Chess _chess, NodeBoard _nodeboard)
	{

		ListChess.Add(new Node(_chess, _nodeboard));
	}
	public void RemoveChess(Node _node)
	{
		for (int i = 0; i < ListChess.Count; i++)
		{
			if (ListChess[i].posNode.x == _node.posNode.x && ListChess[i].posNode.y == _node.posNode.y)
			{
				ListChess.Remove(ListChess[i]);
			}
		}
	}

	public void AutoShow()
	{
		LbRoomName.text = "Bàn " + RoomID;
		float cloneCoin = (float)RoomCoin;
		LbRoomCoin.text = cloneCoin.ToString("C0") ;
		LbRoomTime.text = GameController.instance.CountTiming(RoomTime) + " - " + GameController.instance.CountTiming(RoomTimePerTurn);
	}

	#region SAVE NUOC DI
	public float timeAutoPlay = 0.25f;
	public float timeGoFast = 0.05f;

	public List<int> listPosXStart, listPosYStart;
	public List<int> listPosXEnd, listPosYEnd;
	public List<bool> listIsChessStart, listIsChessEnd;
	public List<bool> listIsShowStart, listIsShowEnd;
	public List<bool> listTeamStart, listTeamEnd;
	public List<bool> listTeamShowStart, listTeamShowEnd;
	public List<TypeChess> listTypeStart, listTypeEnd;
	public List<TypeChess> listTypeStartShow, listTypeEndShow;


	public GameObject objMenuViewer, objMenuWatchRecord;
	public UITexture imgBtnPlay;
	public Texture2D sprPause, sprPlay;

	public int indexMove;
	private bool isAutoPlay = true;
	public bool IsAutoPlay
	{
		get
		{
			return isAutoPlay;
		}
		set
		{
			isAutoPlay = value;
			if (isAutoPlay)
				imgBtnPlay.mainTexture = sprPlay;
			else
				imgBtnPlay.mainTexture = sprPause;
		}
	}

	private bool isViewer;
	public bool Isviewer
	{
		get
		{
			return isViewer;
		}
		set
		{
			isViewer = value;
			if (value == true)
			{
				objMenuViewer.SetActive(true);
				objMenuWatchRecord.SetActive(false);
			}
			else
			{
				objMenuViewer.SetActive(false);
			}
		}
	}

	public void NewViewGame()
	{
		Debug.Log("VO DAY ");
		ClearAll();
		indexMove = 0;

		listPosXStart = new List<int>();
		listPosYStart = new List<int>();
		listPosXEnd = new List<int>();
		listPosYEnd = new List<int>();

		listIsChessStart = new List<bool>();
		listIsChessEnd = new List<bool>();
		listIsShowStart = new List<bool>();
		listIsShowEnd = new List<bool>();
		listTeamStart = new List<bool>();
		listTeamEnd = new List<bool>();
		listTeamShowStart = new List<bool>();
		listTeamShowEnd = new List<bool>();

		listTypeStart = new List<TypeChess>();
		listTypeEnd = new List<TypeChess>();
		listTypeStartShow = new List<TypeChess>();
		listTypeEndShow = new List<TypeChess>();
	}
	public void ClearAll()
	{
		listPosXStart.Clear();
		listPosYStart.Clear();
		listPosXEnd.Clear();
		listPosYEnd.Clear();

		listIsChessStart.Clear();
		listIsChessEnd.Clear();
		listIsShowStart.Clear();
		listIsShowEnd.Clear();
		listTeamStart.Clear();
		listTeamEnd.Clear();
		listTeamShowStart.Clear();
		listTeamShowEnd.Clear();

		listTypeStart.Clear();
		listTypeEnd.Clear();
		listTypeStartShow.Clear();
		listTypeEndShow.Clear();
	}

	private Coroutine coroutineGoEnd, coroutineGoStart;
	public void StopAllCoroutineViewer()
	{
		if (coroutineGoStart != null)
		{
			StopCoroutine(coroutineGoStart);
		}
		if (coroutineGoEnd != null)
		{
			StopCoroutine(coroutineGoEnd);
		}
	}

	public void AddStep(int nodeStartX, int nodeStartY, int _nodeEndX, int _nodeEndY,
		bool isChessStart, bool _isStartUp, bool _isTeamRedStart, bool _isTeamRedShowStart, TypeChess _typeStart, TypeChess _typeStartShow,
		bool isChessEnd, bool _isEndtUp, bool _isTeamRedEnd, bool _isTeamRedShowEnd, TypeChess _typeEnd, TypeChess _typeEndShow)
	{
		listPosXStart.Add(nodeStartX);
		listPosYStart.Add(nodeStartY);
		listPosXEnd.Add(_nodeEndX);
		listPosYEnd.Add(_nodeEndY);

		listIsChessStart.Add(isChessStart);
		listIsChessEnd.Add(isChessEnd);
		listIsShowStart.Add(_isStartUp);
		listIsShowEnd.Add(_isEndtUp);
		listTeamStart.Add(_isTeamRedStart);
		listTeamEnd.Add(_isTeamRedEnd);
		listTeamShowStart.Add(_isTeamRedShowStart);
		listTeamShowEnd.Add(_isTeamRedShowEnd);

		listTypeStart.Add(_typeStart);
		listTypeEnd.Add(_typeEnd);
		listTypeStartShow.Add(_typeStartShow);
		listTypeEndShow.Add(_typeEndShow);
	}
	public void StopCuroutineAll()
	{
		StopCoroutine("ClickAutoPlay");
		StopCoroutine("OnFastNext");
		StopCoroutine("OnFastPre");
	}
	public void ClickAutoPlay()
	{
		StopCuroutineAll();
		StartCoroutine(ClickAutoPlay1());
	}

	public void ClickBtnPause()
	{
		StopCuroutineAll();
		BtnPlay.SetActive(true);
	}
	public void OnNext()
	{
		StopCuroutineAll();
		OnNextCuroutine(timeAutoPlay);
	}

	public void OnPre()
	{
		StopCuroutineAll();
		OnPreCoroutine(timeAutoPlay);
	}
	void OnFastNext()
	{
		StopCuroutineAll();
		StartCoroutine(OnFastNext1());
	}
	void OnFastPre()
	{
		StopCuroutineAll();
		StartCoroutine(OnFastPre1());
	}
	public IEnumerator ClickAutoPlay1()
	{
		yield return new WaitForSeconds(timeAutoPlay + timeAutoPlay / 5);
		if (indexMove < listPosXStart.Count)
		{
			OnNextCuroutine(timeAutoPlay);
			StartCoroutine(ClickAutoPlay1());
			BtnPlay.SetActive(false);
			BtnPause.SetActive(true);
		}
		else
		{
			StopCoroutine("ClickAutoPlay");
			BtnPlay.SetActive(true);
		}
	}
	public IEnumerator OnFastNext1()
	{
		yield return new WaitForSeconds(timeGoFast + timeGoFast / 5);
		if (indexMove < listPosXStart.Count)
		{
			OnNextCuroutine(timeGoFast);
			StartCoroutine(OnFastNext1());
		}
		else
		{
			StopCoroutine("OnFastNext");
		}
	}
	public IEnumerator OnFastPre1()
	{
		yield return new WaitForSeconds(timeGoFast + timeGoFast / 5);
		if (indexMove > 0)
		{
			OnPreCoroutine(timeGoFast);
			StartCoroutine(OnFastPre1());
		}
		else
		{
			StopCoroutine("OnFastPre");
		}
	}
	void OnNextCuroutine(float _TimeFloat)
	{
		if (MovingChess == false)
		{
			if (indexMove < listPosXStart.Count)
			{
				indexMove++;
				int cloneOder = 0;
				if (listTeamStart[indexMove - 1])
					cloneOder = 1;
				else
					cloneOder = 2;
				StartCoroutine(GameController.instance.MoveGameObject(cloneOder, true, indexMove, false, listPosXStart[indexMove - 1], listPosYStart[indexMove - 1], listPosXEnd[indexMove - 1], listPosYEnd[indexMove - 1], _TimeFloat,
					listIsChessStart[indexMove - 1], listIsShowStart[indexMove - 1], listTeamStart[indexMove - 1], listTeamShowStart[indexMove - 1], listTypeStart[indexMove - 1], listTypeStartShow[indexMove - 1],
					listIsChessEnd[indexMove - 1], listIsShowEnd[indexMove - 1], listTeamEnd[indexMove - 1], listTeamShowEnd[indexMove - 1], listTypeEnd[indexMove - 1], listTypeEndShow[indexMove - 1]));
			}
		}
	}
	void OnPreCoroutine(float _TimeFloat)
	{
		if (MovingChess == false)
		{
			if (indexMove > 0)
			{
				indexMove--;
				int cloneOder = 0;
				if (listTeamStart[indexMove])
					cloneOder = 1;
				else
					cloneOder = 2;
				StartCoroutine(GameController.instance.MoveGameObject(cloneOder, false, indexMove, false, listPosXEnd[indexMove], listPosYEnd[indexMove], listPosXStart[indexMove], listPosYStart[indexMove], _TimeFloat,
					listIsChessStart[indexMove], listIsShowStart[indexMove], listTeamStart[indexMove], listTeamShowStart[indexMove], listTypeStart[indexMove], listTypeStartShow[indexMove],
					listIsChessEnd[indexMove], listIsShowEnd[indexMove], listTeamEnd[indexMove], listTeamShowEnd[indexMove], listTypeEnd[indexMove], listTypeEndShow[indexMove]));
			}
		}
	}


	public void SetHistory(int[] arrPosXStart, int[] arrPosYStart, int[] arrIdStart, bool[] arrIsShowStart,
		bool[] arrIsTeamMove, int[] arrIdchessShow,
		int[] arrPosXEnd, int[] arrPosYEnd, int[] arrIdEnd, bool[] arrIsShowEnd)
	{
		Isviewer = true;
		IsAutoPlay = true;
		indexMove = arrPosXStart.Length;
		listPosXStart = new List<int>(arrPosXStart);
		listPosYStart = new List<int>(arrPosYStart);

		listIsShowStart = new List<bool>(arrIsShowStart);


		listTeamStart = new List<bool>(arrIsTeamMove);

		listPosXEnd = new List<int>(arrPosXEnd);
		listPosYEnd = new List<int>(arrPosYEnd);

		listIsShowEnd = new List<bool>(arrIsShowEnd);
	}

	public void AddStepHistory(int posXStart, int posYStart, int idStart, bool isShowStart,
		bool isTeamMove, int idchessShow,
		int posXEnd, int posYEnd, int idEnd, bool isShowEnd)
	{
		listPosXStart.Add(posXStart);
		listPosYStart.Add(posYStart);

		listIsShowStart.Add(isShowStart);

		listTeamStart.Add(isTeamMove);

		listPosXEnd.Add(posXEnd);
		listPosYEnd.Add(posYEnd);

		listIsShowEnd.Add(isShowEnd);

		if (indexMove == listPosXStart.Count - 1)
		{
			ClickBtnNextMove();
		}
	}

	public void RemoveStepHistory(int posXStart, int posYStart, int idStart, bool isShowStart,
		bool isTeamMove, int idchessShow,
		int posXEnd, int posYEnd, int idEnd, bool isShowEnd)
	{
		int index = listPosXStart.Count - 1;
		listPosXStart.RemoveAt(index);
		listPosYStart.RemoveAt(index);

		listIsShowStart.RemoveAt(index);


		listTeamStart.RemoveAt(index);

		listPosXEnd.RemoveAt(index);
		listPosYEnd.RemoveAt(index);

		listIsShowEnd.RemoveAt(index);
	}
	public bool IsOpenChess()
	{
		if (GameController.instance.RoomInfoIG.RoomType == RoomSelect.TruyenThong)
			return true;
		else
			return false;
	}

	public void ClickBtnNextMove()
	{
		IsAutoPlay = false;
		StopAllCoroutineViewer();
		if (indexMove < listPosXStart.Count)
		{
			//GameCoTuongManager.Instance.MoveChess(listIdChessShow[indexMove], listPosXStart[indexMove], listPosYStart[indexMove], listPosXEnd[indexMove], listPosYEnd[indexMove], !listTeamStart[indexMove], listIdChessEnd[indexMove], IsOpenChess());
			indexMove++;
		}
	}
	public void ClickBtnBackMove()
	{
		Board.instance.ClearNode(true);
		IsAutoPlay = false;
		StopAllCoroutineViewer();
		if (indexMove > 0)
		{
			indexMove--;
			//GameCoTuongManager.Instance.PreMoveChess(listPosXStart[indexMove], listPosYStart[indexMove], !listIsShowStart[indexMove], listIdChessStart[indexMove],
			//    listPosXEnd[indexMove], listPosYEnd[indexMove], listIdChessEnd[indexMove], listIsShowEnd[indexMove]);
		}
	}

	public void ClickBtnGoEnd()
	{
		StopAllCoroutineViewer();
		coroutineGoEnd = StartCoroutine(IEAutoEnd(timeGoFast));
	}
	public IEnumerator IEAutoEnd(float time)
	{
		IsAutoPlay = true;
		while (indexMove < listPosXStart.Count &&
			IsAutoPlay == true)
		{
			//GameCoTuongManager.Instance.MoveChess(listIdChessShow[indexMove], listPosXStart[indexMove], listPosYStart[indexMove], listPosXEnd[indexMove], listPosYEnd[indexMove], !listTeamStart[indexMove], listIdChessEnd[indexMove], IsOpenChess());
			indexMove++;
			yield return new WaitForSeconds(time);
		}
	}
	public void ClickBtnGoStart()
	{
		StopAllCoroutineViewer();
		coroutineGoStart = StartCoroutine(IEGoStart(timeGoFast));
	}

	public IEnumerator IEGoStart(float time)
	{
		IsAutoPlay = true;
		while (indexMove > 0 &&
			IsAutoPlay == true)
		{
			indexMove--;
			//GameCoTuongManager.Instance.PreMoveChess(listPosXStart[indexMove], listPosYStart[indexMove], !listIsShowStart[indexMove], listIdChessStart[indexMove],
			//    listPosXEnd[indexMove], listPosYEnd[indexMove], listIdChessEnd[indexMove], listIsShowEnd[indexMove]);
			yield return new WaitForSeconds(time);
		}
	}
	#endregion
	//==================================== ALL RPC CALL HERE ===========================================
	[PunRPC]
	void URoomInfo(string _RoomID, int bet, int timethink, int timemax)
	{
		GameController.instance.CB_URoomInfo(_RoomID, bet, timethink, timemax);
	}

	[PunRPC]
	void ChooseSide(string _ID,int _side)
	{
        if (_ID == UserConfig.Data.UserID)
        {
			if (_side == 0)
				GameController.instance.SpawnMine(0);
			if (_side == 1)
				GameController.instance.SpawnMine(1);
		}
        else
        {
			if(_side == 0)
            {
				GameController.instance.SpawnMine(1);
            }else
				GameController.instance.SpawnMine(0);
		}
	}
	[PunRPC]
	void SpawnPlayer(int _side,string _id,string _name,float _coins,int avatarindex)
    {
		GameController.instance.SpawnPlayer(_side ,_id ,_name ,_coins, avatarindex);
    }
	[PunRPC]
	void CreateBoard()
    {
		StartCoroutine (GameController.instance.CreateBoard());
    }
	[PunRPC]
	void CallStart()
    {
		GameController.instance.CallStartGame();
    }
	//PLAY
	[PunRPC]
	void StartTurn(GameController.TeamChess Team)
	{
		GameController.instance.CB_StartTurn(Team);
	}


	//------------------------------SIGN ODER ==========================

	[PunRPC]
	void CallMTUpdateList(string _ID)
	{
		//GameController.instance.MTUPDATELIST(_ID);
	}

	[PunRPC]
	public void TD(int _Oder, int _TimeLeft,int HeSoThoiGian)
	{//TIME DOWN
		if (_TimeLeft >= 0)
		{
            if (GameController.instance.Players[_Oder].isEndTimeThink)
            {
				GameController.instance.Players[_Oder].EndTimeCount.SetActive(true);
				GameController.instance.Players[_Oder].EndTimeCount.transform.Find("NhanHeSo").GetComponent<UILabel>().text = "x" + HeSoThoiGian.ToString();
            }
			GameController.instance.Players[_Oder].LbTimeLeft.text = GameController.instance.CountTiming(_TimeLeft);
			GameController.instance.Players[_Oder].TimeLeft = _TimeLeft;
		}
		else
		{
			GameController.instance.Players[_Oder].StopTime();
			if (PhotonNetwork.isMasterClient)
			{
				if (_Oder == 0)
					GameController.instance.ClaimWin(1,true,"Hết thời gian tổng",0,"",0,"");
				if (_Oder == 1)
					GameController.instance.ClaimWin(0, true, "Hết thời gian tổng", 0, "", 0, "");
			}
		}
		if (GameController.instance.myOrder == _Oder)
		{
			if (_TimeLeft <= 10 && _TimeLeft > 0)
				GameController.instance.PlayCloseTime();
			if (_TimeLeft == 0)
				GameController.instance.PlayEndTime();
		}
	}
	[PunRPC]
	public void TT(int _Oder, int _ThinkTime)
	{//TIME THINK CD
		if (_ThinkTime >= 0)
		{
			GameController.instance.Players[_Oder].isEndTimeThink = false;
			float cloneTime = (float)_ThinkTime / GameController.instance.RoomInfoIG.RoomTimePerTurn;
			GameController.instance.Players[_Oder].ReconnectSprite.fillAmount = cloneTime;
		}
		else
		{
			GameController.instance.Players[_Oder].isEndTimeThink = true;
		}
		//PLAY SOUND
		if (GameController.instance.myOrder == _Oder)
		{
			if (_ThinkTime <= 10 && _ThinkTime > 0)
				GameController.instance.PlayCloseTime();
			if (_ThinkTime == 0)
			{
				GameController.instance.PlayEndTime();
			}
		}
	}
	
	[PunRPC]
	void SendNextTurn(int _NextTurn)
	{
		TeamStartTurn = _NextTurn;
	}

	[PunRPC]
	void ShowCurrentPlayer(bool _isJoin, string _ID)
	{
		GameController.instance.CB_ShowCurrentPlayer(_isJoin, _ID);
	}
	[PunRPC]
	void OnStartStep4ReceiveCoUp(int _posX, int _posY, bool _isShow, TypeChess _typeChess, bool _Teamred, TypeChess _typeChessShow)
	{
		GameController.instance.board.mNode[_posY, _posX].Createchess(new Chess(_typeChess, _Teamred, _isShow, _typeChessShow), new NodeBoard(_posY, _posX));
	}
	[PunRPC]
	void OnStartStep4ReceiveCoLoan(int _posX, int _posY, bool _isShow, TypeChess _typeChess, bool _Teamred, bool _teamRedShow, TypeChess _typeChessShow)
	{
		GameController.instance.board.mNode[_posY, _posX].Createchess(new Chess(_typeChess, _Teamred, _isShow, _teamRedShow, _typeChessShow), new NodeBoard(_posY, _posX));
	}
	[PunRPC]
	void PlayerDisconnected(string _ID, bool _isDC)
	{
		//GameController.instance.CB_PlayerDisconnected(_ID, _isDC);
	}
	[PunRPC]
	void ReconnectToGame(string _ID, int _NewOder, bool _IsRedTurn,int MatchTime)
	{
		GameController.instance.CB_ReconnectToGame(_ID, _NewOder, _IsRedTurn, MatchTime);
	}
	[PunRPC]
	void RecieveChess(string _ID, int _NodeboardX, int _NodeboardY, bool _isChess, bool _show, TypeChess _typechess, bool _teamred, TypeChess _TypechessShow)
	{
		StartCoroutine(GameController.instance.CB_RecieveChess(_ID, _NodeboardX, _NodeboardY, _isChess, _show, _typechess, _teamred, _TypechessShow));
	}
	[PunRPC]
	void SendEated(string _ID, int _Oder, string _NameChess, bool _IsShow, bool _isTeamRedShow)
	{//SEND FOR NEW PLAYER
		GameController.instance.CB_SendEated(_ID, _Oder, _NameChess, _IsShow, _isTeamRedShow);
	}
	[PunRPC]
	void ClientAddStep(string _ID, int _index, int nodeStartX, int nodeStartY, int nodeEndX, int nodeEndY,
		bool _isChessStart, bool _isStartUp, bool _isTeamRedStart, bool _isTeamRedShowStart, TypeChess _typeStart, TypeChess _typeStartShow,
		bool _isChessEnd, bool _isEndtUp, bool _isTeamRedEnd, bool _isTeamRedShowEnd, TypeChess _typeEnd, TypeChess _typeEndShow)
	{
		GameController.instance.CB_ClientAddStep(_ID, _index, nodeStartX, nodeStartY, nodeEndX, nodeEndY,
				_isChessStart, _isStartUp, _isTeamRedStart, _isTeamRedShowStart, _typeStart, _typeStartShow,
				_isChessEnd, _isEndtUp, _isTeamRedEnd, _isTeamRedShowEnd, _typeEnd, _typeEndShow);

	}
	[PunRPC]
	void ADDCURRENTSCORE(string _ID, string _IDRed, string _IDBlue, float _ScoreRed, float _ScoreBlue, int _Nextturn)
	{
		GameController.instance.CB_ADDCURRENTSCORE(_ID, _IDRed, _IDBlue, _ScoreRed, _ScoreBlue, _Nextturn);
	}
	[PunRPC]
	void SaveMove(int _Oder, bool _OnNext, int _index, int nodeStartX, int nodeStartY, int nodeEndX, int nodeEndY, float _TimeTween, bool _isBotMove,
		bool _isChessStart, bool _isStartShow, bool _isTeamRedStart, bool _isTeamRedShowStart, TypeChess _typeStart, TypeChess _typeStartShow,
		bool _isChessEnd, bool _isEndShow, bool _isTeamRedEnd, bool _isTeamRedShowEnd, TypeChess _typeEnd, TypeChess _typeEndShow)
	{
		GameController.instance.CB_SaveMove(_Oder, _OnNext, _index, nodeStartX, nodeStartY, nodeEndX, nodeEndY, _TimeTween, _isBotMove,
				_isChessStart, _isStartShow, _isTeamRedStart, _isTeamRedShowStart, _typeStart, _typeStartShow,
				_isChessEnd, _isEndShow, _isTeamRedEnd, _isTeamRedShowEnd, _typeEnd, _typeEndShow);

	}
	
	[PunRPC]
	void ShowChieu(bool _isTeamred)
	{
		StartCoroutine(GameController.instance.CB_ShowChieu(_isTeamred));
	}

	[PunRPC]
	void Winz(int _Oder, bool _isSpecial, string _SpecialReason, float _HeSoTheChieu, string _StatusTheChieu, float _HeSoThamChieu, string _StatusThamChieu)
	{
		GameController.instance.CB_Winz(_Oder, _isSpecial, _SpecialReason, _HeSoTheChieu, _StatusTheChieu, _HeSoThamChieu, _StatusThamChieu);
	}
	[PunRPC]
	void Winner(int _Oder, bool _isSpecial, string _SpecialReason, float _HeSoTheChieu, string _StatusTheChieu, float _HeSoThamChieu, string _StatusThamChieu)
	{
		StartCoroutine(GameController.instance.CB_Winner(_Oder, _isSpecial, _SpecialReason, _HeSoTheChieu, _StatusTheChieu, _HeSoThamChieu, _StatusThamChieu));
	}
	[PunRPC]
	void CALLMASTERDRAW(string _Reason)
	{
		GameController.instance.CB_CALLMASTERDRAW(_Reason);
	}
	[PunRPC]
	void DRAWGAME(string _Reason)
	{
		StartCoroutine(GameController.instance.CB_DRAWGAME(_Reason));
	}
	[PunRPC]
	void OpenDraw(int _Oder)
	{
		if (GameController.instance.myOrder != -1 && GameController.instance.myOrder != _Oder)
		{
			StartCoroutine(UltilityGame.instance.TweenPanel(true, GameController.instance.objEffect.objDrawRecive));
			GameController.instance.objEffect.objDrawRecive.transform.Find("Main/NOTICE").GetComponent<UILabel>().text = "Đối phương vừa gợi ý xin hoà , Bạn có đồng ý không ?";
		}
	}

	[PunRPC]
	void SendChat(string _ID, string _Name, int _Oder, string _Chat)
	{
		GameController.instance.CB_SendChat(_ID, _Name, _Oder, _Chat);
	}
	[PunRPC]
	void EChat(int _Oder, int _sprite)
	{
		StartCoroutine(GameController.instance.Players[_Oder].EmotionShow(_sprite));
	}
	[PunRPC]
	void QChat(int _Oder, string _Text)
	{
		StartCoroutine(GameController.instance.Players[_Oder].QuickChatShow(_Text));
	}
	[PunRPC]
	void Kick(string _ID)
	{
		if (UserConfig.Data.UserID == _ID)//NGUOI BI KICK
			GameController.instance.objEffect.OnLeave();
	}
	//REMATCH
	[PunRPC]
	void CheckRematchToggle(string _ID,bool _bool)
    {
		GameController.instance.SetTaiDau(_ID,_bool);
    }


	//--------------------------------------------------------------------------------------------------
}

