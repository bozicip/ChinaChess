using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading;

namespace AICoTuong
{
    #region Point
    public class Point
    {
        public int x;
        public int y;

        public Point(Point point)
        {
            this.x = point.x;
            this.y = point.y;
        }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public bool CheckPoint(int _x, int _y)
        {
            if (this.x == _x && this.y == _y)
                return true;
            return false;
        }
        public bool CheckPoint(Point point)
        {
            if (this.x == point.x && this.y == point.y)
                return true;
            return false;
        }
    }
    #endregion
    public class idChess{
        public const byte KingRed = 1;
        public const byte RookRed =  2;
        public const byte CannonRed = 3; 
        public const byte KnightRed = 4; 
        public const byte ElephantRed = 5; 
        public const byte CBishopRed = 6; 
        public const byte BishopRed=   7;

        public const byte KingBlue = 11;
        public const byte RookBlue = 12;
        public const byte CannonBlue = 13;
        public const byte KnightBlue = 14;
        public const byte ElephantBlue = 15;
        public const byte CBishopBlue = 16;
        public const byte BishopBlue = 17;
    }

    #region State
    public class State
    {
        public Point prev, curr;
        public CellBoard value1 = new CellBoard(), value2 =new CellBoard();
        public int score;
        public State(Point p, Point c, CellBoard val1, CellBoard val2)
        {
            this.prev = new Point(p);
            this.curr = new Point(c);
            this.value1.SetSellBoard( val1);
            this.value2.SetSellBoard( val2);
        }
        public State(State _state)
        {
            this.prev = _state.prev;
            this.curr = _state.curr;
            this.value1.SetSellBoard(_state.value1);
            this.value2.SetSellBoard(_state.value2);
            this.score = _state.score;
        }

    }
    #endregion

    #region Piece
    public abstract class Piece
    {
        public Point CurrMove;
        public BoardAI board;
        public string name;
        public bool RED;
        public List<State> allPossibleMove;
        public bool isShow;
        public Piece(BoardAI _board, Point currMove,bool _isShow)
        {
            this.isShow = _isShow;
            board = _board;
            this.CurrMove = currMove;
            byte val = board.cell[currMove.x, currMove.y].idChess;
            switch (val)
            {
                case 8:
                case 15:
                    name = "KING";
                    break;
                case 9:
                case 16:
                    name = "BISHOP";
                    break;
                case 10:
                case 17:
                    name = "ELEPHANT";
                    break;
                case 11:
                case 18:
                    name = "KNIGHT";
                    break;
                case 12:
                case 19:
                    name = "ROOK";
                    break;
                case 13:
                case 20:
                    name = "CANNON";
                    break;
                case 14:
                case 21:
                    name = "PAWN";
                    break;
            }
            this.RED = val > 14;
            allPossibleMove = null;
        }

        public abstract List<State> FindAllPossibleMoves();

        public abstract bool checkProject(Point King);

        public bool checkMove(int x, int y)
        {
            try
            {
                int n = allPossibleMove.Count;
                for (int i = 0; i < n; i++)
                {
                    Point pos = allPossibleMove[i].curr;
                    if (pos.x == x && pos.y == y)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR checkMove, Piece: " + e.ToString());
            }
            return false;
        }

        public bool checkProject(BoardAI board1, List<Point> arr)
        {
            bool isCheck = false;
            try
            {
                Point pKing = arr[0];
                for (int i = 1; i < arr.Count; i++)
                {
                    Point pos = arr[i];
                    byte value = board1.cell[pos.x, pos.y].idChess;
                    bool isShow  = board1.cell[pos.x, pos.y].isShow;
                    switch (value)
                    {
                        case 8:
                        case 15:
                            CKing king = new CKing(board1, pos, isShow);
                            isCheck = king.checkProject(pKing);
                            break;
                        case 9:
                        case 16:
                            CBishop bishop = new CBishop(board1, pos, isShow);
                            isCheck = bishop.checkProject(pKing);
                            break;
                        case 10:
                        case 17:
                            CElephant elephant = new CElephant(board1, pos, isShow);
                            isCheck = elephant.checkProject(pKing);
                            break;
                        case 11:
                        case 18:
                            CKnight knight = new CKnight(board1, pos, isShow);
                            isCheck = knight.checkProject(pKing);
                            break;
                        case 12:
                        case 19:
                            CRook rook = new CRook(board1, pos, isShow);
                            isCheck = rook.checkProject(pKing);
                            break;
                        case 13:
                        case 20:
                            CCannon cannon = new CCannon(board1, pos, isShow);
                            isCheck = cannon.checkProject(pKing);
                            break;
                        case 14:
                        case 21:
                            CPawn pawn = new CPawn(board1, pos, isShow);
                            isCheck = pawn.checkProject(pKing);
                            break;
                    }
                    if (isCheck)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR checkProject, Piece: " + e.ToString());
            }
            return isCheck;
        }

        public void doMove(int x, int y)
        {
            board.cell[x, y] .SetSellBoard( board.cell[CurrMove.x, CurrMove.y]);
            board.cell[CurrMove.x, CurrMove.y].SetSellBoard(new CellBoard());
        }

        public void reMove(int x, int y, CellBoard value)
        {
            board.cell[CurrMove.x, CurrMove.y].SetSellBoard(board.cell[x, y]);
            board.cell[x, y].SetSellBoard (value);
        }
    }
    #endregion

    #region King
    public class CKing : Piece
    {
        static int[,] KingTable = {
        {0, 0, 0, 0, 0, 0, 0, 0, 0}, /* KING */
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 100000+1,100000+ 1, 100000+1, 0, 0, 0},
        {0, 0, 0, 100000+15,100000+ 15,100000+ 15, 0, 0, 0},
        {0, 0, 0, 100000+30, 100000+35, 100000+30, 0, 0, 0}
    };

        public CKing(BoardAI board, Point currMove, bool _isShow) : base(board, currMove,_isShow) { }

        public override List<State> FindAllPossibleMoves()
        {
            allPossibleMove = new List<State>();
            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };
            for (int i = 0; i < dx.Length; i++)
            {
                int x = CurrMove.x + dx[i];
                int y = CurrMove.y + dy[i];
                if (x >= 0 && x <= 9 && y >= 0 && y <= 8)
                {
                    CellBoard val1 = new CellBoard( board.cell[CurrMove.x, CurrMove.y]);
                    CellBoard val2 = new CellBoard( board.cell[x, y]);
                    if (((RED && ((val2.idChess >= 8 && val2.idChess <= 14) || val2.idChess == 0) && x <= 2) || (!RED && (val2.idChess > 14 || val2.idChess == 0) && x >= 7)) && (y >= 3 && y <= 5))
                    {
                        doMove(x, y);
                        List<Point> arr = board.FindPieces(RED);
                        if (!checkProject(board, arr))
                        {
                            allPossibleMove.Add(new State(CurrMove, new Point(x, y), val1, val2));
                        }
                        reMove(x, y, val2);
                    }
                }
            }
            return allPossibleMove;
        }

        public override bool checkProject(Point King)
        {
            if (CurrMove.y != King.y)
            {
                return false;
            }
            for (int i = Math.Min(CurrMove.x, King.x) + 1; i < Math.Max(CurrMove.x, King.x); i++)
            {
                if (board.cell[i, CurrMove.y].idChess != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static int GetPositionValue(Point pos, bool RED)
        {
            if (RED)
            {
                return KingTable[9 - pos.x, 8 - pos.y];
            }
            return KingTable[pos.x, pos.y];
        }
    }
    #endregion

    #region Elephant
    public class CElephant : Piece
    {
        static int[,] ElephantTable = {
        {0, 0, 0, 0, 0, 0, 0, 0, 0}, /* ELEPHAN */
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 22, 0, 0, 0, 22, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {23, 0, 0, 0, 28, 0, 0, 0, 23},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 25, 0, 0, 0, 25, 0, 0}
    };

        static int[,] ElephantTable_Show = {
        {18, 18, 18, 18, 18, 18, 18, 18, 18}, /* ELEPHAN */
        {18, 18, 18, 18, 18, 18, 18, 18, 18},
        {18, 18, 25, 21, 18, 21, 25, 18, 18},
        {18, 25, 35, 18, 18, 18, 35, 25, 18},
        {18, 18, 25, 18, 18, 18, 25, 18, 18},
        {18, 18, 22, 18, 18, 18, 22, 18, 18},
        {18, 18, 18, 18, 18, 18, 18, 18, 18},
        {18, 18, 18, 18, 18, 18, 18, 18, 18},
        {18, 18, 18, 18, 18, 18, 18, 18, 18},
        {18, 18, 25, 18, 18, 18, 25, 18, 18}
    };

        public CElephant(BoardAI board, Point currMove,bool _isShow) : base(board, currMove,_isShow) { }
        public override List<State> FindAllPossibleMoves()
        {
            allPossibleMove = new List<State>();
            int[] dx = { 2, 2, -2, -2 };
            int[] dy = { 2, -2, 2, -2 };
            for (int i = 0; i < dx.Length; i++)
            {
                int x = CurrMove.x + dx[i];
                int y = CurrMove.y + dy[i];

                if (x >= 0 && x <= 9 && y >= 0 && y <= 8)
                {
                    CellBoard val1 = new CellBoard(board.cell[CurrMove.x, CurrMove.y]);
                    CellBoard val2 = new CellBoard(board.cell[x, y]);
                    if (board.isCoUp == false)
                    {
                        if (((RED && ((val2.idChess >= 8 && val2.idChess <= 14) || val2.idChess == 0) && x <= 4) || (!RED && (val2.idChess > 14 || val2.idChess == 0) && x >= 5)) && isCheck(new Point(x, y)))
                        {
                            doMove(x, y);
                            List<Point> arr = board.FindPieces(RED);
                            if (!checkProject(board, arr))
                            {
                                allPossibleMove.Add(new State(CurrMove, new Point(x, y), val1, val2));
                            }
                            reMove(x, y, val2);
                        }
                    }
                    else
                    {
                        if (((RED && ((val2.idChess >= 8 && val2.idChess <= 14) || val2.idChess == 0) ) || (!RED && (val2.idChess > 14 || val2.idChess == 0) )) && isCheck(new Point(x, y)))
                        {
                            doMove(x, y);
                            List<Point> arr = board.FindPieces(RED);
                            if (!checkProject(board, arr))
                            {
                                allPossibleMove.Add(new State(CurrMove, new Point(x, y), val1, val2));
                            }
                            reMove(x, y, val2);
                        }
                    }

                }

            }
            return allPossibleMove;
        }

        bool isCheck(Point pos)
        {
            int dong = pos.x - CurrMove.x;
            int cot = pos.y - CurrMove.y;
            if (Math.Abs(dong) == 2 && Math.Abs(cot) == 2)
            {
                if (dong > 0)
                {
                    if (cot > 0)
                    {
                        if (board.cell[CurrMove.x + 1, CurrMove.y + 1].idChess == 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (board.cell[CurrMove.x + 1, CurrMove.y - 1].idChess == 0)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (cot > 0)
                    {
                        if (board.cell[CurrMove.x - 1, CurrMove.y + 1].idChess == 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (board.cell[CurrMove.x - 1, CurrMove.y - 1].idChess == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override bool checkProject(Point King)
        {
            return false;
        }

        public static int GetPositionValue(Point pos, bool RED,bool _isShow,bool _isCoUp)
        {
            if (_isCoUp ==false)
            {
                if (RED)
                {
                    return ElephantTable[9 - pos.x, 8 - pos.y];
                }
                return ElephantTable[pos.x, pos.y];
            }
            else
            {
                if (_isShow)
                {
                    if (RED)
                    {
                        return ElephantTable_Show[9 - pos.x, 8 - pos.y];
                    }
                    return ElephantTable_Show[pos.x, pos.y];
                }
                else
                {
                    if (RED)
                    {
                        return ElephantTable[9 - pos.x, 8 - pos.y];
                    }
                    return ElephantTable[pos.x, pos.y];
                }
            }
        }
    }
    #endregion

    #region Rook
    public class CRook : Piece
    {

        static int[,] RookTable = {
        {90, 90, 90, 90, 90, 90, 90, 90, 90}, /* ROOK */
        {90, 92, 91, 91, 90, 91, 91, 92, 90},
        {90, 91, 90, 90, 90, 90, 90, 91, 90},
        {90, 91, 90, 91, 90, 91, 90, 91, 90},
        {90, 93, 90, 91, 90, 91, 90, 93, 90},
        {90, 94, 90, 94, 90, 94, 90, 94, 90},
        {90, 91, 90, 91, 90, 91, 90, 91, 90},
        {90, 92, 90, 91, 90, 91, 90, 92, 90},
        {91, 92, 90, 93, 90, 93, 90, 92, 91},
        {89, 92, 90, 90, 90, 90, 90, 92, 89}
    };
        static int[,] RookTableHidden = {
        {5, 5, 5, 5, 5, 5, 5, 5, 5}, /* ROOK */
        {6, 5, 5, 5, 5, 5, 5, 5, 6},
        {6, 5, 5, 5, 5, 5, 5, 5, 6},
        {6, 5, 5, 5, 5, 5, 5, 5, 6},
        {5, 5, 5, 5, 5, 5, 5, 5, 5},
        {5, 5, 5, 5, 5, 5, 5, 5, 5},
        {5, 5, 5, 5, 5, 5, 5, 5, 5},
        {5, 5, 5, 5, 5, 5, 5, 5, 5},
        {5, 5, 5, 5, 5, 5, 5, 5, 5},
        {40, 10, 10, 5, 1, 5, 10, 10, 40}
    };

        public CRook(BoardAI board, Point currMove,bool _isShow) : base(board, currMove,_isShow) { }

        public override List<State> FindAllPossibleMoves()
        {
            allPossibleMove = new List<State>();
            int x = CurrMove.x;
            int y = CurrMove.y;
            for (int i = y + 1; i <= 8; i++)
            {
                if (getMoveRook(x, i))
                {
                    break;
                }
            }
            for (int i = y - 1; i >= 0; i--)
            {
                if (getMoveRook(x, i))
                {
                    break;
                }
            }
            for (int i = x + 1; i <= 9; i++)
            {
                if (getMoveRook(i, y))
                {
                    break;
                }
            }
            for (int i = x - 1; i >= 0; i--)
            {
                if (getMoveRook(i, y))
                {
                    break;
                }
            }
            return allPossibleMove;
        }

        bool getMoveRook(int x, int y)
        {
            CellBoard val1 = new CellBoard(  board.cell[CurrMove.x, CurrMove.y]);
            CellBoard val2 = new CellBoard( board.cell[x, y]);
            if (val2.idChess == 0 || ((RED && val2.idChess >= 8 && val2.idChess <= 14) || (!RED && val2.idChess > 14)))
            {
                doMove(x, y);
                List<Point> arr = board.FindPieces(RED);
                if (!checkProject(board, arr))
                {
                    allPossibleMove.Add(new State(CurrMove, new Point(x, y), val1, val2));
                }
                reMove(x, y, val2);
            }
            if (val2.idChess != 0)
            {
                return true;
            }
            return false;
        }

        public override bool checkProject(Point King)
        {
            if (CurrMove.x == King.x || CurrMove.y == King.y)
            {
                int count = 0;
                if (CurrMove.x == King.x)
                {
                    for (int i = Math.Min(CurrMove.y, King.y) + 1; i < Math.Max(CurrMove.y, King.y); i++)
                    {
                        if (board.cell[CurrMove.x, i].idChess != 0)
                        {
                            count++;
                        }
                    }
                }
                else
                {
                    for (int i = Math.Min(CurrMove.x, King.x) + 1; i < Math.Max(CurrMove.x, King.x); i++)
                    {
                        if (board.cell[i, CurrMove.y].idChess != 0)
                        {
                            count++;
                        }
                    }
                }
                if (count == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static int GetPositionValue(Point pos, bool RED,bool _isShow,bool _isCoUp)
        {
            if (_isCoUp == false)
            {
                if (RED)
                {
                    return RookTable[9 - pos.x, 8 - pos.y];
                }
                return RookTable[pos.x, pos.y];
            }
            else
            {
                if(_isShow )
                {

                    if (RED)
                    {
                        return RookTable[9 - pos.x, 8 - pos.y];
                    }
                    return RookTable[pos.x, pos.y];
                }
                else
                {

                    if (RED)
                    {
                        return RookTableHidden[9 - pos.x, 8 - pos.y];
                    }
                    return RookTableHidden[pos.x, pos.y];
                }
            }
        }
    }
    #endregion

    #region Cannon
    public class CCannon : Piece
    {

        static int[,] CannonTable = {
        {52, 47, 48, 46, 46, 46, 48, 47, 52}, /* CANNON */
        {48, 47, 48, 46, 46, 46, 48, 47, 48},
        {48, 48, 48, 46, 46, 46, 48, 48, 48},
        {46, 48, 46, 48, 55, 48, 46, 48, 46},
        {46, 48, 46, 52, 52, 52, 46, 48, 46},
        {46, 48, 48, 52, 52, 52, 48, 48, 46},
        {46, 48, 46, 52, 52, 52, 46, 48, 46},
        {46, 48, 48, 52, 52, 52, 48, 48, 46},
        {46, 46, 46, 46, 46, 46, 46, 46, 46},
        {46, 46, 46, 46, 46, 46, 46, 46, 46}
    };
        static int[,] CannonTableHidden = {
        {5, 1, 5, 5, 5, 5, 5, 1, 5}, /* CANNON */
        {5, 5, 5, 5, 5, 5, 5, 5, 5},
        {5, 20, 5, 5, 5, 5, 5, 20, 5},
        {5, 20, 5, 5,5 , 5, 5, 20, 5},
        {5, 20, 5, 5, 5, 5, 5, 20, 5},
        {5, 5, 5, 5, 5, 5, 5, 5, 5},
        {5, 5, 5, 5, 5, 5, 5, 5, 5},
        {10, 30, 15, 5, 5, 5, 15, 30, 10},
        {5, 5, 5, 5, 5, 5, 5, 5, 5},
        {5, 5, 5, 5, 5, 5, 5, 5, 5}
    };

        public CCannon(BoardAI board, Point currMove, bool _isShow) : base(board, currMove, _isShow) { }

        public override List<State> FindAllPossibleMoves()
        {
            allPossibleMove = new List<State>();
            int x = CurrMove.x;
            int y = CurrMove.y;
            for (int i = y + 1; i <= 8; i++)
            {
                if (getCannonMove1(x, i))
                {
                    break;
                }
            }
            for (int i = y - 1; i >= 0; i--)
            {
                if (getCannonMove1(x, i))
                {
                    break;
                }
            }
            for (int i = x + 1; i <= 9; i++)
            {
                if (getCannonMove1(i, y))
                {
                    break;
                }
            }
            for (int i = x - 1; i >= 0; i--)
            {
                if (getCannonMove1(i, y))
                {
                    break;
                }
            }
            return allPossibleMove;
        }

        bool getCannonMove1(int x, int y)
        {
            CellBoard val1 = new CellBoard( board.cell[CurrMove.x, CurrMove.y]);
            CellBoard val2 = new CellBoard( board.cell[x, y]);
            if (val2.idChess == 0)
            {
                getCannonMove3(x, y, val1, val2);
            }
            else
            {
                if (CurrMove.x == x)
                {
                    if (y > CurrMove.y)
                    {
                        for (y = y + 1; y <= 8; y++)
                        {
                            if (getCannonMove2(x, y))
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (y = y - 1; y >= 0; y--)
                        {
                            if (getCannonMove2(x, y))
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (x > CurrMove.x)
                    {
                        for (x = x + 1; x <= 9; x++)
                        {
                            if (getCannonMove2(x, y))
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (x = x - 1; x >= 0; x--)
                        {
                            if (getCannonMove2(x, y))
                            {
                                break;
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        bool getCannonMove2(int x, int y)
        {
            CellBoard val1 = new CellBoard( board.cell[CurrMove.x, CurrMove.y]);
            CellBoard val2 = new CellBoard( board.cell[x, y]);
            if (val2.idChess != 0)
            {
                if ((RED && val2.idChess <= 14) || (!RED && val2.idChess > 14))
                {
                    getCannonMove3(x, y, val1, val2);
                }
                return true;
            }
            return false;
        }

        void getCannonMove3(int x, int y, CellBoard val1, CellBoard val2)
        {
            doMove(x, y);
            List<Point> arr = board.FindPieces(RED);
            if (!checkProject(board, arr))
            {
                allPossibleMove.Add(new State(CurrMove, new Point(x, y), val1, val2));
            }
            reMove(x, y, val2);
        }

        public override bool checkProject(Point King)
        {
            if (CurrMove.x == King.x || CurrMove.y == King.y)
            {
                int count = 0;
                if (CurrMove.x == King.x)
                {
                    for (int i = Math.Min(CurrMove.y, King.y) + 1; i < Math.Max(CurrMove.y, King.y); i++)
                    {
                        if (board.cell[CurrMove.x, i].idChess != 0)
                        {
                            count++;
                        }
                    }
                }
                else
                {
                    for (int i = Math.Min(CurrMove.x, King.x) + 1; i < Math.Max(CurrMove.x, King.x); i++)
                    {
                        if (board.cell[i, CurrMove.y].idChess != 0)
                        {
                            count++;
                        }
                    }
                }
                if (count == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public static int GetPositionValue(Point pos, bool RED,bool _isShow,bool _isCoUp)
        {
            if (_isCoUp == false)
            {
                if (RED)
                {
                    return CannonTable[9 - pos.x, 8 - pos.y];
                }
                return CannonTable[pos.x, pos.y];
            }
            else
            {
                if(_isShow)
                {
                    if (RED)
                    {
                        return CannonTable[9 - pos.x, 8 - pos.y];
                    }
                    return CannonTable[pos.x, pos.y];
                }
                else
                {
                    if (RED)
                    {
                        return CannonTableHidden[9 - pos.x, 8 - pos.y];
                    }
                    return CannonTableHidden[pos.x, pos.y];
                }
            }
        }
    }
    #endregion

    #region Knight
    public class CKnight : Piece
    {
        static int[,] KnightTable = {
        {47, 47, 47, 47, 47, 47, 47, 47, 47}, /* KNIGHT */
        {47, 48, 49, 49, 49, 49, 49, 48, 47},
        {40, 43, 50, 50, 50, 50, 50, 43, 40},
        {40, 43, 50, 50, 50, 50, 50, 43, 40},
        {40, 41, 50, 50, 50, 50, 50, 41, 40},
        {40, 41, 42, 50, 50, 50, 50, 41, 40},
        {40, 40, 40, 40, 40, 40, 40, 40, 40},
        {40, 40, 42, 40, 40, 40, 42, 40, 40},
        {40, 40, 40, 40, 20, 40, 40, 40, 40},
        {40, 35, 40, 40, 40, 40, 40, 35, 40}
    };
        static int[,] KnightTableHidden = {
        {5, 5, 5, 5, 5, 5, 5, 5, 5}, /* KNIGHT */
        {5, 5, 5, 5, 5, 5, 5, 5, 5},
        {5, 5, 5, 5, 5, 5, 5, 5, 5},
        {5, 5, 5, 5, 5, 5, 5, 5, 5},
        {5, 5, 5, 5, 5, 5, 5, 5, 5},
        {5, 5, 5, 5, 5, 5, 5, 5, 5},
        {5, 5, 5, 5, 5, 5, 5, 5, 5},
        {15, 5, 15, 5, 5, 5, 15, 5, 15},
        {5, 5, 5, 10, 5, 7, 5, 5, 5},
        {5, 20, 5, 5, 5, 5, 5, 20, 5}
    };


        public CKnight(BoardAI board, Point currMove, bool _isShow) : base(board, currMove, _isShow) { }

        public override List<State> FindAllPossibleMoves()
        {
            allPossibleMove = new List<State>();
            int[] dx = { 1, 1, 2, 2, -1, -1, -2, -2 };
            int[] dy = { 2, -2, 1, -1, 2, -2, 1, -1 };
            for (int i = 0; i < dx.Length; i++)
            {
                int x = CurrMove.x + dx[i];
                int y = CurrMove.y + dy[i];
                if (x >= 0 && x <= 9 && y >= 0 && y <= 8)
                {
                    CellBoard val1 = new CellBoard(  board.cell[CurrMove.x, CurrMove.y]);
                    CellBoard val2 = new CellBoard( board.cell[x, y]);
                    if ((val2.idChess == 0 || ((RED && val2.idChess >= 8 && val2.idChess <= 14) || (!RED && val2.idChess > 14))) && checkProject(new Point(x, y)))
                    {
                        doMove(x, y);
                        List<Point> arr = board.FindPieces(RED);
                        if (!checkProject(board, arr))
                        {
                            allPossibleMove.Add(new State(CurrMove, new Point(x, y), val1, val2));
                        }
                        reMove(x, y, val2);
                    }
                }
            }
            return allPossibleMove;
        }

        public override bool checkProject(Point King)
        {
            int dong = King.x - CurrMove.x;
            int cot = King.y - CurrMove.y;
            int d = Math.Abs(dong);
            int c = Math.Abs(cot);
            if ((d == 1 && c == 2) || (d == 2 && c == 1))
            {
                if (d == 1)
                {
                    if (cot > 0)
                    {
                        if (board.cell[CurrMove.x, CurrMove.y + 1].idChess == 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (board.cell[CurrMove.x, CurrMove.y - 1].idChess == 0)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (dong > 0)
                    {
                        if (board.cell[CurrMove.x + 1, CurrMove.y].idChess == 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (board.cell[CurrMove.x - 1, CurrMove.y].idChess == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static int GetPositionValue(Point pos, bool RED,bool _isShow,bool _isCoUp)
        {
            if (_isCoUp==false)
            {
                if (RED)
                {
                    return KnightTable[9 - pos.x, 8 - pos.y];
                }
                return KnightTable[pos.x, pos.y];
            }
            else
            {
                if(_isShow)
                {
                    if (RED)
                    {
                        return KnightTable[9 - pos.x, 8 - pos.y];
                    }
                    return KnightTable[pos.x, pos.y];
                }
                else
                {
                    if (RED)
                    {
                        return KnightTableHidden[9 - pos.x, 8 - pos.y];
                    }
                    return KnightTableHidden[pos.x, pos.y];
                }
            }
        }
    }
    #endregion

    #region Bishop
    public class CBishop : Piece
    {
        public static int[,] BishopTable = {
        {0, 0, 0, 0, 0, 0, 0, 0, 0}, /* BISHOP */
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 19, 0, 19, 0, 0, 0},
        {0, 0, 0, 0, 22, 0, 0, 0, 0},
        {0, 0, 0, 20, 0, 20, 0, 0, 0}
    };
      
    public static int BishopShow = 20;

         public static int[,] BishopTableShow = {
        {30, 30, 32, 31, 31, 31, 32, 30, 30}, /* BISHOP */
        {30, 30, 33, 31, 35, 31, 33, 30, 30},
        {30, 30, 32, 31, 35, 31, 32, 30, 30},
        {30, 30, 32, 33, 33, 33, 32, 30, 30},
        {30, 30, 30, 30, 30, 30, 30, 30, 30},
        {30, 30, 30, 30, 30, 30, 30, 30, 30},
        {30, 30, 30, 30, 30, 30, 30, 30, 30},
        {30, 30, 30, 30, 30, 30, 30, 30, 30},
        {30, 30, 30, 30, 30, 30, 30, 30, 30},
        {30, 30, 30, 30, 30, 30, 30, 30, 30}
    };
            public CBishop(BoardAI board, Point currMove, bool _isShow) : base(board, currMove, _isShow) { }

        public override List<State> FindAllPossibleMoves()
        {
            allPossibleMove = new List<State>();
            int[] dx = { 1, 1, -1, -1 };
            int[] dy = { 1, -1, 1, -1 };
            for (int i = 0; i < dx.Length; i++)
            {
                int x = CurrMove.x + dx[i];
                int y = CurrMove.y + dy[i];
                if (board.isCoUp == false)
                {
                    if (x >= 0 && x <= 9 && y >= 3 && y <= 5)
                    {
                        CellBoard val1 = new CellBoard(board.cell[CurrMove.x, CurrMove.y]);
                        CellBoard val2 = new CellBoard(board.cell[x, y]);
                        if ((RED && ((val2.idChess >= 8 && val2.idChess <= 14) || val2.idChess == 0) && x <= 2) || (!RED && (val2.idChess > 14 || val2.idChess == 0) && x >= 7))
                        {
                            doMove(x, y);
                            List<Point> arr = board.FindPieces(RED);
                            if (!checkProject(board, arr))
                            {
                                allPossibleMove.Add(new State(CurrMove, new Point(x, y), val1, val2));
                            }
                            reMove(x, y, val2);
                        }
                    }
                }
                else
                {
                    if (isShow == false)
                    {
                        if (x >= 0 && x <= 9 && y >= 3 && y <= 5)
                        {
                            CellBoard val1 = new CellBoard(board.cell[CurrMove.x, CurrMove.y]);
                            CellBoard val2 = new CellBoard(board.cell[x, y]);
                            if ((RED && ((val2.idChess >= 8 && val2.idChess <= 14) || val2.idChess == 0) && x <= 2) || (!RED && (val2.idChess > 14 || val2.idChess == 0) && x >= 7))
                            {
                                doMove(x, y);
                                List<Point> arr = board.FindPieces(RED);
                                if (!checkProject(board, arr))
                                {
                                    allPossibleMove.Add(new State(CurrMove, new Point(x, y), val1, val2));
                                }
                                reMove(x, y, val2);
                            }
                        }
                    }
                    else
                    {
                        if (x >= 0 && x <= 9 && y >= 0 && y <= 8)
                        {
                            CellBoard val1 = new CellBoard(board.cell[CurrMove.x, CurrMove.y]);
                            CellBoard val2 = new CellBoard(board.cell[x, y]);
                            if ((RED && ((val2.idChess >= 8 && val2.idChess <= 14) || val2.idChess == 0) ) || (!RED && (val2.idChess > 14 || val2.idChess == 0) ))
                            {
                                doMove(x, y);
                                List<Point> arr = board.FindPieces(RED);
                                if (!checkProject(board, arr))
                                {
                                    allPossibleMove.Add(new State(CurrMove, new Point(x, y), val1, val2));
                                }
                                reMove(x, y, val2);
                            }
                        }
                    }
                }
            }
            return allPossibleMove;
        }

        public override bool checkProject(Point King)
        {
            return false;
        }

        public static int GetPositionValue(Point pos, bool RED,bool _isShow,bool _isCoUp)
        {
            if (_isCoUp == false)
            {
                if (RED)
                {
                    return BishopTable[9 - pos.x, 8 - pos.y];
                }
                return BishopTable[pos.x, pos.y];
            }
            else
            {
                if (_isShow)
                {
                    if (RED)
                    {
                        return BishopTableShow[9 - pos.x, 8 - pos.y];
                    }
                    return BishopTableShow[pos.x, pos.y];
                }
                else
                {
                    if (RED)
                    {
                        return BishopTable[9 - pos.x, 8 - pos.y];
                    }
                    return BishopTable[pos.x, pos.y];
                }
            }
        }
    }
    #endregion

    #region CPawn
    public class CPawn : Piece
    {

        static int[,] PawnTable = {
        {11, 12, 13, 14, 14, 14, 13, 12, 11}, /* PAWN*/
        {20, 21, 21, 23, 22, 23, 21, 21, 20},
        {20, 21, 21, 23, 23, 23, 21, 21, 20},
        {20, 21, 21, 22, 22, 22, 21, 21, 20},
        {19, 20, 20, 20, 20, 20, 20, 20, 19},
        {8, 0, 8, 0, 8, 0, 8, 0, 8},
        {8, 0, 8, 0, 8, 0, 8, 0, 8},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0}
    };

        static int[,] PawnTableShow = {
        {11, 12, 13, 14, 14, 14, 13, 12, 11}, /* PAWN*/
        {20, 21, 21, 23, 22, 23, 21, 21, 20},
        {20, 21, 21, 23, 23, 23, 21, 21, 20},
        {20, 21, 21, 22, 22, 22, 21, 21, 20},
        {10, 10, 10, 10, 10, 10, 10, 9, 10},
        {10, 10, 10, 10, 10, 10, 10, 10, 10},
        {10, 10, 10, 10, 10, 10, 10, 10, 10},
        {6, 6, 6, 6, 5, 4, 5, 6, 6},
        {3, 3, 3, 3, 2, 1, 2, 3, 3},
        {3, 3, 3, 3, 3, 3, 3, 3, 3}
    };
        static int[,] PawnTableHidden = {
        {20, 20, 20, 20, 20, 20, 20, 20, 20}, 
        {20, 20, 20, 20, 20, 20, 20, 20, 20},
        {20, 20, 20, 20, 20, 20, 20, 20, 20},
        {20, 20, 20, 20, 20, 20, 20, 20, 20},
        {30, 20, 30, 20, 30, 20, 30, 20, 30},
        {30, 20, 30, 20, 30, 20, 30, 20, 30},
        {20, 20, 20, 20, 20, 20, 20, 20, 20},
        {20, 20, 20, 20, 20, 20, 20, 20, 20},
        {20, 20, 20, 20, 20, 20, 20, 20, 20},
        {20, 20, 20, 20, 20, 20, 20, 20, 20}
    };

    public CPawn(BoardAI board, Point currMove, bool _isShow) : base(board, currMove, _isShow) { }

        public override List<State> FindAllPossibleMoves()
        {
            allPossibleMove = new List<State>();
            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };
            for (int i = 0; i < dx.Length; i++)
            {
                int x = CurrMove.x + dx[i];
                int y = CurrMove.y + dy[i];
                if (x >= 0 && x <= 9 && y >= 0 && y <= 8)
                {
                    CellBoard val1 = new CellBoard( board.cell[CurrMove.x, CurrMove.y]);
                    CellBoard val2 = new CellBoard( board.cell[x, y]);
                    if ((val2.idChess == 0 || ((RED && val2.idChess >= 8 && val2.idChess <= 14) || (!RED && val2.idChess > 14))) && (((!isOver(x, y) && i < 2) || isOver(x, y)) && ((RED && i != 1) || (!RED && i != 0))))
                    {
                        doMove(x, y);
                        List<Point> arr = board.FindPieces(RED);
                        if (!checkProject(board, arr))
                        {
                            allPossibleMove.Add(new State(CurrMove, new Point(x, y), val1, val2));
                        }
                        reMove(x, y, val2);
                    }
                }
            }
            return allPossibleMove;
        }

        bool isOver(int x, int y)
        {
            if ((RED && x > 4) || (!RED && x < 5))
            {
                return true;
            }
            return false;
        }

        public override bool checkProject(Point King)
        {
            if ((RED && King.x < CurrMove.x) || (!RED && (King.x > CurrMove.x)))
            {
                return false;
            }
            int dx = Math.Abs(CurrMove.x - King.x);
            int dy = Math.Abs(CurrMove.y - King.y);
            if ((dx == 1 && dy == 0) || (dx == 0 && dy == 1))
            {
                return true;
            }
            return false;
        }

        public static int GetPositionValue(Point pos, bool RED,bool _isShow,bool _isCoUp)
        {
            if (_isCoUp == false)
            {
                if (RED)
                {
                    return PawnTable[9 - pos.x, 8 - pos.y];
                }
                return PawnTable[pos.x, pos.y];
            }
            else
            {
                if(_isShow)
                {
                    if (RED)
                    {
                        return PawnTableShow[9 - pos.x, 8 - pos.y];
                    }
                    return PawnTableShow[pos.x, pos.y];
                }
                else
                {
                    if (RED)
                    {
                        return PawnTableHidden[9 - pos.x, 8 - pos.y];
                    }
                    return PawnTableHidden[pos.x, pos.y];
                }
            }
        }
    }
    #endregion

    #region Board
    public class CellBoard
    {
       
        public byte idChess;
        public byte idChessShow;
        public bool isShow;
        public CellBoard()
        {
            idChess = 0;
           idChessShow = 0;
            isShow = false;
        }

        public CellBoard(CellBoard _cell)
        {
            this.idChess = _cell.idChess;
            this.idChessShow = _cell.idChessShow;
            this.isShow = _cell.isShow;
        }

        public void SetSellBoard(CellBoard _cell)
        {
            this.idChess = _cell.idChess;
            this.idChessShow = _cell.idChessShow;
            this.isShow = _cell.isShow;
        }
    }

    public class BoardAI
    {
        public CellBoard[,] cell;
        public List<CellBoard> listRed;
        public List<CellBoard> listBlue;
        public bool isCoUp;
        public bool RED;
        public Piece piece;
        public const int ROW = 9;
        public const int COL = 8;
        public List<State> listUndo = new List<State>();
        public int[,] allMoveVar;
        public bool gameOver = false;

        public BoardAI(CellBoard[,] cell, bool _isCoUp,bool RED)
        {
            setBoard(cell);
            this.RED = RED;
            this.isCoUp = _isCoUp;
        }

        //public void solve()
        //{
        //    if (move && selectVar)
        //    {
        //        moveTo(CurrMove.x, CurrMove.y);
        //    }
        //    else
        //    {
        //        byte value = cell[CurrMove.x, CurrMove.y];
        //        if ((RED && value > 14) || (!RED && value >= 8 && value <= 14))
        //        {
        //            select(CurrMove.x, CurrMove.y);
        //            getMoves(piece.FindAllPossibleMoves());
        //        }
        //    }
        //}

        public int[,] getMoves(List<State> possibleMove)
        {
            int n = possibleMove.Count;
            if (n != 0)
            {
                allMoveVar = new int[n, 2];
                int k = 0;
                foreach (State state in possibleMove)   
                {
                    allMoveVar[k, 0] = state.curr.x;
                    allMoveVar[k++, 1] = state.curr.y;
                }
            }
            return allMoveVar;
        }

        public void setBoard(CellBoard[,] board)
        {
            cell = new CellBoard[ROW + 1, COL + 1];
            for (int i = 0; i <= ROW; i++)
            {
                for (int j = 0; j <= COL; j++)
                {
                    cell[i, j] = board[i, j];
                }
            }
        }

        public List<Point> FindPieces(bool _RED)
        {
            List<Point> allpiece = new List<Point>();
            allpiece.Add(new Point(-1, -1));
            for (int i = 0; i <= ROW; i++)
            {
                for (int j = 0; j <= COL; j++)
                {
                    byte val = cell[i, j].idChess;
                    if (val >= 8 && val <= 21)
                    {
                        if ((_RED && val == 15) || (!_RED && val == 8))
                        {
                            allpiece[0] = new Point(i, j);
                        }
                        if ((_RED && val <= 14) || (!_RED && val > 14))
                        {
                            allpiece.Add(new Point(i, j));
                        }
                    }
                }
            }
            return allpiece;
        }

        public List<State> FindAllKingMoves(Point CurrMove,bool isChessRed)
        {
            List<State> allPossibleMove = new List<State>();
            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };
            for (int i = 0; i < dx.Length; i++)
            {
                int x = CurrMove.x + dx[i];
                int y = CurrMove.y + dy[i];
                if (x >= 0 && x <= 9 && y >= 0 && y <= 8)
                {
                    CellBoard val1 = new CellBoard(cell[CurrMove.x, CurrMove.y]);
                    if (((isChessRed && ((cell[x, y].idChess >= 8 && cell[x, y].idChess <= 14) || cell[x, y].idChess == 0) && x <= 2) || (!isChessRed && (cell[x, y].idChess > 14 || cell[x, y].idChess == 0) && x >= 7)) && (y >= 3 && y <= 5))
                    {
                        if (!IsWanning(CurrMove, new Point(x, y)))
                            allPossibleMove.Add(new State(CurrMove, new Point(x, y), val1, cell[x, y]));
                    }
                }
            }
            return allPossibleMove;
        }

        public List<State> FindAllBishopMoves(Point CurrMove,bool isShow,bool isChessRed) 
        {
            List<State> allPossibleMove = new List<State>();
            int[] dx = { 1, 1, -1, -1 };
            int[] dy = { 1, -1, 1, -1 };
            for (int i = 0; i < dx.Length; i++)
            {
                int x = CurrMove.x + dx[i];
                int y = CurrMove.y + dy[i];
                if (isCoUp == false)
                {
                    if (x >= 0 && x <= 9 && y >= 3 && y <= 5)
                    {
                        //CellBoard val1 = new CellBoard(cell[CurrMove.x, CurrMove.y]);
                        CellBoard val2 = new CellBoard(cell[x, y]);
                        if ((isChessRed && ((val2.idChess >= 8 && val2.idChess <= 14) || val2.idChess == 0) && x <= 2) || (!isChessRed && (val2.idChess > 14 || val2.idChess == 0) && x >= 7))
                        {
                            if (!IsWanning(CurrMove, new Point(x, y)))
                                allPossibleMove.Add(new State(CurrMove, new Point(x, y), cell[CurrMove.x, CurrMove.y], val2));
                        }
                    }
                }
                else
                {
                    if (isShow == false)
                    {
                        if (x >= 0 && x <= 9 && y >= 3 && y <= 5)
                        {
                            //CellBoard val1 = new CellBoard(cell[CurrMove.x, CurrMove.y]);
                            //sCellBoard val2 = new CellBoard(cell[x, y]);
                            if ((isChessRed && ((cell[x, y].idChess >= 8 && cell[x, y].idChess <= 14) || cell[x, y].idChess == 0) && x <= 2) || (!isChessRed && (cell[x, y].idChess > 14 || cell[x, y].idChess == 0) && x >= 7))
                            {
                                if (!IsWanning(CurrMove, new Point(x, y)))
                                    allPossibleMove.Add(new State(CurrMove, new Point(x, y), cell[CurrMove.x, CurrMove.y], cell[x, y]));
                            }
                        }
                    }
                    else
                    {
                        if (x >= 0 && x <= 9 && y >= 0 && y <= 8)
                        {
                            //CellBoard val1 = new CellBoard(cell[CurrMove.x, CurrMove.y]);
                            //CellBoard val2 = new CellBoard(cell[x, y]);
                            if ((isChessRed && ((cell[x, y].idChess >= 8 && cell[x, y].idChess <= 14) || cell[x, y].idChess == 0)) || (!isChessRed && (cell[x, y].idChess > 14 || cell[x, y].idChess == 0)))
                            {
                                if (!IsWanning(CurrMove, new Point(x, y)))
                                    allPossibleMove.Add(new State(CurrMove, new Point(x, y), cell[CurrMove.x, CurrMove.y], cell[x, y]));
                            
                            }
                        }
                    }
                }
            }
            return allPossibleMove;
        }
        public List<State> FindAllElephantMoves(Point CurrMove,bool isShow,bool isChessRed)
        {
            List<State> allPossibleMove = new List<State>();
            int[] dx = { 2, 2, -2, -2 };
            int[] dy = { 2, -2, 2, -2 };
            int[] dxfe = { 1, 1, -1, -1 };
            int[] dyfe = { 1, -1, 1, -1 };
            for (int i = 0; i < dx.Length; i++)
            {
                int x = CurrMove.x + dx[i];
                int y = CurrMove.y + dy[i];
                int xfe = CurrMove.x + dxfe[i];
                int yfe = CurrMove.y + dyfe[i];

                if (x >= 0 && x <= 9 && y >= 0 && y <= 8)
                {
                    ///CellBoard val1 = new CellBoard(cell[CurrMove.x, CurrMove.y]);
                   // CellBoard val2 = new CellBoard(cell[x, y]);
                    if (isCoUp == false)
                    {
                        if (((RED && ((cell[x, y].idChess >= 8 && cell[x, y].idChess <= 14) || cell[x, y].idChess == 0) && x <= 4) || (!RED && (cell[x, y].idChess > 14 || cell[x, y].idChess == 0) && x >= 5)))
                        {
                            if(cell[xfe,  yfe].idChess == 0)
                                if (!IsWanning(CurrMove, new Point(x, y)))
                                    allPossibleMove.Add(new State(CurrMove, new Point(x, y), cell[CurrMove.x, CurrMove.y], cell[x, y]));
                        }
                    }
                    else
                    {
                        if (((RED && ((cell[x, y].idChess >= 8 && cell[x, y].idChess <= 14) || cell[x, y].idChess == 0)) || (!RED && (cell[x, y].idChess > 14 || cell[x, y].idChess == 0))))
                        {
                            if (cell[xfe, yfe].idChess == 0)
                            {
                                if (!IsWanning(CurrMove, new Point(x, y)))
                                    allPossibleMove.Add(new State(CurrMove, new Point(x, y), cell[CurrMove.x, CurrMove.y], cell[x, y]));
                            }
                        }
                    }

                }

            }
            return allPossibleMove;
        }

        public List<State> FindAllKnightMoves(Point CurrMove, bool isChessRed)
        {
            List<State> allPossibleMove = new List<State>();
            int[] dx = { 1, 1, 2, 2, -1, -1, -2, -2 };
            int[] dy = { 2, -2, 1, -1, 2, -2, 1, -1 };
            for (int i = 0; i < dx.Length; i++)
            {
                int x = CurrMove.x + dx[i];
                int y = CurrMove.y + dy[i];
                if (x >= 0 && x <= 9 && y >= 0 && y <= 8)
                {
                    // CellBoard val1 = new CellBoard(cell[CurrMove.x, CurrMove.y]);
                    // CellBoard val2 = new CellBoard(cell[x, y]);
                    if ((cell[x, y].idChess == 0 || ((isChessRed && cell[x, y].idChess >= 8 && cell[x, y].idChess <= 14) || (!isChessRed && cell[x, y].idChess > 14))))
                    {
                        if (cell[CurrMove.x + dx[i] / 2, CurrMove.y + dy[i] / 2].idChess == 0)
                        {
                            if (!IsWanning(CurrMove, new Point(x, y)))
                                allPossibleMove.Add(new State(CurrMove, new Point(x, y), cell[CurrMove.x, CurrMove.y], cell[x, y]));
                        }
                    }
                }
            }
            return allPossibleMove;
        }


        public List<State> FindAllRookMoves(Point CurrMove, bool isChessRed)
        {
            List<State> allPossibleMove = new List<State>();
            int x = CurrMove.x;
            int y = CurrMove.y;
            for (int i = y + 1; i <= 8; i++)
            {
                if (cell[x, i].idChess == 0 || ((isChessRed && cell[x, i].idChess >= 8 && cell[x, i].idChess <= 14) || (!isChessRed && cell[x, i].idChess > 14)))
                {
                    if (!IsWanning(CurrMove, new Point(x, i)))
                        allPossibleMove.Add(new State(CurrMove, new Point(x, i), cell[CurrMove.x, CurrMove.y], cell[x, i]));
                }
                if (cell[x, i].idChess != 0)
                {
                    break;
                }
            }
            for (int i = y - 1; i >= 0; i--)
            {
                if (cell[x, i].idChess == 0 || ((isChessRed && cell[x, i].idChess >= 8 && cell[x, i].idChess <= 14) || (!isChessRed && cell[x, i].idChess > 14)))
                {
                    if (!IsWanning(CurrMove, new Point(x, i)))
                        allPossibleMove.Add(new State(CurrMove, new Point(x, i), cell[CurrMove.x, CurrMove.y], cell[x, i]));
                }
                if (cell[x, i].idChess != 0)
                {
                    break;
                }
            }
            for (int i = x + 1; i <= 9; i++)
            {
                if (cell[i, y].idChess == 0 || ((isChessRed && cell[i, y].idChess >= 8 && cell[i, y].idChess <= 14) || (!isChessRed && cell[i, y].idChess > 14)))
                {
                    if (!IsWanning(CurrMove, new Point(i, y)))
                        allPossibleMove.Add(new State(CurrMove, new Point(i, y), cell[CurrMove.x, CurrMove.y], cell[i, y]));
                }
                if (cell[i, y].idChess != 0)
                {
                    break;
                }
            }
            for (int i = x - 1; i >= 0; i--)
            {
                if (cell[i, y].idChess == 0 || ((isChessRed && cell[i, y].idChess >= 8 && cell[i, y].idChess <= 14) || (!isChessRed && cell[i, y].idChess > 14)))
                {
                    if (!IsWanning(CurrMove, new Point(x, y)))
                        allPossibleMove.Add(new State(CurrMove, new Point(i, y), cell[CurrMove.x, CurrMove.y], cell[i, y]));
                }
                if (cell[i, y].idChess != 0)
                {
                    break;
                }
            }
            return allPossibleMove;
        }

        #region cannon
        public List<State> FindAllCanonMoves(Point CurrMove,bool isChessRed)
        {
            List<State> allPossibleMove = new List<State>();
            int x = CurrMove.x;
            int y = CurrMove.y;
            for (int i = y + 1; i <= 8; i++)
            {
                if (getCannonMove1(allPossibleMove,CurrMove, x, i, isChessRed))
                {
                    break;
                }
            }
            for (int i = y - 1; i >= 0; i--)
            {
                if (getCannonMove1(allPossibleMove,CurrMove, x, i, isChessRed))
                {
                    break;
                }
            }
            for (int i = x + 1; i <= 9; i++)
            {
                if (getCannonMove1(allPossibleMove,CurrMove, i, y, isChessRed))
                {
                    break;
                }
            }
            for (int i = x - 1; i >= 0; i--)
            {
                if (getCannonMove1(allPossibleMove,CurrMove, i, y, isChessRed))
                {
                    break;
                }
            }
            return allPossibleMove;
        }
      bool getCannonMove1(List<State> allPossibleMove, Point CurrMove, int x, int y,bool isChessRed)
        {
            
            if (cell[x, y].idChess == 0)
            {
                if (!IsWanning(CurrMove, new Point(x, y)))
                    allPossibleMove.Add(new State(CurrMove, new Point(x, y), cell[CurrMove.x, CurrMove.y], cell[x, y]));
            }
            else
            {
                if (CurrMove.x == x)
                {
                    if (y > CurrMove.y)
                    {
                        for (y = y + 1; y <= 8; y++)
                        {
                            if (getCannonMove2(allPossibleMove,CurrMove, x, y, isChessRed))
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (y = y - 1; y >= 0; y--)
                        {
                            if (getCannonMove2(allPossibleMove,CurrMove, x, y, isChessRed))
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (x > CurrMove.x)
                    {
                        for (x = x + 1; x <= 9; x++)
                        {
                            if (getCannonMove2(allPossibleMove,CurrMove,x, y, isChessRed))
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (x = x - 1; x >= 0; x--)
                        {
                            if (getCannonMove2( allPossibleMove,CurrMove, x, y, isChessRed))
                            {
                                break;
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        bool getCannonMove2(List<State> allPossibleMove, Point CurrMove,int x, int y, bool isChessRed)
        {
            if (cell[x, y].idChess != 0)
            {
                if ((isChessRed && cell[x, y].idChess <= 14) || (!isChessRed && cell[x, y].idChess > 14))
                {
                    if (!IsWanning(CurrMove, new Point(x, y)))
                        allPossibleMove.Add(new State(CurrMove, new Point(x, y), cell[CurrMove.x, CurrMove.y], cell[x, y]));
                }
                return true;
            }
            return false;
        }
        #endregion cannon

        public List<State> FindAllPawnMoves(Point CurrMove, bool isChessRed)
        {
            List<State> allPossibleMove = new List<State>();
            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };
            for (int i = 0; i < dx.Length; i++)
            {
                int x = CurrMove.x + dx[i];
                int y = CurrMove.y + dy[i];
                if (x >= 0 && x <= 9 && y >= 0 && y <= 8)
                {
                    if ((cell[x, y].idChess == 0 || ((isChessRed && cell[x, y].idChess >= 8 && cell[x, y].idChess <= 14) || (!isChessRed && cell[x, y].idChess > 14))) && (((!isOver(x, y, isChessRed) && i < 2) || isOver(x, y, isChessRed)) && ((isChessRed && i != 1) || (!isChessRed && i != 0))))
                    {
                        if(!IsWanning(CurrMove,new Point(x,y)))
                            allPossibleMove.Add(new State(CurrMove, new Point(x, y), cell[CurrMove.x, CurrMove.y], cell[x, y]));
                    }
                }
            }
            return allPossibleMove;
        }

        bool isOver(int x, int y, bool isChessRed)
        {
            if ((isChessRed && x > 4) || (!isChessRed && x < 5))
            {
                return true;
            }
            return false;
        }


        public bool IsWanning(Point pointStart, Point pointEnd)
        {
            //try
            //{
            //    Point King = allpiece[0];
            //    for (int i = 1; i < allpiece.Count; i++)
            //    {
            //        Point CurrMove = allpiece[i];
            //        byte value = cell[CurrMove.x, CurrMove.y].idChess;
            //        //bool isShow = cell[CurrMove.x, CurrMove.y].isShow;
            //        if (!pointEnd.CheckPoint(CurrMove)) {
            //            switch (value)
            //            {
            //                case 8:
            //                case 15:
            //                    if (CurrMove.y != King.y)
            //                    {
            //                        break;
            //                    }
            //                    if (CurrMove.y == pointEnd.y)
            //                    {
            //                        break;
            //                    }
            //                    for (int k = Math.Min(CurrMove.x, King.x) + 1; k < Math.Max(CurrMove.x, King.x); k++)
            //                    {
            //                        if (!pointStart.CheckPoint(k, CurrMove.y))
            //                        {
            //                            if (cell[k, CurrMove.y].idChess != 0)
            //                            {
            //                                break;
            //                            }
            //                        }
            //                    }
            //                    return true;
            //                case 9:
            //                case 16:
            //                    if ((CurrMove.x - King.x == 1 || CurrMove.x - King.x == -1) &&
            //                        CurrMove.y - King.y == 1 || CurrMove.y - King.y == -1)
            //                    {
            //                        return  true;
            //                    }
            //                    break;
            //                case 10:
            //                case 17:
            //                    if ((CurrMove.x - King.x == 2 || CurrMove.x - King.x == -2) &&
            //                        CurrMove.y - King.y == 2 || CurrMove.y - King.y == -2)
            //                    {
            //                        int clockX = (CurrMove.x + King.x) / 2;
            //                        int clockY = (CurrMove.y + King.y) / 2;
            //                        if ((!pointEnd.CheckPoint(clockX, clockY) && cell[clockX, clockY].idChess == 0) ||
            //                            (pointStart.CheckPoint(clockX, clockY)))
            //                            return true;
            //                    }
            //                    break;
            //                case 11:
            //                case 18:
            //                    int dong = CurrMove.x - King.x;
            //                    int cot =  CurrMove.y - King.x ;
            //                    int d = Math.Abs(dong);
            //                    int c = Math.Abs(cot);
            //                    if ((d == 1 && c == 2) || (d == 2 && c == 1))
            //                    {
            //                        int posLockX = CurrMove.x + dong / 2, posLockY = CurrMove.y + cot / 2;
            //                        if (pointStart.CheckPoint(posLockX,posLockY)||
            //                            (!pointEnd.CheckPoint(posLockX, posLockY)&& cell[posLockX, posLockY].idChess == 0))
            //                        {
            //                            return true;
            //                        }
            //                    }
            //                    break;
            //                case 12:
            //                case 19:
            //                    if (CurrMove.x == King.x || CurrMove.y == King.y)
            //                    {
            //                        int count = 0;
            //                        if (CurrMove.x == King.x)
            //                        {
            //                            for (int r = Math.Min(CurrMove.y, King.y) + 1; r < Math.Max(CurrMove.y, King.y); r++)
            //                            {
            //                                if (pointStart.CheckPoint(King.x, r))
            //                                {
            //                                    count--;
            //                                }
            //                                if (cell[CurrMove.x, r].idChess != 0)
            //                                {
            //                                    count++;
            //                                }
            //                                else if (pointEnd.CheckPoint(King.x, r))
            //                                {
            //                                    count++;
            //                                }
            //                            }
            //                        }
            //                        else
            //                        {
            //                            for (int r = Math.Min(CurrMove.x, King.x) + 1; r < Math.Max(CurrMove.x, King.x); r++)
            //                            {
            //                                if (pointStart.CheckPoint(r, King.y))
            //                                {
            //                                    count--;
            //                                }
            //                                if (cell[r, CurrMove.y].idChess != 0)
            //                                {
            //                                    count++;
            //                                }
            //                                else if (pointEnd.CheckPoint(r, King.y))
            //                                {
            //                                    count++;
            //                                }
            //                            }
            //                        }
            //                        if (count == 0)
            //                        {
            //                            return true;
            //                        }
            //                    }
            //                    break;
            //                case 13:
            //                case 20:
            //                    if (CurrMove.x == King.x || CurrMove.y == King.y)
            //                    {
            //                        int count = 0;
            //                        if (CurrMove.x == King.x)
            //                        {
            //                            for (int r = Math.Min(CurrMove.y, King.y) + 1; r < Math.Max(CurrMove.y, King.y); r++)
            //                            {
            //                                if (pointStart.CheckPoint(King.x, r))
            //                                {
            //                                    count--;
            //                                }
            //                                if (cell[CurrMove.x, r].idChess != 0)
            //                                {
            //                                    count++;
            //                                }
            //                                else if (pointEnd.CheckPoint(King.x, r))
            //                                {
            //                                    count++;
            //                                }
            //                            }
            //                        }
            //                        else
            //                        {
            //                            for (int r = Math.Min(CurrMove.x, King.x) + 1; r < Math.Max(CurrMove.x, King.x); r++)
            //                            {
            //                                if (pointStart.CheckPoint(r, King.y))
            //                                {
            //                                    count--;
            //                                }
            //                                if (cell[r, CurrMove.y].idChess != 0)
            //                                {
            //                                    count++;
            //                                }
            //                                else if (pointEnd.CheckPoint(r, King.y))
            //                                {
            //                                    count++;
            //                                }
            //                            }
            //                        }
            //                        if (count == 1)
            //                        {
            //                            return true;
            //                        }
            //                    }
            //                    break;
            //                case 14:
            //                case 21:
            //                    if ((RED && King.x < CurrMove.x) || (!RED && (King.x > CurrMove.x)))
            //                    {
            //                        break;
            //                    }
            //                    int dx = Math.Abs(CurrMove.x - King.x);
            //                    int dy = Math.Abs(CurrMove.y - King.y);
            //                    if ((dx == 1 && dy == 0) || (dx == 0 && dy == 1))
            //                    {
            //                        return true;
            //                    }
            //                    break;
            //            }
                    
            //        }
            //    }

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("ERROR checkProject, Piece: " + e.ToString());
            //}
            return false;
        }

        public void doMove(Point currMove,int x, int y)
        {
            cell[x, y].SetSellBoard(cell[currMove.x, currMove.y]);
            cell[currMove.x, currMove.y].SetSellBoard(new CellBoard());
        }

        public void reMove(Point currMove,int x, int y)
        {
            cell[currMove.x, currMove.y].SetSellBoard(cell[x, y]);
            cell[x, y].SetSellBoard(cell[x, y]);
        }

        List<Point> allpiece;
        public List<State> allMove(bool _RED)
        {
            List<Point> allpiece = FindPieces(_RED);
            List<State> arrMoves = new List<State>();
            for (int i = 1; i < allpiece.Count; i++)
            {
                Point pos = allpiece[i];
                byte val = cell[pos.x, pos.y].idChess;
                bool isShow = cell[pos.x, pos.y].isShow;
                bool isChessRed = val > 14;
                switch (val)
                {
                    case 8:
                    case 15:
                        CKing king = new CKing(this, pos, isShow);
                        arrMoves.AddRange(king.FindAllPossibleMoves());
                        //arrMoves.AddRange(FindAllKingMoves(pos,isChessRed));
                        break;
                    case 9:
                    case 16:
                        //arrMoves.AddRange( FindAllBishopMoves(pos, isShow,isChessRed));
                        CBishop bishop = new CBishop(this, pos, isShow);
                        arrMoves.AddRange(bishop.FindAllPossibleMoves());
                        break;
                    case 10:
                    case 17:
                        //arrMoves.AddRange(FindAllElephantMoves(pos, isShow, isChessRed));
                        CElephant elephant = new CElephant(this, pos, isShow);
                        arrMoves.AddRange(elephant.FindAllPossibleMoves());
                        break;
                    case 11:
                    case 18:
                        //arrMoves.AddRange(FindAllKnightMoves(pos, isChessRed));
                        CKnight knight = new CKnight(this, pos, isShow);
                        arrMoves.AddRange(knight.FindAllPossibleMoves());
                        break;
                    case 12:
                    case 19:
                        //arrMoves.AddRange(FindAllRookMoves(pos, isChessRed));
                        CRook rook = new CRook(this, pos, isShow);
                        arrMoves.AddRange(rook.FindAllPossibleMoves());
                        break;
                    case 13:
                    case 20:
                        CCannon cannon = new CCannon(this, pos, isShow);
                        arrMoves.AddRange(cannon.FindAllPossibleMoves());
                        //arrMoves.AddRange(FindAllCanonMoves(pos, isChessRed));
                        break;
                    case 14:
                    case 21:
                        CPawn pawn = new CPawn(this, pos, isShow);
                        arrMoves.AddRange(pawn.FindAllPossibleMoves());
                        //arrMoves.AddRange(FindAllPawnMoves(pos, isChessRed));
                        break;
                }
            }
            return arrMoves;
        }

        public List<State> allMove1(bool _RED)
        {
            List<Point> allpiece = FindPieces(_RED);
            List<State> arrMoves = new List<State>();
            //allpiece[0]; //unknow what to do
            for (int i = 1; i < allpiece.Count; i++)
            {
                Point pos = allpiece[i];
                byte val = cell[pos.x, pos.y].idChess;
                bool isShow = cell[pos.x, pos.y].isShow;
                switch (val)
                {
                    case 8:
                    case 15:
                        CKing king = new CKing(this, pos, isShow);
                        arrMoves.AddRange(king.FindAllPossibleMoves());
                        break;
                    case 9:
                    case 16:
                        CBishop bishop = new CBishop(this, pos, isShow);
                        arrMoves.AddRange(bishop.FindAllPossibleMoves());
                        break;
                    case 10:
                    case 17:
                        CElephant elephant = new CElephant(this, pos, isShow);
                        arrMoves.AddRange(elephant.FindAllPossibleMoves());
                        break;
                    case 11:
                    case 18:
                        CKnight knight = new CKnight(this, pos, isShow);
                        arrMoves.AddRange(knight.FindAllPossibleMoves());
                        break;
                    case 12:
                    case 19:
                        CRook rook = new CRook(this, pos, isShow);
                        arrMoves.AddRange(rook.FindAllPossibleMoves());
                        break;
                    case 13:
                    case 20:
                        CCannon cannon = new CCannon(this, pos, isShow);
                        arrMoves.AddRange(cannon.FindAllPossibleMoves());
                        break;
                    case 14:
                    case 21:
                        CPawn pawn = new CPawn(this, pos, isShow);
                        arrMoves.AddRange(pawn.FindAllPossibleMoves());
                        break;
                }
            }
            return arrMoves;
        }

        public bool IsGameOver(bool _RED)
        {
            List<Point> allpiece = FindPieces(_RED);
            //allpiece.get(0); //unkonw what to do
            for (int i = 1; i < allpiece.Count; i++)
            {
                Point pos = allpiece[i];
                List<State> arrMoves = null;
                byte val = cell[pos.x, pos.y].idChess;
                bool isShow = cell[pos.x, pos.y].isShow;
                switch (val)
                {
                    case 8:
                    case 15:
                        CKing king = new CKing(this, pos, isShow);
                        arrMoves = king.FindAllPossibleMoves();
                        break;
                    case 9:
                    case 16:
                        CBishop bishop = new CBishop(this, pos, isShow);
                        arrMoves = bishop.FindAllPossibleMoves();
                        break;
                    case 10:
                    case 17:
                        CElephant elephant = new CElephant(this, pos, isShow);
                        arrMoves = elephant.FindAllPossibleMoves();
                        break;
                    case 11:
                    case 18:
                        CKnight knight = new CKnight(this, pos, isShow);
                        arrMoves = knight.FindAllPossibleMoves();
                        break;
                    case 12:
                    case 19:
                        CRook rook = new CRook(this, pos, isShow);
                        arrMoves = rook.FindAllPossibleMoves();
                        break;
                    case 13:
                    case 20:
                        CCannon cannon = new CCannon(this, pos, isShow);
                        arrMoves = cannon.FindAllPossibleMoves();
                        break;
                    case 14:
                    case 21:
                        CPawn pawn = new CPawn(this, pos, isShow);
                        arrMoves = pawn.FindAllPossibleMoves();
                        break;
                }
                if (arrMoves != null && arrMoves.Count != 0)
                {
                    return false;
                }
            }
            return true;
        }


        //public State RandomMove(bool _RED)
        //{
        //    List<State> arrMoves = allMove(_RED);
        //    System.Random rand = new System.Random();
        //    if (arrMoves.Count > 0)
        //    {
        //        int x = rand.Next(arrMoves.Count);
        //        return arrMoves[x];
        //    }
        //    return null;
        //}

        public void switchPlayer()
        {
            RED = !RED;
        }
    }
   
    #endregion

    #region AI
    public class _AI
    {

        BoardAI board;
        private int MaxDepth = 2;
        public State MyBestMove;

        public _AI(BoardAI b)
        {
            this.board = b;
        }

        int Bonous(bool RED)
        {

            int[,] materialnumber = {{5, 2, 2, 2, 2, 2, 1},
            {5, 2, 2, 2, 2, 2, 1}};
            int i, s;
            int[,] bn = {{-2, -3, -3, -4, -4, -5, 0},
            {-2, -3, -3, -4, -4, -5, 0}};

            for (i = 0; i < 2; i++)
            {
                if (materialnumber[i, 1] < 2)
                {
                    bn[1 - i, 5] += 4;
                    bn[1 - i, 3] += 2;
                    bn[1 - i, 0] += 1;
                }

                if (materialnumber[i, 2] < 2)
                {
                    bn[1 - i, 5] += 2;
                    bn[1 - i, 4] += 2;
                    bn[1 - i, 0] += 1;
                }
            }

            if (board.cell[0, 0].idChess == 19 && board.cell[0, 1].idChess == 18)
            {
                bn[0, 6] -= 10;
            }
            if (board.cell[0, 8].idChess == 19 && board.cell[0, 7].idChess == 18)
            {
                bn[0, 6] -= 10;
            }
            if (board.cell[9, 0].idChess == 13 && board.cell[9, 1].idChess == 12)
            {
                bn[1, 6] -= 10;
            }
            if (board.cell[9, 8].idChess == 13 && board.cell[9, 7].idChess == 12)
            {
                bn[1, 6] -= 10;
            }

            int side, xside;
            if (RED)
            {
                side = 0;
                xside = 1;
            }
            else
            {
                side = 1;
                xside = 0;
            }

            s = bn[side, 6] - bn[xside, 6];

            for (i = 0; i < 6; i++)
            {
                s += materialnumber[side, i] * bn[side, i]
                        - materialnumber[xside, i] * bn[xside, i];
            }
            return s;
        }

        int Eval(bool RED)
        {
            int s = 0;
            for (int x = 0; x <= 9; x++)
            {
                for (int y = 0; y <= 8; y++)
                {
                    byte value = board.cell[x, y].idChess;
                    switch (value)
                    {
                        case 8:
                            s -= CKing.GetPositionValue(new Point(x, y), !RED);
                            break;
                        case 15:
                            s += CKing.GetPositionValue(new Point(x, y), RED);
                            break;
                        case 9:
                            s -= CBishop.GetPositionValue(new Point(x, y), !RED, board.cell[x, y].isShow, board.isCoUp);
                            break;
                        case 16:
                            s += CBishop.GetPositionValue(new Point(x, y), RED, board.cell[x, y].isShow, board.isCoUp);
                            break;
                        case 10:
                            s -= CElephant.GetPositionValue(new Point(x, y), !RED, board.cell[x, y].isShow, board.isCoUp);
                            break;
                        case 17:
                            s += CElephant.GetPositionValue(new Point(x, y), RED, board.cell[x, y].isShow, board.isCoUp);
                            break;
                        case 11:
                            s -= CKnight.GetPositionValue(new Point(x, y), !RED, board.cell[x, y].isShow, board.isCoUp);
                            break;
                        case 18:
                            s += CKnight.GetPositionValue(new Point(x, y), RED, board.cell[x, y].isShow, board.isCoUp);
                            break;
                        case 12:
                            s -= CRook.GetPositionValue(new Point(x, y), !RED, board.cell[x, y].isShow, board.isCoUp);
                            break;
                        case 19:
                            s += CRook.GetPositionValue(new Point(x, y), RED, board.cell[x, y].isShow, board.isCoUp);
                            break;
                        case 13:
                            s -= CCannon.GetPositionValue(new Point(x, y), !RED, board.cell[x, y].isShow, board.isCoUp);
                            break;
                        case 20:
                            s += CCannon.GetPositionValue(new Point(x, y), RED, board.cell[x, y].isShow, board.isCoUp);
                            break;
                        case 14:
                            s -= CPawn.GetPositionValue(new Point(x, y), !RED, board.cell[x, y].isShow, board.isCoUp);
                            break;
                        case 21:
                            s += CPawn.GetPositionValue(new Point(x, y), RED, board.cell[x, y].isShow, board.isCoUp);
                            break;
                    }
                }
            }
            if (!RED)
            {
                s = -s;
            }
            return s; //+ Bonous(RED);
        }

        void MakeMove(State state)
        {
            board.cell[state.curr.x, state.curr.y].SetSellBoard( state.value1);
            board.cell[state.prev.x, state.prev.y].SetSellBoard(new CellBoard());
        }

        void UnMakeMove(State state)
        {
            board.cell[state.curr.x, state.curr.y] .SetSellBoard( state.value2);
            board.cell[state.prev.x, state.prev.y] .SetSellBoard( state.value1);
        }

        public List<State> ListBestMove;
        public const int sizeBestMove = 5;
        public void AddBestMove(State _bestMove)
        {
            ListBestMove.Add(_bestMove);
            //xap xep
            for (int i = 0; i < ListBestMove.Count - 1; i++)
            {
                for (int j = i + 1; j < ListBestMove.Count; j++)
                {
                    if (ListBestMove[i].score < ListBestMove[j].score)
                    {
                        State temp = new State(ListBestMove[i]);
                        ListBestMove[i] = ListBestMove[j];
                        ListBestMove[j] = temp;
                    }
                }
            }

            if (ListBestMove.Count > sizeBestMove)
            {
                ListBestMove.RemoveAt(sizeBestMove);
            }
        }
        int sizeBestMoveDepth = 10;
        int sizeCheck = 3;
        public List<List<int>> listScoreDepth = new List<List<int>>();

        // public int Check 
        int numCheck = 0;

        private void RemoveOldDepth(int depth)
        {
            while (listScoreDepth.Count > MaxDepth - depth + 1)
            {
                listScoreDepth.Remove(listScoreDepth[listScoreDepth.Count - 1]);
            }
        }
        private bool IsBestMove(int _score, int _depth)
        {
            if (listScoreDepth.Count <= MaxDepth - _depth)
            {
                listScoreDepth.Add(new List<int>());
            }
            List<int> scoreDepth = listScoreDepth[listScoreDepth.Count - 1];
            if (scoreDepth.Count < sizeBestMoveDepth)
            {
                scoreDepth.Add(_score);
            }
            else
            {
                if (_score > scoreDepth[0])
                {
                    scoreDepth[0] = _score;
                }
                else
                {
                    return false;
                }
            }

            //xap xep
            for (int i = 0; i < scoreDepth.Count - 1; i++)
            {
                for (int j = i + 1; j < scoreDepth.Count; j++)
                {
                    if (scoreDepth[i] > scoreDepth[j])
                    {
                        int temp = scoreDepth[i];
                        scoreDepth[i] = scoreDepth[j];
                        scoreDepth[j] = temp;
                    }
                }
            }
            return true;
        }
        long t = 0;
        public int AlphaBeta(int depth, bool RED, int Alpha, int Beta)
        {
            numCheck++;
            if (depth == 0)
            {           
                return Eval(RED);
            }
            int best = -100000;
            State bestmove = null;
            long k =  System.DateTime.Now.Ticks;
            List<State> arrMoves = board.allMove(!RED);
            t += System.DateTime.Now.Ticks - k;
            //check con tuong hay khong????
            if (arrMoves.Count == 0)
            {
                return -10000 - depth;
            }
            int i = 0;
            while (i < arrMoves.Count && best < Beta)
            {
                State m;
                m = arrMoves[i];
                if (best > Alpha)
                {
                    Alpha = best;
                }
                MakeMove(m);
                RED = !RED;
                int value = -AlphaBeta(depth - 1, RED, -Beta, -Alpha);
                UnMakeMove(m);
                RED = !RED;
                m.score = value;
                if (value > best)
                {
                    best = value;
                    bestmove = m;
                    if (depth == MaxDepth)
                    {
                        if (depth == MaxDepth)
                        {
                            AddBestMove(m);
                        }
                    }
                }
                if (depth == MaxDepth)
					
                {
                    if (value == best)
                        AddBestMove(m);
                }
                i++;
            }
            if (depth == MaxDepth)
            {
                MyBestMove = bestmove;
            }
            return best;
        }

        public List<State> GenerateMove(bool RED)
        {
            int alpha = -100000;
            int beta = 100000;
            ListBestMove = new List<State>();
            numCheck = 0;
            t = 0;
            long xxxx = DateTime.Now.Ticks;
            int sizeMove = board.allMove(!RED).Count + board.allMove(RED).Count;
            System.Random rnd = new System.Random();
			Debug.Log (board.allMove(!RED).Count.ToString() +"+"+ board.allMove(RED).Count.ToString());
            if (sizeMove < 5)
            {
                MaxDepth = 12;
            }
            else if (sizeMove < 10)
            {
                MaxDepth = 9;
            }
            else if (sizeMove < 15)
            {
                MaxDepth = 8;
            }
            else if (sizeMove < 20)
            {
                MaxDepth = 7;
            }
            else if (sizeMove < 30)
            {
                MaxDepth = 6;
            }
            else if (sizeMove < 50)
            {
                MaxDepth = 5;
            }
            else if (sizeMove < 60)
            {
                MaxDepth = rnd.Next(4, 5);
            }
            else if (sizeMove < 100)
            {
				MaxDepth = rnd.Next(2, 4);
            }
            else
                MaxDepth = 2;
            AlphaBeta(MaxDepth, RED, -beta, -alpha);

            return ListBestMove;
        }


    }
    #endregion

	class AIGameCoTuong 
    {
        static CellBoard[,] cell;
        static BoardAI board;
        private static Thread thread;
        public static void AiMoveChess(Node[,] mNode, bool turnRed)
        {
            if (thread != null)
            {
                thread.Interrupt();
                thread = null;
            }

            thread = new Thread(() =>
            {
                cell = new CellBoard[10, 9];
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        cell[i, j] = new CellBoard();
                        cell[i, j].isShow = mNode[i, j].chess.show;
                        if (!mNode[i, j].isChess)
                            cell[i, j].idChess = 0;
                        else
                        {
                            if (!mNode[i, j].chess.teamRed)
                            {
                                switch (mNode[i, j].chess.typeChess)
                                {
                                    case TypeChess.Tuong:
                                        {
                                            cell[i, j].idChess = 15;
                                            break;
                                        }
                                    case TypeChess.Xe:
                                        {
                                            cell[i, j].idChess = 19;
                                            break;
                                        }
                                    case TypeChess.Ma:
                                        {
                                            cell[i, j].idChess = 18;
                                            break;
                                        }
                                    case TypeChess.Tinh:
                                        {
                                            cell[i, j].idChess = 17;
                                            break;
                                        }
                                    case TypeChess.Sy:
                                        {
                                            cell[i, j].idChess = 16;
                                            break;
                                        }
                                    case TypeChess.Phao:
                                        {
                                            cell[i, j].idChess = 20;
                                            break;
                                        }
                                    case TypeChess.Chot:
                                        {
                                            cell[i, j].idChess = 21;
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                switch (mNode[i, j].chess.typeChess)
                                {
                                    case TypeChess.Tuong:
                                        {
                                            cell[i, j].idChess = 8;
                                            break;
                                        }
                                    case TypeChess.Xe:
                                        {
                                            cell[i, j].idChess = 12;
                                            break;
                                        }
                                    case TypeChess.Ma:
                                        {
                                            cell[i, j].idChess = 11;
                                            break;
                                        }
                                    case TypeChess.Tinh:
                                        {
                                            cell[i, j].idChess = 10;
                                            break;
                                        }
                                    case TypeChess.Sy:
                                        {
                                            cell[i, j].idChess = 9;
                                            break;
                                        }
                                    case TypeChess.Phao:
                                        {
                                            cell[i, j].idChess = 13;
                                            break;
                                        }
                                    case TypeChess.Chot:
                                        {
                                            cell[i, j].idChess = 14;
                                            break;
                                        }
                                }
                            }
                        }
                    }
                }
					if(COTUONG.GameController.instance.RoomInfoIG.RoomType == RoomSelect.TruyenThong)
						board = new BoardAI(cell,false,turnRed);
					else
						board = new BoardAI(cell,true,turnRed);
					
                _AI ai = new _AI(board);
                List<State> ListState = ai.GenerateMove(board.RED);

                List<State> listStateAI = new List<State>();
                listStateAI.Add(ai.MyBestMove);
                for (int i = 0; i < ListState.Count; i++)
                {
                    if (ListState[i].score + 3 > ai.MyBestMove.score)
                    {
                        listStateAI.Add(ListState[i]);
                    }
                }
                System.Random rnd = new System.Random();
                int rand = rnd.Next(0, listStateAI.Count);
                //State bestMove = listStateAI[rand];
                // ai.MyBestMove = listStateAI[rand];

                //bestMove

                NodeBoard nodeStart = new NodeBoard(ai.MyBestMove.prev.x, ai.MyBestMove.prev.y);
                NodeBoard nodeEnd = new NodeBoard(ai.MyBestMove.curr.x, ai.MyBestMove.curr.y);
                //int numBatBien = Board.instance.NumBatBien(nodeStart, nodeEnd);
               // if (numBatBien < 1)
               // {
						//GameController.instance.clNodeSX = ai.MyBestMove.prev.y;
						//GameController.instance.clNodeSY = ai.MyBestMove.prev.x;
						//GameController.instance.clNodeEX = ai.MyBestMove.curr.y;
						//GameController.instance.clNodeEY = ai.MyBestMove.curr.x;
						//GameController.instance.BotMoving = true;

                    //return;
               // }
				/*
                for (int cBestMove = 0; cBestMove < ListState.Count; cBestMove++)
                {
                    nodeStart = new NodeBoard(ListState[cBestMove].prev.x, ListState[cBestMove].prev.y);
                    nodeEnd = new NodeBoard(ListState[cBestMove].curr.x, ListState[cBestMove].curr.y);
                    numBatBien = Board.instance.NumBatBien(nodeStart, nodeEnd);
                    if (numBatBien < 2)
                    {
							GameController.instance.BotCallMove(2,GameController.instance.RoomInfoIG.indexMove + 1,ai.MyBestMove.prev.x,ai.MyBestMove.prev.y,ai.MyBestMove.curr.x,ai.MyBestMove.curr.y,true,
							Board.instance.mNode[ai.MyBestMove.prev.y,ai.MyBestMove.prev.x].isChess,Board.instance.mNode[ai.MyBestMove.prev.y,ai.MyBestMove.prev.x].chess.show,Board.instance.mNode[ai.MyBestMove.prev.y,ai.MyBestMove.prev.x].chess.teamRed,Board.instance.mNode[ai.MyBestMove.prev.y,ai.MyBestMove.prev.x].chess.teamRedShow,Board.instance.mNode[ai.MyBestMove.prev.y,ai.MyBestMove.prev.x].chess.typeChess,Board.instance.mNode[ai.MyBestMove.prev.y,ai.MyBestMove.prev.x].chess.typeChessShow,
							Board.instance.mNode[ai.MyBestMove.curr.y,ai.MyBestMove.curr.x].isChess,Board.instance.mNode[ai.MyBestMove.curr.y,ai.MyBestMove.curr.x].chess.show,Board.instance.mNode[ai.MyBestMove.curr.y,ai.MyBestMove.curr.x].chess.teamRed,Board.instance.mNode[ai.MyBestMove.curr.y,ai.MyBestMove.curr.x].chess.teamRedShow,Board.instance.mNode[ai.MyBestMove.curr.y,ai.MyBestMove.curr.x].chess.typeChess,Board.instance.mNode[ai.MyBestMove.curr.y,ai.MyBestMove.curr.x].chess.typeChessShow);
                        return;
                    }
                }
                */
            });
            thread.Name = "Thread_Kỳ_đạo";
            thread.Priority = System.Threading.ThreadPriority.Highest;
            thread.Start();
        }
    }
}