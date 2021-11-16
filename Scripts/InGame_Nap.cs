using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_Nap : MonoBehaviour {
	public GameObject KhungChon;
	public GameObject VT_GO, MB_GO, VN_GO;
	public string stringNhaMang;// default Viettel
	public string mobileNum;
	public string _Message;
	const string Viettel = "VTT";
	const string Mobi = "VMS";
	const string Vina = "VNP";
	const string GAMENAME = "santhu";
	public UILabel LB_soSeri, LB_maThe;
	public string MenhgiaThe;
	public bool TelcoAvailable = false;
	public GameObject DelayPurchase,VTInfo;
	public GameObject[] BtnShow;
	// IAP
	public GameObject[] IAPContent;
	public string[] VN_Coin,EN_Coin,VN_Pay,EN_Pay;
	public List<infoNap> listNap;
	public UILabel[] _messageSMS, _priceSMS;
	void Start(){
		Invoke("onClickViettel",0.4f);
		InitLayout ();
	}

	public void onClickVTInfo(){
		Application.OpenURL ("http://tichhop.vn/home/get.html");
	}
	public void onClickViettel(){
		stringNhaMang = Viettel;// DEFAULT
		iTween.MoveTo(KhungChon,iTween.Hash("x",VT_GO.transform.position.x,"y",VT_GO.transform.position.y,"time",0.5f));
		//VTInfo.SetActive (true);
		Debug.Log ("MANG : " + stringNhaMang);
	}
	public void onClickMobile(){
		stringNhaMang = Mobi;
		iTween.MoveTo(KhungChon,iTween.Hash("x",MB_GO.transform.position.x,"y",MB_GO.transform.position.y,"time",0.5f));
		VTInfo.SetActive (false);
		Debug.Log ("MANG : " + stringNhaMang);
	}
	public void onClickVina(){
		stringNhaMang = Vina;
		iTween.MoveTo(KhungChon,iTween.Hash("x",VN_GO.transform.position.x,"y",VN_GO.transform.position.y,"time",0.5f));
		VTInfo.SetActive (false);
		Debug.Log ("MANG : " + stringNhaMang);
	}
	public void onClickNapThe(){
		//CLOSE COLLIDER NAP THE
		if (MenhgiaThe == "0")
			NGUIMessage.instance.CALLSMALLMESSAGE ("Lỗi nạp ", "Vui Lòng Chọn Mệnh Giá !!!");
		else {
			NGUIMessage.instance.CALLLOADING (true);
			DelayPurchase.GetComponent<BoxCollider> ().enabled = false;

			StartCoroutine (BANCA.ReqAPI.Instance.NapTheCao (GAMENAME, stringNhaMang, LB_maThe.text, LB_soSeri.text, MenhgiaThe, UserConfig.Data.UserID, (aBool, aString) => {
				if (NGUIMessage.Language == NGUIMessage.VNL)
					NGUIMessage.instance.CALLSMALLMESSAGE ("Nạp thẻ !", aString);
				if (NGUIMessage.Language == NGUIMessage.ENL)
					NGUIMessage.instance.CALLSMALLMESSAGE ("Card charge", aString);

				//OPEN AGAIN WHEN END OF FRAME
				DelayPurchase.GetComponent<BoxCollider> ().enabled = true;
				NGUIMessage.instance.CALLLOADING (false);
			}));
		}
	}
	public UILabel temchangeGia,CardValueShow;
	public void onSelectMenhGia(){

		if (temchangeGia.text == "10.000") {
			MenhgiaThe = "10000";
			CardValueShow.text = " Coins : " + listNap[12]._coinplus + " Cash : " + listNap[12]._cashplus;
		} else if (temchangeGia.text == "20.000") {
			MenhgiaThe = "20000";
			CardValueShow.text = " Coins : " + listNap[13]._coinplus + " Cash : " + listNap[13]._cashplus;
		} else if (temchangeGia.text == "30.000") {
			MenhgiaThe = "30000";
			CardValueShow.text = " Coins : " + listNap[14]._coinplus + " Cash : " + listNap[14]._cashplus;
		} else if (temchangeGia.text == "50.000") {
			MenhgiaThe = "50000";
			CardValueShow.text = " Coins : " + listNap[15]._coinplus + " Cash : " + listNap[15]._cashplus;
		} else if (temchangeGia.text == "100.000") {
			MenhgiaThe = "100000";
			CardValueShow.text = " Coins : " + listNap[16]._coinplus + " Cash : " + listNap[16]._cashplus;
		} else if (temchangeGia.text == "200.000") {
			MenhgiaThe = "200000";
			CardValueShow.text = " Coins : " + listNap[17]._coinplus + " Cash : " + listNap[17]._cashplus;
		} else if (temchangeGia.text == "300.000") {
			MenhgiaThe = "300000";
			CardValueShow.text = " Coins : " + listNap[18]._coinplus + " Cash : " + listNap[18]._cashplus;
		} else if (temchangeGia.text == "500.000") {
			MenhgiaThe = "500000";
			CardValueShow.text = " Coins : " + listNap[19]._coinplus + " Cash : " + listNap[19]._cashplus;
		} else if (temchangeGia.text == "1.000.000") {
			MenhgiaThe = "1000000";
			CardValueShow.text = " Coins : " + listNap[20]._coinplus + " Cash : " + listNap[20]._cashplus;
		} else
			MenhgiaThe = "0";
		Debug.Log ("MENH GIA : " + MenhgiaThe);
	}

	public void onClickClose(){
		StartCoroutine(UltilityGame.instance.TweenPanel(false,this.gameObject));
	}

	public void XuliTelco(string _stringTelco){
		Debug.Log (" SO TELCO : "  + _stringTelco);
		string[] arrListStr = _stringTelco.Split('@');
		mobileNum = arrListStr[0];
		_Message = arrListStr[1]+" "+ UserConfig.Data.UserID;
		TelcoAvailable = true;
	}
	public void onSMS10(){
		if (TelcoAvailable = true) {
			string finalString = mobileNum.Replace ("x", "6");
			sendSMS (finalString);
		}
	}
	public void onSMS15(){
		if(TelcoAvailable = true){
			string finalString = mobileNum.Replace("x","7");
			sendSMS (finalString);
		}
	}

	public void sendSMS (string _mobile_num_final){
		string URL = "";
		#if UNITY_ANDROID || UNITY_WP8
		URL = string.Format("sms:{0}?body={1}",_mobile_num_final,System.Uri.EscapeDataString(_Message));
		#endif

		#if UNITY_IOS
		//ios SMS URL - ios requires encoding for sms call to work
		//string URL = string.Format("sms:{0}?&body={1}",mobile_num,WWW.EscapeURL(message)); //Method1 - Works but puts "+" for spaces
		//string URL ="sms:"+mobile_num+"?&body="+WWW.EscapeURL(message); //Method2 - Works but puts "+" for spaces
		//string URL = string.Format("sms:{0}?&body={1}",mobile_num,System.Uri.EscapeDataString(message)); //Method3 - Works perfect
		string URL ="sms:"+_mobile_num_final+"?&body="+ System.Uri.EscapeDataString(_Message); //Method4 - Works perfectly
		#endif
		//Debug.Log ("URL SMS : " + URL);

		Application.OpenURL(URL);
	}
	// IAP HERE
	public void InitLayout(){
		//DEFAULT MENH GIA
		MenhgiaThe = "0";

		if(NGUIMessage.Language == NGUIMessage.VNL){
			// SMS
			for(int i = 0 ; i < _messageSMS.Length ; i++){
				_messageSMS[i].text = "Nhắn tin để nạp "+listNap[i]._coinplus +" Coin," + listNap[i]._cashplus + " Cashs ";
				_priceSMS [i].text = listNap [i]._type;
			}
			// IAP
			for(int i = 0 ; i < IAPContent.Length;i++){
				IAPContent [i].transform.Find ("Label").GetComponent<UILabel> ().text = listNap[i+2]._coinplus + " Xu " + listNap[i+2]._cashplus + " Cashs" ;
				IAPContent [i].transform.Find ("Button/Animation/Label").GetComponent<UILabel> ().text = listNap[i+2]._showvnd;
			}
		}
		if(NGUIMessage.Language == NGUIMessage.ENL){
			// SMS
			for(int i = 0 ; i < _messageSMS.Length ; i++){
				_messageSMS[i].text = "Messaging for "+listNap[i]._coinplus +" Coin," + listNap[i]._cashplus + " Cashs ";
				_priceSMS [i].text = listNap [i]._type;
			}
			// IAP
			for(int i = 0 ; i < IAPContent.Length;i++){
				IAPContent [i].transform.Find ("Label").GetComponent<UILabel> ().text = listNap[i+2]._coinplus + " Couns " + listNap[i+2]._cashplus + " Cashs" ;
				IAPContent [i].transform.Find ("Button/Animation/Label").GetComponent<UILabel> ().text = listNap[i+2]._showdolla;
			}
		}
	} 

	public void onPurchaseP1(){
		//BillingManager.instance.BuyP1 ();
	}
	public void onPurchaseP2(){
		//BillingManager.instance.BuyP2 ();
	}
	public void onPurchaseP3(){
		//BillingManager.instance.BuyP3 ();
	}
	public void onPurchaseP4(){
		//BillingManager.instance.BuyP4 ();
	}
	public void onPurchaseP5(){
		//BillingManager.instance.BuyP5 ();
	}

	public void onPaypal(){
		Application.OpenURL ("http://www.gameco.online/service/paypal/" + UserConfig.Data.UserID);
		//StartCoroutine (ReqAPI.Instance.Paypal(UserConfig.Data.UserID,(aBool,aString) =>{
		//	if(aBool)
		//		Debug.Log("OPEN PAYPAL PAGE");
		//}));
	}
}
