using UnityEngine;
using System.Collections;

[System.Serializable]
public class NodeBoard
{
    public int x, y;

    public NodeBoard(int _posY, int _posX)
    {
        this.x = _posX;
        this.y = _posY;
    }

    public NodeBoard(NodeBoard _nodeBoard)
    {
        this.x = _nodeBoard.x;
        this.y = _nodeBoard.y;
    }

    public void setNode(int _posY, int _posX)
    {
        this.x = _posX;
        this.y = _posY;
    }

    public void setNode(NodeBoard _node)
    {
        x = _node.x;
        y = _node.y;
    }

    public NodeBoard AddPosX(int _addX)
    {
        if (x + _addX < 0 || x + _addX > 8)
            return null;
        return new NodeBoard(y, this.x + _addX);
    }
    public NodeBoard AddPosY(int _addY)
    {
        if (y + _addY < 0 || y + _addY > 9)
            return null;
        return new NodeBoard(this.y + _addY, this.x);
    }

    public NodeBoard AddPos(int _addY, int _addX)
    {
        if (y + _addY < 0 || y + _addY > 9)
            return null;
        if (x + _addX < 0 || x + _addX > 8)
            return null;
        return new NodeBoard(this.y + _addY, this.x + _addX);
    }

    public bool isEquals(NodeBoard _node)
    {
        if (_node.x == x && _node.y == y)
        {
            return true;
        }
        return false;
    }
}
