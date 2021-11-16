using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tuong  {


    public static void ChessClick(NodeBoard _posStart)
    {
        var listNode =  AllPosMove(_posStart);
        for(int i =0;i<listNode.Count;i++)
        {
            Board.instance.mNode[listNode[i].y, listNode[i].x].OnNodeMove(true);
        }
    }

    public static List<NodeBoard> AllPosMove(NodeBoard _posStart)
    {
        List<NodeBoard> listNode = new List<NodeBoard>();
        if (_posStart.AddPosY(1) != null)
        {
            if (_posStart.AddPosY(1).y != 3)
            {
                if (Board.instance.isMove(_posStart, _posStart.AddPosY(1)))
                {
                    listNode.Add(_posStart.AddPosY(1));
                }
            }
        }

        if (_posStart.AddPosY(-1) != null)
        {
            if (_posStart.AddPosY(-1).y != 6)
            {
                if (Board.instance.isMove(_posStart, _posStart.AddPosY(-1)))
                {
                    listNode.Add(_posStart.AddPosY(-1));
                }
            }
        }


        if (_posStart.AddPosX(1).x != 6)
        {
            if (Board.instance.isMove(_posStart, _posStart.AddPosX(1)))
            {
                listNode.Add(_posStart.AddPosX(1));
            }
        }
        if (_posStart.AddPosX(-1).x != 2)
        {
            if (Board.instance.isMove(_posStart, _posStart.AddPosX(-1)))
            {
                listNode.Add(_posStart.AddPosX(-1));
            }
        }

        return listNode;
    }
    public static List<NodeBoard> AllPosCanMoveWhenBi(NodeBoard _posStart)
    {
        List<NodeBoard> listNode = new List<NodeBoard>();
        if (_posStart.AddPosY(1) != null)
        {
            if (_posStart.AddPosY(1).y != 3)
            {
                if (Board.instance.isEmuMoveBi(_posStart, _posStart.AddPosY(1)))
                {
                    listNode.Add(_posStart.AddPosY(1));
                }
            }
        }

        if (_posStart.AddPosY(-1) != null)
        {
            if (_posStart.AddPosY(-1).y != 6)
            {
                if (Board.instance.isEmuMoveBi(_posStart, _posStart.AddPosY(-1)))
                {
                    listNode.Add(_posStart.AddPosY(-1));
                }
            }
        }


        if (_posStart.AddPosX(1).x != 6)
        {
            if (Board.instance.isEmuMoveBi(_posStart, _posStart.AddPosX(1)))
            {
                listNode.Add(_posStart.AddPosX(1));
            }
        }
        if (_posStart.AddPosX(-1).x != 2)
        {
            if (Board.instance.isEmuMoveBi(_posStart, _posStart.AddPosX(-1)))
            {
                listNode.Add(_posStart.AddPosX(-1));
            }
        }

        return listNode;
    }


}