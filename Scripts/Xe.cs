using UnityEngine;
using System.Collections.Generic;

public class Xe
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
        // move left
        for (int i = _posStart.x - 1; i >= 0; i--)
        {
            if (Board.instance.isMove(_posStart, new NodeBoard(_posStart.y, i)))
            {
                listNode.Add(new NodeBoard(_posStart.y, i));
            }
            if (Board.instance.mNode[_posStart.y, i].isChess)
            {
                break;
            }
        }
        //move right
        for (int i = _posStart.x + 1; i < 9; i++)
        {
            if (Board.instance.isMove(_posStart, new NodeBoard(_posStart.y, i)))
            {
                listNode.Add(new NodeBoard(_posStart.y, i));
            }
            if (Board.instance.mNode[_posStart.y, i].isChess)
            {
                break;
            }
        }

        for (int i = _posStart.y - 1; i >= 0; i--)
        {
            if (Board.instance.isMove(_posStart, new NodeBoard(i, _posStart.x)))
            {
                listNode.Add(new NodeBoard(i, _posStart.x));
            }
            if (Board.instance.mNode[i, _posStart.x].isChess)
            {
                break;
            }
        }

        for (int i = _posStart.y + 1; i < 10; i++)
        {
            if (Board.instance.isMove(_posStart, new NodeBoard(i, _posStart.x)))
            {
                listNode.Add(new NodeBoard(i, _posStart.x));
            }
            if (Board.instance.mNode[i, _posStart.x].isChess)
            {
                break;
            }
        }

        return listNode;
    }
}
