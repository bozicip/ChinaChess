using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
public class GameCoTuongManager : GameManager
{
    public bool isOnline = true;
    public bool isCoup = true;
    public bool isTest = true;
    public bool isBotMove = false;
    public bool isTurnRed = false;
    private TeamChess teamPlayer;
    public Effect objEffect;
    public SettingGame settingGame;
    public PlayerManager PlayerLeft, PlayerRight;
    public Board board;
    public int ruleGame;
    public MenuGameControler menuGame;

    
    /////////////////
    public int MyOrder
    {
        get
        {
            return myOrder;
        }
        set
        {
            myOrder = value;
            if (myOrder == 1)
            {
                board.transform.localRotation = Quaternion.Euler(0, 0, 180);
            }
            else
            {
                board.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if (myOrder == 0)
            {
                teamPlayer = TeamChess.red;
            }
            else if (myOrder == 1)
            {
                teamPlayer = TeamChess.blue;
            }
            objEffect.SetMenuGame(IsPlayGame, myOrder, GameKind);
        }
    }

    public bool IsPlayGame
    {
        get
        {
            if (gameState == GameState.Started)
                return true;
            else
                return false;
        }
        set
        {            
            gameState = GameState.Idle;
            if (value == true)
            {
                gameState = GameState.Started;
                objEffect.SetStateBtnReady(IsHost, false, false);
            }
            else
            {
                if (MyOrder == -1)
                {
                    objEffect.SetStateBtnReady(IsHost, false, false);
                }
                else
                {
                    objEffect.SetStateBtnReady(IsHost, false, true);
                }
                PlayerLeft.IsReady = false;
                PlayerRight.IsReady = false;
            }
            objEffect.SetMenuGame(gameState == GameState.Started, MyOrder, GameKind);
        }
    }

    public ViewerManager viewerManager;

    public bool IsHost
    {
        get { return isHost; }
        set
        {
            isHost = value;
            if (MyOrder != -1)
            {
                if (!IsTournament)
                {
                    PlayerLeft.IsHost = value;
                    PlayerRight.IsHost = !value;
                }
                else
                {
                    PlayerLeft.IsHost = false;
                    PlayerRight.IsHost = false;
                }
                objEffect.SetStateBtnReady(IsHost, false, !IsPlayGame);
            }
        }
    }

    public RecordManager objReplay;

    void Awake()
    {
        if (SmartFoxConnection.smartFox == null)
        {
            SceneManager.LoadScene(Scenes.OPTION_LOGIN);
        }
        Instance = this;
        if (GameConfig.isReconnect)
        {
            CreateSFS();
            InitGame(SmartFoxConnection.PersistentData);
            if(SmartFoxConnection.PersistentData.GetBool(KeySFSObject.IS_START_GAME))
            {
                SendRequestSFS.GetChess();
                SendRequestSFS.UpdateTime();
                objEffect.SetStateBtnReady(false, false, false);
            }
            bool turn = SmartFoxConnection.PersistentData.GetBool(KeySFSObject.TURN);
            SetTurn(turn);
            IsPlayGame = true;
            GameConfig.isReconnect = false;
            Invoke("Ping", 10);
            SmartFoxConnection.SendReadyMsg(new SFSObject(), SFCommands.CHECK_IS_TOURNAMENT);
        }
        else
        {
            if (isRecord)
            {
                objEffect.ShowMenuRecord();
                isRecord = false;
            }
            else
            {
                objEffect.ShowMenuGame();
                IsPlayGame = false;
                if (isOnline)
                {
                    #region online
                    if (GameKind == global::TypeGame.CoUp)
                        isCoup = true;
                    else
                        isCoup = false;
                    if (SmartFoxConnection.PersistentData != null)
                    {
                        CreateSFS();
                        InitGame(SmartFoxConnection.PersistentData);
                    }
                    else
                    {
                        SceneManager.LoadScene(Scenes.OPTION_LOGIN);
                    }

                    SendRequestSFS.UpdateTime();
                    Invoke("Ping", 10);
                    SmartFoxConnection.SendReadyMsg(new SFSObject(), SFCommands.CHECK_IS_TOURNAMENT);
                    #endregion online
                }
                else
                {
                    Offline();
                }
                
            }
        }
        StartCoroutine(PlayAudioJoinRoom());
    }

    private void InitGame(SFSObject sfsParams)
    {
        MyOrder = sfsParams.GetInt(KeySFSObject.ORDER_PLAYER);
        SetInforGame(sfsParams);        
        ISFSArray arrSfsPlayer = sfsParams.GetSFSArray(KeySFSObject.PLAYERS);            
        PlayerLeft.IsHasPlayer = false;
        PlayerRight.IsHasPlayer = false;
        List<string> arrIdUsers = new List<string>();
        int orderhost = sfsParams.GetInt(KeySFSObject.HOST);
        foreach (SFSObject sfsPlayer in arrSfsPlayer)
        {
            string _idPlayer = sfsPlayer.GetUtfString(KeySFSObject.ID_USER);
            int _orderPlayer = sfsPlayer.GetInt(KeySFSObject.ORDER);
            arrIdUsers.Add(_idPlayer);
            if (MyOrder == 1)
            {
                PlayerLeft.IdUser = GameConfig.User.IdUser;
                if (_orderPlayer == 0)
                {
                    PlayerRight.IdUser = _idPlayer;
                    PlayerRight.Order = 0;
                }
                else
                {
                    teamPlayer = TeamChess.blue;
                    PlayerLeft.IsTeamRed = false;
                    PlayerRight.IsTeamRed = true;
                    PlayerLeft.Order = 1;
                    objEffect.SetStateBtnReady(IsHost, false, true);
                }
            }
            else if (MyOrder == 0)
            {
                PlayerLeft.IdUser = GameConfig.User.IdUser;
                if (_orderPlayer == 1)
                {
                    PlayerRight.IdUser = _idPlayer;
                    PlayerRight.Order = 1;
                }
                else
                {
                    teamPlayer = TeamChess.red;
                    PlayerLeft.IsTeamRed = true;
                    PlayerRight.IsTeamRed = false;
                    PlayerLeft.Order = 0;
                    objEffect.SetStateBtnReady(IsHost, false, true);
                }
            }
            else
            {
                if (_orderPlayer == 1)
                {
                    PlayerRight.IdUser = _idPlayer;
                }
                if (_orderPlayer == 0)
                {
                    PlayerLeft.IdUser = _idPlayer;
                }
                PlayerLeft.IsHost = false;
                PlayerRight.IsHost = false;
                if (orderhost == 0)
                {
                    PlayerLeft.IsHost = true;
                }
                else if (orderhost == 1)
                {
                    PlayerRight.IsHost = true;
                }

                teamPlayer = TeamChess.none;
                PlayerLeft.IsTeamRed = true;
                PlayerRight.IsTeamRed = false;
                objEffect.SetStateBtnReady(false, false, false);
                //send get chess if start game;
                if (SmartFoxConnection.PersistentData.GetBool(KeySFSObject.IS_START_GAME))
                {
                    SendRequestSFS.UpdateTime();
                    SendRequestSFS.GetChess();
                    SendRequestSFS.SendGetHistory();
                }
            }
        }
    }
    public void SetInforGame(ISFSObject sfsParams)
    {
        roomMoney = sfsParams.GetLong(KeySFSObject.MONEY);
        int timeTurn = sfsParams.GetInt(KeySFSObject.TIME_TURN);
        int timeSum = sfsParams.GetInt(KeySFSObject.TIME_SUM);
        int timeLuyTuyen = sfsParams.GetInt(KeySFSObject.TIME_LUY_TUYEN);
        ruleGame = sfsParams.GetInt(KeySFSObject.LUAT_GAME);
        roomName = sfsParams.GetUtfString(KeySFSObject.NAMEROOM);
        int orderhost = sfsParams.GetInt(KeySFSObject.HOST);
        idRoom = sfsParams.GetInt(KeySFSObject.ID_ROOM);
        txtIdRoom.text = "Bàn: " + idRoom;
        IsHost = (orderhost == MyOrder) ? true : false;
        settingGame.SetSetting(roomMoney, roomName, timeTurn, timeSum, timeLuyTuyen, ruleGame);   
    }

    public IEnumerator PlayAudioJoinRoom()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlayAudioJoinRoom();
    }

    void OnEnable()
    {
        SoundManager.Instance.PlayAudioClickBtn();
    }

    public void Offline()
    {
        if (isCoup)
        {
            board.NewGame(isCoup);
        }
    }

    void Update()
    {
        if (null != SmartFoxConnection.Connection)
            SmartFoxConnection.Connection.ProcessEvents();
    }

    public void Ping()
    {
        SendRequestSFS.Ping();
        Invoke("Ping", 10);
    }

    protected override void OnPingPong(Sfs2X.Core.BaseEvent evt)
    { 
        int clientServerLag = (int)evt.Params["lagValue"] / 2;
        Debug.Log("lag = " + clientServerLag);
    }

    protected override void OnExtensionResponse(Sfs2X.Core.BaseEvent evt)
    {
        string cmd = evt.Params[MySFSParams.PARAM_CMD] as string;
        ISFSObject dataObj = evt.Params["params"] as SFSObject;
        CoTuongSFSResponse sfsResponse = new CoTuongSFSResponse();
        sfsResponse.ExcuteCMD(dataObj, cmd, this);
    }

    public void SetTurn(bool _isTurnRed)
    {
        isTurnRed = _isTurnRed;
        if (PlayerLeft.IsTeamRed)
        {
            PlayerLeft.IsTurnPlayer = isTurnRed;
            PlayerRight.IsTurnPlayer = !isTurnRed;
        }
        else
        {
            PlayerLeft.IsTurnPlayer = !isTurnRed;
            PlayerRight.IsTurnPlayer = isTurnRed;
        }
    }

    public bool GetTurn()
    {
        return isTurnRed;
    }

    public bool isMyTurn()
    {
        if (isTest)
            return true;
        if ((teamPlayer == TeamChess.red && isTurnRed == true) ||
            (teamPlayer == TeamChess.blue && isTurnRed == false))
        {
            return true;
        }
        return false;
    }
    #region xu ly luon sfs tra ve 
    public void PlayerJoinRoom(int _order, string _idUser, string _nameUser)
    {
        if (_order != -1)
        {
            if (MyOrder != -1)
            {
                PlayerRight.IsHasPlayer = true;
                PlayerRight.IdUser = _idUser;
            }
            else
            {
                if (_order == 0)
                {
                    PlayerLeft.IsHasPlayer = true;
                    PlayerLeft.Order = 0;
                    PlayerLeft.IdUser = _idUser;
                }
                if (_order == 1)
                {
                    PlayerRight.IsHasPlayer = true;
                    PlayerRight.Order = 1;
                    PlayerRight.IdUser = _idUser;
                }
            }
        }
        SoundManager.Instance.GuestJoinRoom();
    }

    public void PlayerReady(bool isReady, int order)
    {
        if (order == MyOrder)
        {
            objEffect.SetStateBtnReady(IsHost, isReady, true);
            PlayerLeft.IsReady = isReady;
        }
        else if (MyOrder != -1)
        {
            PlayerRight.IsReady = isReady;
        }
    }


    public void StartGame(bool isTurnTeamRed)
    {
        txtNotifyTournament.gameObject.SetActive(false);
        if (MyOrder == 1)
        {
            board.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            board.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        SoundManager.Instance.PlayAudioEndGame();
        objEffect.SetStateBtnReady(IsHost, true, false);
        SetTurn(isTurnRed);

    }

    public void StartGame(bool _isTurnTeamRed, long _money, int _timeSumRed, int _timeTurnRed, int _timeSumBlue, int _timeTurnBlue)
    {
        txtNotifyTournament.gameObject.SetActive(false);
        menuGame.NumCauHoa = 3;

        SoundManager.Instance.PlayAudioEndGame();
        PlayerLeft.ResetPlayer();
        PlayerRight.ResetPlayer();
        if (MyOrder == 1)
        {
            board.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            board.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        objEffect.SetStateBtnReady(IsHost, true, false);
        //        board.NewGame();

        if (PlayerLeft.IsTeamRed)
        {
            PlayerLeft.TimeSum = _timeSumRed;
            PlayerLeft.TimeTurn = _timeTurnRed;

            PlayerRight.TimeSum = _timeSumBlue;
            PlayerRight.TimeTurn = _timeTurnBlue;
        }
        else
        {
            PlayerLeft.TimeSum = _timeSumBlue;
            PlayerLeft.TimeTurn = _timeTurnBlue;

            PlayerRight.TimeSum = _timeSumRed;
            PlayerRight.TimeTurn = _timeTurnRed;
        }
        txtMoney.text = Utility.ConvertMoneyFormat(_money);
        SetTurn(_isTurnTeamRed);
        IsPlayGame = true;
    }

    public void PlayerInGame(int _order, string _userId, int _orderHost)
    {
        if (_order != -1)
        {
            if (MyOrder != -1)
            {
                PlayerRight.Order = _order;
                PlayerRight.IdUser = _userId;
            }
            else
            {
                if (_order == 0)
                {
                    PlayerLeft.Order = _order;
                    PlayerLeft.IdUser = _userId;
                }
                else if (_order == 1)
                {
                    PlayerRight.Order = _order;
                    PlayerRight.IdUser = _userId;
                }
            }
            if (GameConfig.User.IdUser.Equals(_userId))
            {
                MyOrder = _order;
            }
            IsPlayGame = false;
        }
        else
        {
            if (GameConfig.User.IdUser == _userId)
            {
                MyOrder = -1;
            }

            if (PlayerLeft.IsHasPlayer)
            {
                if (PlayerLeft.IdUser == _userId)
                {
                    if (PlayerRight.IsHasPlayer)
                    {
                        if (PlayerLeft.Order == 1)
                        {
                            PlayerLeft.IdUser = PlayerRight.IdUser;
                            PlayerLeft.Order = PlayerRight.Order;
                            PlayerLeft.IsHasPlayer = true;
                            PlayerRight.IsHasPlayer = false;
                        }
                        else
                        {
                            PlayerLeft.IsHasPlayer = false;
                        }
                    }
                    else
                    {
                        PlayerLeft.IsHasPlayer = false;
                    }
                }
            }

            if (PlayerRight.IsHasPlayer)
            {
                if (PlayerRight.IdUser == _userId)
                {
                    PlayerRight.IsHasPlayer = false;
                }
            }
            IsPlayGame = false;
        }

        PlayerRight.IsHasPlayer = PlayerRight.IsHasPlayer;
        PlayerLeft.IsHasPlayer = PlayerLeft.IsHasPlayer;

        if (MyOrder != -1)
        {
            if (PlayerLeft.IdUser != GameConfig.User.IdUser ||
                PlayerLeft.IsHasPlayer == false)
            {
                //swap 2 player left <-> right
                if (PlayerLeft.IsHasPlayer)
                {
                    PlayerRight.IdUser = PlayerLeft.IdUser;
                    PlayerRight.Order = PlayerLeft.Order;
                    PlayerRight.IsHasPlayer = true;

                    PlayerLeft.IdUser = GameConfig.User.IdUser;
                    PlayerLeft.Order = MyOrder;
                    PlayerLeft.IsHasPlayer = true;
                }
                else
                {
                    PlayerLeft.IdUser = GameConfig.User.IdUser;
                    PlayerLeft.Order = MyOrder;
                    PlayerLeft.IsHasPlayer = true;
                    PlayerRight.IsHasPlayer = false;
                }
            }
        }
        SetPlayerHost(_orderHost);
        if (MyOrder != -1)
        {
            if (MyOrder == _orderHost)
            {
                IsHost = true;
            }
            else
            {
                IsHost = false;
            }
        }
        else
        {
            IsHost = false;
        }
    }

    public void SetPlayerHost(int orderHost)
    {
        if (orderHost != -1)
        {
            if (PlayerLeft.Order == orderHost &&
                PlayerLeft.IsHasPlayer == true)
            {
                PlayerLeft.IsHost = true;
                PlayerRight.IsHost = false;
            }
            else if (PlayerRight.Order == orderHost &&
                PlayerRight.IsHasPlayer == true)
            {
                PlayerLeft.IsHost = false;
                PlayerRight.IsHost = true;
            }
        }
        else
        {
            PlayerLeft.IsHost = false;
            PlayerRight.IsHost = false;
        }
    }

    public void MoveChess(int _idChess, int _posStartX, int _posStartY, int _posEndX, int _posEndY, bool _isTurnRed, int _chessEat, bool _isLatQuan)
    {
        board.MoveChess(_idChess, _posStartX, _posStartY, _posEndX, _posEndY, _chessEat, _isLatQuan);
        SetTurn(_isTurnRed);

    }
    public void AI_Move()
    {
        if (PlayerLeft.IsTurnPlayer == false)
        {
            Debug.Log("Start AI");
            if (GameKind == global::TypeGame.CoUp)
                isCoup = true;
            else
                isCoup = false;
            AICoTuong.AIGameCoTuong.AiMoveChess(board.mNode, !isTurnRed);
        }
    }
    public void PreMoveChess(int _posOldX, int _posOldY, bool _isUpChessOld, int _idChessUp, int _posMoveX, int _posMoveY, int _idChessEat, bool _isOpen)
    {
        bool teamChessMove = board.mNode[_posMoveY, _posMoveX].chess.teamRed;
        if (teamChessMove)
        {
            for (int i = 0; i < Board.instance.listChessRed.Count; i++)
            {
                if (Board.instance.listChessRed[i].isEquals(new NodeBoard(_posMoveY, _posMoveX)))
                {
                    Board.instance.listChessRed[i].setNode(new NodeBoard(_posOldY, _posOldX));
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < Board.instance.listChessBlue.Count; i++)
            {
                if (Board.instance.listChessBlue[i].isEquals(new NodeBoard(_posMoveY, _posMoveX)))
                {
                    Board.instance.listChessBlue[i].setNode(new NodeBoard(_posOldY, _posOldX));
                    break;
                }
            }
        }

        if (_isUpChessOld == false)
        {
            board.mNode[_posOldY, _posOldX].SetChess(board.mNode[_posMoveY, _posMoveX].chess);
        }
        else
        {
            Chess chessMove = new Chess((TypeChess)_idChessUp, teamChessMove, false);
            board.mNode[_posOldY, _posOldX].SetChess(chessMove);
            if (teamChessMove == true)
            {
                board.listChessBlue.Add(new NodeBoard(_posMoveY, _posOldX));
            }
            else
            {
                board.listChessRed.Add(new NodeBoard(_posMoveY, _posOldX));
            }
        }
        bool teamChessEat = !board.mNode[_posMoveY, _posMoveX].chess.teamRed;
        Chess chessEat = new Chess((TypeChess)_idChessEat, teamChessEat, _isOpen);
        board.mNode[_posMoveY, _posMoveX].SetChess(chessEat);

        board.mNode[_posOldY, _posOldX].DrawChess();
        board.mNode[_posMoveY, _posMoveX].DrawChess();
        if (_idChessEat != 0)
        {
            if (teamChessMove)
            {
                board.mNode[_posMoveY, _posMoveX].transform.position = objEffect.tfPosRightEat.position;
                PlayerLeft.RemoveChessEat();
            }
            else
            {
                board.mNode[_posMoveY, _posMoveX].transform.position = objEffect.tfPosLeftEat.position;
                PlayerRight.RemoveChessEat();
            }
        }
        //iTween.MoveTo(board.mNode[_posOldY, _posOldX].gameObject,
        //    iTween.Hash("position",
        //    new Vector3(Board.instance.PosChess(board.mNode[_posOldY, _posOldX].posNode).x,
        //    board.mNode[_posOldY, _posOldX].posNode.y, 0), "islocal", true, "time", GameConfig.timeMoveChess));
        //yield return new WaitForSeconds(GameConfig.timeMoveChess);

        //iTween.MoveTo(board.mNode[_posMoveY, _posMoveX].gameObject,
        //iTween.Hash("position",
        //new Vector3(Board.instance.PosChess(board.mNode[_posMoveY, _posMoveX].posNode).x,
        //board.mNode[_posMoveY, _posMoveX].posNode.y, 0), "islocal", true, "time", GameConfig.timeMoveChess));
        //yield return new WaitForSeconds(GameConfig.timeMoveChess);
        board.mNode[_posMoveY, _posMoveX].gameObject.transform.localPosition = board.PosChess(board.mNode[_posMoveY, _posMoveX].posNode);
        board.mNode[_posOldY, _posOldX].gameObject.transform.localPosition = board.PosChess(board.mNode[_posOldY, _posOldX].posNode);
    }

    public void PlayerDisconnect(int _order, string _idUser, int orderHost, bool isStartGame)
    {
        if (isStartGame)
        {
            if (_order == -1)
            {
                Debug.Log("nguoi xem thoat");
            }
            else
            {
                if (PlayerRight.Order == _order)
                    PlayerRight.IsDisconnect = true;
                else
                    PlayerLeft.IsDisconnect = true;
            }
        }
        else
        {
            if (PlayerLeft.Order == _order)
            {
                PlayerLeft.IsHasPlayer = false;
            }
            else if (PlayerRight.Order == _order)
            {
                PlayerRight.IsHasPlayer = false;
            }

            if (PlayerLeft.Order == orderHost)
            {
                PlayerLeft.IsHost = true;
                PlayerRight.IsHost = false;
            }
            else
            {
                PlayerRight.IsHost = true;
                PlayerLeft.IsHost = false;
            }
        }
    }

    public void PlayerReconnect(int _order)
    {
        if (PlayerRight.Order == _order)
            PlayerRight.IsDisconnect = true;
        else
            PlayerLeft.IsDisconnect = true;
    }

    public void PlayerLeaveRoom(int order, string _idUser, int orderHost, bool _isStartGame)
    {
        if (MyOrder == orderHost)
        {
            IsHost = true;
        }
        else
        {
            IsHost = false;
        }

        if (order == -1)
        {
            Debug.Log("nguoi xem thoat");
        }
        else
        {
            if (_isStartGame)
            { // tran dau dang dien ra
                if (MyOrder != -1)
                {
                    PlayerRight.IsHasPlayer = false;
                    board.NewBoard();
                    objEffect.SetStateBtnReady(IsHost, false, true);
                }
            }
            if (PlayerLeft.Order == order)
            {
                PlayerLeft.IsHasPlayer = false;
            }
            if (PlayerRight.Order == order)
            {
                PlayerRight.IsHasPlayer = false;
            }
            IsPlayGame = false;
            PlayerLeft.IsReady = false;
            PlayerRight.IsReady = false;
        }

    }

    public void DrawBoard(int[] _idChessRed, int[] _posRedX, int[] _posRedY, bool[] _arrIsShowTeamRed, int[] _idChessBlue, int[] _posBlueX, int[] _posBlueY, bool[] _arrIsShowTeamBlue)
    {
        board.DrawBoard(_idChessRed, _posRedX, _posRedY, _arrIsShowTeamRed, _idChessBlue, _posBlueX, _posBlueY, _arrIsShowTeamBlue);
    }


    public void UpdateTime(bool _turn, int timeDown, int _timeSumRed, int _timeTurnRed, int _timeSumBlue, int _timeTurnBlue)
    {
        if (PlayerLeft.IsTeamRed)
        {
            PlayerLeft.UpdateTime(timeDown, _timeTurnRed, _timeSumRed);
            PlayerRight.UpdateTime(timeDown, _timeTurnBlue, _timeSumBlue);
        }
        else
        {
            PlayerRight.UpdateTime(timeDown, _timeTurnRed, _timeSumRed);
            PlayerLeft.UpdateTime(timeDown, _timeTurnBlue, _timeSumBlue);
        }
        if (_turn == true)
        {
            if (_timeSumRed < 10 || _timeTurnRed < 10)
            {
                SoundManager.Instance.TimeAlarm();
            }
        }
        else
        {
            if (_timeSumBlue < 10 || _timeTurnBlue < 10)
            {
                SoundManager.Instance.TimeAlarm();
            }
        }
    }

    public void UpdateTime(bool _turn, int timeDown, int _timeTurn, int _timeSum)
    {
        if (_timeSum < 10 || _timeTurn < 10)
        {
            SoundManager.Instance.TimeAlarm();
        }
        if (PlayerLeft.IsTeamRed == _turn)
        {
            PlayerLeft.UpdateTime(timeDown, _timeTurn, _timeSum);

        }
        else
        {
            PlayerRight.UpdateTime(timeDown, _timeTurn, _timeSum);
        }
    }


    public void TimeOut(bool _isTurnRed)
    {
        //objEffect.ShowWin(!_isTurnRed);
    }
    #endregion

    public void SendGetChess()
    {
        SendRequestSFS.GetChess();
    }

    public void Setting(ISFSObject param)
    {
        SetInforGame(param);
        if (IsHost)
            Toast.instance.ShowToast("Bạn đã đặt thành công");
        else
            Toast.instance.ShowToast("Chủ bàn đã cài đặt lại bàn");
        PlayerLeft.IsReady = false;
        PlayerRight.IsReady = false;
        objEffect.SetStateBtnReady(IsHost, false, true);
    }
}
*/
