
using System.Collections.Generic;

public class Phao
{
    public static void ChessClick(NodeBoard _posStart)
    {
        var listNode = AllPosMove(_posStart);
        for (int i = 0; i < listNode.Count; i++)
        {
            Board.instance.mNode[listNode[i].y, listNode[i].x].OnNodeMove(true);
        }
    }

     public static List<NodeBoard> AllPosMove(NodeBoard _posStart)
    {
        List<NodeBoard> listNode = new List<NodeBoard>();

        for (int i = _posStart.x - 1; i >= 0; i--)
        {
            if (Board.instance.mNode[_posStart.y, i].chess.typeChess != TypeChess.None)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (Board.instance.mNode[_posStart.y, j].chess.typeChess != TypeChess.None)
                    {
                        if (Board.instance.isMove(_posStart, new NodeBoard(_posStart.y, j)))
                            listNode.Add(new NodeBoard(_posStart.y, j));
                        break;
                    }
                }
                break;
            }
            else
            {
                if (Board.instance.isMove(_posStart, new NodeBoard(_posStart.y, i)))
                {
                    listNode.Add(new NodeBoard(_posStart.y, i));
                }
            }
        }
    
        for (int i = _posStart.x+1; i <9; i++)
        {
            if (Board.instance.mNode[_posStart.y, i].chess.typeChess!=TypeChess.None)
            {
                for (int j = i + 1; j < 9; j++)
                {
                    if (Board.instance.mNode[_posStart.y, j].chess.typeChess != TypeChess.None)
                    {
                        if (Board.instance.isMove(_posStart, new NodeBoard(_posStart.y, j)))
                            listNode.Add(new NodeBoard(_posStart.y, j));
                        break;
                    }
                }
                break;
            }
            else
            {
                if (Board.instance.isMove(_posStart, new NodeBoard(_posStart.y, i)))
                {
                    listNode.Add(new NodeBoard(_posStart.y, i));
                }
            }
        }

        for (int i = _posStart.y - 1; i >= 0; i--)
        {
            if (Board.instance.mNode[ i,_posStart.x].chess.typeChess != TypeChess.None)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (Board.instance.mNode[ j,_posStart.x].chess.typeChess != TypeChess.None)
                    {
                        if (Board.instance.isMove(_posStart, new NodeBoard( j,_posStart.x)))
                            listNode.Add(new NodeBoard(j,_posStart.x));
                        break;
                    }
                }
                break;
            }
            else
            {
                if (Board.instance.isMove(_posStart, new NodeBoard( i,_posStart.x)))
                {
                    listNode.Add(new NodeBoard(i,_posStart.x));
                }
            }
        }

        for (int i = _posStart.y+1; i < 10; i++)
        {
            if (Board.instance.mNode[i, _posStart.x].chess.typeChess != TypeChess.None)
            {
                for (int j = i + 1; j < 10; j++)
                {
                    if (Board.instance.mNode[j, _posStart.x].chess.typeChess != TypeChess.None)
                    {
                        if (Board.instance.isMove(_posStart, new NodeBoard(j, _posStart.x)))
                            listNode.Add(new NodeBoard(j, _posStart.x));
                        break;
                    }
                }
                break;
            }
            else
            {
                if (Board.instance.isMove(_posStart, new NodeBoard(i, _posStart.x)))
                {
                    listNode.Add(new NodeBoard(i, _posStart.x));
                }
            }
        }

        return listNode;
    }
}
