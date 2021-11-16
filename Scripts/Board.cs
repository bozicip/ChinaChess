using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using COTUONG;
public class Board : MonoBehaviour {
    //obj 
    public Node node;

    //value 
    public Node[,] mNode;
    public NodeBoard nodeSellect;
    public bool typeGame = false;
    public List<NodeBoard> listChessRed, listChessBlue;
    public GameObject objListChess;
    public static Board instance;
    // Use this for initialization
	//public UIAtlas InGameAtlas;
	public Vector2 posStar;
	public NodeBoard posTuongXanh, posTuongDo;

	void Start(){
		instance = this;
		listChessRed = new List<NodeBoard>();
		listChessBlue = new List<NodeBoard>();
		InitBoard ();
	}
	//test
    public void NewBoard()
    {
        foreach (Transform child in objListChess.transform)
        {
            Destroy(child.gameObject);
        }
    }
	public void InitBoard(){//DRAW NODE
		NewBoard ();
		mNode = new Node[10, 9];
		for (int i = 0; i < 10; i++)
			for (int j = 0; j < 9; j++)
			{
				Vector2 pos = posStar - new Vector2(posStar.x * 2f * j / 8, posStar.y * 2 * i / 9);
				mNode[i, j] = Instantiate(node) as Node;
				mNode[i, j].transform.parent = objListChess.transform;
				mNode[i, j].transform.localPosition = pos;
				mNode[i, j].transform.localScale = new Vector2(1, 1);
				mNode[i, j].SetChess(new NodeBoard(i, j));
				mNode[i, j].gameObject.name =  "["+i+":" + j+ "]";
			}
	}
	public Vector2 PosChess(NodeBoard _nodeBoard)
	{
		return posStar - new Vector2(posStar.x * 2 * _nodeBoard.x / 8, posStar.y * 2 * _nodeBoard.y / 9);
	}
	public void EMULATIONGAME(){
		listChessBlue = new List<NodeBoard>();
		listChessRed = new List<NodeBoard>();
		//QUAN XANH
		TypeChess[] _listChessBlueRandom = RandomChess (new TypeChess[] {
			TypeChess.Xe, TypeChess.Xe,
			TypeChess.Phao, TypeChess.Phao,
			TypeChess.Ma, TypeChess.Ma,
			TypeChess.Tinh, TypeChess.Tinh,
			TypeChess.Sy, TypeChess.Sy,
			TypeChess.Chot, TypeChess.Chot, TypeChess.Chot, TypeChess.Chot, TypeChess.Chot
		});

		mNode [0, 4].Createchess (new Chess (TypeChess.Tuong, false, true), new NodeBoard (0, 4));//tuong

		mNode [1, 0].Createchess (new Chess (TypeChess.Xe, false, false, _listChessBlueRandom [0]), new NodeBoard (1,0));
		mNode [1, 1].Createchess (new Chess (TypeChess.Xe, false, false, _listChessBlueRandom [1]), new NodeBoard (1,1));
		mNode [1, 2].Createchess (new Chess (TypeChess.Phao, false, false, _listChessBlueRandom [2]), new NodeBoard (1,2));
		mNode [1, 3].Createchess (new Chess (TypeChess.Phao, false, false, _listChessBlueRandom [3]), new NodeBoard (1,3));
		mNode [1, 4].Createchess (new Chess (TypeChess.Ma, false, false, _listChessBlueRandom [4]), new NodeBoard (1,4));
		mNode [1, 5].Createchess (new Chess (TypeChess.Ma, false, false, _listChessBlueRandom [5]), new NodeBoard (1,5));
		mNode [1, 6].Createchess (new Chess (TypeChess.Tinh, false, false, _listChessBlueRandom [6]), new NodeBoard (1,6));
		mNode [1, 7].Createchess (new Chess (TypeChess.Tinh, false, false, _listChessBlueRandom [7]), new NodeBoard (1,7));
		mNode [1, 8].Createchess (new Chess (TypeChess.Sy, false, false, _listChessBlueRandom [8]), new NodeBoard (1,8));

		mNode [2, 2].Createchess (new Chess (TypeChess.Chot, false, false, _listChessBlueRandom [10]), new NodeBoard (2,2));
		mNode [2, 3].Createchess (new Chess (TypeChess.Chot, false, false, _listChessBlueRandom [11]), new NodeBoard (2,3));
		mNode [2, 4].Createchess (new Chess (TypeChess.Chot, false, false, _listChessBlueRandom [12]), new NodeBoard (2,4));
		mNode [2, 5].Createchess (new Chess (TypeChess.Chot, false, false, _listChessBlueRandom [13]), new NodeBoard (2,5));
		mNode [2, 6].Createchess (new Chess (TypeChess.Chot, false, false, _listChessBlueRandom [14]), new NodeBoard (2,6));

		mNode [3, 4].Createchess (new Chess (TypeChess.Sy, false, false, _listChessBlueRandom [9]), new NodeBoard (3,4));
		//QUAN DO
		TypeChess[] _listChessRedRandom = RandomChess (new TypeChess[] {
			TypeChess.Xe, TypeChess.Xe,
			TypeChess.Phao, TypeChess.Phao,
			TypeChess.Ma, TypeChess.Ma,
			TypeChess.Tinh, TypeChess.Tinh,
			TypeChess.Sy, TypeChess.Sy,
			TypeChess.Chot, TypeChess.Chot, TypeChess.Chot, TypeChess.Chot, TypeChess.Chot
		});

		mNode [9, 4].Createchess (new Chess (TypeChess.Tuong, true, true), new NodeBoard (9, 4));//tuong

		mNode [8, 8].Createchess (new Chess (TypeChess.Xe, true, false, _listChessRedRandom [0]), new NodeBoard (8, 8));
		mNode [8, 7].Createchess (new Chess (TypeChess.Xe, true, false, _listChessRedRandom [1]), new NodeBoard (8, 7));
		mNode [8, 6].Createchess (new Chess (TypeChess.Phao, true, false, _listChessRedRandom [2]), new NodeBoard(8, 6));
		mNode [8, 5].Createchess (new Chess (TypeChess.Phao, true, false, _listChessRedRandom [3]), new NodeBoard(8, 5));
		mNode [8, 4].Createchess (new Chess (TypeChess.Ma, true, false, _listChessRedRandom [4]), new NodeBoard (8, 4));
		mNode [8, 3].Createchess (new Chess (TypeChess.Ma, true, false, _listChessRedRandom [5]), new NodeBoard (8, 3));
		mNode [8, 2].Createchess (new Chess (TypeChess.Tinh, true, false, _listChessRedRandom [6]), new NodeBoard (8, 2));
		mNode [8, 1].Createchess (new Chess (TypeChess.Tinh, true, false, _listChessRedRandom [7]), new NodeBoard (8, 1));
		mNode [8, 0].Createchess (new Chess (TypeChess.Sy, true, false, _listChessRedRandom [8]), new NodeBoard (8, 0));

		mNode [7, 6].Createchess (new Chess (TypeChess.Sy, true, false, _listChessRedRandom [9]), new NodeBoard (7, 6));
		mNode [7, 5].Createchess (new Chess (TypeChess.Chot, true, false, _listChessRedRandom [10]), new NodeBoard (7, 5));
		mNode [7, 4].Createchess (new Chess (TypeChess.Chot, true, false, _listChessRedRandom [11]), new NodeBoard (7, 4));
		mNode [7, 3].Createchess (new Chess (TypeChess.Chot, true, false, _listChessRedRandom [12]), new NodeBoard (7, 3));
		mNode [7, 2].Createchess (new Chess (TypeChess.Chot, true, false, _listChessRedRandom [13]), new NodeBoard (7, 2));
	
		mNode [6, 4].Createchess (new Chess (TypeChess.Chot, true, false, _listChessRedRandom [14]), new NodeBoard (6, 4));
	}
	public void NewGame(string  _TypeGame)
    {
        listChessBlue = new List<NodeBoard>();
        listChessRed = new List<NodeBoard>();
       
		if (_TypeGame == RoomSelect.CoUp) {
			//Tao quan xanh
			TypeChess[] _listChessBlueRandom = RandomChess (new TypeChess[] {
				TypeChess.Xe, TypeChess.Xe,
				TypeChess.Phao, TypeChess.Phao,
				TypeChess.Ma, TypeChess.Ma,
				TypeChess.Tinh, TypeChess.Tinh,
				TypeChess.Sy, TypeChess.Sy,
				TypeChess.Chot, TypeChess.Chot, TypeChess.Chot, TypeChess.Chot, TypeChess.Chot
			});

			mNode [0, 4].Createchess (new Chess (TypeChess.Tuong, false, true), new NodeBoard (0, 4));//tuong

			mNode [0, 0].Createchess (new Chess (TypeChess.Xe, false, false, _listChessBlueRandom [0]), new NodeBoard (0, 0));
			mNode [0, 8].Createchess (new Chess (TypeChess.Xe, false, false, _listChessBlueRandom [1]), new NodeBoard (0, 8));

			mNode [2, 1].Createchess (new Chess (TypeChess.Phao, false, false, _listChessBlueRandom [2]), new NodeBoard (2, 1));
			mNode [2, 7].Createchess (new Chess (TypeChess.Phao, false, false, _listChessBlueRandom [3]), new NodeBoard (2, 7));

			mNode [0, 1].Createchess (new Chess (TypeChess.Ma, false, false, _listChessBlueRandom [4]), new NodeBoard (0, 1));
			mNode [0, 7].Createchess (new Chess (TypeChess.Ma, false, false, _listChessBlueRandom [5]), new NodeBoard (0, 7));

			mNode [0, 2].Createchess (new Chess (TypeChess.Tinh, false, false, _listChessBlueRandom [6]), new NodeBoard (0, 2));
			mNode [0, 6].Createchess (new Chess (TypeChess.Tinh, false, false, _listChessBlueRandom [7]), new NodeBoard (0, 6));

			mNode [0, 3].Createchess (new Chess (TypeChess.Sy, false, false, _listChessBlueRandom [8]), new NodeBoard (0, 3));
			mNode [0, 5].Createchess (new Chess (TypeChess.Sy, false, false, _listChessBlueRandom [9]), new NodeBoard (0, 5));

			mNode [3, 0].Createchess (new Chess (TypeChess.Chot, false, false, _listChessBlueRandom [10]), new NodeBoard (3, 0));
			mNode [3, 2].Createchess (new Chess (TypeChess.Chot, false, false, _listChessBlueRandom [11]), new NodeBoard (3, 2));
			mNode [3, 4].Createchess (new Chess (TypeChess.Chot, false, false, _listChessBlueRandom [12]), new NodeBoard (3, 4));
			mNode [3, 6].Createchess (new Chess (TypeChess.Chot, false, false, _listChessBlueRandom [13]), new NodeBoard (3, 6));
			mNode [3, 8].Createchess (new Chess (TypeChess.Chot, false, false, _listChessBlueRandom [14]), new NodeBoard (3, 8));

			//Tao quan do
			TypeChess[] _listChessRedRandom = RandomChess (new TypeChess[] {
				TypeChess.Xe, TypeChess.Xe,
				TypeChess.Phao, TypeChess.Phao,
				TypeChess.Ma, TypeChess.Ma,
				TypeChess.Tinh, TypeChess.Tinh,
				TypeChess.Sy, TypeChess.Sy,
				TypeChess.Chot, TypeChess.Chot, TypeChess.Chot, TypeChess.Chot, TypeChess.Chot
			});

			mNode [9, 4].Createchess (new Chess (TypeChess.Tuong, true, true), new NodeBoard (9, 4));//tuong

			mNode [9, 0].Createchess (new Chess (TypeChess.Xe, true, false, _listChessRedRandom [0]), new NodeBoard (9, 0));
			mNode [9, 8].Createchess (new Chess (TypeChess.Xe, true, false, _listChessRedRandom [1]), new NodeBoard (9, 8));

			mNode [7, 1].Createchess (new Chess (TypeChess.Phao, true, false, _listChessRedRandom [2]), new NodeBoard (7, 1));
			mNode [7, 7].Createchess (new Chess (TypeChess.Phao, true, false, _listChessRedRandom [3]), new NodeBoard (7, 7));

			mNode [9, 1].Createchess (new Chess (TypeChess.Ma, true, false, _listChessRedRandom [4]), new NodeBoard (9, 1));
			mNode [9, 7].Createchess (new Chess (TypeChess.Ma, true, false, _listChessRedRandom [5]), new NodeBoard (9, 7));

			mNode [9, 2].Createchess (new Chess (TypeChess.Tinh, true, false, _listChessRedRandom [6]), new NodeBoard (9, 2));
			mNode [9, 6].Createchess (new Chess (TypeChess.Tinh, true, false, _listChessRedRandom [7]), new NodeBoard (9, 6));

			mNode [9, 3].Createchess (new Chess (TypeChess.Sy, true, false, _listChessRedRandom [8]), new NodeBoard (9, 3));
			mNode [9, 5].Createchess (new Chess (TypeChess.Sy, true, false, _listChessRedRandom [9]), new NodeBoard (9, 5));

			mNode [6, 0].Createchess (new Chess (TypeChess.Chot, true, false, _listChessRedRandom [10]), new NodeBoard (6, 0));
			mNode [6, 2].Createchess (new Chess (TypeChess.Chot, true, false, _listChessRedRandom [11]), new NodeBoard (6, 2));
			mNode [6, 4].Createchess (new Chess (TypeChess.Chot, true, false, _listChessRedRandom [12]), new NodeBoard (6, 4));
			mNode [6, 6].Createchess (new Chess (TypeChess.Chot, true, false, _listChessRedRandom [13]), new NodeBoard (6, 6));
			mNode [6, 8].Createchess (new Chess (TypeChess.Chot, true, false, _listChessRedRandom [14]), new NodeBoard (6, 8));
		} else if (_TypeGame == RoomSelect.TruyenThong) {
	
			//tao quan xanh

			mNode [0, 4].Createchess (new Chess (TypeChess.Tuong, false, true), new NodeBoard (0, 4));

			mNode [0, 0].Createchess (new Chess (TypeChess.Xe, false, true), new NodeBoard (0, 0));
			mNode [0, 8].Createchess (new Chess (TypeChess.Xe, false, true), new NodeBoard (0, 8));

			mNode [2, 1].Createchess (new Chess (TypeChess.Phao, false, true), new NodeBoard (2, 1));
			mNode [2, 7].Createchess (new Chess (TypeChess.Phao, false, true), new NodeBoard (2, 7));

			mNode [0, 1].Createchess (new Chess (TypeChess.Ma, false, true), new NodeBoard (0, 1));
			mNode [0, 7].Createchess (new Chess (TypeChess.Ma, false, true), new NodeBoard (0, 7));

			mNode [0, 2].Createchess (new Chess (TypeChess.Tinh, false, true), new NodeBoard (0, 2));
			mNode [0, 6].Createchess (new Chess (TypeChess.Tinh, false, true), new NodeBoard (0, 6));

			mNode [0, 3].Createchess (new Chess (TypeChess.Sy, false, true), new NodeBoard (0, 3));
			mNode [0, 5].Createchess (new Chess (TypeChess.Sy, false, true), new NodeBoard (0, 5));

			mNode [3, 0].Createchess (new Chess (TypeChess.Chot, false, true), new NodeBoard (3, 0));
			mNode [3, 2].Createchess (new Chess (TypeChess.Chot, false, true), new NodeBoard (3, 2));
			mNode [3, 4].Createchess (new Chess (TypeChess.Chot, false, true), new NodeBoard (3, 4));
			mNode [3, 6].Createchess (new Chess (TypeChess.Chot, false, true), new NodeBoard (3, 6));
			mNode [3, 8].Createchess (new Chess (TypeChess.Chot, false, true), new NodeBoard (3, 8));

			// tao quan do
			mNode [9, 4].Createchess (new Chess (TypeChess.Tuong, true, true), new NodeBoard (9, 4));

			mNode [9, 0].Createchess (new Chess (TypeChess.Xe, true, true), new NodeBoard (9, 0));
			mNode [9, 8].Createchess (new Chess (TypeChess.Xe, true, true), new NodeBoard (9, 8));

			mNode [7, 1].Createchess (new Chess (TypeChess.Phao, true, true), new NodeBoard (7, 1));
			mNode [7, 7].Createchess (new Chess (TypeChess.Phao, true, true), new NodeBoard (7, 7));

			mNode [9, 1].Createchess (new Chess (TypeChess.Ma, true, true), new NodeBoard (9, 1));
			mNode [9, 7].Createchess (new Chess (TypeChess.Ma, true, true), new NodeBoard (9, 7));

			mNode [9, 2].Createchess (new Chess (TypeChess.Tinh, true, true), new NodeBoard (9, 2));
			mNode [9, 6].Createchess (new Chess (TypeChess.Tinh, true, true), new NodeBoard (9, 6));

			mNode [9, 3].Createchess (new Chess (TypeChess.Sy, true, true), new NodeBoard (9, 3));
			mNode [9, 5].Createchess (new Chess (TypeChess.Sy, true, true), new NodeBoard (9, 5));

			mNode [6, 0].Createchess (new Chess (TypeChess.Chot, true, true), new NodeBoard (6, 0));
			mNode [6, 2].Createchess (new Chess (TypeChess.Chot, true, true), new NodeBoard (6, 2));
			mNode [6, 4].Createchess (new Chess (TypeChess.Chot, true, true), new NodeBoard (6, 4));
			mNode [6, 6].Createchess (new Chess (TypeChess.Chot, true, true), new NodeBoard (6, 6));
			mNode [6, 8].Createchess (new Chess (TypeChess.Chot, true, true), new NodeBoard (6, 8));
	
		} else { // CO LOAN
			TypeBlueRedChess[] _listChessBlueRedRandom = new TypeBlueRedChess[] {
				
				new TypeBlueRedChess(false, TypeChess.Xe), new TypeBlueRedChess(false, TypeChess.Xe),
				new TypeBlueRedChess(false, TypeChess.Phao), new TypeBlueRedChess(false, TypeChess.Phao),
				new TypeBlueRedChess(false, TypeChess.Ma), new TypeBlueRedChess(false, TypeChess.Ma),
				new TypeBlueRedChess(false, TypeChess.Tinh), new TypeBlueRedChess(false, TypeChess.Tinh),
				new TypeBlueRedChess(false, TypeChess.Sy), new TypeBlueRedChess(false, TypeChess.Sy),
				new TypeBlueRedChess(false, TypeChess.Chot), new TypeBlueRedChess(false, TypeChess.Chot),
				new TypeBlueRedChess(false, TypeChess.Chot), new TypeBlueRedChess(false, TypeChess.Chot), 
				new TypeBlueRedChess(false, TypeChess.Chot),

				new TypeBlueRedChess(true, TypeChess.Xe), new TypeBlueRedChess(true, TypeChess.Xe),
				new TypeBlueRedChess(true, TypeChess.Phao), new TypeBlueRedChess(true, TypeChess.Phao),
				new TypeBlueRedChess(true, TypeChess.Ma), new TypeBlueRedChess(true, TypeChess.Ma),
				new TypeBlueRedChess(true, TypeChess.Tinh), new TypeBlueRedChess(true, TypeChess.Tinh),
				new TypeBlueRedChess(true, TypeChess.Sy), new TypeBlueRedChess(true, TypeChess.Sy),
				new TypeBlueRedChess(true, TypeChess.Chot), new TypeBlueRedChess(true, TypeChess.Chot), 
				new TypeBlueRedChess(true, TypeChess.Chot), new TypeBlueRedChess(true, TypeChess.Chot), 
				new TypeBlueRedChess(true, TypeChess.Chot)
			};
			TypeBlueRedChess[] _listChessRandom = RandomChessBoard(_listChessBlueRedRandom);

			mNode [0, 4].Createchess (new Chess (TypeChess.Tuong, false,false, true), new NodeBoard (0, 4));//tuong xanh

			mNode [0, 0].Createchess (new Chess (TypeChess.Xe, false, false, _listChessRandom [0]), new NodeBoard (0, 0));
			mNode [0, 8].Createchess (new Chess (TypeChess.Xe, false, false, _listChessRandom [1]), new NodeBoard (0, 8));

			mNode [2, 1].Createchess (new Chess (TypeChess.Phao, false, false, _listChessRandom [2]), new NodeBoard (2, 1));
			mNode [2, 7].Createchess (new Chess (TypeChess.Phao, false, false, _listChessRandom [3]), new NodeBoard (2, 7));

			mNode [0, 1].Createchess (new Chess (TypeChess.Ma, false, false, _listChessRandom [4]), new NodeBoard (0, 1));
			mNode [0, 7].Createchess (new Chess (TypeChess.Ma, false, false, _listChessRandom [5]), new NodeBoard (0, 7));

			mNode [0, 2].Createchess (new Chess (TypeChess.Tinh, false, false, _listChessRandom [6]), new NodeBoard (0, 2));
			mNode [0, 6].Createchess (new Chess (TypeChess.Tinh, false, false, _listChessRandom [7]), new NodeBoard (0, 6));

			mNode [0, 3].Createchess (new Chess (TypeChess.Sy, false, false, _listChessRandom [8]), new NodeBoard (0, 3));
			mNode [0, 5].Createchess (new Chess (TypeChess.Sy, false, false, _listChessRandom [9]), new NodeBoard (0, 5));

			mNode [3, 0].Createchess (new Chess (TypeChess.Chot, false, false, _listChessRandom [10]), new NodeBoard (3, 0));
			mNode [3, 2].Createchess (new Chess (TypeChess.Chot, false, false, _listChessRandom [11]), new NodeBoard (3, 2));
			mNode [3, 4].Createchess (new Chess (TypeChess.Chot, false, false, _listChessRandom [12]), new NodeBoard (3, 4));
			mNode [3, 6].Createchess (new Chess (TypeChess.Chot, false, false, _listChessRandom [13]), new NodeBoard (3, 6));
			mNode [3, 8].Createchess (new Chess (TypeChess.Chot, false, false, _listChessRandom [14]), new NodeBoard (3, 8));


			mNode [9, 4].Createchess (new Chess (TypeChess.Tuong, true, true,true), new NodeBoard (9, 4));//tuong

			mNode [9, 0].Createchess (new Chess (TypeChess.Xe, true, false, _listChessRandom [15]), new NodeBoard (9, 0));
			mNode [9, 8].Createchess (new Chess (TypeChess.Xe, true, false, _listChessRandom [16]), new NodeBoard (9, 8));

			mNode [7, 1].Createchess (new Chess (TypeChess.Phao, true, false, _listChessRandom [17]), new NodeBoard (7, 1));
			mNode [7, 7].Createchess (new Chess (TypeChess.Phao, true, false, _listChessRandom [18]), new NodeBoard (7, 7));

			mNode [9, 1].Createchess (new Chess (TypeChess.Ma, true, false, _listChessRandom [19]), new NodeBoard (9, 1));
			mNode [9, 7].Createchess (new Chess (TypeChess.Ma, true, false, _listChessRandom [20]), new NodeBoard (9, 7));

			mNode [9, 2].Createchess (new Chess (TypeChess.Tinh, true, false, _listChessRandom [21]), new NodeBoard (9, 2));
			mNode [9, 6].Createchess (new Chess (TypeChess.Tinh, true, false, _listChessRandom [22]), new NodeBoard (9, 6));

			mNode [9, 3].Createchess (new Chess (TypeChess.Sy, true, false, _listChessRandom [23]), new NodeBoard (9, 3));
			mNode [9, 5].Createchess (new Chess (TypeChess.Sy, true, false, _listChessRandom [24]), new NodeBoard (9, 5));

			mNode [6, 0].Createchess (new Chess (TypeChess.Chot, true, false, _listChessRandom [25]), new NodeBoard (6, 0));
			mNode [6, 2].Createchess (new Chess (TypeChess.Chot, true, false, _listChessRandom [26]), new NodeBoard (6, 2));
			mNode [6, 4].Createchess (new Chess (TypeChess.Chot, true, false, _listChessRandom [27]), new NodeBoard (6, 4));
			mNode [6, 6].Createchess (new Chess (TypeChess.Chot, true, false, _listChessRandom [28]), new NodeBoard (6, 6));
			mNode [6, 8].Createchess (new Chess (TypeChess.Chot, true, false, _listChessRandom [29]), new NodeBoard (6, 8));
		}
    }

    public TypeChess[] RandomChess(TypeChess[] _listChess)
    {
        for(int i = 0;i<_listChess.Length;i++)
        {
            int indexRandom = Random.Range(i, _listChess.Length-1);
            TypeChess temp = _listChess[i];
            _listChess[i] = _listChess[indexRandom];
            _listChess[indexRandom] = temp;
        }
        return _listChess;
    }

	public TypeBlueRedChess[] RandomChessBoard(TypeBlueRedChess[] _listChess)
	{
		for (int i = 0; i < _listChess.Length; i++) {
			int indexRandom = Random.Range (i, _listChess.Length - 1);
			TypeBlueRedChess temp = _listChess [i];
			_listChess [i] = _listChess [indexRandom];
			_listChess [indexRandom] = temp;
		}

		return _listChess;
	}
    public void DrawBoard(List<Chess> _listChessRed,List<Chess> _listChessBlue)
    {
        NewBoard();
        listChessBlue = new List<NodeBoard>();
        listChessRed = new List<NodeBoard>();
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 9; j++)
            {
                Vector2 pos = posStar - new Vector2(posStar.x * 2 * j / 8, posStar.y * 2 * i / 9);
                mNode[i, j] = Instantiate(node) as Node;
                mNode[i, j].transform.parent = objListChess.transform;
                mNode[i, j].transform.localPosition = pos;
                mNode[i, j].transform.localScale = new Vector2(1, 1);
                mNode[i, j].transform.localRotation = this.transform.localRotation;
                mNode[i, j].SetChess(new NodeBoard(i, j));
            }
        for (int i = 0; i < _listChessRed.Count; i++)
        {
            mNode[_listChessRed[i].node.y, _listChessRed[i].node.x].Createchess(_listChessRed[i], new NodeBoard(_listChessRed[i].node.y, _listChessRed[i].node.x));
        }

       
        for (int i = 0; i < _listChessBlue.Count; i++)
        {
            mNode[_listChessBlue[i].node.y, _listChessBlue[i].node.x].Createchess(_listChessBlue[i], new NodeBoard(_listChessBlue[i].node.y, _listChessBlue[i].node.x));
        }
    } 

    public void DrawBoard( int[] _idChessRed, int[] _posRedX, int[] _posRedY,bool[] _arrIsShowTeamRed, int[] _idChessBlue, int[] _posBlueX, int[] _posBlueY,bool[] _arrIsShowTeamBlue)
    {
        //remove all chess
        NewBoard();
        listChessBlue = new List<NodeBoard>();
        listChessRed = new List<NodeBoard>();
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 9; j++)
            {
                Vector2 pos = posStar - new Vector2(posStar.x * 2 * j / 8, posStar.y * 2 * i / 9);
                mNode[i, j] = Instantiate(node) as Node;
                mNode[i, j].transform.parent = objListChess.transform;
                mNode[i, j].transform.localPosition = pos;
                mNode[i, j].transform.localScale = new Vector2(1, 1);
                mNode[i,j].transform.localRotation = this.transform.localRotation;
                mNode[i, j].SetChess(new NodeBoard(i, j));
				mNode [i, j].name = "[" + i + "," + j + "]";
            }
        //Create team blue
        for(int i = 0; i < _idChessBlue.Length; i++)
        {
            mNode[_posBlueY[i], _posBlueX[i]].Createchess(new Chess((TypeChess)_idChessBlue[i], false,_arrIsShowTeamBlue[i]),new NodeBoard(_posBlueY[i], _posBlueX[i]));
        }

        //create team red
        for (int i = 0; i < _idChessRed.Length; i++)
        {
            mNode[_posRedY[i], _posRedX[i]].Createchess(new Chess((TypeChess)_idChessRed[i], true,_arrIsShowTeamRed[i]), new NodeBoard(_posRedY[i], _posRedX[i]));
        }
    }

    public void ClearNode(bool _isMoveChess)
    {
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 9; j++)
            {
                mNode[i, j].objNode.SetActive(false);
				mNode [i, j].objOldMove.SetActive (false);
				mNode [i, j].ObjChessMove.SetActive (false);
            }
    }
    public NodeBoard CurrentNodeClick; 
    //IMPORTANT
    public void CheckThuaCuoc(int posStartX, int posStartY, int posEndX, int posEndY)
    {
        NodeBoard NodeStart = new NodeBoard(posStartY,posStartX);
        NodeBoard NodeEnd = new NodeBoard(posEndY,posEndX);
        Debug.Log("NodeStart" +  NodeStart.y + ";" + NodeStart.x);
        Debug.Log("NodeEnd" + NodeEnd.y + ";" + NodeEnd.x);
        Node[,] Newboard = new Node[10, 9];
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 9; j++)
            {
                Newboard[i, j] = new Node(mNode[i, j]);
            }

        Newboard[NodeEnd.y,NodeEnd.x].SetChess(Newboard[NodeStart.y,NodeStart.x].chess);
        Newboard[posStartY, posStartX].SetChess(new Chess());

        List<NodeBoard> tempListChessTeamRed = new List<NodeBoard>();
        for (int i = 0; i <  listChessRed.Count; i ++)
        {
            tempListChessTeamRed.Add(new NodeBoard(listChessRed[i]));
        }

        List<NodeBoard> tempListChessTeamBlue = new List<NodeBoard>();
        for (int i = 0; i < listChessBlue.Count; i++)
        {
            tempListChessTeamBlue.Add(new NodeBoard(listChessBlue[i]));
        }

        for (int i = 0; i < tempListChessTeamRed.Count; i++)
        {
            if (tempListChessTeamRed[i].isEquals(NodeStart))
            {
                tempListChessTeamRed[i].setNode(NodeEnd);
                break;
            }
        }
        for (int m = 0; m < tempListChessTeamBlue.Count; m++)
        {
            if (tempListChessTeamBlue[m].isEquals(NodeStart))
            {
                tempListChessTeamBlue[m].setNode(NodeEnd);
                break;
            }
        }
        //XU LI
        switch (GameController.instance.CaseLosing)
        {
            case 1://numChieu
                bool isChieu = ChieuDoiThu(GameController.instance.CheckMyTeam(),tempListChessTeamRed,tempListChessTeamBlue,Newboard);
                if (isChieu == true )
                {
                    if (cloneNode != null)
                    {
                        if (cloneNode.isEquals(NodeStart))
                        {
                            Debug.Log("inCase Chieu  cloneNode" + cloneNode.y + "," + cloneNode.x);
                            Debug.Log("NodeStart " + NodeStart.y + "," + NodeStart.x);
                            GameController.instance.ShowAlertLose("Chiếu nhiều lần , Nếu đi nước cờ này bạn sẽ bị xử thua (x10 tiền bàn)!!\n  Bạn có đồng ý không ? ");
                        }else
                            mNode[NodeEnd.y, NodeEnd.x].AfterCheckLoseMove();
                    }
                }else
                {
                    mNode[NodeEnd.y, NodeEnd.x].AfterCheckLoseMove();
                }
                break;
            case 2://numMaxChieu
                bool Chieu = ChieuDoiThu(GameController.instance.CheckMyTeam(), tempListChessTeamRed, tempListChessTeamBlue, Newboard);
                if (Chieu)
                {
                    GameController.instance.ShowAlertLose("Chiếu nhiều lần , Nếu đi nước cờ này bạn sẽ bị xử thua (x10 tiền bàn)!!\n  Bạn có đồng ý đi quân không ? ");
                }
                else
                    mNode[NodeEnd.y, NodeEnd.x].AfterCheckLoseMove();
                break;
            case 3://numBatQuan
                //SAVE CLONE LIST
                List<NodeBoard> cloneListQuanBiBat = new List<NodeBoard>();
                List<NodeBoard> cloneListQuanDangBiBat = new List<NodeBoard>();
                for (int i = 0; i < listQuanBiBat.Count; i++)
                    cloneListQuanBiBat.Add(listQuanBiBat[i]);
                for (int i = 0; i <QuanDangBiBat.Count; i++)
                    cloneListQuanDangBiBat.Add(QuanDangBiBat[i]);

                bool isLosing = false;
                CheckBatQuan(GameController.instance.CheckMyTeam(),tempListChessTeamRed,tempListChessTeamBlue,Newboard);
                if (listQuanBiBat.Count > 0)
                {
                    if (QuanDangBiBat.Count > 0)
                    {
                        for (int i = 0; i < QuanDangBiBat.Count; i++)
                            for (int j = 0; j < listQuanBiBat.Count; j++)
                                if (QuanDangBiBat[i].isEquals(listQuanBiBat[j]))
                                {
                                    isLosing = true;
                                    break;
                                }
                    }
                }
                if (isLosing)
                {
                    listQuanBiBat = new List<NodeBoard>();
                    QuanDangBiBat = new List<NodeBoard>();
                    for (int i = 0; i < cloneListQuanBiBat.Count; i++)
                        listQuanBiBat.Add(cloneListQuanBiBat[i]);
                    for (int i = 0; i < cloneListQuanDangBiBat.Count; i++)
                        QuanDangBiBat.Add(cloneListQuanDangBiBat[i]);
                    //HIEN THONG BAO
                    GameController.instance.ShowAlertLose("Bắt quân đối thủ nhìu lần ! Nếu tiếp tục đi nước này bạn sẽ bị xử thua (x10 tiền bàn)!\n Bạn có đồng ý đi quân không ?  ");
                }
                else
                    mNode[NodeEnd.y, NodeEnd.x].AfterCheckLoseMove();
                break;
            case 4://numBatBien
                if (mNode[posEndY, posEndX].chess.typeChess == TypeChess.None)
                {
                    int num = 0;
                    for (int i = listChessRedSave.Count - 2; i >= 0; i = i - 2)
                    {
                        if (isEqualsListNodeBoard(listChessRedSave[i], tempListChessTeamRed) == true &&
                           isEqualsListNodeBoard(listChessBlueSave[i], tempListChessTeamBlue) == true)
                        {
                            num++;
                        }
                    }
                    if(num > 3)
                    {
                        //THONG BAO
                        GameController.instance.ShowAlertLose("Bàn cờ lặp lại nhiều lần ! Nếu đi nước này cả 2 sẽ hoà ! \n Bạn có đồng ý đi quân không ? ");
                    }else
                        mNode[NodeEnd.y, NodeEnd.x].AfterCheckLoseMove();
                }
                else
                    mNode[NodeEnd.y, NodeEnd.x].AfterCheckLoseMove();
                break;
            case 5://numAnQuan
                if (mNode[posEndY, posEndX].chess.typeChess == TypeChess.None)
                {
                    GameController.instance.ShowAlertLose("Không có quân bị triệt tiêu sau 60 nước đi ! Nếu đi nước này cả 2 sẽ hoà ! \n Bạn có đồng ý đi quân không ? ");
                }else
                    mNode[NodeEnd.y, NodeEnd.x].AfterCheckLoseMove();
                break;
            case 6:
                break;

        }
    }
    public bool isWanning(NodeBoard _postart, NodeBoard _posEnd)//DI QUAN NAY , TUONG CO BI DE DOA HAY KO ?
    { 
        if(mNode[_postart.y,_postart.x].chess.teamRed)
        {
    
            for (int i =0;i<listChessBlue.Count; i++)
            {
                if(listChessBlue[i].isEquals(_posEnd)==false)
                {
                    switch (mNode[listChessBlue[i].y, listChessBlue[i].x].chess.typeChess)
                    {
                        case TypeChess.Tuong:
                            {
           
							if(CountLineY( listChessBlue[i],listChessRed[0],_postart,_posEnd) ==0)
                                {
                                    return true;
                                }
                                break;
                     
                            }
                        case TypeChess.Xe:
                            {
							if (CountLineY(listChessBlue[i], listChessRed[0], _postart, _posEnd) == 0)
                                    return true;
							if (CountLineX(listChessBlue[i], listChessRed[0], _postart, _posEnd) == 0)
                                    return true;
                                break;
                            }
                        case TypeChess.Phao:
                            {
							if (CountLineY(listChessBlue[i], listChessRed[0], _postart, _posEnd) == 1)
                                    return true;
							if (CountLineX(listChessBlue[i], listChessRed[0], _postart, _posEnd) == 1)
                                    return true;
                                break;
                            }
                        case TypeChess.Ma:
                            {
                                if(isChieuMa(listChessBlue[i], listChessRed[0], _postart, _posEnd))
                                    return true;
                                break;
                            }
                        case TypeChess.Chot:
                            {
                  
                                if (_posEnd.isEquals(listChessBlue[i]) == false)
                                {
                                    if ((listChessBlue[i].x == listChessRed[0].x &&
                                        listChessBlue[i].y + 1 == listChessRed[0].y) ||
                                        (listChessBlue[i].y == listChessRed[0].y &&
                                        listChessBlue[i].x - 1 == listChessRed[0].x) ||
                                        (listChessBlue[i].y == listChessRed[0].y &&
                                        listChessBlue[i].x + 1 == listChessRed[0].x))
                                    {
                                        if(_postart.isEquals(listChessRed[0])==false)
                                            return true;
                                    }
                                    if (_postart.isEquals(listChessRed[0]))
                                    {
                                        if ((listChessBlue[i].x == _posEnd.x &&
                                        listChessBlue[i].y + 1 == _posEnd.y) ||
                                        (listChessBlue[i].y == _posEnd.y &&
                                        listChessBlue[i].x - 1 == _posEnd.x) ||
                                        (listChessBlue[i].y == _posEnd.y &&
                                        listChessBlue[i].x + 1 == _posEnd.x))
                                        {
                                            return true;
                                        }
                                    }
                                }

                                    break;
                            
                            }
                        case TypeChess.Sy:
                            {
                                if (isChieuSy(listChessBlue[i], listChessRed[0], _postart, _posEnd))
                                    return true;
                                break;
                            }
                        case TypeChess.Tinh:
                            {
                                if(isChieuTinh(listChessBlue[i], listChessRed[0], _postart, _posEnd))
                                    return true;
                                break;
                            }
                    }
                }
            }
  
        }
        else
        {
            for (int i = 0; i < listChessRed.Count; i++)
            {
                if (listChessRed[i].isEquals(_posEnd) == false)
                {
                    switch (mNode[listChessRed[i].y, listChessRed[i].x].chess.typeChess)
                    {
                        case TypeChess.Tuong:
                            {
                       
                                if (CountLineY(listChessRed[i], listChessBlue[0], _postart, _posEnd) == 0)
                                {
                                    return true;
                                }
                                break;
                     
                            }
                        case TypeChess.Xe:
                            {
                                if (CountLineY(listChessRed[i], listChessBlue[0], _postart, _posEnd) == 0)
                                    return true;
                                if (CountLineX(listChessRed[i], listChessBlue[0], _postart, _posEnd) == 0)
                                    return true;
                                break;
                            }
                        case TypeChess.Phao:
                            {
                                if (CountLineY(listChessRed[i], listChessBlue[0], _postart, _posEnd) == 1)
                                    return true;
                                if (CountLineX(listChessRed[i], listChessBlue[0], _postart, _posEnd) == 1)
                                    return true;
                                break;
                            }
                        case TypeChess.Ma:
                            {
                                if (isChieuMa(listChessRed[i], listChessBlue[0], _postart, _posEnd))
                                    return true;
                                break;
                            }
                        case TypeChess.Chot:
                            {
                    
                                if (_posEnd.isEquals(listChessRed[i]) == false)
                                {
                                    if ((listChessRed[i].x == listChessBlue[0].x &&
                                        listChessRed[i].y - 1 == listChessBlue[0].y) ||
                                        (listChessRed[i].y == listChessBlue[0].y &&
                                        listChessRed[i].x - 1 == listChessBlue[0].x) ||
                                        (listChessRed[i].y == listChessBlue[0].y &&
                                        listChessRed[i].x + 1 == listChessBlue[0].x))
                                    {
                                        if (_postart.isEquals(listChessBlue[0]) == false)
                                            return true;
                                    }
                                    if (_postart.isEquals(listChessBlue[0]))
                                    {
                                        if ((listChessRed[i].x == _posEnd.x &&
                                        listChessRed[i].y - 1 == _posEnd.y) ||
                                        (listChessRed[i].y == _posEnd.y &&
                                        listChessRed[i].x - 1 == _posEnd.x) ||
                                        (listChessRed[i].y == _posEnd.y &&
                                        listChessRed[i].x + 1 == _posEnd.x))
                                        {
                                            return true;
                                        }
                                    }
                                }

                                break;
                    

                            }
                        case TypeChess.Sy:
                            {
                                if (isChieuSy(listChessRed[i], listChessBlue[0], _postart, _posEnd))
                                    return true;
                                break;
                            }
                        case TypeChess.Tinh:
                            {
                                if (isChieuTinh(listChessRed[i], listChessBlue[0], _postart, _posEnd))
                                    return true;
                                break;
                            }
                    }
                }
            }
 
        }
        return false;
    }
    
    // ================ MAIN BAT QUAN ==============================
    List<NodeBoard> listQuanBiBat = new List<NodeBoard>();
    List <NodeBoard> QuanDangBiBat = new List<NodeBoard>();
    public void CheckQuanDiChuyen(int _nodeStartX,int _nodeStartY,int _nodeEndX,int _nodeEndY)//TINH QUAN DOI THU CHAY 
    {
        if (listQuanBiBat.Count > 0)
        {
            Debug.Log("DOI PHUONG DI CHUYEN");
            List <NodeBoard> _QuanBiBat = new List<NodeBoard>();
            for (int i = 0; i < listQuanBiBat.Count; i++)
            {
                if(listQuanBiBat[i].x == _nodeStartX && listQuanBiBat[i].y == _nodeStartY)
                {
                    _QuanBiBat.Add(new NodeBoard(_nodeEndY,_nodeEndX));
                }
            }
            QuanDangBiBat = new List<NodeBoard>();
            for (int i = 0; i < _QuanBiBat.Count; i++)
                QuanDangBiBat.Add(_QuanBiBat[i]);
        }
        //REFRESH LIST AND SAVE LIST CU 
    }
    void AddListQuanBiBat(NodeBoard _QuanBiBat)
    {
        bool canAdd = true;
        bool team = GameController.instance.CheckMyTeam();
        if(team == true)
        {
            if (listChessBlue[0].isEquals(_QuanBiBat)){
                canAdd = false;
            }
        }
        else
        {
            if(listChessRed[0].isEquals(_QuanBiBat))
                canAdd = false;
        }
            
        for (int i = 0; i < listQuanBiBat.Count;i++)
        {
            if(listQuanBiBat[i].isEquals(_QuanBiBat))
            {
                canAdd = false;
                break;
            }
        }
        if (canAdd)
        {
            listQuanBiBat.Add(_QuanBiBat);
        }
    }
    void RemoveFromBatQuan(NodeBoard _QuanBiBat)
    {
        for (int i = 0; i < listQuanBiBat.Count; i++)
        {
            if (listQuanBiBat[i].isEquals(_QuanBiBat))
            {
                listQuanBiBat.Remove(listQuanBiBat[i]);
            }
        }
    }
    public void CheckBatQuan(bool isTeamRed,List<NodeBoard> listRed,List<NodeBoard> listBlue , Node[,] newBoard)
    {
        listQuanBiBat = new List<NodeBoard>();
        List<NodeBoard> listRedShowed = new List<NodeBoard>();
        List<NodeBoard> listBlueShowed = new List<NodeBoard>();
        for (int i = 0; i < listRed.Count; i++)
        {
            if (newBoard[listRed[i].y, listRed[i].x].chess.show)
            {
                listRedShowed.Add(new NodeBoard(listRed[i].y, listRed[i].x));
            }
        }
        for (int i = 0; i < listBlue.Count; i++)
        {
            if (newBoard[listBlue[i].y, listBlue[i].x].chess.show)
            {
                listBlueShowed.Add(new NodeBoard(listBlue[i].y, listBlue[i].x));
            }
        }
        //FIND MOVE RED
        if (isTeamRed)
        {
            for (int i = 0; i < listBlueShowed.Count; i++)//CHECK NHUNG CON BLUE BI BAT
            {
                CreateListBatQuan(isTeamRed,listRedShowed, listBlueShowed, i,newBoard);
            }
        }
        else
        {
            for (int i = 0; i < listRedShowed.Count; i++)
            {
                CreateListBatQuan(isTeamRed,listRedShowed, listBlueShowed, i,newBoard);
            }
        }
        //SAU KHI CO LIST QUAN BI BAT , TIM NHUNG QUAN KHONG CO CHAN GIU TRONG LIST
        if (listQuanBiBat.Count > 0)
            CheckQuanCoLap(isTeamRed,listRed,listBlue,newBoard);

        // SO SANH LIST CU
        if (listQuanBiBat.Count > 0)
        {
            if(QuanDangBiBat.Count > 0)
            {
                for(int i = 0; i < QuanDangBiBat.Count; i ++)
                    for(int j = 0; j < listQuanBiBat.Count;j++)
                        if (QuanDangBiBat[i].isEquals(listQuanBiBat[j]))
                        {
                            GameController.numBatQuan++;
                            break;
                        }
            }
            else
            {
                GameController.numBatQuan = 0;
            }
        }
        else
        {
            GameController.numBatQuan = 0;
        }
        if(QuanDangBiBat.Count > 0 )
            Debug.Log("Quan Dang Bi Bat " + QuanDangBiBat[0].y + "," + QuanDangBiBat[0].x);
    }
    void CheckQuanCoLap(bool isTeamRed,List<NodeBoard> listRed,List<NodeBoard> listBlue , Node[,] _newBoard)
    {
        if (isTeamRed)
        {
            for (int i = 0; i < listBlue.Count; i++)
            {
                switch (_newBoard[listBlue[i].y, listBlue[i].x].chess.typeChess)
                {
                    case TypeChess.Tuong:
                        for (int j = 0; j < listQuanBiBat.Count; j++)
                        {
                            if(!listQuanBiBat[j].isEquals(listBlue[i]))
                            {
                                if (listBlue[i].x == listQuanBiBat[j].x && listQuanBiBat[j].y - 1 == listBlue[i].y ||
                            listBlue[i].x == listQuanBiBat[j].x && listQuanBiBat[j].y + 1 == listBlue[i].y ||
                            listBlue[i].y == listQuanBiBat[j].y && listQuanBiBat[j].x - 1 == listBlue[i].x ||
                            listBlue[i].y == listQuanBiBat[j].y && listQuanBiBat[j].x - 1 == listBlue[i].x)
                                {
                                    RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
   
                                }
                            }
                        }
                        break;
                    case TypeChess.Xe:
                        for (int j = 0; j < listQuanBiBat.Count; j++)
                        {
                            if (!listQuanBiBat[j].isEquals(listBlue[i]))
                            {
                                if ((listBlue[i].y == listQuanBiBat[j].y &&
                                CoutLineX(listBlue[i], listQuanBiBat[j]) == 0) ||
                                (listBlue[i].x == listQuanBiBat[j].x &&
                                CoutLineY(listBlue[i], listQuanBiBat[j]) == 0))
                                {
                                    RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
  
                                }
                            }
                        }
                        break;
                    case TypeChess.Phao:
                        for (int j = 0; j < listQuanBiBat.Count; j++)
                        {
                            if (!listQuanBiBat[j].isEquals(listBlue[i]))
                            {
                                if (CoutLineX(listBlue[i], listQuanBiBat[j]) == 1 ||
                                CoutLineY(listBlue[i], listQuanBiBat[j]) == 1)
                                {
                                    RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
       
                                }
                            }
                        }
                        break;
                    case TypeChess.Ma:
                        for (int j = 0; j < listQuanBiBat.Count; j++)
                        {
                            if (!listQuanBiBat[j].isEquals(listBlue[i]))
                            {
                                if (isMaMove(listBlue[i], listQuanBiBat[j]) == true)
                                {
                                    RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
        
                                }
                            }
                        }
                        break;
                    case TypeChess.Chot:
                        for (int j = 0; j < listQuanBiBat.Count; j++)
                        {
                            if (!listQuanBiBat[j].isEquals(listBlue[i]))
                            {
                                if (listBlue[i].y < 4)
                                {
                                    if (listBlue[i].x == listQuanBiBat[j].x && listBlue[i].y + 1 == listQuanBiBat[j].y)
                                    {
  
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
   
                                    }

                                }
                                else
                                {
                                    if ((listBlue[i].x == listQuanBiBat[j].x &&
                                listBlue[i].y + 1 == listQuanBiBat[j].y) ||
                                (listBlue[i].y == listQuanBiBat[j].y &&
                                listBlue[i].x - 1 == listQuanBiBat[j].x) ||
                                (listBlue[i].y == listQuanBiBat[j].y &&
                                listBlue[i].x + 1 == listQuanBiBat[j].x))
                                    {
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
                                    }
                                }
                            }
                        }
                        break;
                    case TypeChess.Sy:
                        for (int j = 0; j < listQuanBiBat.Count; j++)
                        {
                            if (!listQuanBiBat[j].isEquals(listBlue[i]))
                            {
                                if (_newBoard[listBlue[i].y, listBlue[i].x].chess.show)
                                {
                                    if (((listBlue[i].x - listQuanBiBat[j].x == 1) ||
                                (listBlue[i].x - listQuanBiBat[j].x == -1)) &&
                                ((listBlue[i].y - listQuanBiBat[j].y == 1) ||
                                (listBlue[i].y - listQuanBiBat[j].y == -1)))
                                    {
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
       
                                    }
                                }
                                else
                                {
                                    if(listQuanBiBat[j].y == 1 && listQuanBiBat[j].x == 5)
                                    {
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
         
                                    } 
                                }
                            }
                        }
                        break;
                    case TypeChess.Tinh:
                        for (int j = 0; j < listQuanBiBat.Count; j++)
                        {
                            if (!listQuanBiBat[j].isEquals(listBlue[i]))
                            {
                                if ((listBlue[i].x - listQuanBiBat[j].x == 2) &&
                               (listBlue[i].y - listQuanBiBat[j].y == 2))
                                {
                                    if (_newBoard[listBlue[i].y - 1, listBlue[i].y - 1].isChess == false)
                                    {
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
                
                                    }
                                }
                                else if ((listBlue[i].x - listQuanBiBat[j].x == 2) &&
                                   (listBlue[i].y - listQuanBiBat[j].y == -2))
                                {
                                    if (_newBoard[listBlue[i].y + 1, listBlue[i].x - 1].isChess == false)
                                    {
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
                
                                    }
                                }
                                else if ((listBlue[i].x - listQuanBiBat[j].x == -2) &&
                                   (listBlue[i].y - listQuanBiBat[j].y == 2))
                                {
                                    if (_newBoard[listBlue[i].y - 1, listBlue[i].x + 1].isChess == false)
                                    {
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
           
                                    }
                                }
                                else if ((listBlue[i].x - listQuanBiBat[j].x == -2) &&
                                   (listBlue[i].y - listQuanBiBat[j].y == -2))
                                {
                                    if (_newBoard[listBlue[i].y + 1, listBlue[i].x + 1].isChess == false)
                                    {
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
          
                                    }
                                }
                                
                            }
                        }
                        break;
                }
            }
        }
        else
        {
            for (int i = 0; i < listRed.Count; i++)
            {
                switch (_newBoard[listRed[i].y, listRed[i].x].chess.typeChess)
                {
                    case TypeChess.Tuong:
                        for (int j = 0; j < listQuanBiBat.Count; j++)
                        {
                            if (!listQuanBiBat[j].isEquals(listChessRed[i]))
                            {
                                if (listRed[i].x == listQuanBiBat[j].x && listQuanBiBat[j].y - 1 == listRed[i].y ||
                            listRed[i].x == listQuanBiBat[j].x && listQuanBiBat[j].y + 1 == listRed[i].y ||
                            listRed[i].y == listQuanBiBat[j].y && listQuanBiBat[j].x - 1 == listRed[i].x ||
                            listRed[i].y == listQuanBiBat[j].y && listQuanBiBat[j].x - 1 == listRed[i].x)
                                {
                                    RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));

                                }
                            }
                        }
                        break;
                    case TypeChess.Xe:
                        for (int j = 0; j < listQuanBiBat.Count; j++)
                        {
                            if (!listQuanBiBat[j].isEquals(listRed[i]))
                            {
                                if ((listRed[i].y == listQuanBiBat[j].y &&
                                CoutLineX(listRed[i], listQuanBiBat[j]) == 0) ||
                                (listRed[i].x == listQuanBiBat[j].x &&
                                CoutLineY(listRed[i], listQuanBiBat[j]) == 0))
                                {
                                    RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));

                                }
                            }
                        }
                        break;
                    case TypeChess.Phao:
                        for (int j = 0; j < listQuanBiBat.Count; j++)
                        {
                            if (!listQuanBiBat[j].isEquals(listRed[i]))
                            {
                                if (CoutLineX(listRed[i], listQuanBiBat[j]) == 1 ||
                                CoutLineY(listRed[i], listQuanBiBat[j]) == 1)
                                {
                                    RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));

                                }
                            }
                        }
                        break;
                    case TypeChess.Ma:
                        for (int j = 0; j < listQuanBiBat.Count; j++)
                        {
                            if (!listQuanBiBat[j].isEquals(listRed[i]))
                            {
                                if (isMaMove(listRed[i], listQuanBiBat[j]) == true)
                                {
                                    RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
                                }
                            }
                        }
                        break;
                    case TypeChess.Chot:
                        for (int j = 0; j < listQuanBiBat.Count; j++)
                        {
                            if (!listQuanBiBat[j].isEquals(listRed[i]))
                            {
                                if (listRed[i].y > 4)//CHUA QUA SONG
                                {
                                    if (listRed[i].x == listQuanBiBat[j].x && listRed[i].y - 1 == listQuanBiBat[j].y)
                                    {
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
                                    }
                                }
                                else
                                {
                                    if ((listRed[i].x == listQuanBiBat[j].x &&
                                listRed[i].y - 1 == listQuanBiBat[j].y) ||
                                (listRed[i].y == listQuanBiBat[j].y &&
                                listRed[i].x - 1 == listQuanBiBat[j].x) ||
                                (listRed[i].y == listQuanBiBat[j].y &&
                                listRed[i].x + 1 == listQuanBiBat[j].x))
                                    {
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
                                    }
                                }
                            }
                        }
                        break;
                    case TypeChess.Sy:
                        for (int j = 0; j < listQuanBiBat.Count; j++)
                        {
                            if (!listQuanBiBat[j].isEquals(listRed[i]))
                            {
                                if (_newBoard[listRed[i].y, listRed[i].x].chess.show)
                                {
                                    if (((listRed[i].x - listQuanBiBat[j].x == 1) ||
                                (listRed[i].x - listQuanBiBat[j].x == -1)) &&
                                ((listRed[i].y - listQuanBiBat[j].y == 1) ||
                                (listRed[i].y - listQuanBiBat[j].y == -1)))
                                    {
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
                                    }
                                }
                                else
                                {
                                    if (listQuanBiBat[j].x == 5 && listQuanBiBat[j].y == 8)
                                    {
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
                                    }
                                }   
                            }
                        }
                        break;
                    case TypeChess.Tinh:
                        for (int j = 0; j < listQuanBiBat.Count; j++)
                        {
                            if (!listQuanBiBat[j].isEquals(listRed[i]))
                            {
                                if ((listRed[i].x - listQuanBiBat[j].x == 2) &&
                               (listRed[i].y - listQuanBiBat[j].y == 2))
                                {
                                    if (_newBoard[listRed[i].y - 1, listRed[i].y - 1].isChess == false)
                                    {
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
                                    }
                                }
                                else if ((listRed[i].x - listQuanBiBat[j].x == 2) &&
                                   (listRed[i].y - listQuanBiBat[j].y == -2))
                                {
                                    if (_newBoard[listRed[i].y + 1, listRed[i].x - 1].isChess == false)
                                    {
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
                                    }
                                }
                                else if ((listRed[i].x - listQuanBiBat[j].x == -2) &&
                                   (listRed[i].y - listQuanBiBat[j].y == 2))
                                {
                                    if (_newBoard[listRed[i].y - 1, listRed[i].x + 1].isChess == false)
                                    {
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
                                    }
                                }
                                else if ((listRed[i].x - listQuanBiBat[j].x == -2) &&
                                   (listRed[i].y - listQuanBiBat[j].y == -2))
                                {
                                    if (_newBoard[listRed[i].y + 1, listRed[i].x + 1].isChess == false)
                                    {
                                        RemoveFromBatQuan(new NodeBoard(listQuanBiBat[j].y, listQuanBiBat[j].x));
                                    }
                                }

                            }
                        }
                        break;
                }
            }
        }
    }
    void CreateListBatQuan(bool isTeamRed, List<NodeBoard> listTeamRed, List<NodeBoard> listTeamBlue,int inDex,Node[,] _newBoard)
    {
        if (isTeamRed)
        {
            for (int i = 0; i < listTeamRed.Count; i++)
            {
                switch (_newBoard[listTeamRed[i].y, listTeamRed[i].x].chess.typeChess)
                {
                    case TypeChess.Tuong:
                        if (listTeamRed[i].x == listTeamBlue[inDex].x && listTeamBlue[inDex].y - 1 == listTeamRed[i].y ||
                            listTeamRed[i].x == listTeamBlue[inDex].x && listTeamBlue[inDex].y + 1 == listTeamRed[i].y ||
                            listTeamRed[i].y == listTeamBlue[inDex].y && listTeamBlue[inDex].x - 1 == listTeamRed[i].x ||
                            listTeamRed[i].y == listTeamBlue[inDex].y && listTeamBlue[inDex].x - 1 == listTeamRed[i].x)
                        {
                            AddListQuanBiBat(new NodeBoard(listTeamBlue[inDex].y, listTeamBlue[inDex].x));
                        }
                        break;
                    case TypeChess.Xe:
                        if ((listTeamRed[i].y == listTeamBlue[inDex].y &&
                                CoutLineX(listTeamRed[i], listTeamBlue[inDex]) == 0) ||
                                (listTeamRed[i].x == listTeamBlue[inDex].x &&
                                CoutLineY(listTeamRed[i], listTeamBlue[inDex]) == 0))
                        {
                            AddListQuanBiBat(new NodeBoard(listTeamBlue[inDex].y, listTeamBlue[inDex].x));
    
                        }
                        break;
                    case TypeChess.Phao:
                        if (CoutLineX(listTeamRed[i], listTeamBlue[inDex]) == 1 ||
                                CoutLineY(listTeamRed[i], listTeamBlue[inDex]) == 1)
                        {
                            AddListQuanBiBat(new NodeBoard(listTeamBlue[inDex].y, listTeamBlue[inDex].x));
    
                        }
                        break;
                    case TypeChess.Ma:
                        {
                            if (isMaMove(listTeamRed[i], listTeamBlue[inDex]) == true)
                            {
                                AddListQuanBiBat(new NodeBoard(listTeamBlue[inDex].y, listTeamBlue[inDex].x));
       
                            }
                            break;

                        }
                    case TypeChess.Chot:
                        {
                            if (listTeamRed[i].y > 4)//CHUA QUA SONG
                            {
                                if(listTeamRed[i].x == listTeamBlue[inDex].x && listTeamRed[i].y -1 == listTeamBlue[inDex].y)
                                {
                                    AddListQuanBiBat(new NodeBoard(listTeamBlue[inDex].y, listTeamBlue[inDex].x));
     
                                }
                            }
                            else
                            {
                                if ((listTeamRed[i].x == listTeamBlue[inDex].x &&
                                    listTeamRed[i].y - 1 == listTeamBlue[inDex].y) ||
                                    (listTeamRed[i].y == listTeamBlue[inDex].y &&
                                    listTeamRed[i].x - 1 == listTeamBlue[inDex].x) ||
                                    (listTeamRed[i].y == listTeamBlue[inDex].y &&
                                    listTeamRed[i].x + 1 == listTeamBlue[inDex].x))
                                {
                                    AddListQuanBiBat(new NodeBoard(listTeamBlue[inDex].y, listTeamBlue[inDex].x));
         
                                }
                            }
                            break;
                        }
                    case TypeChess.Sy:
                        {
                            if (((listTeamRed[i].x - listTeamBlue[inDex].x == 1) ||
                                (listTeamRed[i].x - listTeamBlue[inDex].x == -1)) &&
                               ((listTeamRed[i].y - listTeamBlue[inDex].y == 1) ||
                                (listTeamRed[i].y - listTeamBlue[inDex].y == -1)))
                            {
                                AddListQuanBiBat(new NodeBoard(listTeamBlue[inDex].y, listTeamBlue[inDex].x));
        
                            }
                            break;
                        }
                    case TypeChess.Tinh:
                        {
                            if ((listTeamRed[i].x - listTeamBlue[inDex].x == 2) &&
                               (listTeamRed[i].y - listTeamBlue[inDex].y == 2))
                            {
                                if (_newBoard[listTeamRed[i].y - 1, listTeamRed[i].y - 1].isChess == false)
                                {
                                    AddListQuanBiBat(new NodeBoard(listTeamBlue[inDex].y, listTeamBlue[inDex].x));
    
                                }
                            }
                            else if ((listTeamRed[i].x - listTeamBlue[inDex].x == 2) &&
                               (listTeamRed[i].y - listTeamBlue[inDex].y == -2))
                            {
                                if (_newBoard[listTeamRed[i].y + 1, listTeamRed[i].x - 1].isChess == false)
                                {
      
                                    AddListQuanBiBat(new NodeBoard(listTeamBlue[inDex].y, listTeamBlue[inDex].x));
                                }
                            }
                            else if ((listTeamRed[i].x - listTeamBlue[inDex].x == -2) &&
                               (listTeamRed[i].y - listTeamBlue[inDex].y == 2))
                            {
                                if (_newBoard[listTeamRed[i].y - 1, listTeamRed[i].x + 1].isChess == false)
                                {
       
                                    AddListQuanBiBat(new NodeBoard(listTeamBlue[inDex].y, listTeamBlue[inDex].x));
                                }
                            }
                            else if ((listTeamRed[i].x - listTeamBlue[inDex].x == -2) &&
                               (listTeamRed[i].y - listTeamBlue[inDex].y == -2))
                            {
                                if (_newBoard[listTeamRed[i].y + 1, listTeamRed[i].x + 1].isChess == false)
                                {
    
                                    AddListQuanBiBat(new NodeBoard(listTeamBlue[inDex].y, listTeamBlue[inDex].x));
                                }
                            }
                            break;
                        }
                }
            }
        }
        else
        {
            for (int i = 0; i < listTeamBlue.Count; i++)
            {
                switch (_newBoard[listTeamBlue[i].y, listTeamBlue[i].x].chess.typeChess)
                {
                    case TypeChess.Tuong:
                            if (listTeamBlue[i].x == listTeamRed[inDex].x && listTeamRed[inDex].y - 1 == listTeamBlue[i].y ||
                                listTeamBlue[i].x == listTeamRed[inDex].x && listTeamRed[inDex].y + 1 == listTeamBlue[i].y ||
                                listTeamBlue[i].y == listTeamRed[inDex].y && listTeamRed[inDex].x - 1 == listTeamBlue[i].x ||
                                listTeamBlue[i].y == listTeamRed[inDex].y && listTeamRed[inDex].x - 1 == listTeamBlue[i].x)
                            {
                                AddListQuanBiBat(new NodeBoard(listTeamRed[inDex].y, listTeamRed[inDex].x));
   
                            }
                        break;
                    case TypeChess.Xe:
                        if ((listTeamBlue[i].y == listTeamRed[inDex].y &&
                                CoutLineX(listTeamBlue[i], listTeamRed[inDex]) == 0) ||
                                (listTeamBlue[i].x == listTeamRed[inDex].x &&
                                CoutLineY(listTeamBlue[i], listTeamRed[inDex]) == 0))
                        {
 
                            AddListQuanBiBat(new NodeBoard(listTeamRed[inDex].y, listTeamRed[inDex].x));
                        }
                        break;
                    case TypeChess.Phao:
                        if (CoutLineX(listTeamBlue[i], listTeamRed[inDex]) == 1 ||
                                CoutLineY(listTeamBlue[i], listTeamRed[inDex]) == 1)
                        {
      
                            AddListQuanBiBat(new NodeBoard(listTeamRed[inDex].y, listTeamRed[inDex].x));
                        }
                        break;
                    case TypeChess.Ma:
                        {
                            if (isMaMove(listTeamBlue[i], listTeamRed[inDex]) == true)
                            {
       
                                AddListQuanBiBat(new NodeBoard(listTeamRed[inDex].y, listTeamRed[inDex].x));
                            }
                            break;

                        }
                    case TypeChess.Chot:
                        {
                            if (listTeamBlue[i].y  < 4)
                            {
                                if (listTeamBlue[i].x == listTeamRed[inDex].x && listTeamBlue[i].y + 1 == listTeamRed[inDex].y)
                                {
                                    AddListQuanBiBat(new NodeBoard(listTeamRed[inDex].y, listTeamRed[inDex].x));
         
                                }

                            }
                            else
                            {
                                if ((listTeamBlue[i].x == listTeamRed[inDex].x &&
                                listTeamBlue[i].y + 1 == listTeamRed[inDex].y) ||
                                (listTeamBlue[i].y == listTeamRed[inDex].y &&
                                listTeamBlue[i].x - 1 == listTeamRed[inDex].x) ||
                                (listTeamBlue[i].y == listTeamRed[inDex].y &&
                                listTeamBlue[i].x + 1 == listTeamRed[inDex].x))
                                {
                                    AddListQuanBiBat(new NodeBoard(listTeamRed[inDex].y, listTeamRed[inDex].x));
                                }
                            }
                            
                            break;
                        }
                    case TypeChess.Sy:
                        {
                            if (((listTeamBlue[i].x - listTeamRed[inDex].x == 1) ||
                                (listTeamBlue[i].x - listTeamRed[inDex].x == -1)) &&
                               ((listTeamBlue[i].y - listTeamRed[inDex].y == 1) ||
                                (listTeamBlue[i].y - listTeamRed[inDex].y == -1)))
                            {
      
                                AddListQuanBiBat(new NodeBoard(listTeamRed[inDex].y, listTeamRed[inDex].x));
                            }
                            break;
                        }
                    case TypeChess.Tinh:
                        {
                            if ((listTeamBlue[i].x - listTeamRed[inDex].x == 2) &&
                               (listTeamBlue[i].y - listTeamRed[inDex].y == 2))
                            {
                                    if (_newBoard[listTeamBlue[i].y - 1, listTeamBlue[i].y - 1].isChess == false)
                                    {
        
                                        AddListQuanBiBat(new NodeBoard(listTeamRed[inDex].y, listTeamRed[inDex].x));
                                    }
                            }
                            else if ((listTeamBlue[i].x - listTeamRed[inDex].x == 2) &&
                               (listTeamBlue[i].y - listTeamRed[inDex].y == -2))
                            {
                                    if (_newBoard[listTeamBlue[i].y + 1, listTeamBlue[i].x - 1].isChess == false)
                                    {
            
                                        AddListQuanBiBat(new NodeBoard(listTeamRed[inDex].y, listTeamRed[inDex].x));
                                    }
                            }
                            else if ((listTeamBlue[i].x - listTeamRed[inDex].x == -2) &&
                               (listTeamBlue[i].y - listTeamRed[inDex].y == 2))
                            {
                                    if (_newBoard[listTeamBlue[i].y - 1, listTeamBlue[i].x + 1].isChess == false)
                                    {
          
                                        AddListQuanBiBat(new NodeBoard(listTeamRed[inDex].y, listTeamRed[inDex].x));
                                    }
                            }
                            else if ((listTeamBlue[i].x - listTeamRed[inDex].x == -2) &&
                               (listTeamBlue[i].y - listTeamRed[inDex].y == -2))
                            {
                                    if (_newBoard[listTeamBlue[i].y + 1, listTeamBlue[i].x + 1].isChess == false)
                                    {
     
                                        AddListQuanBiBat(new NodeBoard(listTeamRed[inDex].y, listTeamRed[inDex].x));
                                    }
                            }
                            break;
                        }
                }
            }
        }
    }
    //====================MAIN CHIEU ===============================
    public void CheckChieu(bool isTeamRed,int NodeStartX,int NodeStartY,int NodeEndX,int NodeEndY)//CALLED IN MY TURN
    {
        bool isChieu = ChieuDoiThu(isTeamRed,listChessRed,listChessBlue,mNode);//SET QUAN CHIEU
        //check chiếu bí
        bool isBi = true;
       if(!isTeamRed)
        {
            for(int i =0;i<listChessRed.Count;i++)
            {
                if (mNode[listChessRed[i].y, listChessRed[i].x].FindChessMove().Count != 0) // CHECK TAT CA QUAN , CON NUOC DE DI 
                {
                    isBi = false;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < listChessBlue.Count; i++)
            {
                if (mNode[listChessBlue[i].y, listChessBlue[i].x].FindChessMove().Count != 0)
                {
                    isBi = false;
                    break;
                }
            }
        }
       
       if(isBi)
        {
            if (isChieu) {//CHU DONG CHIEU
                CheckTheChieu(GameController.instance.CheckMyTeam());//LUU SAN SANG RESULT
                CheckThamChieu(GameController.instance.CheckMyTeam());

                float HeSoThamChieu = LayHeSoTheChieu(listThamChieu);
                if (listThamChieu.Count > 0) {
                    string cloneSt = "Tham Chiếu : ";
                    for (int i = 0; i < listThamChieu.Count; i++)
                    {
                        cloneSt += "[ " + UltilityGame.instance.GetNameTypeChess(mNode[listThamChieu[i].y, listThamChieu[i].x].chess.typeChess) + " ]";
                    }
                    GameController.instance.objEffect.objResult._ThamChieu = new TheChieu { Heso = HeSoThamChieu, Status = cloneSt };
                }
            }
            else//EP BI DOI THU
            {
                GameController.instance.objEffect.objResult._TheChieu = new TheChieu {Heso = 5, Status = "Ép bí"};
            }
            if (isTeamRed)
                GameController.instance.ClaimWin(0,false,"", GameController.instance.objEffect.objResult._TheChieu.Heso, GameController.instance.objEffect.objResult._TheChieu.Status,
                    GameController.instance.objEffect.objResult._ThamChieu.Heso, GameController.instance.objEffect.objResult._ThamChieu.Status);
            else
                GameController.instance.ClaimWin(1,false,"", GameController.instance.objEffect.objResult._TheChieu.Heso, GameController.instance.objEffect.objResult._TheChieu.Status,
                    GameController.instance.objEffect.objResult._ThamChieu.Heso, GameController.instance.objEffect.objResult._ThamChieu.Status);
        }
       if(isChieu && isBi==false)
        {
            //GameController.numBatQuan = 0;
			GameController.numChieuMax++;
			if (cloneNode == null) {
				cloneNode = new NodeBoard (NodeEndY, NodeEndX);
				GameController.numChieu++;
			} else {
				if (cloneNode.x == NodeStartX && cloneNode.y == NodeStartY) {
					GameController.numChieu++;
				} else {
					GameController.numChieu = 1;
				}
				cloneNode = new NodeBoard (NodeEndY, NodeEndX);
			}

			GameController.instance.DangChieu (GameController.instance.CheckMyTeam ());
        }
		if (isChieu == false) {
			cloneNode = null;
			GameController.numChieu = 0;
			GameController.numChieuMax = 0;
		}
    }
	NodeBoard cloneNode;
    List<NodeBoard> listTheChieu = new List<NodeBoard>();
    List<NodeBoard> listThamChieu = new List<NodeBoard>();
    float LayHeSoTheChieu(List<NodeBoard> listTC)
    {
        float A = 0;
        for (int i = 0; i < listTC.Count; i++)
        {
            switch (mNode[listTC[i].y, listTC[i].x].chess.typeChess)
            {
                case TypeChess.Tuong:
                    A += 1.5f;
                    break;
                case TypeChess.Xe:
                    A += 1.5f;
                    break;
                case TypeChess.Sy:
                    A += 2f;
                    break;
                case TypeChess.Tinh:
                    A += 2f;
                    break;
                case TypeChess.Chot:
                    A += 2f;
                    break;
                case TypeChess.Phao:
                    A += 1.5f;
                    break;
                case TypeChess.Ma:
                    A += 1.5f;
                    break;
            }
        }
        return A;
    }
    //
    void CheckTheChieu(bool isTeamRed)
    {
        listTheChieu = new List<NodeBoard>();
        listTheChieu = LayQuanDangChieu(isTeamRed,mNode,listChessRed,listChessBlue);
        //CHECK PHAO TRUNG
        bool isCoPhao = false;
        bool isCoXe = false;
        bool isCoMa = false;
        bool isCo2PhaoTrongList = false;
        bool isPhaoTrung = false;
        bool isTamChieu = false;
        NodeBoard Phao = new NodeBoard(0,0);
        for (int i = 0; i < listTheChieu.Count; i++)
        {
            if (mNode[listTheChieu[i].y, listTheChieu[i].x].chess.typeChess == TypeChess.Phao)
            {
                isCoPhao = true;
                Phao = new NodeBoard(listTheChieu[i].y, listTheChieu[i].x);
                break;
            }
        }
        for (int i = 0; i < listTheChieu.Count; i++)
        {
            if (mNode[listTheChieu[i].y, listTheChieu[i].x].chess.typeChess == TypeChess.Xe)
            {
                isCoXe = true;
                break;
            }
        }
        for (int i = 0; i < listTheChieu.Count; i++)
        {
            if (mNode[listTheChieu[i].y, listTheChieu[i].x].chess.typeChess == TypeChess.Ma)
            {
                isCoMa = true;
                break;
            }
        }
        // CHECK HERE
        if(isCoPhao && isCoXe && isCoMa)// THE TAM CHIEU
        {
            isTamChieu = true;
            if(GameController.instance.objEffect.objResult._TheChieu.Heso < 5)
                GameController.instance.objEffect.objResult._TheChieu = new TheChieu { Heso = 5, Status = "Thế Tam Chiếu Xe Pháo Mã" };
        }
        if (isCoPhao)//CHECK PHAO TRUNG
        {
            List<NodeBoard> list2Phao = new List<NodeBoard>();
            if (isTeamRed)
            {
                for (int i = 0; i < listChessRed.Count; i++)
                {
                    if (mNode[listChessRed[i].y, listChessRed[i].x].chess.show && mNode[listChessRed[i].y, listChessRed[i].x].chess.typeChess == TypeChess.Phao)
                    {
                        list2Phao.Add(new NodeBoard(listChessRed[i].y, listChessRed[i].x));
                    }
                }
                if (list2Phao.Count == 2)
                    isCo2PhaoTrongList = true;
            }
            else {
                for (int i = 0; i < listChessBlue.Count; i++)
                {
                    if (mNode[listChessBlue[i].y, listChessBlue[i].x].chess.show && mNode[listChessBlue[i].y, listChessBlue[i].x].chess.typeChess == TypeChess.Phao)
                    {
                        list2Phao.Add(new NodeBoard(listChessRed[i].y, listChessRed[i].x));
                    }
                }
                if (list2Phao.Count == 2)
                    isCo2PhaoTrongList = true;
            }
            //CHECK NAM TREN 1 DUONG
            if (isCo2PhaoTrongList)
            {
                NodeBoard PhaoChinh = new NodeBoard(0, 1), PhaoPhu = new NodeBoard(0, 1);
                for(int i = 0; i < list2Phao.Count; i++)
                {
                    if (list2Phao[i].isEquals(Phao))
                        PhaoChinh = list2Phao[i];
                    else
                        PhaoPhu = list2Phao[i];
                }
                if (PhaoChinh.x == PhaoPhu.x)
                {
                    //COUNT Y
                    if (isTeamRed)
                    {
                        if (PhaoChinh.y > PhaoPhu.y && PhaoPhu.y > listChessBlue[0].y || PhaoChinh.y < PhaoPhu.y && PhaoPhu.y < listChessBlue[0].y)// 
                            isPhaoTrung = true;
                    }
                    else
                    {
                        if (PhaoChinh.y > PhaoPhu.y && PhaoPhu.y > listChessRed[0].y || PhaoChinh.y < PhaoPhu.y && PhaoPhu.y < listChessRed[0].y)// 
                            isPhaoTrung = true;
                    }
                }
                if (PhaoChinh.y == PhaoPhu.y)
                {
                    //COUNT X
                    if (isTeamRed)
                    {
                        if (PhaoChinh.x > PhaoPhu.x && PhaoPhu.x > listChessBlue[0].x || PhaoChinh.x < PhaoPhu.x && PhaoPhu.x < listChessBlue[0].x)// 
                            isPhaoTrung = true;
                    }
                    else
                    {
                        if (PhaoChinh.x > PhaoPhu.x && PhaoPhu.x > listChessRed[0].x || PhaoChinh.x < PhaoPhu.x && PhaoPhu.x < listChessRed[0].x)// 
                            isPhaoTrung = true;
                    }
                }
            }

            // RESULT HERE
            if (isPhaoTrung)
            {
                if (GameController.instance.objEffect.objResult._TheChieu.Heso < 5)
                    GameController.instance.objEffect.objResult._TheChieu = new TheChieu { Heso = 5, Status = "Thế Pháo Trùng " };
            }
        }

        // THE CHIEU BINH THUONG
        if(!isPhaoTrung && !isTamChieu)
        {
            float _Heso = LayHeSoTheChieu(listTheChieu);
            string cloneStatus = "Thế Chiếu : ";
            for (int i = 0; i < listTheChieu.Count; i++)
            {
                cloneStatus += "[ " + UltilityGame.instance.GetNameTypeChess(mNode[listTheChieu[i].y, listTheChieu[i].x].chess.typeChess) + " ]";
            }
            if (_Heso > 5)
                _Heso = 5;
            GameController.instance.objEffect.objResult._TheChieu = new TheChieu { Heso = _Heso, Status = cloneStatus};
        }
    }

    public void CheckThamChieu(bool isTeamRed)
    {
        listThamChieu = new List<NodeBoard>();
        List<NodeBoard> ListTuongMove = new List<NodeBoard>();

        NodeBoard nodeStart = new NodeBoard(0,0);
        NodeBoard nodeEnd = new NodeBoard(0, 0);
        if (isTeamRed)
        {
            ListTuongMove = Tuong.AllPosCanMoveWhenBi(new NodeBoard(listChessBlue[0].y, listChessBlue[0].x));//NOTE TUONG MOI
            for (int i = 0; i < ListTuongMove.Count; i++)
            {
                Debug.Log("Node Move [" + ListTuongMove[i].y + "," + ListTuongMove[i].x + "]");
            }
            nodeStart = new NodeBoard(listChessBlue[0].y, listChessBlue[0].x);

            for (int k = 0; k < ListTuongMove.Count; k++)
            {
                List<NodeBoard> tempListChessTeamRed = new List<NodeBoard>();
                for (int i = 0; i < listChessRed.Count; i++)
                {
                    tempListChessTeamRed.Add(new NodeBoard(listChessRed[i]));
                }

                List<NodeBoard> tempListChessTeamBlue = new List<NodeBoard>();
                for (int i = 0; i < listChessBlue.Count; i++)
                {
                    tempListChessTeamBlue.Add(new NodeBoard(listChessBlue[i]));
                }

                nodeEnd = new NodeBoard(ListTuongMove[k].y, ListTuongMove[k].x);
                Node[,] board = new Node[10, 9];
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 9; j++)
                    {
                        board[i, j] = new Node(mNode[i, j]);
                    }

                board[nodeEnd.y, nodeEnd.x].SetChess(board[nodeStart.y, nodeStart.x].chess);
                board[nodeStart.y, nodeStart.x].SetChess(new Chess());

                for (int m = 0; m < tempListChessTeamRed.Count; m++)
                {
                    if (tempListChessTeamRed[m].isEquals(nodeStart))
                    {
                        tempListChessTeamRed[m].setNode(nodeEnd);
                        break;
                    }
                }
                for (int m = 0; m < tempListChessTeamBlue.Count; m++)
                {
                    if (tempListChessTeamBlue[m].isEquals(nodeStart))
                    {
                        tempListChessTeamBlue[m].setNode(nodeEnd);
                        break;
                    }
                }
                //ADD LIST HERE
                Debug.Log("Vi Tri Tuong " + nodeEnd.y + "-" + nodeEnd.x);
                Debug.Log("Vi Tri Tuong in Team " + tempListChessTeamBlue[0].y + "-" + tempListChessTeamBlue[0].x);
                List<NodeBoard> cloneListTC = LayQuanDangChieu(isTeamRed, board, tempListChessTeamRed, tempListChessTeamBlue);

                for (int i = 0; i < cloneListTC.Count; i++)
                {
                    Debug.Log(i + "-" + cloneListTC[i].y + "," + cloneListTC[i].x + "_" + board[cloneListTC[i].y, cloneListTC[i].x].chess.typeChess);
                    AddQuanThamChieu(cloneListTC[i]);
                }
                
            }
        }
        else
        {
            ListTuongMove = Tuong.AllPosCanMoveWhenBi(new NodeBoard(listChessRed[0].y, listChessRed[0].x));//NOTE TUONG MOI
            for (int i = 0; i < ListTuongMove.Count; i++)
            {
                Debug.Log("Node Move [" + ListTuongMove[i].y + "," + ListTuongMove[i].x + "]");
            }
            nodeStart = new NodeBoard(listChessRed[0].y, listChessRed[0].x);
            for (int k = 0; k < ListTuongMove.Count; k++)
            {
                List<NodeBoard> tempListChessTeamRed = new List<NodeBoard>();
                for (int i = 0; i < listChessRed.Count; i++)
                {
                    tempListChessTeamRed.Add(new NodeBoard(listChessRed[i]));
                }

                List<NodeBoard> tempListChessTeamBlue = new List<NodeBoard>();
                for (int i = 0; i < listChessBlue.Count; i++)
                {
                    tempListChessTeamBlue.Add(new NodeBoard(listChessBlue[i]));
                }

                nodeEnd = new NodeBoard(ListTuongMove[k].y, ListTuongMove[k].x);
                Node[,] board = new Node[10, 9];
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 9; j++)
                    {
                        board[i, j] = new Node(Board.instance.mNode[i, j]);
                    }

                board[nodeEnd.y, nodeEnd.x].SetChess(board[nodeStart.y, nodeStart.x].chess);
                board[nodeStart.y, nodeStart.x].SetChess(new Chess());

                for (int m = 0; m < tempListChessTeamRed.Count; m++)
                {
                    if (tempListChessTeamRed[m].isEquals(nodeStart))
                    {
                        tempListChessTeamRed[m].setNode(nodeEnd);
                        break;
                    }
                }
                for (int m = 0; m < tempListChessTeamBlue.Count; m++)
                {
                    if (tempListChessTeamBlue[m].isEquals(nodeStart))
                    {
                        tempListChessTeamBlue[m].setNode(nodeEnd);
                        break;
                    }
                }
                //ADD LIST HERE
                Debug.Log("Vi Tri Tuong " + nodeEnd.y + "-" + nodeEnd.x);
                Debug.Log("Vi Tri Tuong in Team " + tempListChessTeamRed[0].y + "-" + tempListChessTeamRed[0].x);
                List<NodeBoard> cloneListTC = LayQuanDangChieu(isTeamRed, board, tempListChessTeamRed, tempListChessTeamBlue);

                for (int i = 0; i < cloneListTC.Count; i++)
                {
                    Debug.Log(i + "-" + cloneListTC[i].y + "," + cloneListTC[i].x + "_" + board[cloneListTC[i].y, cloneListTC[i].x].chess.typeChess);
                    AddQuanThamChieu(cloneListTC[i]);
                }
                
            }
        }
        SoSanhTheChieu();
    }
    void AddQuanThamChieu(NodeBoard _node)
    {
        bool canAdd = true;
        for (int i = 0; i < listThamChieu.Count; i++)
        {
            if (_node.isEquals(listThamChieu[i]))
            {
                canAdd = false;
            }
        }
        if(canAdd)
            listThamChieu.Add(_node);
    }
    void SoSanhTheChieu()
    {
        for (int i = 0; i < listTheChieu.Count; i++)
        {
            Debug.Log("TheChieu " + i + ": [" + listTheChieu[i].y + "," + listTheChieu[i].x + "]");
        }
        for (int i = 0; i < listThamChieu.Count; i++)
        {
            Debug.Log("ThamChieu " + i + ": [" + listThamChieu[i].y + "," + listThamChieu[i].x + "]");
        }
        for (int i = 0; i < listTheChieu.Count; i ++)
            for(int j = 0; j < listThamChieu.Count; j++)
            {
                if (listThamChieu[j].isEquals(listTheChieu[i]))
                    listThamChieu.Remove(listThamChieu[j]);
            }
    }
    public List<NodeBoard> LayQuanDangChieu(bool isTeamRed, Node[,] _Board, List<NodeBoard> _listteamRed, List<NodeBoard> _listteamBlue)
    {
        List<NodeBoard> listQuanDangChieu = new List<NodeBoard>();
        if (isTeamRed)
        {
            for (int i = 0; i < _listteamRed.Count; i++)
            {
                switch (_Board[_listteamRed[i].y, _listteamRed[i].x].chess.typeChess)
                {
                    case TypeChess.Tuong:
                        {
                            if (_listteamRed[i].x == _listteamBlue[0].x && CoutLineY(_listteamRed[i], _listteamBlue[0]) == 0)
                            {
                                Debug.Log("ADD TUONG THAM CHIEU" + i);
                                listQuanDangChieu.Add(new NodeBoard(_listteamRed[i].y, _listteamRed[i].x));
                            }
                        }
                        break;
                    case TypeChess.Xe:
                        {
                                
                            if ((_listteamRed[i].y == _listteamBlue[0].y &&
                                CoutLineX(_listteamRed[i], _listteamBlue[0]) == 0) ||
                                (_listteamRed[i].x == _listteamBlue[0].x &&
                                CoutLineY(_listteamRed[i], _listteamBlue[0]) == 0))
                            {
                                Debug.Log("ADD XE CHIEU");
                                listQuanDangChieu.Add(new NodeBoard(_listteamRed[i].y, _listteamRed[i].x));
                            }
                            break;
                        }
                    case TypeChess.Phao:
                        {
                            if (CoutLineX(_listteamRed[i], _listteamBlue[0]) == 1 ||
                                CoutLineY(_listteamRed[i], _listteamBlue[0]) == 1)
                            {
                                listQuanDangChieu.Add(new NodeBoard(_listteamRed[i].y, _listteamRed[i].x));
                            }
                            break;
                        }
                    case TypeChess.Ma:
                        {
                            if (isMaMove(_listteamRed[i], _listteamBlue[0]) == true)
                            {
                                listQuanDangChieu.Add(new NodeBoard(_listteamRed[i].y, _listteamRed[i].x));
                            }
                            break;
                        }
                    case TypeChess.Chot:
                        {
                            if ((_listteamRed[i].x == _listteamBlue[0].x &&
                                _listteamRed[i].y - 1 == _listteamBlue[0].y) ||
                                (_listteamRed[i].y == _listteamBlue[0].y &&
                                _listteamRed[i].x - 1 == _listteamBlue[0].x) ||
                                (_listteamRed[i].y == _listteamBlue[0].y &&
                                _listteamRed[i].x + 1 == _listteamBlue[0].x))
                            {
                                listQuanDangChieu.Add(new NodeBoard(_listteamRed[i].y, _listteamRed[i].x));
                            }
                            break;
                        }
                    case TypeChess.Sy:
                        {
                            if (((_listteamRed[i].x - _listteamBlue[0].x == 1) ||
                                (_listteamRed[i].x - _listteamBlue[0].x == -1)) &&
                               ((_listteamRed[i].y - _listteamBlue[0].y == 1) ||
                                (_listteamRed[i].y - _listteamBlue[0].y == -1)))
                            {
                                listQuanDangChieu.Add(new NodeBoard(_listteamRed[i].y, _listteamRed[i].x));
                            }
                            break;
                        }
                    case TypeChess.Tinh:
                        {
                            if ((_listteamRed[i].x - _listteamBlue[0].x == 2) &&
                               (_listteamRed[i].y - _listteamBlue[0].y == 2))
                            {
                                if (mNode[_listteamRed[i].y - 1, _listteamRed[i].y - 1].isChess == false)
                                    listQuanDangChieu.Add(new NodeBoard(_listteamRed[i].y, _listteamRed[i].x));
                            }
                            else if ((_listteamRed[i].x - _listteamBlue[0].x == 2) &&
                               (_listteamRed[i].y - _listteamBlue[0].y == -2))
                            {
                                if (mNode[_listteamRed[i].y + 1, _listteamRed[i].x - 1].isChess == false)
                                    listQuanDangChieu.Add(new NodeBoard(_listteamRed[i].y, _listteamRed[i].x));
                            }
                            else if ((_listteamRed[i].x - _listteamBlue[0].x == -2) &&
                               (_listteamRed[i].y - _listteamRed[0].y == 2))
                            {
                                if (mNode[_listteamRed[i].y - 1, _listteamRed[i].x + 1].isChess == false)
                                    listQuanDangChieu.Add(new NodeBoard(_listteamRed[i].y, _listteamRed[i].x));
                            }
                            else if ((_listteamRed[i].x - _listteamBlue[0].x == -2) &&
                               (_listteamRed[i].y - _listteamBlue[0].y == -2))
                            {
                                if (mNode[_listteamRed[i].y + 1, _listteamRed[i].x + 1].isChess == false)
                                    listQuanDangChieu.Add(new NodeBoard(_listteamRed[i].y, _listteamRed[i].x));
                            }
                            break;
                        }
                }
            }
        }
        else
        {
            for (int i = 0; i < _listteamBlue.Count; i++)
            {
                switch (_Board[_listteamBlue[i].y, _listteamBlue[i].x].chess.typeChess)
                {
                    case TypeChess.Tuong:
                        {
                            if (_listteamBlue[i].x == _listteamRed[0].x && CoutLineY(_listteamBlue[i], _listteamRed[0]) == 0)
                            {
                                Debug.Log("ADD TUONG THAM CHIEU" + i);
                                listQuanDangChieu.Add(new NodeBoard(_listteamBlue[i].y, _listteamBlue[i].x));
                            }
                            break;
                
                        }
                    case TypeChess.Xe:
                        {
                 
                            if ((_listteamBlue[i].y == _listteamRed[0].y &&
                                CoutLineX(_listteamBlue[i], _listteamRed[0]) == 0) ||
                                (_listteamBlue[i].x == _listteamRed[0].x &&
                                CoutLineY(_listteamBlue[i], _listteamRed[0]) == 0))
                            {
                                Debug.Log("ADD XE CHIEU");
                                listQuanDangChieu.Add(new NodeBoard(_listteamBlue[i].y, _listteamBlue[i].x));
                            }

                            break;
                        }
                    case TypeChess.Phao:
                        {
                            if (CoutLineX(_listteamBlue[i], _listteamRed[0]) == 1 ||
                                CoutLineY(_listteamBlue[i], _listteamRed[0]) == 1)
                            {
                                listQuanDangChieu.Add(new NodeBoard(_listteamBlue[i].y, _listteamBlue[i].x));
                            }
                            break;
                        }
                    case TypeChess.Ma:
                        {
                            if (isMaMove(_listteamBlue[i], _listteamRed[0]) == true)
                            {
                                listQuanDangChieu.Add(new NodeBoard(_listteamBlue[i].y, _listteamBlue[i].x));
                            }
                            break;
                        }
                    case TypeChess.Chot:
                        {
                            if ((_listteamBlue[i].x == _listteamRed[0].x &&
                                _listteamBlue[i].y + 1 == _listteamRed[0].y) ||
                                (_listteamBlue[i].y == _listteamRed[0].y &&
                                _listteamBlue[i].x - 1 == _listteamRed[0].x) ||
                                (_listteamBlue[i].y == _listteamRed[0].y &&
                                _listteamBlue[i].x + 1 == _listteamRed[0].x))
                            {
                                listQuanDangChieu.Add(new NodeBoard(_listteamBlue[i].y, _listteamBlue[i].x));
                            }
                            break;
                        }
                    case TypeChess.Sy:
                        {
                            if (((_listteamBlue[i].x - _listteamRed[0].x == 1) ||
                                (_listteamBlue[i].x - _listteamRed[0].x == -1)) &&
                               ((_listteamBlue[i].y - _listteamRed[0].y == 1) ||
                                (_listteamBlue[i].y - _listteamRed[0].y == -1)))
                            {
                                listQuanDangChieu.Add(new NodeBoard(_listteamBlue[i].y, _listteamBlue[i].x));
                            }
                            break;
                        }
                    case TypeChess.Tinh:
                        {
                            if ((_listteamBlue[i].x - _listteamRed[0].x == 2) &&
                               (_listteamBlue[i].y - _listteamRed[0].y == 2))
                            {
                                if (mNode[_listteamBlue[i].y - 1, _listteamBlue[i].x - 1].isChess == false)
                                    listQuanDangChieu.Add(new NodeBoard(_listteamBlue[i].y, _listteamBlue[i].x));
                            }
                            else if ((_listteamBlue[i].x - _listteamRed[0].x == 2) &&
                               (_listteamBlue[i].y - _listteamRed[0].y == -2))
                            {
                                if (mNode[_listteamBlue[i].y + 1, _listteamBlue[i].x - 1].isChess == false)
                                    listQuanDangChieu.Add(new NodeBoard(_listteamBlue[i].y, _listteamBlue[i].x));
                            }
                            else if ((_listteamBlue[i].x - _listteamRed[0].x == -2) &&
                               (_listteamBlue[i].y - _listteamRed[0].y == 2))
                            {
                                if (mNode[_listteamBlue[i].y - 1, _listteamBlue[i].x + 1].isChess == false)
                                    listQuanDangChieu.Add(new NodeBoard(_listteamBlue[i].y, _listteamBlue[i].x));
                            }
                            else if ((_listteamBlue[i].x - _listteamRed[0].x == -2) &&
                               (_listteamBlue[i].y - _listteamRed[0].y == -2))
                            {
                                if (mNode[_listteamBlue[i].y + 1, _listteamBlue[i].x + 1].isChess == false)
                                    listQuanDangChieu.Add(new NodeBoard(_listteamBlue[i].y, _listteamBlue[i].x));
                            }
                            break;
                        }
                }
            }
        }
        return listQuanDangChieu;
    }
    
    
    
	//---------------------------------------------------
    public bool ChieuDoiThu(bool isTeamRed,List<NodeBoard> listRed,List<NodeBoard> listBlue, Node[,] newBoard)//WARNING DANG CHIEU TUONG DOI THU
    {
        if (isTeamRed)
        {

            for (int i = 0; i < listRed.Count; i++)
            {
                switch (newBoard[listRed[i].y, listRed[i].x].chess.typeChess)
                {
                    case TypeChess.Tuong:
                        {
              
                            if(listRed[i].x == listBlue[0].x && CoutLineY(listRed[i], listBlue[0])==0)
                            {
                                return true;
                            }                            
                            break;
               
                        }
                    case TypeChess.Xe:
                        {
             
                            if ((listRed[i].y == listBlue[0].y &&
                                CoutLineX(listRed[i], listBlue[0]) == 0) ||
                                (listRed[i].x == listBlue[0].x &&
                                CoutLineY(listRed[i], listBlue[0]) == 0))
                            {
                                return true;
                            }

                            break;
                    
                        }
                    case TypeChess.Phao:
                        {
             
                            if (CoutLineX(listRed[i], listBlue[0]) == 1 ||
                                CoutLineY(listRed[i], listBlue[0]) == 1)
                            {
                                return true;
                            }
                            break;
                   
                        }
                    case TypeChess.Ma:
                        {
             
                            if (isMaMove(listRed[i], listBlue[0]) == true)
                            {
                                return true;
                            }
                            break;
                   
                        }
                    case TypeChess.Chot:
                        {
               
                            if ((listRed[i].x == listBlue[0].x &&
                                listRed[i].y - 1 == listBlue[0].y) ||
                                (listRed[i].y == listBlue[0].y &&
                                listRed[i].x - 1 == listBlue[0].x) ||
                                (listRed[i].y == listBlue[0].y &&
                                listRed[i].x + 1 == listBlue[0].x))
                            {
                                return true;
                            }
                            break;
                   

                        }
                    case TypeChess.Sy:
                        {
                            if(((listRed[i].x - listBlue[0].x==1)||
                                (listRed[i].x - listBlue[0].x== -1))&&
                               ((listRed[i].y - listBlue[0].y == 1)||
                                (listRed[i].y - listBlue[0].y == -1)))
                            {
                                return true;
                            }
                            break;
                        }
                    case TypeChess.Tinh:
                        {
                           if((listRed[i].x - listBlue[0].x ==2)&&
                              (listRed[i].y - listBlue[0].y == 2))
                            {
                                if (newBoard[listRed[i].y - 1, listRed[i].y - 1].isChess==false)
                                    return true;
                            }
                           else if ((listRed[i].x - listBlue[0].x == 2) &&
                              (listRed[i].y - listBlue[0].y == -2))
                            {
                                if (newBoard[listRed[i].y + 1, listRed[i].x - 1].isChess==false)
                                    return true;
                            }
                            else if ((listRed[i].x - listBlue[0].x == -2) &&
                               (listRed[i].y - listBlue[0].y == 2))
                            {
                                if (newBoard[listRed[i].y - 1, listRed[i].x + 1].isChess==false)
                                    return true;
                            }
                            else if ((listRed[i].x - listBlue[0].x == -2) &&
                               (listRed[i].y - listBlue[0].y == -2))
                            {
                                if (newBoard[listRed[i].y + 1, listRed[i].x + 1].isChess==false)
                                    return true;
                            }
                            break;
                        }
                }
            }
     
        }
        else
        {            

            for (int i = 0; i < listBlue.Count; i++)
            {
                switch (newBoard[listBlue[i].y, listBlue[i].x].chess.typeChess)
                {
                    case TypeChess.Tuong:
                        {
              
                            if (listBlue[i].x == listRed[0].x && CoutLineY(listBlue[i], listRed[0]) == 0)
                            {
                                return true;
                            }
                            break;
         
                        }
                    case TypeChess.Xe:
                        {
             
                            if ((listBlue[i].y == listRed[0].y &&
                                CoutLineX(listBlue[i], listRed[0]) == 0) ||
                                (listBlue[i].x == listRed[0].x &&
                                CoutLineY(listBlue[i], listRed[0]) == 0))
                            {
                                return true;
                            }

                            break;
         
                        }
                    case TypeChess.Phao:
                        {
          
                            if (CoutLineX(listBlue[i], listRed[0]) == 1 ||
                                CoutLineY(listBlue[i], listRed[0]) == 1)
                            {
                                return true;
                            }
                            break;
              
                        }
                    case TypeChess.Ma:
                        {
                
                            if (isMaMove(listBlue[i], listRed[0]) == true)
                            {
                                return true;
                            }
                            break;
                        
                        }
                    case TypeChess.Chot:
                        {
                 
                            if ((listBlue[i].x == listRed[0].x &&
                                listBlue[i].y + 1 == listRed[0].y) ||
                                (listBlue[i].y == listRed[0].y &&
                                listBlue[i].x - 1 == listRed[0].x) ||
                                (listBlue[i].y == listRed[0].y &&
                                listBlue[i].x + 1 == listRed[0].x))
                            {
                                return true;
                            }
                            break;
             

                        }
                    case TypeChess.Sy:
                        {
                            if (((listBlue[i].x - listRed[0].x == 1) ||
                                (listBlue[i].x - listRed[0].x == -1)) &&
                               ((listBlue[i].y - listRed[0].y == 1) ||
                                (listBlue[i].y - listRed[0].y == -1)))
                            {
                                return true;
                            }
                            break;
                        }
                    case TypeChess.Tinh:
                        {
                            if ((listBlue[i].x - listRed[0].x == 2) &&
                               (listBlue[i].y - listRed[0].y == 2))
                            {
                                if (newBoard[listBlue[i].y - 1, listBlue[i].x - 1].isChess==false)
                                    return true;
                            }
                            else if ((listBlue[i].x - listRed[0].x == 2) &&
                               (listBlue[i].y - listRed[0].y == -2))
                            {
                                if (newBoard[listBlue[i].y + 1, listBlue[i].x - 1].isChess==false)
                                    return true;
                            }
                            else if ((listBlue[i].x - listRed[0].x == -2) &&
                               (listBlue[i].y - listRed[0].y == 2))
                            {
                                if (newBoard[listBlue[i].y - 1, listBlue[i].x + 1].isChess==false)
                                    return true;
                            }
                            else if ((listBlue[i].x - listRed[0].x == -2) &&
                               (listBlue[i].y - listRed[0].y == -2))
                            {
                                if (newBoard[listBlue[i].y + 1, listBlue[i].x + 1].isChess==false)
                                    return true;
                            }
                            break;
                        }
                }
            }
   
        }
        return false;
    }

   public int CoutLineX(NodeBoard _posChessA, NodeBoard _posChessB)
    {
        int num = 0;

        if (_posChessA.y == _posChessB.y)
        {
            for (int i = 0; i < listChessBlue.Count; i++)
            {
                if (listChessBlue[i].y == _posChessA.y)
                {
                    if ((_posChessA.x > listChessBlue[i].x && _posChessB.x < listChessBlue[i].x) ||
                        (_posChessA.x < listChessBlue[i].x && _posChessB.x > listChessBlue[i].x))
                        num++;
                }
            }

            for(int i = 0;i<listChessRed.Count;i++)
            {
                if (listChessRed[i].y == _posChessA.y)
                {
                    if ((_posChessA.x > listChessRed[i].x && _posChessB.x < listChessRed[i].x) ||
                        (_posChessA.x < listChessRed[i].x && _posChessB.x > listChessRed[i].x))
                        num++;
                }
            }
        }
                
        return num;
    }

    public int CoutLineY(NodeBoard _posStart, NodeBoard _posEnd)
    {
        int num = 0;

        if (_posStart.x == _posEnd.x)
        {
            for (int i = 0; i < listChessBlue.Count; i++)
            {
                if (listChessBlue[i].x == _posStart.x)
                {
                    if ((_posStart.y > listChessBlue[i].y && _posEnd.y < listChessBlue[i].y) ||
                        (_posStart.y < listChessBlue[i].y && _posEnd.y > listChessBlue[i].y))
                        num++;
                }
            }

            for (int i = 0; i < listChessRed.Count; i++)
            {
                if (listChessRed[i].x == _posStart.x)
                {
                    if ((_posStart.y > listChessRed[i].y && _posEnd.y < listChessRed[i].y) ||
                        (_posStart.y < listChessRed[i].y && _posEnd.y > listChessRed[i].y))
                        num++;
                }
            }

        }

        return num;
    }

    int CountLineX(NodeBoard _posChessA, NodeBoard _posChessB, NodeBoard _posStart, NodeBoard _posEnd)
    {
        int num = 0;
        if (!_posChessA.isEquals(_posStart) &&
            !_posChessA.isEquals(_posEnd) &&
            !_posChessB.isEquals(_posStart) &&
            !_posChessB.isEquals(_posEnd))
        {
            if (_posChessA.y == _posChessB.y)
            {
   
                for (int i = 0; i < listChessBlue.Count; i++)
                {
                    if (!listChessBlue[i].isEquals(_posStart))
                    {
                        if (!listChessBlue[i].isEquals(_posEnd) &&
                            (listChessBlue[i].y == _posChessA.y))
                        {
                            if ((_posChessA.x > listChessBlue[i].x && _posChessB.x < listChessBlue[i].x) ||
                                (_posChessA.x < listChessBlue[i].x && _posChessB.x > listChessBlue[i].x))
                            {
                                num++;
                                if (num > 1)
                                    return num;
                            }

                        }

                    }
                    else
                    {
                        if (_posEnd.y == _posChessA.y)
                        {
                            if ((_posChessA.x > _posEnd.x && _posChessB.x < _posEnd.x) ||
                                    (_posChessA.x < _posEnd.x && _posChessB.x > _posEnd.x))
                            {
                                num++;
                                if (num > 1)
                                    return num;
                            }
                        }
                    }
                }
      
                for (int i = 0; i < listChessRed.Count; i++)
                {
                    if (!listChessRed[i].isEquals(_posStart))
                    {
                        if (!listChessRed[i].isEquals(_posEnd) &&
                            (listChessRed[i].y == _posChessA.y))
                        {
                            if ((_posChessA.x > listChessRed[i].x && _posChessB.x < listChessRed[i].x) ||
                                (_posChessA.x < listChessRed[i].x && _posChessB.x > listChessRed[i].x))
                            {
                                num++;
                                if (num > 1)
                                    return num;
                            }

                        }

                    }
                    else
                    {
                        if (_posEnd.y == _posChessA.y)
                        {
                            if ((_posChessA.x > _posEnd.x && _posChessB.x < _posEnd.x) ||
                                    (_posChessA.x < _posEnd.x && _posChessB.x > _posEnd.x))
                            {
                                num++;
                                if (num > 1)
                                    return num;
                            }
                        }
                    }
                }
      
            }
            else
            {
                return 2;
            }
        }
        else
        {
            if (_posChessA.isEquals(_posStart))
            {

                if (_posEnd.y == _posChessB.y)
                {
               
                    //blue
                    for (int i = 0; i < listChessBlue.Count; i++)
                    {
                        if (!listChessBlue[i].isEquals(_posChessA))
                        {
                            if (listChessBlue[i].y == _posEnd.y)
                            {
                                if ((_posEnd.x > listChessBlue[i].x && _posChessB.x < listChessBlue[i].x) ||
                                        (_posEnd.x < listChessBlue[i].x && _posChessB.x > listChessBlue[i].x))
                                {
                                    num++;
                                    if (num > 1)
                                    {
                                        return num;
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < listChessRed.Count; i++)
                    {
                        if (!listChessRed[i].isEquals(_posChessA))
                        {
                            if (listChessRed[i].y == _posEnd.y)
                            {
                                if ((_posEnd.x > listChessRed[i].x && _posChessB.x < listChessRed[i].x) ||
                                    (_posEnd.x < listChessRed[i].x && _posChessB.x > listChessRed[i].x))
                                {
                                    num++;
                                    if (num > 1)
                                    {
                                        return num;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    return 2;
                }
            }

            if (_posChessB.isEquals(_posStart))
            {

                if (_posEnd.y == _posChessA.y)
                {
     
                    //blue
                    for (int i = 0; i < listChessBlue.Count; i++)
                    {
                        if (!listChessBlue[i].isEquals(_posChessB))
                        {
                            if (listChessBlue[i].y == _posEnd.y)
                            {
                                if ((_posEnd.x > listChessBlue[i].x && _posChessA.x < listChessBlue[i].x) ||
                                        (_posEnd.x < listChessBlue[i].x && _posChessA.x > listChessBlue[i].x))
                                {
                                    num++;
                                    if (num > 1)
                                    {
                                        return num;
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < listChessRed.Count; i++)
                    {
                        if (!listChessRed[i].isEquals(_posChessB))
                        {
                            if (listChessRed[i].y == _posEnd.y)
                                if ((_posEnd.x > listChessRed[i].x && _posChessA.x < listChessRed[i].x) ||
                                        (_posEnd.x < listChessRed[i].x && _posChessA.x > listChessRed[i].x))
                                {
                                    num++;
                                    if (num > 1)
                                    {
                                        return num;
                                    }
                                }
                        }
                    }

   
                }
                else
                {
                    return 2;
                }
            }
        }
        return num;

    }

    int CountLineY(NodeBoard _posChessA, NodeBoard _posChessB, NodeBoard _posStart, NodeBoard _posEnd)
    {
        int num = 0;
        if (!_posChessA.isEquals(_posStart) &&
            !_posChessA.isEquals(_posEnd) &&
            !_posChessB.isEquals(_posStart) &&
            !_posChessB.isEquals(_posEnd))
        {
            if (_posChessA.x == _posChessB.x)
            {
    
                for (int i = 0; i < listChessBlue.Count; i++)
                {
                    if (!listChessBlue[i].isEquals(_posStart))
                    {
                        if (!listChessBlue[i].isEquals(_posEnd) &&
                            (listChessBlue[i].x == _posChessA.x))
                        {
                            if ((_posChessA.y > listChessBlue[i].y && _posChessB.y < listChessBlue[i].y) ||
                                (_posChessA.y < listChessBlue[i].y && _posChessB.y > listChessBlue[i].y))
                            {
                                num++;
                                if (num > 1)
                                    return num;
                            }

                        }

                    }
                    else
                    {
                        if (_posEnd.x == _posChessA.x)
                        {
                            if ((_posChessA.y > _posEnd.y && _posChessB.y < _posEnd.y) ||
                                    (_posChessA.y < _posEnd.y && _posChessB.y > _posEnd.y))
                            {
                                num++;
                                if (num > 1)
                                    return num;
                            }
                        }
                    }
                }
 
                for (int i = 0; i < listChessRed.Count; i++)
                {
                    if (!listChessRed[i].isEquals(_posStart))
                    {
                        if (!listChessRed[i].isEquals(_posEnd) &&
                            (listChessRed[i].x == _posChessA.x))
                        {
                            if ((_posChessA.y > listChessRed[i].y && _posChessB.y < listChessRed[i].y) ||
                                (_posChessA.y < listChessRed[i].y && _posChessB.y > listChessRed[i].y))
                            {
                                num++;
                                if (num > 1)
                                    return num;
                            }

                        }

                    }
                    else
                    {
                        if (_posEnd.x == _posChessA.x)
                        {
                            if ((_posChessA.y > _posEnd.y && _posChessB.y < _posEnd.y) ||
                                    (_posChessA.y < _posEnd.y && _posChessB.y > _posEnd.y))
                            {
                                num++;
                                if (num > 1)
                                    return num;
                            }
                        }
                    }
                }
      
            }
            else
            {
                return 2;
            }
        }
        else
        {
            if (_posChessA.isEquals(_posStart))
            {

                if (_posEnd.x == _posChessB.x)
                {
        
                    //blue
                    for (int i = 0; i < listChessBlue.Count; i++)
                    {
                        if (!listChessBlue[i].isEquals(_posChessA))
                        {
                            if (listChessBlue[i].x == _posEnd.x)
                            {
                                if ((_posEnd.y > listChessBlue[i].y && _posChessB.y < listChessBlue[i].y) ||
                                        (_posEnd.y < listChessBlue[i].y && _posChessB.y > listChessBlue[i].y))
                                {
                                    num++;
                                    if (num > 1)
                                    {
                                        return num;
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < listChessRed.Count; i++)
                    {
                        if (!listChessRed[i].isEquals(_posChessA))
                        {
                            if (listChessRed[i].x == _posEnd.x)
                            {
                                if ((_posEnd.y > listChessRed[i].y && _posChessB.y < listChessRed[i].y) ||
                                    (_posEnd.y < listChessRed[i].y && _posChessB.y > listChessRed[i].y))
                                {
                                    num++;
                                    if (num > 1)
                                    {
                                        return num;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    return 2;
                }
            }

            if (_posChessB.isEquals(_posStart))
            {
                
                if (_posEnd.x == _posChessA.x)
                {
    
                    //blue
                    for (int i = 0; i < listChessBlue.Count; i++)
                    {
                        if (!listChessBlue[i].isEquals(_posChessB))
                        {
                            if (listChessBlue[i].x == _posEnd.x)
                            {
                                if ((_posEnd.y > listChessBlue[i].y && _posChessA.y < listChessBlue[i].y) ||
                                        (_posEnd.y < listChessBlue[i].y && _posChessA.y > listChessBlue[i].y))
                                {
                                    num++;
                                    if (num > 1)
                                    {
                                        return num;
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < listChessRed.Count; i++)
                    {
                        if (!listChessRed[i].isEquals(_posChessB))
                        {
                            if (listChessRed[i].x == _posEnd.x)
                                if ((_posEnd.y > listChessRed[i].y && _posChessA.y < listChessRed[i].y) ||
                                        (_posEnd.y < listChessRed[i].y && _posChessA.y > listChessRed[i].y))
                                {
                                    num++;
                                    if (num > 1)
                                    {
                                        return num;
                                    }
                                }
                        }
                    }
                }
                else
                {
                    return 2;
                }
            }
        }
        return num;
       
    }

    bool isMaMove(NodeBoard _posStart, NodeBoard _posEnd)
    {
        int x = _posEnd.x - _posStart.x;
        int y = _posEnd.y - _posStart.y;
        if(Mathf.Abs(x)+Mathf.Abs(y) == 3)
        {
            if(x==2)
            {
                if(mNode[_posStart.y,_posStart.x+1].chess.typeChess == TypeChess.None)
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
                if (mNode[_posStart.y+1, _posStart.x].chess.typeChess == TypeChess.None)
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

    bool isChieuSy(NodeBoard _posSy, NodeBoard _posTuong, NodeBoard _posStart, NodeBoard _posEnd)
    {
        // NodeBoard posSy, posTuong;
        NodeBoard posSy, posTuong;
        if (!_posSy.isEquals(_posStart) &&
            !_posSy.isEquals(_posEnd) &&
            !_posTuong.isEquals(_posStart) &&
            !_posTuong.isEquals(_posEnd))
        {
            posSy = new NodeBoard(_posSy);
            posTuong = new NodeBoard(_posTuong);
        }
        else
        {
            if (_posSy.isEquals(_posStart))
            {
                posSy = new NodeBoard(_posEnd);
                posTuong = new NodeBoard(_posTuong);
            }
            else
            {
                posSy = new NodeBoard(_posSy);
                posTuong = new NodeBoard(_posEnd);
            }
        }

        if (((posSy.x - posTuong.x == 1) ||
            (posSy.x - posTuong.x == -1)) &&
            ((posSy.y - posTuong.y == 1) ||
            (posSy.y - posTuong.y == -1)))
        {
            return true;
        }
        return false;
    }

    bool isChieuTinh(NodeBoard _posTinh, NodeBoard _posTuong, NodeBoard _posStart, NodeBoard _posEnd)
    {
        NodeBoard posTinh, posTuong;
        if (!_posTinh.isEquals(_posStart) &&
            !_posTinh.isEquals(_posEnd) &&
            !_posTuong.isEquals(_posStart) &&
            !_posTuong.isEquals(_posEnd))
        {
            posTinh = new NodeBoard(_posTinh);
            posTuong = new NodeBoard(_posTuong);
        }
        else
        {
            if (_posTinh.isEquals(_posStart))
            {
                posTinh = new NodeBoard(_posEnd);
                posTuong = new NodeBoard(_posTuong);
            }
            else
            {
                posTinh = new NodeBoard(_posTinh);
                posTuong = new NodeBoard(_posEnd);
            }
        }

        if ((posTinh.x - posTuong.x == 2) &&
           (posTinh.y - posTuong.y == 2))
        {
            if (posTinh.AddPos(-1, -1).isEquals(_posStart))
            {
                return true;
            }
            else
            {
                if (mNode[posTinh.y-1, posTinh.x - 1].chess.typeChess == TypeChess.None &&
                    posTinh.AddPos(-1,-1).isEquals(_posEnd) == false)
                {
                    return true;
                }
            }
        }


        if ((posTinh.x - posTuong.x == 2) &&
           (posTinh.y - posTuong.y == -2))
        {
            if (posTinh.AddPos(1, -1).isEquals(_posStart))
            {
                return true;
            }
            else
            {
                if (mNode[posTinh.y + 1, posTinh.x - 1].chess.typeChess == TypeChess.None &&
                    posTinh.AddPos(+1, -1).isEquals(_posEnd) == false)
                {
                    return true;
                }
            }
        }

        if ((posTinh.x - posTuong.x == -2) &&
           (posTinh.y - posTuong.y == 2))
        {
            if (posTinh.AddPos(-1, 1).isEquals(_posStart))
            {
                return true;
            }
            else
            {
                if (mNode[posTinh.y - 1, posTinh.x + 1].chess.typeChess == TypeChess.None &&
                    posTinh.AddPos(-1, +1).isEquals(_posEnd) == false)
                {
                    return true;
                }
            }
        }
        
        if ((posTinh.x - posTuong.x == - 2) &&
           (posTinh.y - posTuong.y == - 2))
        {
            if (posTinh.AddPos(1, 1).isEquals(_posStart))
            {
                return true;
            }
            else
            {
                if (mNode[posTinh.y + 1, posTinh.x + 1].chess.typeChess == TypeChess.None &&
                    posTinh.AddPos(+1, +1).isEquals(_posEnd) == false)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool isChieuMa(NodeBoard _posMa, NodeBoard _posTuong, NodeBoard _posStart, NodeBoard _posEnd)
    {
        NodeBoard posMa, posTuong;
        if (!_posMa.isEquals(_posStart) &&
            !_posMa.isEquals(_posEnd) &&
            !_posTuong.isEquals(_posStart) &&
            !_posTuong.isEquals(_posEnd))
        {
            posMa = new NodeBoard(_posMa);
            posTuong = new NodeBoard(_posTuong);
        }
        else
        {
            if (_posMa.isEquals(_posStart))
            {
                posMa = new NodeBoard(_posEnd);
                posTuong = new NodeBoard(_posTuong);
            }
            else
            {
                posMa = new NodeBoard(_posMa);
                posTuong = new NodeBoard(_posEnd);
            }
        }
        int x =  posTuong.x - posMa.x;
        int y =  posTuong.y - posMa.y;
        if (Mathf.Abs(x) + Mathf.Abs(y) == 3)
        {
            if (x == 2)
            {
                if (posMa.AddPosX(1).isEquals(_posStart) == true)
                {
                    return true;
                }
                else
                {
                    if (mNode[posMa.y, posMa.x + 1].chess.typeChess == TypeChess.None &&
                    posMa.AddPosX(1).isEquals(_posEnd) == false)
                    {
                        return true;
                    }
                }
            }
            if (x == -2)
            {
                if (posMa.AddPosX(-1).isEquals(_posStart) == true)
                {
                    return true;
                }
                else
                {
                    if (mNode[posMa.y, posMa.x - 1].chess.typeChess == TypeChess.None &&
                    posMa.AddPosX(-1).isEquals(_posEnd) == false)
                    {
                        return true;
                    }
                }
            }
            if (y == 2)
            {
                if (posMa.AddPosY(1).isEquals(_posStart) == true)
                {
                    return true;
                }
                else
                {
                    if (mNode[posMa.y+1, posMa.x].chess.typeChess == TypeChess.None &&
                    posMa.AddPosY(1).isEquals(_posEnd) == false)
                    {
                        return true;
                    }
                }
            }
            if (y == -2)
            {
                if (posMa.AddPosY(-1).isEquals(_posStart) == true)
                {
                    return true;
                }
                else
                {
                    if (mNode[posMa.y - 1, posMa.x].chess.typeChess == TypeChess.None &&
                    posMa.AddPosY(-1).isEquals(_posEnd) == false)
                    {
                        return true;
                    }
                }
            }

        }

        return false;
    }

    public bool isMove(NodeBoard _posStart, NodeBoard _posMove)//CHECK CAN MOVE NEW POSITION
    {
        if (mNode[_posMove.y, _posMove.x].chess.typeChess != TypeChess.None)
        {
            if (mNode[_posMove.y, _posMove.x].chess.teamRed == mNode[_posStart.y, _posStart.x].chess.teamRed)
            {
                return false;
            }
            else
            {
                return !isWanning(_posStart, _posMove);//DI DUOC , CO THE AN QUAN
            }
        }
        else
        {
            return !isWanning(_posStart, _posMove);//DI DUOC , KHONG CO QUAN
        }
    }
    public bool isEmuMoveBi(NodeBoard _posStart, NodeBoard _posMove)
    {
        if (mNode[_posMove.y, _posMove.x].chess.typeChess != TypeChess.None)
        {
            if (mNode[_posMove.y, _posMove.x].chess.teamRed == mNode[_posStart.y, _posStart.x].chess.teamRed)
            {
                return false;
            }
            else//KHAC TEAM , AN DUOC KO ?
            {
                if (isWanning(_posStart, _posMove))
                    return true;
                else
                    return false;
            }
        }
        else//CHO TRONG
        {
            if (isWanning(_posStart, _posMove))
                return true;
            else
                return false;
        }
    }

	public void RemoveChess(int _Oder,bool _OnNext,int _index,bool _isRealTime,TypeChess _isTypeEnd,bool _isEndShow,bool _isChessEnd,bool _isTeamRedEnd,bool _isTeamRedShowEnd,NodeBoard _node,NodeBoard _nodeSpecial)
    {//TINH AN QUAN
		if (_OnNext) {
			if (_isChessEnd) {
				if (mNode [_node.y, _node.x].chess.teamRed) {
					for (int i = 0; i < listChessRed.Count; i++) {
						if (listChessRed [i].y == _node.y && listChessRed [i].x == _node.x) {
							GameController.instance.Players[_Oder].EatedChessBool.Add (mNode [_node.y, _node.x].chess.show);
							GameController.instance.Players[_Oder].EatedChessRedTeam.Add (mNode [_node.y, _node.x].chess.teamRedShow);
							GameController.instance.Players[_Oder].EatedChess.Add (mNode [_node.y, _node.x].chess.typeChessShow.ToString ());
							GameController.instance.Players[_Oder].CreateChessEated (_Oder, _isTypeEnd.ToString(),_isEndShow,_isTeamRedShowEnd);
							listChessRed.RemoveAt (i);
							break;
						}
					}
				} else {
					for (int i = 0; i < listChessBlue.Count; i++) {
						if (listChessBlue [i].y == _node.y && listChessBlue [i].x == _node.x) {
							GameController.instance.Players[_Oder].EatedChessBool.Add (mNode [_node.y, _node.x].chess.show);
							GameController.instance.Players[_Oder].EatedChessRedTeam.Add (mNode [_node.y, _node.x].chess.teamRedShow);
							GameController.instance.Players[_Oder].EatedChess.Add (mNode [_node.y, _node.x].chess.typeChessShow.ToString ());
							GameController.instance.Players[_Oder].CreateChessEated (_Oder, _isTypeEnd.ToString (), _isEndShow,_isTeamRedShowEnd);
							listChessBlue.RemoveAt (i);
							break;
						}
					}
				}
				//GameController.instance.PlayEatChess ();//SOUND
				if (_isRealTime) {
					int RdomEffect = UnityEngine.Random.Range (0, LoadPrefab.instance.EatEffect.Length);
					GameObject CloneEffect = GameObject.Instantiate (LoadPrefab.instance.EatEffect [RdomEffect]);
					CloneEffect.transform.position = mNode [_node.y, _node.x].transform.position;
					CloneEffect.transform.rotation = mNode [_node.y, _node.x].transform.rotation;
				}
				listChessBlueSave = new List<List<NodeBoard>>();
				listChessRedSave = new List<List<NodeBoard>>();
			}
		} else {
			if (_isChessEnd) {
				if (_isTeamRedEnd)
					Board.instance.listChessRed.Add (_nodeSpecial);
				else
					Board.instance.listChessBlue.Add (_nodeSpecial);
				GameController.instance.Players[_Oder].DeleteFinalChess (_Oder);
			}
		}
        
       	AddListOldChess();
    }

    void AddListOldChess()
    {
        List<NodeBoard> listOldChessBlue = new List<NodeBoard>();
        List<NodeBoard> listOldChessRed = new List<NodeBoard>();
        for(int i =0;i<listChessBlue.Count;i++)
        {
            listOldChessBlue.Add(new NodeBoard(listChessBlue[i]));
        }
        for (int i = 0; i < listChessRed.Count; i++)
        {
            listOldChessRed.Add(new NodeBoard(listChessRed[i]));
        }

        listChessBlueSave.Add(listOldChessBlue);
        listChessRedSave.Add(listOldChessRed);
    }

    //luat choi 
    public List<List<NodeBoard>> listChessRedSave = new List<List<NodeBoard>>(), listChessBlueSave = new List<List<NodeBoard>>();

    public void CheckBatBien(NodeBoard _node,List<NodeBoard> listRed,List<NodeBoard> listBlue,Node[,] _newBoard)
    {
        if (_newBoard[_node.y, _node.x].chess.typeChess != TypeChess.None)
        {
            GameController.numBatBien = 0;
            GameController.numKhongAnQuan = 0;
        }
        else
        {
            GameController.numKhongAnQuan++;
            GameController.numBatBien = 0;
            for (int i = listChessRedSave.Count - 2; i >= 0; i = i - 2)
            {
                if(isEqualsListNodeBoard(listChessRedSave[i],listChessRed) == true &&
                   isEqualsListNodeBoard(listChessBlueSave[i], listChessBlue) == true)
                {
                    GameController.numBatBien++;
                }
            }
        }
    }

    bool isEqualsListNodeBoard(List<NodeBoard> listA, List<NodeBoard> listB)
    {
        if(listA.Count == listB.Count)
        {
            for(int i=0;i<listA.Count;i++)
            {
                if(!listA[i].isEquals(listB[i]))
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            return false;
        }
        
    }

/////////////////////// online

    public void MoveChess(int _idChess,int posStartX,int posStartY,int posEndX,int posEndY,int _typeChessEat,bool _isLatQuan)
    {
       // mNode[posEndY, posEndX].MoveGameObject( new NodeBoard(posStartY,posStartX),_idChess,_typeChessEat, _isLatQuan);
        
    }

    public int NumBatBien(NodeBoard nodeStart,NodeBoard nodeEnd)
    {
        if (mNode[nodeEnd.y, nodeEnd.x].isChess)
            return 0;
        else
        {
            if (mNode[nodeStart.y, nodeStart.x].chess.show == false)
                return 0;

            List<NodeBoard> tempListChessTeamRed = new List<NodeBoard>();
            foreach(var node in listChessRed)
            {
                tempListChessTeamRed.Add(new NodeBoard( node));
            }

            List<NodeBoard> tempListChessTeamBlue = new List<NodeBoard>();
            foreach (var node in listChessBlue)
            {
                tempListChessTeamBlue.Add(new NodeBoard(node));
            }

            //xu ly 
            if (mNode[nodeStart.y, nodeStart.x].chess.teamRed)
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

            GameController.numBatBien = 0;
            for (int i = listChessRedSave.Count - 2; i >= 0; i = i-2)
            {
                if (isEqualsListNodeBoard(listChessRedSave[i], tempListChessTeamRed) == true &&
                   isEqualsListNodeBoard(listChessBlueSave[i], tempListChessTeamBlue) == true)
                {
                    GameController.numBatBien++;
                }
            }
            return GameController.numBatBien;
        }
    }

}

