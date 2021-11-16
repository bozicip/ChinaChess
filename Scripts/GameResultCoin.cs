using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TheChieu
{
    public float Heso = 0;
    public string Status = "";
}

public class GameResultCoin : MonoBehaviour
{
    [SerializeField]
    private UILabel lbTienBan;
    [SerializeField]
    private UILabel lbTienAnQuan;
    [SerializeField]
    private UILabel lbTienTong;

    public UILabel Status;
    public TheChieu _TheChieu,_ThamChieu;
    public UILabel lbStatusspecial,lbTienChieu;
    private float tienchieu,tienanquan,tientong;
    private void OnEnable()
    {
        TienChieu = 0;
        TienAnQuan = 0;
        TienTong = 0;
        Status.text = "";
        lbTienChieu.text = "";
    }
    public float TienChieu
    {
        get
        {
            return tienchieu;
        }
        set
        {
            tienchieu = value;
            if (tienchieu > 0)
            {
                lbTienBan.color = Color.yellow;
                lbTienBan.text = tienchieu.ToString("C0");
            }
            else if (tienchieu < 0)
            {
                lbTienBan.color = Color.red;
                lbTienBan.text = "- " + tienchieu.ToString("C0");
            }
            else
            {
                lbTienBan.color = Color.white;
                lbTienBan.text = "$ 0";
            }
        }
    }
    public float TienAnQuan
    {
        get
        {
            return tienanquan;
        }
        set
        {
            tienanquan = value;
            if (tienanquan > 0)
            {
                lbTienAnQuan.color = Color.yellow;
                lbTienAnQuan.text = tienanquan.ToString("C0");
            }
            else if (tienanquan < 0)
            {
                lbTienAnQuan.color = Color.red;
                lbTienAnQuan.text = "- " + tienanquan.ToString("C0");
            }
            else
            {
                lbTienAnQuan.color = Color.white;
                lbTienAnQuan.text = "$ 0";
            }
        }
    }
    public float TienTong
    {
        get
        {
            return tientong;
        }
        set
        {
            tientong = value;
            if (tientong > 0)
            {
                lbTienTong.color = Color.yellow;
                lbTienTong.text = tientong.ToString("C0");
            }
            else if (tientong < 0)
            {
                lbTienTong.color = Color.red;
                lbTienTong.text = "- " + tientong.ToString("C0");
            }
            else
            {
                lbTienTong.color = Color.white;
                lbTienTong.text = "$ 0";
            }
        }
    }
}
