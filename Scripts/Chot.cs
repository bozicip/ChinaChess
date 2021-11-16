using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Chot  {
    public static void ChessClick(NodeBoard _posStart)
    {
        var listNode = AllPosMove(_posStart);
        for (int i = 0; i < listNode.Count; i++)
        {
            Board.instance.mNode[listNode[i].y, listNode[i].x].OnNodeMove(true);
			Debug.Log ("Nuoc co the di :" + listNode.Count);
        }
    }
    public static List<NodeBoard> AllPosMove(NodeBoard _posStart)
    {
        List<NodeBoard> listNode = new List<NodeBoard>();
        if(Board.instance.mNode[_posStart.y,_posStart.x].chess.typeChess == TypeChess.Chot)
        {
            if(Board.instance.mNode[_posStart.y,_posStart.x].chess.teamRed == false)
            {
                if(_posStart.AddPosY(1)!= null)
                {
                    if (Board.instance.isMove(_posStart, _posStart.AddPosY(1)))
                    {
                        listNode.Add(_posStart.AddPosY(1));
                    }
                }
                if(_posStart.y > 4)
                {
                    if (_posStart.AddPosX(-1) != null)
                    {
                        if( Board.instance.isMove(_posStart, _posStart.AddPosX(-1)))
                        {
                            listNode.Add(_posStart.AddPosX(-1));
                        }
                    }

                    if (_posStart.AddPosX(1) != null)
                    {
                        if (Board.instance.isMove(_posStart, _posStart.AddPosX(1)))
                        {
                            listNode.Add(_posStart.AddPosX(1));
                        }
                    }
                }
            }
            else
            {
                if (_posStart.AddPosY(-1) != null)
                {
                    if (Board.instance.isMove(_posStart, _posStart.AddPosY(-1)))
                    {
                        listNode.Add(_posStart.AddPosY(-1));
                    }
                }
                if (_posStart.y <=4)
                {
                    if (_posStart.AddPosX(-1) != null)
                    {
                        if (Board.instance.isMove(_posStart, _posStart.AddPosX(-1)))
                        {
                            listNode.Add(_posStart.AddPosX(-1));
                        }
                    }

                    if (_posStart.AddPosX(1) != null)
                    {
                        if (Board.instance.isMove(_posStart, _posStart.AddPosX(1)))
                        {
                            listNode.Add(_posStart.AddPosX(1));
                        }
                    }
                }
            }

        }
        return listNode;
    }
}
