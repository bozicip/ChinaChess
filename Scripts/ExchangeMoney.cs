using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeMoney : MonoBehaviour
{
    public UIInput inputMoney;
    int GametoChess = 0;
    public UILabel TextMoney;
    public UILabel CoinAvailable;
    bool canChange = false;
    private void OnEnable()
    {
        if(GametoChess == 0)
            CoinAvailable.text = UserConfig.Data.UserCoins.ToString("C0");
        if(GametoChess == 1)
            CoinAvailable.text = UserConfig.Data.UserCoins.ToString("C0");
    }
    void OnGameToChess()
    {
        GametoChess = 0;
        CoinAvailable.text = UserConfig.Data.UserCoins.ToString("C0");
    }
    void OnChessToGame()
    {
        GametoChess = 1;
        CoinAvailable.text = UserConfig.Data.UserCoins.ToString("C0");
    }
    void OnExchangeMoney()
    {
        if (canChange) {
            NGUIMessage.instance.CALLLOADING(true);
            StartCoroutine(COTUONG.ReqAPI.instance.DoiTien(UserConfig.Data.UserName, inputMoney.value, GametoChess.ToString(), (aBool,aMessage) => {
                NGUIMessage.instance.CALLLOADING(false);
                if (aBool)
                {
                    StartCoroutine(NGUIMessage.instance.CALLALERT(" Chuyển tiền thành công  !"));
                    string cloneName = UserConfig.Data.UserName.Replace(" ", "");
                    StartCoroutine(BANCA.ReqAPI.Instance.GetUserInfo(UserConfig.Data.UserID, (abool,_GETUSERIDHERE) => {
                        UserConfig.Data = _GETUSERIDHERE;
                        //SHOW INFO AFTER LOAD

                        RoomSelect.instance.pName.text = UserConfig.Data.UserName;
                        RoomSelect.instance.pCoin.text = UserConfig.Data.UserCoins.ToString("C0");
                        RoomSelect.instance.isLoadCompleted = true;
                        CheckDuLieuGia();
                        OnCloseExchange();
                    }));
                }
                else
                {
                    StartCoroutine(NGUIMessage.instance.CALLALERT(" Điều chỉnh lại tiền cần chuyển !"));
                }
            }));
        }
        else
        {
            StartCoroutine(NGUIMessage.instance.CALLALERT(" Điều chỉnh lại tiền cần chuyển !"));
        }
    }
    void CheckDuLieuGia()
    {
        if(GametoChess == 0)
        {
            UserConfig.Data.UserCoins -= int.Parse(inputMoney.value);
        }
        if(GametoChess == 1)
        {
            UserConfig.Data.UserCoins += int.Parse(inputMoney.value);
        }
    }
    public void SubmitMoney()
    {
        int Clone = int.Parse(inputMoney.value);
        TextMoney.text = Clone.ToString("C0");
        if (GametoChess == 1 )
        {
            if (int.Parse(inputMoney.value) > UserConfig.Data.UserCoins)
            {
                StartCoroutine(NGUIMessage.instance.CALLALERT("Không đủ tiền "));
            }
            else
            {
                canChange = true;
            } 
        }
        else
        {
            if(int.Parse(inputMoney.value) > UserConfig.Data.UserCoins)
            {
                StartCoroutine(NGUIMessage.instance.CALLALERT("Không đủ tiền "));
            }
            else
            {
                canChange = true;
            }
        }
    }
    void OnCloseExchange()
    {
        StartCoroutine(UltilityGame.instance.TweenPanel(false,this.gameObject));
    }
}

