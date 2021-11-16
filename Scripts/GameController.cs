using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;
using System;
using UnityEngine.Events;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace COTUONG
{
	public class GameController : MonoBehaviour
	{
		public static GameController instance;
		public GameObject Adminzone;
		public NetworkManager RPCNetwork;
		public PlayerManager[] Players;
		public ObscuredInt NumDraw;
		// Use this for initialization
		public int myOrder;
		public Board board;
		public TeamChess teamPlayer;
		public Effect objEffect;
		public PlayerManager MyPlayer;
		private bool canPlay = false;
		public bool _isMyTurn = false;
		public bool GameStarted = false;

		Queue<IEnumerator> aActions = new Queue<IEnumerator>();
		public bool isRequesting = false;
		public RoomInfoInGame RoomInfoIG;//IMPORTANT

		//public WebCamTexture webcamTexture;
		//public WebCamDevice[] devices;
		public static int numChieu, numChieuMax, numBatBien, numKhongAnQuan, numBatQuan;
		public UILabel TurnStatus, RoomStatus, TotalTimeMatch;
		public int MatchTime = 0;

		public const string CoUp = "Cờ Úp";
		public UILabel AlertLose;
		int TinhHesoThoigian()
		{
			int Heso = 0;
			if (PhotonNetwork.inRoom)
			{
				if ((int)PhotonNetwork.room.CustomProperties["timemax"] == 300) { Heso = 2; }
				if ((int)PhotonNetwork.room.CustomProperties["timemax"] == 600) { Heso = 3; }
				if ((int)PhotonNetwork.room.CustomProperties["timemax"] == 900) { Heso = 4; }
				if ((int)PhotonNetwork.room.CustomProperties["timemax"] == 1200) { Heso = 5; }
				if ((int)PhotonNetwork.room.CustomProperties["timemax"] == 1500) { Heso = 6; }
				if ((int)PhotonNetwork.room.CustomProperties["timemax"] == 1800) { Heso = 7; }
			}
			return Heso;
		}
		public bool ComingLosing = false;
		public int CaseLosing = 0;
		public GameObject PanelAlertLose;
		public UILabel StatusAlertLose;

		public void ShowAlertLose(string _StatusLose)
		{
			StartCoroutine(UltilityGame.instance.TweenPanel(true, PanelAlertLose));
			StatusAlertLose.text = _StatusLose;
		}
		public void OnOKDiQuan()
		{
			board.mNode[board.CurrentNodeClick.y, board.CurrentNodeClick.x].AfterCheckLoseMove();
			OnCloseAlertLose();
		}
		public void OnCloseAlertLose()
		{
			StartCoroutine(UltilityGame.instance.TweenPanel(false, PanelAlertLose));
		}
		public void AlertLosing(bool _Enable, int _Oder, string _Status)
		{
			if (_Enable == false)
			{
				AlertLose.gameObject.SetActive(false);
				Players[_Oder].AlertWrongMove.SetActive(false);
			}
			else
			{
				Players[_Oder].AlertWrongMove.SetActive(true);
				AlertLose.gameObject.SetActive(true);
				AlertLose.text = "Chú ý : " + _Status;
				//GUIUtility.systemCopyBuffer
			}
		}
		public void CheckAlert()
		{
			//numChieu
			if (numChieu == 4)
			{
				ComingLosing = true;
				CaseLosing = 1;
				AlertLosing(true, myOrder, "1 Quân chiếu liên tiếp nhiều lần !");
			}
			else
			{
				if (numChieu > 4)
				{
					if (myOrder == 0)
						ClaimWin(1, true, "Chiếu dai", 0, "", 0, "");
					else
						ClaimWin(0, true, "Chiếu dai", 0, "", 0, "");
				}
			}
			//numMaxChieu
			if (numChieuMax == 8)
			{
				ComingLosing = true;
				CaseLosing = 2;
				AlertLosing(true, myOrder, "Chiếu liên tiếp nhiều lần !");
			}
			else
			{
				if (numChieuMax > 8)
				{
					if (myOrder == 0)
						ClaimWin(1, true, "Chiếu liên tiếp 9 lần ", 0, "", 0, "");
					else
						ClaimWin(0, true, "Chiếu liên tiếp 9 lần ", 0, "", 0, "");
				}
			}
			//numBatQuan
			if (numBatQuan == 4)
			{
				ComingLosing = true;
				CaseLosing = 3;
				AlertLosing(true, myOrder, "Bắt quân đối thủ nhiều lần ! \n Vui lòng thay đổi nước đi ");
			}
			else
			{
				if (numBatQuan > 4)
				{
					if (myOrder == 0)
						ClaimWin(1, true, "Bắt quân nhiều lần !", 0, "", 0, "");
					else
						ClaimWin(0, true, "Bắt quân nhiều lần !", 0, "", 0, "");
				}
			}
			Debug.Log("num bat quan " + numBatQuan);
			//numBatBien
			if (numBatBien == 3)
			{
				AlertLosing(true, myOrder, "[Hoà]Bàn cờ lặp lại nhiều lần !\n Vui lòng thay đổi nước đi");
				ComingLosing = true;
				CaseLosing = 4;
			}
			else
			{
				if (numBatBien > 4)
					StartCoroutine(ClaimDraw("Luật bất biến"));
			}
			//numKhongAnQuan
			if (numKhongAnQuan == 59)
			{
				ComingLosing = true;
				CaseLosing = 5;
				AlertLosing(true, myOrder, "[Hoà]Không có quân bị triệt tiêu !");
			}
			else
			{
				if (numKhongAnQuan > 59)
					StartCoroutine(ClaimDraw("Không có quân bị triệt tiêu !"));
			}
		}

		void Start()
		{
			instance = this;
			TextListControl.Clear();
			NGUIMessage.instance.CALLLOADING(false);

			canPlay = false;
			_isMyTurn = false;
			//InitButton (-1);
			MatchTime = 0;
			GameStarted = false;

			board.InitBoard();
			objEffect.PlayGround.SetActive(false);
			Players[0].UI.SetActive(false);
			Players[1].UI.SetActive(false);
			if (RoomSelect.EnableAdmin)
			{
				Adminzone.SetActive(true);
			}
			else
				Adminzone.SetActive(false);
		}
		//============================== EXTENCEN FUNCTION ========================================================
		public void EnqueueAction(IEnumerator aAction)
		{
			aActions.Enqueue(aAction);
		}
		private IEnumerator Process()
		{
			if (!isRequesting)
			{
				if (aActions.Count > 0)
				{
					isRequesting = true;
					yield return StartCoroutine(aActions.Dequeue());
				}
				else
					yield return null;
			}
			yield return new WaitForSeconds(1);
			StartCoroutine(Process());
		}

		public bool CheckMyTeam()
		{
			bool TEAM = false;
			if (teamPlayer == TeamChess.red)
				TEAM = true;
			if (teamPlayer == TeamChess.blue)
				TEAM = false;
			return TEAM;
		}

		public string CountTiming(int _CountTime)
		{
			int clone1 = _CountTime / 60;
			int clone2 = _CountTime % 60;
			return clone1.ToString() + ":" + clone2.ToString("00");
		}
		//-----------------------------------------------------------------------------------------------------
		//
		//==================================== INIT GAME ======================================================
		public bool isPlayingGame;

		public bool CheckDuTien()
		{
			bool _enoughMoney = true;
			if (Players[0].PlayerCoin < RoomInfoIG.RoomCoin ||
				Players[1].PlayerCoin < RoomInfoIG.RoomCoin)
				_enoughMoney = false;
			return _enoughMoney;
		}
		bool PlayerKhongDuTien(int _Slot)
		{
			bool DuTien = true;
			if (_Slot == 1)
				if (Players[0].PlayerCoin < RoomInfoIG.RoomCoin)
					DuTien = false;

			if (_Slot == 2)
				if (Players[1].PlayerCoin < RoomInfoIG.RoomCoin)
					DuTien = false;
			return DuTien;
		}

		public void CreateRoomName()
		{
			GameObject cloneRoomName = PhotonNetwork.Instantiate(LoadPrefab.instance.RoomNamepref.name, LoadPrefab.instance.RoomNamepref.transform.position, LoadPrefab.instance.RoomNamepref.transform.rotation, 0);
			string cloneID = PhotonNetwork.room.CustomProperties["roomid"].ToString();
			int bet = (int)PhotonNetwork.room.CustomProperties["bet"];
			int maxTime = (int)PhotonNetwork.room.CustomProperties["timemax"];
			int timethink = (int)PhotonNetwork.room.CustomProperties["timethink"];
			cloneRoomName.GetComponent<PhotonView>().RPC("URoomInfo", PhotonTargets.AllBufferedViaServer, cloneID, bet, timethink, maxTime);
		}


		public void InitBoardLayer(int _Oder)
		{//XOAY BAN CO DUNG HUONG :D 
			if (_Oder == 0)
			{
				board.transform.localRotation = Quaternion.Euler(0, 0, 0);
				for (int i = 0; i < 10; i++)
					for (int j = 0; j < 9; j++)
					{
						board.mNode[i, j].transform.localRotation = Quaternion.Euler(0, 0, 0);
					}
			}
			if (_Oder == 1)
			{
				board.transform.localRotation = Quaternion.Euler(0, 0, 180);
				for (int i = 0; i < 10; i++)
					for (int j = 0; j < 9; j++)
					{
						board.mNode[i, j].transform.localRotation = Quaternion.Euler(0, 0, 180);
					}
			}
		}

		public IEnumerator CheckIsPlayerRejoin(string _ID, int _MatchTime)
		{
			yield return new WaitForSeconds(2f);
			if (PhotonNetwork.isMasterClient && GameStarted)
			{
				if (Players[0].PlayerID == _ID)
				{
					RoomInfoIG.GetComponent<PhotonView>().RPC("ReconnectToGame", PhotonTargets.All, _ID, 0, RoomInfoIG.ISTEAMREDPLAY, _MatchTime);
				}
				if (Players[1].PlayerID == _ID)
				{
					RoomInfoIG.GetComponent<PhotonView>().RPC("ReconnectToGame", PhotonTargets.All, _ID, 1, RoomInfoIG.ISTEAMREDPLAY, _MatchTime);
				}
			}
		}
		public IEnumerator UpdateAllPlayer()
		{
			yield return new WaitForSeconds(2);
			RoomInfoIG.GetComponent<PhotonView>().RPC("SpawnPlayer", PhotonTargets.Others, 0, Players[0].PlayerID, Players[0].PlayerName, Players[0].PlayerCoin, Players[0].PlayerAvatarIndex);
			RoomInfoIG.GetComponent<PhotonView>().RPC("SpawnPlayer", PhotonTargets.Others, 1, Players[1].PlayerID, Players[1].PlayerName, Players[1].PlayerCoin, Players[1].PlayerAvatarIndex);
		}
		public void DeletePlayer(string _ID)
		{
			if (Players[0].PlayerID == _ID)
			{
				Players[0].UI.SetActive(false);
				Players[0].RESET();
			}
			if (Players[1].PlayerID == _ID)
			{
				Players[1].UI.SetActive(false);
				Players[1].RESET();
			}
		}
		public void WaitReconnect(string _ID)
		{// XOA NGAY LAP TUC
			if (PhotonNetwork.isMasterClient)
			{
				if (Players[0].PlayerID == _ID)
				{
					Players[0].isPlayerDisconnected = true;
					Players[0].DCShow(true);
				}
				if (Players[1].PlayerID == _ID)
				{
					Players[1].isPlayerDisconnected = true;
					Players[1].DCShow(true);
				}
			}
		}

		//-------------------------------------------------------------------------------------------------------

		#region CALL RPC
		//CALL RPC HERE ---------------------------------

		//
		#endregion
		#region RPC CALLBACK
		public void SpawnMine(int _side)
		{
			myOrder = _side;
			if (RoomInfoIG == null)
			{
				RoomInfoIG = GameObject.FindGameObjectWithTag("RoomInfo").GetComponent<RoomInfoInGame>();
			}
			RoomInfoIG.GetComponent<PhotonView>().RPC("SpawnPlayer", PhotonTargets.All, _side, UserConfig.Data.UserID, UserConfig.Data.UserName, UserConfig.Data.UserCoins, RoomSelect.AvatarSelec);
		}
		//=============================================================== RPC CALL BACK ---------------------------
		public void CB_URoomInfo(string _roomID, int bet, int timethink, int timemax)
		{
			RoomInfoIG = GameObject.FindGameObjectWithTag("RoomInfo").GetComponent<RoomInfoInGame>();
			RoomInfoIG.RoomTime = timemax;
			RoomInfoIG.RoomTimePerTurn = timethink;
			RoomInfoIG.RoomCoin = bet;
			RoomInfoIG.RoomID = _roomID;

			RoomInfoIG.MatchID = "1";
			RoomInfoIG.AutoShow();
		}
		public void SpawnPlayer(int _side, string _id, string _name, float _coins, int avatarindex)
		{
			Players[_side].PlayerID = _id;
			Players[_side].PlayerName = _name;
			Players[_side].PlayerOder = _side;
			Players[_side].PlayerAvatarIndex = avatarindex;
			Players[_side].PlayerCoin = _coins;

			Players[_side].LbName.text = _name;
			Players[_side].LbCoins.text = _coins.ToString("C0");
			Players[_side].AvatarDF.sprite2D = LoadPrefab.instance.SpriteAvatars[avatarindex];
			Players[_side].UI.SetActive(true);
			if (_id == UserConfig.Data.UserID)
			{
				MyPlayer = Players[_side];
			}
			//DRAW SIDE
			Players[_side].lbSide.text = _name;
		}

		public IEnumerator CreateBoard()
		{
			if (!GameStarted)
			{
				board.listChessBlue = new List<NodeBoard>();
				board.listChessRed = new List<NodeBoard>();
				board.listChessBlueSave = new List<List<NodeBoard>>();
				board.listChessRedSave = new List<List<NodeBoard>>();
				RoomInfoIG.NewViewGame();
				/*
				if (!RoomInfoIG.ReviewButton.activeInHierarchy)
				{
					RoomInfoIG.ReviewButton.SetActive(true);
					iTween.ScaleFrom(RoomInfoIG.ReviewButton, iTween.Hash("x", 0, "y", 0, "time", 0.5f));
				}
				*/

				RoomInfoIG.CheckTiso(Players[0].PlayerID, Players[1].PlayerID, 0, 0);

				_isMyTurn = false;
				//CREATE CHESS
				if (GameController.instance.myOrder == 0)
				{//CO DO DANH TIEN
					teamPlayer = TeamChess.red;
				}
				if (GameController.instance.myOrder == 1)
				{//CO XANH DANH HAU
					teamPlayer = TeamChess.blue;
				}
				if (GameController.instance.myOrder == -1)
				{//KHAN GIA
					canPlay = false;
					teamPlayer = TeamChess.none;
				}
				//PLAYERS
				Players[0].TimeLeft = RoomInfoIG.RoomTime;
				Players[0].LbTimeLeft.text = GameController.instance.CountTiming(RoomInfoIG.RoomTime);
				Players[1].TimeLeft = RoomInfoIG.RoomTime;
				Players[1].LbTimeLeft.text = GameController.instance.CountTiming(RoomInfoIG.RoomTime);

				//RESET TAI DAU
				RoomInfoIG.GetComponent<PhotonView>().RPC("CheckRematchToggle", PhotonTargets.All, UserConfig.Data.UserID, CheckRematch.value);

				InitBoardLayer(myOrder);
				// FOR MASTER START TURN
				if (PhotonNetwork.isMasterClient)
				{
					board.NewGame(RoomSelect.CoUp);
					yield return new WaitForSeconds(1f);

					for (int i = 0; i < board.listChessRed.Count; i++)
					{
						RoomInfoIG.GetComponent<PhotonView>().RPC("OnStartStep4ReceiveCoUp", PhotonTargets.Others,
							board.listChessRed[i].x, board.listChessRed[i].y,
							board.mNode[board.listChessRed[i].y, board.listChessRed[i].x].chess.show,
							board.mNode[board.listChessRed[i].y, board.listChessRed[i].x].chess.typeChess,
							board.mNode[board.listChessRed[i].y, board.listChessRed[i].x].chess.teamRed,
							board.mNode[board.listChessRed[i].y, board.listChessRed[i].x].chess.typeChessShow);
					}
					for (int i = 0; i < board.listChessBlue.Count; i++)
					{
						RoomInfoIG.GetComponent<PhotonView>().RPC("OnStartStep4ReceiveCoUp", PhotonTargets.Others,
							board.listChessBlue[i].x, board.listChessBlue[i].y,
							board.mNode[board.listChessBlue[i].y, board.listChessBlue[i].x].chess.show,
							board.mNode[board.listChessBlue[i].y, board.listChessBlue[i].x].chess.typeChess,
							board.mNode[board.listChessBlue[i].y, board.listChessBlue[i].x].chess.teamRed,
							board.mNode[board.listChessBlue[i].y, board.listChessBlue[i].x].chess.typeChessShow);
					}
				}
			}
		}
		float timeStartGame = 5;
		public GameObject PanelStart;
		public UILabel lbTimeStart;
		public void CallStartGame()
		{
			timeStartGame = 5;
			StartCoroutine(UltilityGame.instance.TweenPanel(true, PanelStart));
			InvokeRepeating("ReapareGame", 1, 1);
		}
		void CheckTurn()
		{
			if (Players[0].PlayerID == RoomInfoIG.IDPlayerRed && Players[1].PlayerID == RoomInfoIG.IDPlayerBlue)
			{
				RoomInfoIG.isNewPlayerCome = false;
			}
			else
				RoomInfoIG.isNewPlayerCome = true;
		}
		void ReapareGame()
		{
			timeStartGame--;
			lbTimeStart.text = timeStartGame.ToString();
			PlayCloseTime();

			if (timeStartGame == 0 && PhotonNetwork.playerList.Length == 2)
			{
				objEffect.objResult._TheChieu = new TheChieu { Heso = 0, Status = "" };
				objEffect.objResult._ThamChieu = new TheChieu { Heso = 0, Status = "" };
				CancelInvoke("ReapareGame");
				GameStarted = true;
				canPlay = true;
				PlayStartGame();
				InvokeRepeating("CountTimeTotal", 0.25f, 1);
				objEffect.PlayGround.SetActive(true);

				StartCoroutine(UltilityGame.instance.TweenPanel(false, PanelStart));

				CheckTurn();

				RoomInfoIG.IDPlayerRed = Players[0].PlayerID;
				RoomInfoIG.IDPlayerBlue = Players[1].PlayerID;
				//SET HESO THOI GIAN
				RoomSelect.HeSoThoiGian = TinhHesoThoigian();

				StartCoroutine(ReqAPI.instance.StartOrEndGame(RoomInfoIG.RoomID, Players[0].PlayerID, Players[1].PlayerID, RoomInfoIG.RoomCoin.ToString(), "1", false, RoomInfoIG.RoomID, UserConfig.Data.UserID, false, "CoUp", UserConfig.Data.UserKey, (isSucc, NewSecurity) =>
				{
					if (isSucc)
					{//START GAME
						UserConfig.Data.UserKey = NewSecurity;
						if (PhotonNetwork.isMasterClient)
						{
							if (RoomInfoIG.isNewPlayerCome)
							{
								callTurn(TeamChess.red);
							}
							else
							{
								if (RoomInfoIG.IDPlayerRed == RoomInfoIG.PlayerLoseLastMatch)
								{
									callTurn(TeamChess.red);
								}
								else
									callTurn(TeamChess.blue);
							}
						}
					}
				}));

			}
		}
		void CountTimeTotal()
		{
			MatchTime++;
			TotalTimeMatch.text = CountTiming(MatchTime);
		}
		//END INIT GAME===================================================================

		public void CB_ShowCurrentPlayer(bool _isJoin, string _ID)
		{//JOIN AND LEFT HERE
			int OP = PhotonNetwork.playerList.Length;
			GameObject.FindGameObjectWithTag("CurrentPlayer").GetComponent<UILabel>().text = "Mời chơi : " + OP.ToString();
			if (_isJoin)
			{
				GameController.instance.PlayJoin();
			}
			else
			{
				GameController.instance.PlayOut();
			}

			if (UserConfig.Data.UserID == _ID && RoomInfoIG != null)
				if (UserConfig.Data.UserCoins < RoomInfoIG.RoomCoin)
					NGUIMessage.instance.CALLBIGMESSAGE("Gợi Ý", "Bạn không đủ tiền cược , bấm Xác nhận để vào cửa hàng !!!", this.gameObject, "CallOpenNap");
		}

		public void CB_ReconnectToGame(string _ID, int _NewOder, bool _IsRedTurn, int _MatchTime)
		{
			if (UserConfig.Data.UserID == _ID)
			{//MASTER CALL CHO THANG VUA VAO PHONG NEU DUNG ID
				myOrder = _NewOder;
				canPlay = true;
				GameStarted = true;
				MyPlayer = Players[_NewOder];

				MatchTime = _MatchTime + 3;
				InvokeRepeating("CountTimeTotal", 0.1f, 1);
				objEffect.PlayGround.SetActive(true);

				if (_NewOder == 0)
				{
					teamPlayer = TeamChess.red;
					if (_IsRedTurn)
						_isMyTurn = true;
					else
						_isMyTurn = false;
				}
				else
				{
					teamPlayer = TeamChess.blue;
					if (!_IsRedTurn)
						_isMyTurn = true;
					else
						_isMyTurn = false;

					board.transform.localRotation = Quaternion.Euler(0, 0, 180);
					for (int i = 0; i < 10; i++)
						for (int j = 0; j < 9; j++)
						{
							board.mNode[i, j].transform.localRotation = Quaternion.Euler(0, 0, 180);
						}
				}
				if (_isMyTurn)
					StartCoroutine(NGUIMessage.instance.CALLALERT("Đến lượt đi của bạn "));
				InitBoardLayer(_NewOder);
			}
			Players[_NewOder].DCShow(false);
		}
		public IEnumerator CB_RecieveChess(string _ID, int _NodeboardX, int _NodeboardY, bool _isChess, bool _show, TypeChess _typechess, bool _teamred, TypeChess _TypechessShow)
		{
			yield return new WaitForSeconds(0.5f);
			if (UserConfig.Data.UserID == _ID)
			{
				if (RoomInfoIG == null)
					RoomInfoIG = GameObject.FindGameObjectWithTag("RoomInfo").GetComponent<RoomInfoInGame>();
				yield return new WaitForSeconds(0.5f);
				board.mNode[_NodeboardY, _NodeboardX].Createchess(new Chess(_typechess, _teamred, _show, _TypechessShow), new NodeBoard(_NodeboardY, _NodeboardX));
				GameStarted = true;
			}
		}
		public void CB_SendEated(string _ID, int _Oder, string _NameChess, bool _IsShow, bool _isTeamRedShow)
		{//SEND FOR NEW PLAYER
			if (UserConfig.Data.UserID == _ID)
			{
				Players[_Oder].EatedChess.Add(_NameChess);
				Players[_Oder].EatedChessBool.Add(_IsShow);
				Players[_Oder].EatedChessRedTeam.Add(_isTeamRedShow);
				Players[_Oder].CreateChessEated(_Oder, _NameChess, _IsShow, _isTeamRedShow);
			}
		}
		public void CB_ClientAddStep(string _ID, int _index, int nodeStartX, int nodeStartY, int nodeEndX, int nodeEndY,
			bool _isChessStart, bool _isStartUp, bool _isTeamRedStart, bool _isTeamRedShowStart, TypeChess _typeStart, TypeChess _typeStartShow,
			bool _isChessEnd, bool _isEndtUp, bool _isTeamRedEnd, bool _isTeamRedShowEnd, TypeChess _typeEnd, TypeChess _typeEndShow)
		{
			if (UserConfig.Data.UserID == _ID)
			{
				RoomInfoIG.indexMove = _index;
				RoomInfoIG.AddStep(nodeStartX, nodeStartY, nodeEndX, nodeEndY,
					_isChessStart, _isStartUp, _isTeamRedStart, _isTeamRedShowStart, _typeStart, _typeStartShow,
					_isChessEnd, _isEndtUp, _isTeamRedEnd, _isTeamRedShowEnd, _typeEnd, _typeEndShow);
			}
		}
		public void CB_ADDCURRENTSCORE(string _ID, string _IDRed, string _IDBlue, float _ScoreRed, float _ScoreBlue, int _Nextturn)
		{
			if (UserConfig.Data.UserID == _ID)
			{
				if (RoomInfoIG == null)
					RoomInfoIG = GameObject.FindGameObjectWithTag("RoomInfo").GetComponent<RoomInfoInGame>();
				else
				{
					RoomInfoIG.IDPlayerRed = _IDRed;
					RoomInfoIG.IDPlayerBlue = _IDBlue;
					RoomInfoIG.CurrentScoreRed = _ScoreRed;
					RoomInfoIG.CurrentScoreBlue = _ScoreBlue;
					RoomInfoIG.TeamStartTurn = _Nextturn;
				}
				if (GameStarted)
				{
					RoomInfoIG.ShowTiso();
				}
			}
		}
		public void CB_SaveMove(int _Oder, bool _OnNext, int _index, int nodeStartX, int nodeStartY, int nodeEndX, int nodeEndY, float _TimeTween, bool _isBotMove,
			bool _isChessStart, bool _isStartShow, bool _isTeamRedStart, bool _isTeamRedShowStart, TypeChess _typeStart, TypeChess _typeStartShow,
			bool _isChessEnd, bool _isEndShow, bool _isTeamRedEnd, bool _isTeamRedShowEnd, TypeChess _typeEnd, TypeChess _typeEndShow)
		{
			RoomInfoIG.AddStep(nodeStartX, nodeStartY, nodeEndX, nodeEndY,
				_isChessStart, _isStartShow, _isTeamRedStart, _isTeamRedShowStart, _typeStart, _typeStartShow,
				_isChessEnd, _isEndShow, _isTeamRedEnd, _isTeamRedShowEnd, _typeEnd, _typeEndShow);
			//SHOW STEP COUNT FROM ROOM INFO
			RoomInfoIG.UpdateStepMove(_index);//COLOR

			//NEXT TURN

			if (myOrder == _Oder)
			{
				if (teamPlayer == TeamChess.red && _isMyTurn)
				{
					callTurn(TeamChess.blue);
				}
				if (teamPlayer == TeamChess.blue && _isMyTurn)
				{
					callTurn(TeamChess.red);
				}
			}

			if (RoomInfoIG.indexMove == (_index - 1))
			{
				StartCoroutine(MoveGameObject(_Oder, true, _index, true, nodeStartX, nodeStartY, nodeEndX, nodeEndY, _TimeTween,
					_isChessStart, _isStartShow, _isTeamRedStart, _isTeamRedShowStart, _typeStart, _typeStartShow,
					_isChessEnd, _isEndShow, _isTeamRedEnd, _isTeamRedShowEnd, _typeEnd, _typeEndShow));
			}
		}

		public void CB_StartTurn(TeamChess Team)
		{
			if (teamPlayer != TeamChess.none)
			{
				if (teamPlayer == Team)
				{//NGUOI CO TURN
					_isMyTurn = true;
					TurnStatus.text = "Đến lượt đi của bạn";
					TurnStatus.color = Color.green;
					PlayHasTurn();
				}
				else
				{
					_isMyTurn = false;
					TurnStatus.text = "Đến lượt đi của đối thủ";
					TurnStatus.color = Color.red;
				}
			}
			Debug.Log("My turn ? ");
			Players[1].StopTime();
			Players[0].StopTime();
			if (Team == TeamChess.red)
			{
				RoomInfoIG.ISTEAMREDPLAY = true;
				Players[0].Timedown();
			}
			else
			{
				RoomInfoIG.ISTEAMREDPLAY = false;
				Players[1].Timedown();
			}
		}

		public IEnumerator CB_ShowChieu(bool _isTeamred)
		{
			PlayChieu();
			if (!_isTeamred)
			{
				iTween.PunchScale(board.mNode[board.listChessRed[0].y, board.listChessRed[0].x].gameObject, iTween.Hash("x", 1.5f, "time", 0.3f));
				Players[1].objChieu.SetActive(true);
				yield return new WaitForSeconds(2f);
				Players[1].objChieu.SetActive(false);
			}
			else
			{
				iTween.PunchScale(board.mNode[board.listChessBlue[0].y, board.listChessBlue[0].x].gameObject, iTween.Hash("x", 1.5f, "time", 0.3f));
				Players[0].objChieu.SetActive(true);
				yield return new WaitForSeconds(2f);
				Players[0].objChieu.SetActive(false);
			}
		}


		public void CB_SendChat(string _ID, string _Name, int _Oder, string _Chat)
		{
			if (_Chat != "")
			{
				if (!RoomInfoIG.IsTournament)
				{
					ShowchatX(_Oder, _Name, _Chat);
					CancelInvoke("TimeToOff");
					ChatLine.GetComponent<TweenAlpha>().value = 1;
					ChatLine.text = _Name + " : " + _Chat;
					Invoke("TimeToOff", 4);
				}
				else
				{//NEU TRONG GIAI DAU
					if (myOrder == -1)
					{//KHAN GIA THAY ALL
						ShowchatX(_Oder, _Name, _Chat);
					}
					if (_Oder == 0 || _Oder == 1)
					{
						if (myOrder == 0 || myOrder == 1)
							ShowchatX(_Oder, _Name, _Chat);
					}
				}
			}
			else
			{
				StartCoroutine(NGUIMessage.instance.CALLALERT("Nhập đoạn chat "));
			}
		}

		void CallOpenNap()
		{
			//NGUIMessage.instance.OpenNapthePanel();
			NGUIMessage.instance.onClickCancelBigMessage();
		}



		//--------------------------------------------------------------------------------------------------------
		#endregion

		void OnStartStep4()
		{
			board.NewGame(RoomSelect.TruyenThong);

			if (myOrder == 1)
			{
				board.transform.localRotation = Quaternion.Euler(0, 0, 180);
				for (int i = 0; i < 10; i++)
					for (int j = 0; j < 9; j++)
					{
						board.mNode[i, j].transform.localRotation = Quaternion.Euler(0, 0, 180);
					}
			}

		}

		IEnumerator OnStartStep4CoLoan()
		{
			//MainMenu.GameName = RoomSelect.CoLoan;
			board.NewGame(RoomSelect.CoLoan);
			yield return new WaitForSeconds(1f);

			for (int i = 0; i < board.listChessRed.Count; i++)
			{
				RoomInfoIG.GetComponent<PhotonView>().RPC("OnStartStep4ReceiveCoLoan", PhotonTargets.Others,
					board.listChessRed[i].x, board.listChessRed[i].y,
					board.mNode[board.listChessRed[i].y, board.listChessRed[i].x].chess.show,
					board.mNode[board.listChessRed[i].y, board.listChessRed[i].x].chess.typeChess,
					board.mNode[board.listChessRed[i].y, board.listChessRed[i].x].chess.teamRed,
					board.mNode[board.listChessRed[i].y, board.listChessRed[i].x].chess.teamRedShow,
					board.mNode[board.listChessRed[i].y, board.listChessRed[i].x].chess.typeChessShow);
			}
			for (int i = 0; i < board.listChessBlue.Count; i++)
			{
				RoomInfoIG.GetComponent<PhotonView>().RPC("OnStartStep4ReceiveCoLoan", PhotonTargets.Others,
					board.listChessBlue[i].x, board.listChessBlue[i].y,
					board.mNode[board.listChessBlue[i].y, board.listChessBlue[i].x].chess.show,
					board.mNode[board.listChessBlue[i].y, board.listChessBlue[i].x].chess.typeChess,
					board.mNode[board.listChessBlue[i].y, board.listChessBlue[i].x].chess.teamRed,
					board.mNode[board.listChessBlue[i].y, board.listChessBlue[i].x].chess.teamRedShow,
					board.mNode[board.listChessBlue[i].y, board.listChessBlue[i].x].chess.typeChessShow);
			}
		}

		//=================================SHOW INFO ================

		public void UpdateMyInfo(int _Oder, string _ID, string _Name, string _Coin, byte[] _Ava, int _TimeLeft, string _Elo, string _Level, string _TotalMatch, string _WinMatch)
		{
			RoomInfoIG.GetComponent<PhotonView>().RPC("UpdateInfo", PhotonTargets.All, _Oder, _ID, _Name, _Coin, _Ava, _TimeLeft, _Elo, _Level, _TotalMatch, _WinMatch);
		}

		public void CurrentPlayer(bool _isJoin, string _ID)
		{//MASTER ONLY
			RoomInfoIG.GetComponent<PhotonView>().RPC("ShowCurrentPlayer", PhotonTargets.All, _isJoin, _ID);
			if (_isJoin == false)
			{//THOAT GAME
				for (int i = 0; i < RoomInfoIG.ListID.Count; i++)
				{
					if (RoomInfoIG.ListID[i] == _ID)
					{//CO TRONG LIST
						RoomInfoIG.ListID.RemoveAt(i);
					}
				}
			}
		}

		// =========================== ON PLAYER LEAVE ===========================================


		// ================= RECIEVE CHESS ====================
		public IEnumerator AddChessIfStarted(string _ID)
		{
			yield return new WaitForSeconds(2);
			if (PhotonNetwork.isMasterClient && GameStarted)
			{
				GameObject[] AllNode = GameObject.FindGameObjectsWithTag("Node");
				//SHOW QUAN CO
				for (int i = 0; i < board.listChessRed.Count; i++)
				{
					RoomInfoIG.GetComponent<PhotonView>().RPC("RecieveChess", PhotonTargets.All, _ID,
						board.listChessRed[i].x, board.listChessRed[i].y,
						board.mNode[board.listChessRed[i].y, board.listChessRed[i].x].isChess,
						board.mNode[board.listChessRed[i].y, board.listChessRed[i].x].chess.show,
						board.mNode[board.listChessRed[i].y, board.listChessRed[i].x].chess.typeChess,
						board.mNode[board.listChessRed[i].y, board.listChessRed[i].x].chess.teamRed,
						board.mNode[board.listChessRed[i].y, board.listChessRed[i].x].chess.typeChessShow);
				}
				for (int i = 0; i < board.listChessBlue.Count; i++)
				{
					RoomInfoIG.GetComponent<PhotonView>().RPC("RecieveChess", PhotonTargets.All, _ID,
						board.listChessBlue[i].x, board.listChessBlue[i].y,
						board.mNode[board.listChessBlue[i].y, board.listChessBlue[i].x].isChess,
						board.mNode[board.listChessBlue[i].y, board.listChessBlue[i].x].chess.show,
						board.mNode[board.listChessBlue[i].y, board.listChessBlue[i].x].chess.typeChess,
						board.mNode[board.listChessBlue[i].y, board.listChessBlue[i].x].chess.teamRed,
						board.mNode[board.listChessBlue[i].y, board.listChessBlue[i].x].chess.typeChessShow);
				}
				//SEND TOA DO DIEM QUAN CO
				for (int i = 0; i < Players[0].EatedChess.Count; i++)
				{
					RoomInfoIG.GetComponent<PhotonView>().RPC("SendEated", PhotonTargets.All, _ID, 1, Players[0].EatedChess[i], Players[0].EatedChessBool[i], Players[0].EatedChessRedTeam[i]);
				}

				for (int i = 0; i < Players[1].EatedChess.Count; i++)
				{
					RoomInfoIG.GetComponent<PhotonView>().RPC("SendEated", PhotonTargets.All, _ID, 2, Players[1].EatedChess[i], Players[1].EatedChessBool[i], Players[1].EatedChessRedTeam[i]);
				}
			}
		}


		public IEnumerator ADDSTEPFOROTHER(string _ID)
		{
			yield return new WaitForSeconds(2.5f);
			if (PhotonNetwork.isMasterClient && GameStarted)
			{
				//RoomInfoIG.GetComponent<PhotonView>().RPC ("SendCurrentIndex",PhotonTargets.All,_ID,RoomInfoIG.indexMove);
				for (int i = 0; i < RoomInfoIG.listPosXStart.Count; i++)
				{
					RoomInfoIG.GetComponent<PhotonView>().RPC("ClientAddStep", PhotonTargets.All,
						_ID, RoomInfoIG.indexMove,
						RoomInfoIG.listPosXStart[i],
						RoomInfoIG.listPosYStart[i],
						RoomInfoIG.listPosXEnd[i],
						RoomInfoIG.listPosYEnd[i],

						RoomInfoIG.listIsChessStart[i],
						RoomInfoIG.listIsShowStart[i],
						RoomInfoIG.listTeamStart[i],
						RoomInfoIG.listTeamShowStart[i],
						RoomInfoIG.listTypeStart[i],
						RoomInfoIG.listTypeStartShow[i],

						RoomInfoIG.listIsChessEnd[i],
						RoomInfoIG.listIsShowEnd[i],
						RoomInfoIG.listTeamEnd[i],
						RoomInfoIG.listTeamShowEnd[i],
						RoomInfoIG.listTypeEnd[i],
						RoomInfoIG.listTypeEndShow[i]
					);
				}
			}
		}

		public void ADDTISO(string _ID)
		{
			if (PhotonNetwork.isMasterClient)
				RoomInfoIG.GetComponent<PhotonView>().RPC("ADDCURRENTSCORE", PhotonTargets.All, _ID, RoomInfoIG.IDPlayerRed, RoomInfoIG.IDPlayerBlue, RoomInfoIG.CurrentScoreRed, RoomInfoIG.CurrentScoreBlue, RoomInfoIG.TeamStartTurn);
		}

		//====================================================

		// ===================== MOVE CO =====================
		public void BotCallMove(int _Oder, int _index, int nodeStartX, int nodeStartY, int _nodeEndX, int _nodeEndY, bool _isBotMove,
			bool _isChessStart, bool _isStartShow, bool _isTeamRedStart, bool _isTeamRedShowStart, TypeChess _typeStart, TypeChess _typeStartShow,
			bool _isChessEnd, bool _isEndShow, bool _isTeamRedEnd, bool _isTeamRedShowEnd, TypeChess _typeEnd, TypeChess _typeEndShow)
		{
			StartCoroutine(CallMove(_Oder, _index, nodeStartX, nodeStartY, _nodeEndX, _nodeEndY, _isBotMove,
				_isChessStart, _isStartShow, _isTeamRedStart, _isTeamRedShowStart, _typeStart, _typeStartShow,
				_isChessEnd, _isEndShow, _isTeamRedEnd, _isTeamRedShowEnd, _typeEnd, _typeEndShow));
		}
		public IEnumerator CallMove(int _Oder, int _index, int nodeStartX, int nodeStartY, int _nodeEndX, int _nodeEndY, bool _isBotMove,
			bool _isChessStart, bool _isStartShow, bool _isTeamRedStart, bool _isTeamRedShowStart, TypeChess _typeStart, TypeChess _typeStartShow,
			bool _isChessEnd, bool _isEndShow, bool _isTeamRedEnd, bool _isTeamRedShowEnd, TypeChess _typeEnd, TypeChess _typeEndShow)
		{
			//_isMyTurn = false;
			RoomInfoIG.GetComponent<PhotonView>().RPC("SaveMove", PhotonTargets.All, _Oder, true, _index, nodeStartX, nodeStartY, _nodeEndX, _nodeEndY, RoomInfoIG.timeAutoPlay, _isBotMove,
				_isChessStart, _isStartShow, _isTeamRedStart, _isTeamRedShowStart, _typeStart, _typeStartShow,
				_isChessEnd, _isEndShow, _isTeamRedEnd, _isTeamRedShowEnd, _typeEnd, _typeEndShow);
			//STOP TURN LOCAL

			if (_isBotMove)
			{
				yield return new WaitForSeconds(RoomInfoIG.timeAutoPlay + RoomInfoIG.timeAutoPlay / 2);
				_isMyTurn = true;
			}

		}

		public IEnumerator MoveGameObject(int _Oder, bool _OnNext, int _index, bool _isRealTime, int nodeStartX, int nodeStartY, int nodeEndX, int nodeEndY, float _TimeTween,
			bool _isChessStart, bool _isStartShow, bool _isTeamRedStart, bool _isTeamRedShowStart, TypeChess _typeStart, TypeChess _typeStartShow,
			bool _isChessEnd, bool _isEndShow, bool _isTeamRedEnd, bool _isTeamRedShowEnd, TypeChess _typeEnd, TypeChess _typeEndShow)
		{
			if (_isRealTime)
				RoomInfoIG.indexMove = _index;
			else
				RoomInfoIG.MovingChess = true;
			//SOUND MOVE
			PlayMove();
			if (RoomInfoIG.RoomType != RoomSelect.CoLoan)
			{
				if (_isTeamRedStart)
				{
					for (int i = 0; i < board.listChessRed.Count; i++)
					{
						if (board.listChessRed[i].x == nodeStartX && board.listChessRed[i].y == nodeStartY)
						{
							board.listChessRed[i] = new NodeBoard(nodeEndY, nodeEndX);
							break;
						}
					}
				}
				else
				{
					for (int i = 0; i < Board.instance.listChessBlue.Count; i++)
					{
						if (board.listChessBlue[i].x == nodeStartX && board.listChessBlue[i].y == nodeStartY)
						{
							board.listChessBlue[i] = new NodeBoard(nodeEndY, nodeEndX);
							break;
						}
					}
				}
			}
			else
			{//CO LOAN
				if (_isTeamRedStart)
				{
					if (_isTeamRedShowStart)
					{
						bool alreadyhave = false;
						for (int i = 0; i < board.listChessRed.Count; i++)
						{
							if (board.listChessRed[i].x == nodeStartX && board.listChessRed[i].y == nodeStartY)
							{
								alreadyhave = true;
								board.listChessRed[i] = new NodeBoard(nodeEndY, nodeEndX);
							}
						}
						if (!alreadyhave)
							board.listChessRed.Add(new NodeBoard(nodeEndY, nodeEndX));
					}
					else
					{//LAT CO XANH
						for (int i = 0; i < board.listChessRed.Count; i++)
						{
							if (board.listChessRed[i].x == nodeStartX && board.listChessRed[i].y == nodeStartY)
							{
								board.listChessRed.RemoveAt(i);
							}
						}
						bool alreadyhave = false;
						for (int i = 0; i < board.listChessBlue.Count; i++)
						{
							if (board.listChessBlue[i].x == nodeStartX && board.listChessBlue[i].y == nodeStartY)
							{
								alreadyhave = true;
								board.listChessBlue[i] = new NodeBoard(nodeEndY, nodeEndX);
							}
						}
						if (!alreadyhave)
							board.listChessBlue.Add(new NodeBoard(nodeEndY, nodeEndX));
					}
				}
				else
				{//ON CO XANH
					if (!_isTeamRedShowStart)
					{
						bool alreadyhave = false;
						for (int i = 0; i < board.listChessBlue.Count; i++)
						{
							if (board.listChessBlue[i].x == nodeStartX && board.listChessBlue[i].y == nodeStartY)
							{
								alreadyhave = true;
								board.listChessBlue[i] = new NodeBoard(nodeEndY, nodeEndX);
							}
						}
						if (!alreadyhave)
							board.listChessBlue.Add(new NodeBoard(nodeEndY, nodeEndX));
					}
					else
					{//LAT CO DO
						for (int i = 0; i < board.listChessBlue.Count; i++)
						{
							if (board.listChessBlue[i].x == nodeStartX && board.listChessBlue[i].y == nodeStartY)
							{
								board.listChessBlue.RemoveAt(i);
							}
						}
						bool alreadyhave = false;
						for (int i = 0; i < board.listChessRed.Count; i++)
						{
							if (board.listChessRed[i].x == nodeStartX && board.listChessRed[i].y == nodeStartY)
							{
								alreadyhave = true;
								board.listChessRed[i] = new NodeBoard(nodeEndY, nodeEndX);
							}
						}
						if (!alreadyhave)
							board.listChessRed.Add(new NodeBoard(nodeEndY, nodeEndX));

					}
				}
			}

			//LAY HINH ANH , XONG XU LI TEAM ELEMENT
			Vector2 vec = board.mNode[nodeStartY, nodeStartX].transform.localPosition;
			board.RemoveChess(_Oder, _OnNext, _index, _isRealTime, _typeEndShow, _isEndShow, _isChessEnd, _isTeamRedEnd, _isTeamRedShowEnd, new NodeBoard(nodeEndY, nodeEndX), new NodeBoard(nodeStartY, nodeStartX));//NEU VI TRI VUA DI CO QUAN DICH THI CLEAR 
			board.ClearNode(true);//CLEAN HUONG DAN NUOC DI
			objEffect.objEffectSelectChess.SetActive(false);
			//board.mNode[nodeEndY,nodeEndX].MoveChess(false,board.mNode[nodeEndY,nodeEndX].chess.teamRed,new NodeBoard(nodeEndY,nodeEndX),_TimeTween);
			board.mNode[nodeStartY, nodeStartX].MoveChess(_OnNext, board.mNode[nodeEndY, nodeEndX].chess.teamRed, new NodeBoard(nodeEndY, nodeEndX), new NodeBoard(nodeStartY, nodeStartX), _TimeTween);
			yield return new WaitForSeconds(_TimeTween + _TimeTween / 10);
			board.mNode[nodeEndY, nodeEndX].objOldMove.SetActive(true);
			board.mNode[nodeStartY, nodeStartX].ObjChessMove.SetActive(true);
			//AFTER MOVE CHESS , DRAW CHESS THEN REPOSITION
			//---------DRAWCHESS-------------
			board.mNode[nodeStartY, nodeStartX].transform.localPosition = vec;
			board.mNode[nodeEndY, nodeEndX].SetChess(new Chess(_typeStart, _isTeamRedStart, _isStartShow, _isTeamRedShowStart, _typeStartShow), new NodeBoard(nodeEndY, nodeEndX));
			if (board.mNode[nodeEndY, nodeEndX].chess.show == false && _OnNext == true)
			{
				//board.mNode [nodeEndY, nodeEndX].chess.show = true;
				if (RoomInfoIG.RoomType == RoomSelect.CoLoan)
					board.mNode[nodeEndY, nodeEndX].chess.ShowChessCL();
				else
					board.mNode[nodeEndY, nodeEndX].chess.ShowChess();
				//iTween.PunchScale (board.mNode[nodeEndY, nodeEndX].objChess,iTween.Hash("x",1.1f,"y",1.1f,"time",_TimeTween/2));
			}

			board.mNode[nodeStartY, nodeStartX].SetChess(new Chess());
			if (_OnNext == false && _isChessEnd == true)
			{
				board.mNode[nodeStartY, nodeStartX].SetChess(new Chess(_typeEnd, _isTeamRedEnd, _isEndShow, _isTeamRedShowEnd, _typeEndShow), new NodeBoard(nodeEndY, nodeEndX));
			}
			board.mNode[nodeEndY, nodeEndX].DrawChess();//NEW POSITION
			board.mNode[nodeStartY, nodeStartX].DrawChess();//RESET TO DEFAULT NODE
															//-------REPOSITION-----------
			yield return new WaitForSeconds(_TimeTween / 10);
			board.mNode[nodeStartY, nodeStartX].transform.localPosition = new Vector2(board.PosChess(new NodeBoard(nodeStartY, nodeStartX)).x,
				board.PosChess(new NodeBoard(nodeStartY, nodeStartX)).y);
			board.mNode[nodeEndY, nodeEndX].transform.localPosition = new Vector2(board.PosChess(new NodeBoard(nodeEndY, nodeEndX)).x,
				board.PosChess(new NodeBoard(nodeEndY, nodeEndX)).y);

			if (_isRealTime == false)
				RoomInfoIG.MovingChess = false;

			//CHECK CHIEU && BAT BIEN
			if (myOrder == _Oder && _isRealTime)
			{
				board.CheckChieu(CheckMyTeam(), nodeStartX, nodeStartY, nodeEndX, nodeEndY);
				board.CheckBatQuan(GameController.instance.CheckMyTeam(), board.listChessRed, board.listChessBlue, board.mNode);
				board.CheckBatBien(new NodeBoard(nodeEndY, nodeEndX), board.listChessRed, board.listChessBlue, board.mNode);
				CheckAlert();
			}
			if (myOrder != _Oder)
			{
				board.CheckQuanDiChuyen(nodeStartX, nodeStartY, nodeEndX, nodeEndY);
			}
			//if(myOrder != _Oder)
			//if(RoomInfoIG.RoomID == "1" || RoomInfoIG.RoomID == "11" || RoomInfoIG.RoomID == "21")
			//board.CheckChieu (!CheckMyTeam (), nodeStartX, nodeStartY, nodeEndX, nodeEndY);
		}
		public void callTurn(TeamChess Team)
		{
			RoomInfoIG.GetComponent<PhotonView>().RPC("StartTurn", PhotonTargets.All, Team);
		}

		public void DangChieu(bool isTeamRed)
		{
			RoomInfoIG.GetComponent<PhotonView>().RPC("ShowChieu", PhotonTargets.All, isTeamRed);
		}

		public bool isMyTurn()
		{
			return true;
		}
		public bool GetTurn()
		{
			return true;
		}

		//CLAIM THANG THUA
		// DAC BIET LA DAU HANG , SAI LUAT 
		public void ClaimWin(int _Oder, bool _isSpecial, string _SpecialReason, float _HeSoTheChieu, string _StatusTheChieu, float _HeSoThamChieu, string _StatusThamChieu)
		{
			RoomInfoIG.GetComponent<PhotonView>().RPC("Winz", PhotonTargets.MasterClient, _Oder, _isSpecial, _SpecialReason, _HeSoTheChieu, _StatusTheChieu, _HeSoThamChieu, _StatusThamChieu);
		}
		public void CB_Winz(int _Oder, bool _isSpecial, string _SpecialReason, float _HeSoTheChieu, string _StatusTheChieu, float _HeSoThamChieu, string _StatusThamChieu)//ONLY MASTER DO 
		{
			if (_isSpecial)
			{
				float tien = RoomInfoIG.RoomCoin * 10;
				if (_Oder == 0)
				{
					StartCoroutine(ReqAPI.instance.StartOrEndGame(RoomInfoIG.RoomID, Players[0].PlayerID, Players[1].PlayerID, tien.ToString(), "0", false, RoomInfoIG.RoomID, UserConfig.Data.UserID, RoomInfoIG.IsTournament, RoomInfoIG.MatchID, UserConfig.Data.UserKey, (isSucc, NewSecurity) =>
					{
						if (isSucc)
						{//GUI THONG TIN 2 THANG NGUOI CHOI THEN END
							UserConfig.Data.UserKey = NewSecurity;
						}
					}));
				}
				if (_Oder == 1)
				{
					StartCoroutine(ReqAPI.instance.StartOrEndGame(RoomInfoIG.RoomID, Players[1].PlayerID, Players[0].PlayerID, tien.ToString(), "0", false, RoomInfoIG.RoomID, UserConfig.Data.UserID, RoomInfoIG.IsTournament, RoomInfoIG.MatchID, UserConfig.Data.UserKey, (isSucc, NewSecurity) =>
					{
						if (isSucc)
						{//GUI THONG TIN 2 THANG NGUOI CHOI THEN END
							UserConfig.Data.UserKey = NewSecurity;
						}
					}));
				}
			}
			else
			{
				float TienAnQuanRed = Players[0].GetTienAnQuan();
				float TienAnQuanBlue = Players[1].GetTienAnQuan();
				float heso = _HeSoThamChieu + _HeSoTheChieu;
				if (heso > 6)//TINH LUON TIEN BAN
					heso = 6;

				if (_Oder == 0)
				{
					float tien = 0;
					tien += (float)RoomInfoIG.RoomCoin * heso;
					if (TienAnQuanRed > TienAnQuanBlue)
					{
						tien += TienAnQuanRed - TienAnQuanBlue;
					}
					StartCoroutine(ReqAPI.instance.StartOrEndGame(RoomInfoIG.RoomID, Players[0].PlayerID, Players[1].PlayerID, tien.ToString(), "0", false, RoomInfoIG.RoomID, UserConfig.Data.UserID, RoomInfoIG.IsTournament, RoomInfoIG.MatchID, UserConfig.Data.UserKey, (isSucc, NewSecurity) =>
					{
						if (isSucc)
						{//GUI THONG TIN 2 THANG NGUOI CHOI THEN END
							UserConfig.Data.UserKey = NewSecurity;
						}
					}));
				}
				if (_Oder == 1)
				{
					float tien = 0;
					tien += (float)RoomInfoIG.RoomCoin * heso;
					if (TienAnQuanBlue > TienAnQuanRed)
					{
						tien += TienAnQuanBlue - TienAnQuanRed;
					}
					StartCoroutine(ReqAPI.instance.StartOrEndGame(RoomInfoIG.RoomID, Players[1].PlayerID, Players[0].PlayerID, tien.ToString(), "0", false, RoomInfoIG.RoomID, UserConfig.Data.UserID, RoomInfoIG.IsTournament, RoomInfoIG.MatchID, UserConfig.Data.UserKey, (isSucc, NewSecurity) =>
					{
						if (isSucc)
						{//GUI THONG TIN 2 THANG NGUOI CHOI THEN END
							UserConfig.Data.UserKey = NewSecurity;
						}
					}));
				}
			}

			RoomInfoIG.GetComponent<PhotonView>().RPC("Winner", PhotonTargets.All, _Oder, _isSpecial, _SpecialReason, _HeSoTheChieu, _StatusTheChieu, _HeSoThamChieu, _StatusThamChieu);//SEND HIEN KET QUA
		}

		public IEnumerator CB_Winner(int _Oder, bool _isSpecial, string _SpecialReason, float _HeSoTheChieu, string _StatusTheChieu, float _HeSoThamChieu, string _StatusThamChieu)
		{//SHOW KET QUA HERE
			yield return new WaitForSeconds(0.1f);
			Players[0].StopTime();
			Players[1].StopTime();
			CancelInvoke("CountTimeTotal");
			TurnStatus.color = Color.red;
			TurnStatus.text = "Ván đấu đã kết thúc";
			NGUIMessage.instance.CALLLOADING(false);
			board.InitBoard();//CLEAN HUONG DAN NUOC DI
			_isMyTurn = false;
			GameStarted = false;
			//board.InitBoard();
			//HIEU UNG TE LE
			float TienAnQuanRed = Players[0].GetTienAnQuan();
			float TienAnQuanBlue = Players[1].GetTienAnQuan();
			float tienTong = 0;
			float tienAnQuan = 0;
			float tienchieu = 0;
			float heso = _HeSoTheChieu + _HeSoThamChieu + 1;
			if (heso > 6) heso = 6;
			tienchieu = heso * RoomInfoIG.RoomCoin;
			StartCoroutine(UltilityGame.instance.TweenPanel(true, objEffect.objResult.gameObject));
			if (myOrder == _Oder)
			{
				PlayWin();
				if (myOrder == 0)//RED WIN
				{
					if (_isSpecial)
					{
						objEffect.objResult.TienTong = (float)(RoomInfoIG.RoomCoin * 10) * 98 / 100;
						objEffect.objResult.Status.text = "Chúc mừng bạn  đã thắng ván đấu " + _SpecialReason;
					}
					else
					{
						if (TienAnQuanRed > TienAnQuanBlue)
							tienAnQuan = TienAnQuanRed - TienAnQuanBlue;
						tienTong = tienAnQuan + tienchieu;
						objEffect.objResult.TienTong = tienTong * 98 / 100;
						objEffect.objResult.Status.text = "Chúc mừng bạn  đã thắng ván đấu ";
					}
				}
				if (myOrder == 1)
				{
					if (_isSpecial)
					{
						objEffect.objResult.TienTong = (float)(RoomInfoIG.RoomCoin * 10) * 98 / 100;
						objEffect.objResult.Status.text = "Chúc mừng bạn  đã thắng ván đấu " + _SpecialReason;
					}
					else
					{
						if (TienAnQuanBlue > TienAnQuanRed)
							tienAnQuan = TienAnQuanBlue - TienAnQuanRed;
						tienTong = tienAnQuan + tienchieu;
						objEffect.objResult.TienTong = tienTong * 98 / 100;
						objEffect.objResult.Status.text = "Chúc mừng bạn  đã thắng ván đấu ";
					}
				}

				objEffect.objResult.TienChieu = tienchieu;
				objEffect.objResult.TienAnQuan = tienAnQuan;
				objEffect.objResult.lbTienChieu.text = _StatusTheChieu + " " + _StatusThamChieu;
			}
			else
			{
				if (myOrder != -1)
				{
					PlayLose();
					if (myOrder == 0)
					{
						if (_isSpecial)
						{
							objEffect.objResult.TienTong = -((float)RoomInfoIG.RoomCoin * 10);
							objEffect.objResult.Status.text = "Bạn  đã thua ván đấu !!" + _SpecialReason;
						}
						else
						{
							if (TienAnQuanRed < TienAnQuanBlue)
								tienAnQuan = TienAnQuanBlue - TienAnQuanRed;//TIEN THUA QUAN
							tienTong = -(tienAnQuan + tienchieu);
							objEffect.objResult.TienTong = tienTong;
							objEffect.objResult.Status.text = "Bạn  đã thua ván đấu !!";
						}
					}
					if (myOrder == 1)
					{
						if (_isSpecial)
						{
							objEffect.objResult.TienTong = -((float)RoomInfoIG.RoomCoin * 10);
							objEffect.objResult.Status.text = "Bạn  đã thua ván đấu !!" + _SpecialReason;
						}
						else
						{
							if (TienAnQuanRed > TienAnQuanBlue)
								tienAnQuan = TienAnQuanRed - TienAnQuanBlue;//TIEN THUA QUAN
							tienTong = -(tienAnQuan + tienchieu);
							objEffect.objResult.TienTong = tienTong;
							objEffect.objResult.Status.text = "Bạn  đã thua ván đấu !!";
						}
					}
					objEffect.objResult.TienAnQuan = tienAnQuan;
					objEffect.objResult.TienChieu = tienchieu;
					objEffect.objResult.lbTienChieu.text = _StatusTheChieu + " & " + _StatusThamChieu;
				}
			}

			yield return new WaitForSeconds(4);
			UpdateScoreWhenFinish();
			ResetGame();
			StartCoroutine(UltilityGame.instance.TweenPanel(false, objEffect.objResult.gameObject));
		}


		public void UpdateScoreWhenFinish()
		{
			if (Players[0].UI.activeSelf)
				StartCoroutine(BANCA.ReqAPI.instance.GetUserInfo(Players[0].PlayerID, (aBool, DATA) =>
				{
					if (aBool)
					{
						Players[0].PlayerCoin = DATA.UserCoins;
						Players[0].LbCoins.text = DATA.UserCoins.ToString("C0");
					}
				}));
			if (Players[1].UI.activeSelf)
				StartCoroutine(BANCA.ReqAPI.instance.GetUserInfo(Players[1].PlayerID, (aBool, DATA) =>
				{
					if (aBool)
					{
						Players[1].PlayerCoin = DATA.UserCoins;
						Players[1].LbCoins.text = DATA.UserCoins.ToString("C0");
					}
				}));
		}

		public IEnumerator ClaimDraw(string _Reason)
		{
			yield return new WaitForSeconds(0.25f);
			RoomInfoIG.GetComponent<PhotonView>().RPC("CALLMASTERDRAW", PhotonTargets.MasterClient, _Reason);
		}
		public void CB_CALLMASTERDRAW(string _Reason)
		{
			StartCoroutine(ReqAPI.Instance.StartOrEndGame(RoomInfoIG.RoomID, Players[0].PlayerID, Players[1].PlayerID, RoomInfoIG.RoomCoin.ToString(), "0", false, RoomInfoIG.RoomID, UserConfig.Data.UserID,
				false, "CoUp", UserConfig.Data.UserKey, (isSucc, NewSecurity) =>
			{
				if (isSucc)
				{//START GAME
					UserConfig.Data.UserKey = NewSecurity;
					RoomInfoIG.GetComponent<PhotonView>().RPC("DRAWGAME", PhotonTargets.All, _Reason);
				}
			}));
		}
		public IEnumerator CB_DRAWGAME(string _Reason)
		{
			CancelInvoke("CountTimeTotal");
			UpdateScoreWhenFinish();//VAN TRU TIEN BAN
			Players[0].StopTime();
			Players[1].StopTime();
			RoomInfoIG.CheckTiso(RoomInfoIG.IDPlayerRed, RoomInfoIG.IDPlayerBlue, 0.5f, 0.5f);
			if (myOrder != -1)
			{
				StartCoroutine(NGUIMessage.instance.CALLALERT("Bạn vừa hòa đối thủ vì " + _Reason));
			}
			NGUIMessage.instance.CALLLOADING(false);
			board.InitBoard();
			//board.ClearNode(true);//CLEAN HUONG DAN NUOC DI
			_isMyTurn = false;
			GameStarted = false;
			TurnStatus.color = Color.red;
			TurnStatus.text = "Ván đấu đã kết thúc";
			yield return new WaitForSeconds(4f);
			ResetGame();
			TurnStatus.color = Color.yellow;
			TurnStatus.text = "Ván đấu chuẩn bị bắt đầu";
		}
		//==================DRAW SHOW ==================
		public void ShowDraw(int _Oder)
		{
			RoomInfoIG.GetComponent<PhotonView>().RPC("OpenDraw", PhotonTargets.All, _Oder);
		}

		//=============================================

		void ResetGame()
		{
			MatchTime = 0;
			canPlay = false;
			_isMyTurn = false;
			teamPlayer = TeamChess.none;
			NumDraw = 0;
			numChieu = 0;
			numChieuMax = 0;
			numBatQuan = 0;
			objEffect.PlayGround.SetActive(false);
			objEffect.objEffectSelectChess.SetActive(false);

			for (int i = 0; i < Players.Length; i++)
			{
				Players[i].EatedChess.Clear();
				Players[i].EatedChessBool.Clear();
				Players[i].EatedChessRedTeam.Clear();
				Players[i].DeleteAllCloneChess();
				Players[i].WrongStep.color = Color.white;
				Players[i].LbTimeLeft.text = "";
				if (Players[i].DelayConnect.isActiveAndEnabled)
				{
					Players[i].UI.SetActive(false);
					Players[i].RESET();
				}
			}

			RoomInfoIG.Stepcount.text = "";
			//RESETLAI VI TRI
			RoomInfoIG.ReviewButton.SetActive(false);
			checkRematchHere();
		}
		// check REMATCH HERE
		public UIToggle CheckRematch;
		public GameObject objRematch;
		public int CountTimeRematch = 0;
		public UILabel NoticeRematch;
		int iCountTimeRematch = 10;
		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
			SceneManager.LoadScene("NewRoomSelect");
		}
		public void OnToggleRematchChange()
		{
			RoomInfoIG.GetComponent<PhotonView>().RPC("CheckRematchToggle", PhotonTargets.All, UserConfig.Data.UserID, CheckRematch.value);
		}

		public void SetTaiDau(string _ID, bool setBool)
		{
			for (int i = 0; i < Players.Length; i++)
			{
				if (Players[i].PlayerID == _ID)
					Players[i].isCheckRematch = setBool;
			}
		}
		void checkRematchHere()//HIEN THONG BAO
		{
			if (MyPlayer.PlayerCoin >= (float)RoomInfoIG.RoomCoin)
			{
				if (!MyPlayer.isCheckRematch)
				{
					iCountTimeRematch = 10;
					InvokeRepeating("Count_Time_Rematch", 0.1f, 1);
					StartCoroutine(UltilityGame.instance.TweenPanel(true, objRematch));
				}
				else
				{
					TurnStatus.text = "Chờ đối thủ thao tác ";
				}
				InvokeRepeating("CheckAutoStartGame", 2, 1);
			}
			else
			{
				LeaveRoom();
			}
		}
		void Count_Time_Rematch()
		{
			iCountTimeRematch--;
			NoticeRematch.text = "Thời gian tái đấu sễ bắt đầu sau " + iCountTimeRematch.ToString() + "s nữa !.\n Bấm Đồng Ý để tiếp tục hoặc Thoát để thoát khỏi phòng .";
			if (iCountTimeRematch == 0)
			{
				OnOKRematch();
			}
		}
		void CheckAutoStartGame()
		{
			if (Players[0].isCheckRematch && Players[1].isCheckRematch && PhotonNetwork.playerList.Length == 2)//CON 2 NGUOI CHOI , 2 NG DEU SAN SANG
			{
				//MASTER START HERE
				if (PhotonNetwork.isMasterClient)
				{
					RoomInfoIG.GetComponent<PhotonView>().RPC("CreateBoard", PhotonTargets.All);
					RoomInfoIG.GetComponent<PhotonView>().RPC("CallStart", PhotonTargets.All);
				}
				CancelInvoke("CheckAutoStartGame");
			}

			if (PhotonNetwork.playerList.Length == 1)
			{
				StartCoroutine(NGUIMessage.instance.CALLALERT("Đối thủ đã rời phòng bạn "));
				if (MyPlayer.isCheckRematch)
				{
					CancelInvoke("CheckAutoStartGame");
					LoadCurrentChannnel();
				}
			}
		}
		void OnOKRematch()
		{
			CancelInvoke("Count_Time_Rematch");
			//CheckRematch.value = true;
			CheckRematch.isChecked = true;
			RoomInfoIG.GetComponent<PhotonView>().RPC("CheckRematchToggle", PhotonTargets.All, UserConfig.Data.UserID, true);
			StartCoroutine(UltilityGame.instance.TweenPanel(false, objRematch));
		}
		void OnHuyRematch()
		{
			CancelInvoke("Count_Time_Rematch");
			LeaveRoom();
		}
		void LoadCurrentChannnel()
		{
			if (UserConfig.Data.UserCoins >= RoomSelect.instance.CURRENTCHANNEL.BetCoins * 10)
			{
				PhotonNetwork.LeaveRoom();
				InvokeRepeating("StartBackToCurrentRoom", 1, 1);
			}
			else
			{
				StartCoroutine(NGUIMessage.instance.CALLALERT("Bạn không đủ tiền để vào bàn , vui lòng chọn bàn phù hợp !"));
				SceneManager.LoadScene("NewRoomSelect");
			}
		}
		void StartBackToCurrentRoom()
		{
			if (PhotonNetwork.insideLobby)
			{
				CancelInvoke("StartBackToCurrentRoom");
				RoomSelect.instance.CurrentRoom = new NewRoomInfo()
				{
					RoomID = RoomSelect.instance.CURRENTCHANNEL.RoomID,
					BetCoins = RoomSelect.instance.CURRENTCHANNEL.BetCoins,
					TimeThink = RoomSelect.instance.CURRENTCHANNEL.TimeThink,
					TimeMax = RoomSelect.instance.CURRENTCHANNEL.TimeMax
				};

				ExitGames.Client.Photon.Hashtable customPropreties = new ExitGames.Client.Photon.Hashtable();
				customPropreties["ismatchmaking"] = true;
				customPropreties["roomid"] = RoomSelect.instance.CURRENTCHANNEL.RoomID;
				customPropreties["bet"] = RoomSelect.instance.CURRENTCHANNEL.BetCoins;
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
				PhotonNetwork.JoinOrCreateRoom(RoomSelect.instance.CURRENTCHANNEL.RoomID, roomOptions, TypedLobby.Default);
			}
		}
		#region PLAY SOUND 
		public void PlayCloseTime()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.SoundTime, Vector3.zero);
		}
		public void PlayEndTime()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.SoundEndTime, Vector3.zero);
		}
		public void PlayMove()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.SoundMove, Vector3.zero);
		}
		public void PlayStartGame()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.SoundStart, Vector3.zero);
		}
		public void PlayEmotion()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.SoundEat, Vector3.zero);
		}
		public void PlayWin()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.SoundWin, Vector3.zero);
		}
		public void PlayLose()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.SoundLose, Vector3.zero);
		}
		public void PlayChieu()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.SoundChieu, Vector3.zero);
		}
		public void PlayChieuHet()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.SoundChieuHet, Vector3.zero);
		}
		public void PlayChat()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.SoundChat, Vector3.zero);
		}
		public void PlayJoin()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.SoundJoin, Vector3.zero);
		}
		public void PlaySit()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.SoundSit, Vector3.zero);
		}
		public void PlayOut()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.SoundOut, Vector3.zero);
		}
		public void PlayChonQuan()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.ChonQuan, Vector3.zero);
		}
		public void PlaySaiQuan()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.SaiQuan, Vector3.zero);
		}
		public void PlayHasTurn()
		{
			AudioSource.PlayClipAtPoint(LoadPrefab.instance.SoundStartTurn, Vector3.zero);
		}
		#endregion
		#region CHAT
		//public List<string> ChatList = new List<string>();
		public UILabel ChatLine;
		public UIInput ChatInput;
		public UITextList TextListControl;
		public void OnClickSendChat()
		{
			if (ChatInput.value == "~cheat-admin-tch")
			{
				NGUIMessage.instance.CALLALERT("OPEN ADMIN MODE ");
				Adminzone.SetActive(true);
				RoomSelect.EnableAdmin = true;
			}
			else
			{
				char[] ChatText = ChatInput.value.ToCharArray();
				if (ChatText[0] == '~')
				{
					ChatInput.value = "Error Validate";
				}
				else
				{
					RoomInfoIG.GetComponent<PhotonView>().RPC("SendChat", PhotonTargets.All, UserConfig.Data.UserID, UserConfig.Data.UserName, myOrder, ChatInput.value);
					ChatInput.value = "";
				}
			}
		}

		void ShowchatX(int _Oder, string _Name, string _Chat)
		{
			if (_Oder == 0)
				TextListControl.Add("[FF4500FF]" + _Name + " : " + _Chat + "[-]");
			if (_Oder == 1)
				TextListControl.Add("[00B5FFFF]" + _Name + " : " + _Chat + "[-]");
			if (_Oder == -1)
				TextListControl.Add(_Name + " : " + _Chat);
		}
		void TimeToOff()
		{
			ChatLine.GetComponent<TweenAlpha>().PlayReverse();
		}
		//==========================================CHAT==========
		public void CreateEmotion(int _sprite)
		{
			RoomInfoIG.GetComponent<PhotonView>().RPC("EChat", PhotonTargets.All, myOrder, _sprite);
		}
		public void CreateQuickChat(string _Text)
		{
			RoomInfoIG.GetComponent<PhotonView>().RPC("QChat", PhotonTargets.All, myOrder, _Text);
		}

		#endregion

		public void ForceQuit(string _ID)
		{
			RoomInfoIG.GetComponent<PhotonView>().RPC("Kick", PhotonTargets.All, _ID);
		}

		public enum TeamChess
		{
			none,
			red,
			blue
		}
	}
}
