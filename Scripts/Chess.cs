using System;
    
 public enum TypeChess
{
    None,
    Tuong,
    Xe,
    Phao,
    Ma,
    Tinh,
    Sy,
    Chot
}

public class TypeBlueRedChess
{
	public bool isRed;
	public TypeChess typeChess;

	public TypeBlueRedChess(bool isRed, TypeChess typeChess) {
		this.isRed = isRed;
		this.typeChess = typeChess;
	}
}

[Serializable]
public class Chess
{
    public NodeBoard node;
    public bool show; // quân có hiện không?     
    public bool teamRed; // loại quân true quân đỏ, false quân xanh
	public bool teamRedShow;
    public TypeChess typeChess; //quân sử dụng 
    public TypeChess typeChessShow; //Quân cờ active  nêu cờ  úp 


    //public string typeChess {
    //    get
    //    {
    //        return typeChess.ToString();
    //    }
    //    set
    //    {
    //        typeChess = ParseEnum<TypeChess>(value);
    //    }
    //}
    public TypeChess TypeChessShow
    {
        get {
            return typeChessShow; }
        set
        {
            typeChess = value;
            typeChessShow = value;
        }
    }
    public Chess()
    {
        show = false;
        typeChess = TypeChess.None;
    }
    public  Chess(Chess _chess)
    {
        this.typeChess = _chess.typeChess;
        this.show = _chess.show;
        this.teamRed = _chess.teamRed;
		this.teamRedShow = _chess.teamRedShow;
        this.TypeChessShow = _chess.typeChessShow;
    }
    public Chess(TypeChess _typeChess, bool _isTeamRed,bool _isShow)
    {
        show = _isShow;
        this.teamRed = _isTeamRed;
        this.typeChess = _typeChess;
        if (show)
        {
            this.typeChessShow = _typeChess;
        }
        else
        {
            this.typeChessShow = TypeChess.None;
        }
    }
	public Chess(TypeChess _typeChess, bool _isTeamRed,bool _isTeamRedShow, bool _isShow )//DUNG CHO TUONG
	{
		show = _isShow;
		typeChess = _typeChess;
		teamRed = _isTeamRed;
		teamRedShow = _isTeamRedShow;
		typeChessShow = _typeChess;
	}
	public Chess(TypeChess _typeChess, bool _isTeamRed, bool _isShow , TypeChess _typeChessShow)
	{
		show = _isShow;
		typeChess = _typeChess;
		teamRed = _isTeamRed;
		teamRedShow = _isTeamRed;
		typeChessShow = _typeChessShow;
	}
	public Chess(TypeChess _typeChess, bool _isTeamRed, bool _isShow,bool _isTeamRedShow, TypeChess _typeChessShow)
    {
        show = _isShow;
        typeChess = _typeChess;
        teamRed = _isTeamRed;
		teamRedShow = _isTeamRedShow;
        typeChessShow = _typeChessShow;
    }
	public Chess(TypeChess _typeChess, bool _isTeamRed, bool _isShow, TypeBlueRedChess _typeChessShow)
	{
		show = _isShow;
		typeChess = _typeChess;
		teamRed = _isTeamRed;
		teamRedShow = _typeChessShow.isRed;
		typeChessShow = _typeChessShow.typeChess;
	}

    public void ShowChess()
    {
        show = true;
        typeChess = typeChessShow;
    }
	public void ShowChessCL(){
		show = true;
		teamRed = teamRedShow;
		typeChess = typeChessShow;
	}

    public void ShowChess(TypeChess _typeChessShow)
    {
        show = true;
        typeChess = _typeChessShow;
    }

    public void SetChess(Chess _chess)
    {
        this.show = _chess.show;
        this.typeChess = _chess.typeChess;
        this.teamRed = _chess.teamRed;
        typeChessShow = _chess.typeChessShow;
    }

    public void SetChess()
    {
        show = false;
        typeChess = TypeChess.None;
    }

}


