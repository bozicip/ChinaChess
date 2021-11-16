using UnityEngine;
using System.Collections.Generic;

public class Tinh  {

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

        if (_posStart.AddPos(-2, -2) != null)
        {
            if (Board.instance.mNode[_posStart.y - 1, _posStart.x - 1].isChess == false)
                if (Board.instance.isMove(_posStart, _posStart.AddPos(-2, -2)))
                    listNode.Add(_posStart.AddPos(-2, -2));
        }

        if (_posStart.AddPos(-2, 2) != null)
        {
            if (Board.instance.mNode[_posStart.y - 1, _posStart.x + 1].isChess == false)
                if (Board.instance.isMove(_posStart, _posStart.AddPos(-2, 2)))
                    listNode.Add(_posStart.AddPos(-2, 2));
        }

        if (_posStart.AddPos(2, -2) != null)
        {
            if (Board.instance.mNode[_posStart.y + 1, _posStart.x - 1].isChess == false)
                if (Board.instance.isMove(_posStart, _posStart.AddPos(2, -2)))
                    listNode.Add(_posStart.AddPos(2, -2));
        }

        if (_posStart.AddPos(2, 2) != null)
        {
            if (Board.instance.mNode[_posStart.y + 1, _posStart.x + 1].isChess == false)
                if (Board.instance.isMove(_posStart, _posStart.AddPos(2, 2)))
                    listNode.Add(_posStart.AddPos(2, 2));
        }

		if (COTUONG.GameController.instance.RoomInfoIG.RoomType == RoomSelect.TruyenThong)
        {
            for (int i = listNode.Count - 1; i >= 0; i--)
            {
                if (listNode[i].y == 3 || listNode[i].y == 6)
                    listNode.RemoveAt(i);
            }
        }
        return listNode;
    }

}
