using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using UnityEngineInternal;
using COTUONG;
public class Node : Photon.MonoBehaviour { 
    
    public bool isChess;
    public Chess chess;
	public NodeBoard posNode;
    public GameObject objOldMove, ObjChessMove;
    private bool isChessMove = false;
    //private const string linkChessSet = "GamePlay/ChinaChess/ChessSet/ChessSet1/";
	private const string linkChessSet = "GamePlay/ChinaChess/ChessSet/NewChess/";

    public void IsOldmove(bool value, bool _oldTeamRed)
    {
        objOldMove.SetActive(value);
        if (_oldTeamRed)
        {
			objChess.GetComponent<UI2DSprite>().color = Color.gray;
        }
        else
        {
			objChess.GetComponent<UI2DSprite>().color = Color.gray;
        }
    }
    public bool IsMoveChess
    {
        get
        {
            return isChessMove;
        }
        set
        {
            isChessMove = value;
            ObjChessMove.SetActive(value);
            if(chess.teamRed)
            {
				//objChess.GetComponent<UI2DSprite>().color = Color.red;
            }
            else
            {
				//objChess.GetComponent<UI2DSprite>().color = Color.blue;
            }
        }
    }

    
 //   public Vector2 locationChess;

    // Game object 
    public GameObject objChess, objNode;

    public Node(NodeBoard _posNode)
    {
        isChess = false;
        posNode = _posNode;
        chess= new Chess();
    }

    public Node(Chess _chess, NodeBoard _posNode)
    {
        isChess = true;
        chess = _chess;
        posNode = _posNode;
    }

    public Node(Node _node)
    {
        isChess = _node.isChess;
        chess = _node.chess;     
    }

    public void SetChess()
    {
        isChess = false;
        chess = new Chess();
        objChess.SetActive(false);
        objNode.SetActive(false); 
    }

    public void ClearnChess()
    {
        IsMoveChess = false;
        IsOldmove(false,false);
    }

    public void SetChess(NodeBoard _posNode)
    {
        this.posNode = _posNode;
        isChess = false;
        chess = new Chess();
        objChess.SetActive(false);
        objNode.SetActive(false);
    }

    public void SetChess(Chess _chess, NodeBoard _posNode)
    {
        chess = _chess;
		chess.typeChess = _chess.typeChess;
		chess.typeChessShow = _chess.typeChessShow;
        posNode = _posNode;
        isChess = true;
        objChess.SetActive(true);
        objNode.SetActive(false);
        DrawChess();
    }

    public void Createchess(Chess _chess, NodeBoard _posNode)//HAM TAO CHESS
    {
		if(_chess.teamRed){
			Board.instance.listChessRed.Add (_posNode);
			if (_chess.typeChess == TypeChess.Tuong)
				Board.instance.posTuongDo = _posNode;
		}
		if(!_chess.teamRed){
			Board.instance.listChessBlue.Add (_posNode);
			if (_chess.typeChess == TypeChess.Tuong)
				Board.instance.posTuongXanh = _posNode;
		}
        chess = _chess;
        posNode = _posNode;
        isChess = true;
        objChess.SetActive(true);
        objNode.SetActive(false);

        DrawChess();
    }
    
    public void SetChess(Chess _chess)
    {
        chess = _chess;
        if (_chess.typeChess == TypeChess.None)
            isChess = false;
        else
            isChess = true;
    }

    public void OnNodeMove(bool isMove)//HIEN NUOC DI KHI CLICK VAO QUAN
    {
        if(isMove)
        {
            
        }
        else
        {

        }
        objNode.SetActive(isMove);
    }

    #region chess
    public void DrawChess()// HAM TAO HINH CON CO , LAY TU RESOURCE
    {
        this.objNode.SetActive(false);
        if (chess.typeChess == TypeChess.None)
            objChess.SetActive(false);
        else
            objChess.SetActive(true);

		//SpriteRenderer imgChess = objChess.GetComponent<SpriteRenderer>();
		UI2DSprite imgChess = objChess.GetComponent<UI2DSprite>();
        if (chess.show)
        {
            string path = linkChessSet;  
			string ChessName = "";
			if (chess.teamRed) {
				path = path + "1" + chess.typeChess.ToString ();
				//ChessName = "1" + chess.typeChess.ToString ();
			} else {
				path = path + "2" + chess.typeChess.ToString();
				//ChessName = "2" + chess.typeChess.ToString ();
			}
                
			imgChess.sprite2D = Resources.Load(path, typeof(Sprite)) as Sprite;
        }
        else
        {
            string path = linkChessSet;

			if (chess.teamRed) {
				path += "1Up"; 
			} else {
				path += "2Up";
			}
			imgChess.sprite2D = Resources.Load(path, typeof(Sprite)) as Sprite;
        }
        imgChess.color = Color.white;
    }
	public void EmuDrawChess()// HAM TAO HINH CON CO , LAY TU RESOURCE
	{
		this.objNode.SetActive(false);
		if (chess.typeChess == TypeChess.None)
			objChess.SetActive(false);
		else
			objChess.SetActive(true);
		
		objChess.GetComponent<UIButtonMessage> ().enabled = false;

		UI2DSprite imgChess = objChess.GetComponent<UI2DSprite>();
		if (chess.show){
			string path = linkChessSet;  
			string ChessName = "";
			if (chess.teamRed) {
				path = path + "1" + chess.TypeChessShow.ToString ();
			} else {
				path = path + "2" + chess.TypeChessShow.ToString();
			}

			imgChess.sprite2D = Resources.Load(path, typeof(Sprite)) as Sprite;
		}else{
			string path = linkChessSet;
			if (chess.teamRed) {
				path += "1Up"; 
			} else {
				path += "2Up";
			}
			imgChess.sprite2D = Resources.Load(path, typeof(Sprite)) as Sprite;
		}
		imgChess.color = Color.white;
	}
    public void DrawChessShow()
    {
        this.objNode.SetActive(false);
        if (chess.TypeChessShow == TypeChess.None)
            objChess.SetActive(false);
        else
            objChess.SetActive(true);

		UI2DSprite imgChess = objChess.GetComponent<UI2DSprite>();
        if (chess.show == false)
        {
            string path = linkChessSet;
            if (chess.teamRed)
                path = path + "1" + chess.TypeChessShow.ToString();
            else
                path = path + "2" + chess.TypeChessShow.ToString();
			imgChess.sprite2D = Resources.Load(path, typeof(Sprite)) as Sprite;
            imgChess.color = Color.gray;
        }
    }
	/*
	public UITexture ImageChess(int _idChess)
    {
        TypeChess typeChess = (TypeChess)_idChess;
        string path = linkChessSet;
        if (chess.teamRed)
            path = path + "1" + typeChess.ToString();
        else
            path = path + "2" + typeChess.ToString();
		return Resources.Load(path, typeof(UITexture)) as UITexture;
    }
 	*/
    public void OnClickChess()
    {
		if(GameController.instance.RoomInfoIG.indexMove == GameController.instance.RoomInfoIG.listPosXStart.Count && GameController.instance._isMyTurn){
            GameController.instance.PlayChonQuan();
			bool isMyChess = false;
			bool isTeamRed = false;
            isTeamRed = Board.instance.mNode [this.posNode.y, this.posNode.x].chess.teamRed;
			if (GameController.instance.myOrder == 0 && isTeamRed)
				isMyChess = true;
			if (GameController.instance.myOrder == 1 && !isTeamRed)
				isMyChess = true;
			if (GameController.instance._isMyTurn && isMyChess)
	        {            
	            Board.instance.ClearNode(false);//KO DI LAI 1 CHO
	            Board.instance.nodeSellect  = new NodeBoard(posNode);
				GameController.instance.objEffect.onEffectSelectChess(this.gameObject.transform);
	            TypeChess typeChess;
	            if (this.chess.show)
	                typeChess = chess.typeChessShow;
	            else
	                typeChess = chess.typeChess;
	            switch (typeChess)
	            {
	                case TypeChess.Chot:
	                    {
	                        Chot.ChessClick(this.posNode);
	                        break;
	                    }
	                case TypeChess.Sy:
	                    {
						    Sy.ChessClick(this.posNode);
	                        break;
	                    }
	                case TypeChess.Tinh:
	                    {
						    Tinh.ChessClick(this.posNode);
	                        break;
	                    }
	                case TypeChess.Ma:
	                    {
						    Ma.ChessClick(this.posNode);
	                        break;
	                    }
	                case TypeChess.Phao:
	                    {
						    Phao.ChessClick(this.posNode);
	                        break;
	                    }
	                case TypeChess.Xe:
	                    {
						    Xe.ChessClick(this.posNode);
	                        break;
	                    }
	                case TypeChess.Tuong:
	                    {
						    Tuong.ChessClick(this.posNode);
	                        break;
	                    }
	            }
	        }
        }
        else
        {
            GameController.instance.PlaySaiQuan();
        }
	}

    public List<NodeBoard> FindChessMove()
    {
        switch (chess.typeChess)
        {
            case TypeChess.Chot:
                {
                    return Chot.AllPosMove(posNode);
                }
            case TypeChess.Sy:
                {
                    return Sy.AllPosMove(posNode);
                }
            case TypeChess.Tinh:
                {
                    return Tinh.AllPosMove(posNode);
                }
            case TypeChess.Ma:
                {
                    return Ma.AllPosMove(posNode);
                }
            case TypeChess.Phao:
                {
                    return Phao.AllPosMove(posNode);
                }
            case TypeChess.Xe:
                {
                    return Xe.AllPosMove(posNode);
                }
            case TypeChess.Tuong:
                {
                    return Tuong.AllPosMove(posNode);
                }
        }
        return new List<NodeBoard>();
    }
    int numBatQuan = 0;
    bool isSaiLuat = false;
    bool CheckSaiLuat()
    {
        bool saiLuat = false;

        return saiLuat;
    }

    public void OnClickMoveGameObject()//CLIENT SELF CHECK
    {
        Board.instance.CurrentNodeClick = new NodeBoard(this.posNode.y,this.posNode.x);
        //CHECK HERE
        if (GameController.instance.ComingLosing)
        {
            GameController.instance.board.CheckThuaCuoc(Board.instance.nodeSellect.x, Board.instance.nodeSellect.y, this.posNode.x, this.posNode.y);
        }
        else
        {
            AfterCheckLoseMove();
        }
    }
    public void AfterCheckLoseMove()
    {
        GameController.instance.ComingLosing = false;
        Board.instance.ClearNode(false);
        GameController.instance.AlertLosing(false, GameController.instance.myOrder, "");

        int cloneindex = GameController.instance.RoomInfoIG.indexMove + 1;
        StartCoroutine(GameController.instance.CallMove(GameController.instance.myOrder, cloneindex, Board.instance.nodeSellect.x, Board.instance.nodeSellect.y, Board.instance.CurrentNodeClick.x, Board.instance.CurrentNodeClick.y, false,
            Board.instance.mNode[Board.instance.nodeSellect.y, Board.instance.nodeSellect.x].isChess, Board.instance.mNode[Board.instance.nodeSellect.y, Board.instance.nodeSellect.x].chess.show, Board.instance.mNode[Board.instance.nodeSellect.y, Board.instance.nodeSellect.x].chess.teamRed, Board.instance.mNode[Board.instance.nodeSellect.y, Board.instance.nodeSellect.x].chess.teamRedShow,
            Board.instance.mNode[Board.instance.nodeSellect.y, Board.instance.nodeSellect.x].chess.typeChess, Board.instance.mNode[Board.instance.nodeSellect.y, Board.instance.nodeSellect.x].chess.TypeChessShow,
            this.isChess, this.chess.show, this.chess.teamRed, this.chess.teamRedShow, this.chess.typeChess, this.chess.TypeChessShow));

        GameController.numBatBien = Board.instance.NumBatBien(Board.instance.nodeSellect, posNode);
    }
	/////////////////////////////////////////


    public bool isBatQuan (bool isTeamRed,List<NodeBoard> listChessRed,
        List<NodeBoard> listChessBlue,
        Node[,] mNode, int indexChess )
    {
        if (isTeamRed)
        {
            #region
            for (int i = 0; i < listChessRed.Count; i++)
            {
                switch (mNode[listChessRed[i].y, listChessRed[i].x].chess.typeChess)
                {
					case TypeChess.Tuong:
						{
							#region tuong
						if (listChessRed[i].x == listChessBlue[0].x && Board.instance.CoutLineY(listChessRed[i], listChessBlue[0]) == 0)
							{
								return true;
							}
							break;
							#endregion tuong
						}

                    case TypeChess.Xe:
                        {
                            if ((listChessRed[i].y == listChessBlue[indexChess].y &&
                                Board.instance.CoutLineX(listChessRed[i], listChessBlue[indexChess]) == 0) ||
                                (listChessRed[i].x == listChessBlue[indexChess].x &&
                               Board.instance.CoutLineY(listChessRed[i], listChessBlue[indexChess]) == 0))
                            {
                                return true;
                            }

                            break;
                        }
                    case TypeChess.Phao:
                        {
                            if (Board.instance.CoutLineX(listChessRed[i], listChessBlue[0]) == 1 ||
                                Board.instance.CoutLineY(listChessRed[i], listChessBlue[0]) == 1)
                            {
                                return true;
                            }
                            break;
                        }
                    case TypeChess.Ma:
                        {
                            #region ma:
                            if (isMaMove(listChessRed[i], listChessBlue[0],mNode) == true)
                            {
                                return true;
                            }
                            break;
                            #endregion ma
                        }
                    case TypeChess.Chot:
                        {
                            #region chot
                            if ((listChessRed[i].x == listChessBlue[0].x &&
                                listChessRed[i].y - 1 == listChessBlue[0].y) ||
                                (listChessRed[i].y == listChessBlue[0].y &&
                                listChessRed[i].x - 1 == listChessBlue[0].x) ||
                                (listChessRed[i].y == listChessBlue[0].y &&
                                listChessRed[i].x + 1 == listChessBlue[0].x))
                            {
                                return true;
                            }
                            break;
                            #endregion

                        }
                    case TypeChess.Sy:
                        {
                            if (((listChessRed[i].x - listChessBlue[0].x == 1) ||
                                (listChessRed[i].x - listChessBlue[0].x == -1)) &&
                               ((listChessRed[i].y - listChessBlue[0].y == 1) ||
                                (listChessRed[i].y - listChessBlue[0].y == -1)))
                            {
                                return true;
                            }
                            break;
                        }
                    case TypeChess.Tinh:
                        {
                            if ((listChessRed[i].x - listChessBlue[0].x == 2) &&
                               (listChessRed[i].y - listChessBlue[0].y == 2))
                            {
                                if (mNode[listChessRed[i].y - 1, listChessRed[i].y - 1].isChess == false)
                                    return true;
                            }
                            else if ((listChessRed[i].x - listChessBlue[0].x == 2) &&
                               (listChessRed[i].y - listChessBlue[0].y == -2))
                            {
                                if (mNode[listChessRed[i].y + 1, listChessRed[i].x - 1].isChess == false)
                                    return true;
                            }
                            else if ((listChessRed[i].x - listChessBlue[0].x == -2) &&
                               (listChessRed[i].y - listChessBlue[0].y == 2))
                            {
                                if (mNode[listChessRed[i].y - 1, listChessRed[i].x + 1].isChess == false)
                                    return true;
                            }
                            else if ((listChessRed[i].x - listChessBlue[0].x == -2) &&
                               (listChessRed[i].y - listChessBlue[0].y == -2))
                            {
                                if (mNode[listChessRed[i].y + 1, listChessRed[i].x + 1].isChess == false)
                                    return true;
                            }
                            break;
                        }
                }
            }
            #endregion
        }
        else
        {
            #region
            for (int i = 0; i < listChessBlue.Count; i++)
            {
                switch (mNode[listChessBlue[i].y, listChessBlue[i].x].chess.typeChess)
                {
                    case TypeChess.Tuong:
                        {
                            #region tuong
                            if (listChessBlue[i].x == listChessRed[0].x && Board.instance.CoutLineY(listChessBlue[i], listChessRed[0]) == 0)
                            {
                                return true;
                            }
                            break;
                            #endregion tuong
                        }
                    case TypeChess.Xe:
                        {
                            #region xe
                            if ((listChessBlue[i].y == listChessRed[0].y &&
                                Board.instance.CoutLineX(listChessBlue[i], listChessRed[0]) == 0) ||
                                (listChessBlue[i].x == listChessRed[0].x &&
                                Board.instance.CoutLineY(listChessBlue[i], listChessRed[0]) == 0))
                            {
                                return true;
                            }

                            break;
                            #endregion 
                        }
                    case TypeChess.Phao:
                        {
                            #region phao
                            if (Board.instance. CoutLineX(listChessBlue[i], listChessRed[0]) == 1 ||
                                Board.instance. CoutLineY(listChessBlue[i], listChessRed[0]) == 1)
                            {
                                return true;
                            }
                            break;
                            #endregion phao
                        }
                    case TypeChess.Ma:
                        {
                            #region ma:
                            if (isMaMove(listChessBlue[i], listChessRed[0],mNode) == true)
                            {
                                return true;
                            }
                            break;
                            #endregion ma
                        }
                    case TypeChess.Chot:
                        {
                            #region chot
                            if ((listChessBlue[i].x == listChessRed[0].x &&
                                listChessBlue[i].y + 1 == listChessRed[0].y) ||
                                (listChessBlue[i].y == listChessRed[0].y &&
                                listChessBlue[i].x - 1 == listChessRed[0].x) ||
                                (listChessBlue[i].y == listChessRed[0].y &&
                                listChessBlue[i].x + 1 == listChessRed[0].x))
                            {
                                return true;
                            }
                            break;
                            #endregion

                        }
                    case TypeChess.Sy:
                        {
                            if (((listChessBlue[i].x - listChessRed[0].x == 1) ||
                                (listChessBlue[i].x - listChessRed[0].x == -1)) &&
                               ((listChessBlue[i].y - listChessRed[0].y == 1) ||
                                (listChessBlue[i].y - listChessRed[0].y == -1)))
                            {
                                return true;
                            }
                            break;
                        }
                    case TypeChess.Tinh:
                        {
                            if ((listChessBlue[i].x - listChessRed[0].x == 2) &&
                               (listChessBlue[i].y - listChessRed[0].y == 2))
                            {
                                if (mNode[listChessBlue[i].y - 1, listChessBlue[i].x - 1].isChess == false)
                                    return true;
                            }
                            else if ((listChessBlue[i].x - listChessRed[0].x == 2) &&
                               (listChessBlue[i].y - listChessRed[0].y == -2))
                            {
                                if (mNode[listChessBlue[i].y + 1, listChessBlue[i].x - 1].isChess == false)
                                    return true;
                            }
                            else if ((listChessBlue[i].x - listChessRed[0].x == -2) &&
                               (listChessBlue[i].y - listChessRed[0].y == 2))
                            {
                                if (mNode[listChessBlue[i].y - 1, listChessBlue[i].x + 1].isChess == false)
                                    return true;
                            }
                            else if ((listChessBlue[i].x - listChessRed[0].x == -2) &&
                               (listChessBlue[i].y - listChessRed[0].y == -2))
                            {
                                if (mNode[listChessBlue[i].y + 1, listChessBlue[i].x + 1].isChess == false)
                                    return true;
                            }
                            break;
                        }
                }
            }
            #endregion
        }
        return false;
    }

    bool isMaMove(NodeBoard _posStart, NodeBoard _posEnd ,Node [,]mNode)
    {
        int x = _posEnd.x - _posStart.x;
        int y = _posEnd.y - _posStart.y;
        if (Mathf.Abs(x) + Mathf.Abs(y) == 3)
        {
            if (x == 2)
            {
                if (mNode[_posStart.y, _posStart.x + 1].chess.typeChess == TypeChess.None)
                {
                    return true;
                }
            }
            if (x == -2)
            {
                if (mNode[_posStart.y, _posStart.x - 1].chess.typeChess == TypeChess.None)
                {
                    return true;
                }
            }

            if (y == 2)
            {
                if (mNode[_posStart.y + 1, _posStart.x].chess.typeChess == TypeChess.None)
                {
                    return true;
                }
            }
            if (y == -2)
            {
                if (mNode[_posStart.y - 1, _posStart.x].chess.typeChess == TypeChess.None)
                {
                    return true;
                }
            }
        }

        return false;
    }


	public bool CheckMoveChieuTuong(NodeBoard nodeStart,NodeBoard nodeEnd)
    {
        List<NodeBoard> tempListChessTeamRed = new List<NodeBoard>();
        foreach (var node in Board.instance.listChessRed)
        {
            tempListChessTeamRed.Add(new NodeBoard(node));
        }

        List<NodeBoard> tempListChessTeamBlue = new List<NodeBoard>();
        foreach (var node in Board.instance.listChessBlue)
        {
            tempListChessTeamBlue.Add(new NodeBoard(node));
        }
        //xu ly 
        if (Board.instance.mNode[nodeStart.y, nodeStart.x].chess.teamRed)//MO PHONG NUOC DI
        {   
            for (int i = 0; i < tempListChessTeamRed.Count; i++)
            {
                if (tempListChessTeamRed[i].isEquals(nodeStart))
                {
                    tempListChessTeamRed[i].setNode(nodeEnd);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < tempListChessTeamBlue.Count; i++)
            {
                if (tempListChessTeamBlue[i].isEquals(nodeStart))
                {
                    tempListChessTeamBlue[i].setNode(nodeEnd);
                    break;
                }
            }
        }


        Node[,] board = new Node[10,9];
        for (int i  = 0; i < 10;i++)
            for(int j = 0; j < 9; j++)
            {
                board[i, j] = new Node(Board.instance.mNode[i, j]);
            }

        board[nodeEnd.y, nodeEnd.x].SetChess(board[nodeStart.y, nodeStart.x].chess);
		board[nodeStart.y, nodeStart.x].SetChess(new Chess());

		return isBatQuan(this.chess.teamRed, tempListChessTeamRed, tempListChessTeamBlue, board, 0);// BAT QUAN 0 LA CON TUONG
    }

    

    #region bat bien 

#endregion

    

    //float time = 1;
	public void MoveChess(bool _isStart, bool _isRedMove,NodeBoard _posEnd,NodeBoard _posStart,float _TimeTween)
    {
		IEMoveChess(_isStart, _isRedMove,_posEnd,_posStart,_TimeTween);
    }

	void IEMoveChess(bool _onNext,bool _isRedMove,NodeBoard _posEnd,NodeBoard _posStart,float _TimeTween)
    {
		iTween.MoveTo(this.gameObject, iTween.Hash("position",new Vector3(Board.instance.PosChess(_posEnd).x, Board.instance.PosChess(_posEnd).y, 0) ,"islocal", true, "time",_TimeTween));
    }
    #endregion chess
}
