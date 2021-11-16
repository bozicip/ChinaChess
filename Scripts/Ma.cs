using UnityEngine;
using System.Collections.Generic;

public class Ma : MonoBehaviour {

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

        if (_posStart.AddPosX(-1) != null && Board.instance.mNode[_posStart.y,_posStart.x-1].isChess==false)
        {
            if (_posStart.AddPos(-1, -2) != null)
            {
                    if (Board.instance.isMove(_posStart, _posStart.AddPos(-1, -2)))
                        listNode.Add(_posStart.AddPos(-1, -2));
            }
            if (_posStart.AddPos(1, -2) != null)
            {
                if (Board.instance.isMove(_posStart, _posStart.AddPos(1, -2)))
                    listNode.Add(_posStart.AddPos(1, -2));
            }
        }

        if (_posStart.AddPosX(1) != null && Board.instance.mNode[_posStart.y, _posStart.x + 1].isChess == false)
        {
            if (_posStart.AddPos(1, 2) != null)
            {
                if (Board.instance.isMove(_posStart, _posStart.AddPos(1, 2)))
                    listNode.Add(_posStart.AddPos(1, 2));
            }
            if (_posStart.AddPos(-1, 2) != null)
            {
                if (Board.instance.isMove(_posStart, _posStart.AddPos(-1, 2)))
                    listNode.Add(_posStart.AddPos(-1, 2));
            }
        }

        if (_posStart.AddPosY(-1) != null && Board.instance.mNode[_posStart.y-1, _posStart.x].isChess == false)
        {
            if (_posStart.AddPos(-2,-1) != null)
            {
                if (Board.instance.isMove(_posStart, _posStart.AddPos(-2,-1)))
                    listNode.Add(_posStart.AddPos( -2,-1));
            }
            if (_posStart.AddPos( -2,1) != null)
            {
                if (Board.instance.isMove(_posStart, _posStart.AddPos(- 2,1)))
                    listNode.Add(_posStart.AddPos(-2,1));
            }
        }

        if (_posStart.AddPosY(1) != null && Board.instance.mNode[_posStart.y + 1, _posStart.x].isChess == false)
        {
            if (_posStart.AddPos(2, 1) != null)
            {
                if (Board.instance.isMove(_posStart, _posStart.AddPos(2, 1)))
                    listNode.Add(_posStart.AddPos(2, 1));
            }
            if (_posStart.AddPos(2, -1) != null)
            {
                if (Board.instance.isMove(_posStart, _posStart.AddPos(2, -1)))
                    listNode.Add(_posStart.AddPos(2, -1));
            }
        }

        return listNode;
    }
}
