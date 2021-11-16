using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using PlayFab.ClientModels;
using System.Runtime.InteropServices;
//using Facebook.MiniJSON;
using System.Security.Cryptography.X509Certificates;
using System.Security;
using System.Net;
namespace BANCA
{
	public class ReqAPI : MonoBehaviour
	{
		public static ReqAPI instance = new ReqAPI();

		public static ReqAPI Instance
		{
			get
			{
				if (instance == null)
					instance = new ReqAPI();
				return instance;
			}
		}

		public IEnumerator SendLanguage(string _userID, string _languageid, Action<bool, string, string> aSuccess)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkSendLanguage + _userID + "/" + _languageid, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				JSON[] dataLanguage = itemJson.ToArray<JSON>("data");
				string a = dataLanguage[0].ToString("language");
				aSuccess(true, itemJson.ToString("message"), a);
			}
		}
		public IEnumerator GetMailBox(string _userID, Action<List<MailList>> acListMail)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkMailbox + _userID, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				JSON[] dataMail = itemJson.ToArray<JSON>("data");
				//Debug.LogError(dataMail.Length);

				if (dataMail.Length != 0)
				{
					Debug.Log(dataMail[0].ToString("title"));
					List<MailList> listMail = new List<MailList>();
					for (int i = 0; i < dataMail.Length; i++)
					{
						MailList tempMail = new MailList()
						{
							mail_id = int.Parse(dataMail[i].ToString("id")),
							mail_title = dataMail[i].ToString("title"),
							mail_message = dataMail[i].ToString("message"),
							mail_coin = dataMail[i].ToString("coin"),
							mail_readed = int.Parse(dataMail[i].ToString("readed")),
							mail_recieved = int.Parse(dataMail[i].ToString("recieved")),
							mail_createday = dataMail[i].ToString("created")
						};
						listMail.Add(tempMail);
					}
					acListMail(listMail);
				}
				else
				{// MAIL = 0;
					List<MailList> listMail = new List<MailList>();
					acListMail(listMail);// Set as Null
				}
			}
			else
			{
				Debug.Log(itemJson.ToString("message"));
			}
		}
		public IEnumerator OpenMail(int MailID, Action<bool, string, string> aPlayerInfo)
		{
			var form = new WWWForm();
			form.AddField("id", MailID);
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkOpenMail + MailID.ToString(), form);
			Debug.Log("link request = " + www.url);
			yield return www;
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				JSON[] allData = itemJson.ToArray<JSON>("data");
				Debug.Log("All data : " + allData.Length);
				//GET INFO FIRST
				JSON[] useridData = allData[0].ToArray<JSON>("User");
				string tempinfo = useridData[0].ToString("coin");
				aPlayerInfo(true, itemJson.ToString("message"), tempinfo);
			}
			else
			{
				aPlayerInfo(false, itemJson.ToString("message"), "");
			}

		}
		public IEnumerator DeleteMail(int MailID, Action<bool, string> aSuccess)
		{
			var form = new WWWForm();
			form.AddField("id", MailID);
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkDeleteMail + "/" + MailID.ToString(), form);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				aSuccess(true, itemJson.ToString("message"));
			}
			else
			{
				aSuccess(false, itemJson.ToString("message"));
			}
		}
		public IEnumerator NhapGiftcode(string _userID, string _giftcode, Action<bool, string> aSuccess)
		{
			var form = new WWWForm();
			form.AddField("id", _userID);
			form.AddField("giftcode", _giftcode);
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkNhapGiftcode + _userID.ToString() + "/" + _giftcode, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				aSuccess(true, itemJson.ToString("message"));
			}
			else
			{
				aSuccess(false, itemJson.ToString("message"));
			}
		}
		public IEnumerator Userrate(string UserID, Action<string> aSuccess)
		{
			var form = new WWWForm();

			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkUserrate + "/" + UserID, form);
			yield return www;
			Debug.Log("link request = " + www.url);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			aSuccess(itemJson.ToString("message"));

		}

		public IEnumerator OnlineReward(string _userID, string _timeOnline, Action<bool, string> aSuccess)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkOnlineReward + _userID + "/" + _timeOnline, form);
			yield return www;
			Debug.Log("link request = " + www.url);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				aSuccess(true, itemJson.ToString("message"));
			}
			else
			{
				aSuccess(false, itemJson.ToString("message"));
			}
		}

		public IEnumerator Spining(string _userID, Action<bool, string, string, int> aQuaySoForce)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkSpin + _userID, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				JSON[] allData = itemJson.ToArray<JSON>("data");
				Debug.Log("All data : " + allData.Length);
				//GET INFO FIRST
				JSON[] useridData = allData[0].ToArray<JSON>("User");
				string cloneCoinResult = useridData[0].ToString("coin");
				string clonenbSpin = useridData[0].ToString("nbspin");

				JSON[] userResult = allData[1].ToArray<JSON>("Result");
				int cloneResult = userResult[0].ToInt("result");
				aQuaySoForce(true, cloneCoinResult, clonenbSpin, cloneResult);
			}
			else
			{
				aQuaySoForce(false, itemJson.ToString("message"), null, 0);
			}
		}
		public IEnumerator BuySpin(string _userID, string _luotquay, Action<bool, string, string, string> aBuySpin)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkMuaSpin + _userID + "/" + _luotquay, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				JSON[] allData = itemJson.ToArray<JSON>("data");

				JSON[] useridData = allData[0].ToArray<JSON>("User");
				string cloneCoinResult = useridData[0].ToString("coin");
				string clonenbSpin = useridData[0].ToString("nbspin");

				aBuySpin(true, itemJson.ToString("message"), cloneCoinResult, clonenbSpin);
			}
			else
			{
				aBuySpin(false, itemJson.ToString("message"), null, null);
			}
		}
		public IEnumerator Rename(string _userID, string _newName, Action<bool, string, string> aSuccess)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkRename + _userID + "/" + _newName, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				aSuccess(true, itemJson.ToString("message"), _newName);
			}
			else
			{
				aSuccess(false, itemJson.ToString("message"), null);
			}
		}
		public IEnumerator NapTheCao(string _gameName, string _nhaMang, string _Mathe, string _soSeri, string _menhgia, string _userID, Action<bool, string> aSuccess)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			string templink = APILinked.linkNapTheCao + _gameName + "/" + _nhaMang + "/" + _Mathe + "/" + _soSeri + "/" + _menhgia + "/" + _userID;
			string a = templink.Replace(" ", "");
			WWW www = new WWW(a, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				aSuccess(true, itemJson.ToString("message"));
			}
			else
			{
				aSuccess(false, itemJson.ToString("message"));
			}
		}

		public IEnumerator GetLeaderBoard(string _type, string _datetime, string _fishtype, Action<List<ListLeaderBoard>> aList)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkLeaderBoard + _type + "/" + _datetime + "/" + _fishtype, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				JSON[] allData = itemJson.ToArray<JSON>("data");
				if (allData.Length != 0)
				{
					List<ListLeaderBoard> cloneList = new List<ListLeaderBoard>();
					for (int i = 0; i < allData.Length; i++)
					{
						ListLeaderBoard tempLeaderBoard = new ListLeaderBoard()
						{
							LB_userID = allData[i].ToString("id"),
							LB_userName = allData[i].ToString("username"),
							LB_coin = allData[i].ToString("coin"),
							LB_totalFishtype = allData[i].ToString("totalfishtype"),
							LB_usingwheel = allData[i].ToString("usingluckywheel"),
							LB_exp = allData[i].ToString("exp")
						};
						cloneList.Add(tempLeaderBoard);
					}
					aList(cloneList);
				}
			}
		}

		public IEnumerator SaveData(string _userID, string _score, string _nbFish, Action<bool, string> aSuccess)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			string cloneSecurity = NGUIMessage.instance.XuLiMd5Key(_userID + _score + _nbFish + UserConfig.Data.UserKey + "HDGAMES");
			WWW www = new WWW(APILinked.linkSaveData + _userID + "/" + _score + "/" + _nbFish + "/" + cloneSecurity, form);
			yield return www;
			Debug.Log("link request = " + www.url);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			Debug.Log(www.text);
			if (itemJson.ToBoolean("error") == false)
			{
				JSON[] allData = itemJson.ToArray<JSON>("data");
				JSON[] useridData = allData[0].ToArray<JSON>("User");
				string newKey = useridData[0].ToString("security");
				aSuccess(true, newKey);
			}
		}
		public IEnumerator Exchange(string _userID, string _type, string _value, string _telco, Action<bool, string, string, string> aSuccess)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkDoiThuong + _userID + "/" + _type + "/" + _value + "/" + _telco, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				JSON[] allData = itemJson.ToArray<JSON>("data");
				JSON[] useridData = allData[0].ToArray<JSON>("User");
				string cloneCoin = useridData[0].ToString("coin");
				string cloneCash = useridData[0].ToString("cash");
				aSuccess(true, itemJson.ToString("message"), cloneCoin, cloneCash);
			}
			else
			{
				aSuccess(false, itemJson.ToString("message"), "", "");
			}
		}
		public IEnumerator UpdateInfo(string _userID, string _mail, string _phone, Action<bool, string> aSuccess)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkUpdateinfo + _userID + "/" + _mail + "/" + _phone, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				aSuccess(true, itemJson.ToString("message"));
			}
			else
				aSuccess(false, itemJson.ToString("message"));
		}

		public IEnumerator RegisterAccount(string _ID, string _Pass, string _Email, string _Name , Action<bool,string> aSuccsecc)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkDangKi + _ID + "/" + _Pass + "/" + _Email + "/" + _Name, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				aSuccsecc(true, itemJson.ToString("message"));
			}else
				aSuccsecc(false, itemJson.ToString("message"));
		}

		public IEnumerator LoginGame(string _ID, string _Pass ,Action<bool, string> aSuccsecc)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkLogin + _ID + "/" + _Pass, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				aSuccsecc(true, itemJson.ToString("message"));
			}
			else
				aSuccsecc(false, itemJson.ToString("message"));
		}

		public IEnumerator ChangePassword(string _ID,string _OldPass, string _NewPass, Action<bool, string> aSuccsecc)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkChangePassword + _ID + "/" + _OldPass + "/" + _NewPass, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				aSuccsecc(true, itemJson.ToString("message"));
			}
			else
				aSuccsecc(false, itemJson.ToString("message"));
		}

		public IEnumerator ForgetPassword(string _ID, string _Email,  Action<bool, string> aSuccsecc)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkForgetPassword + _ID + "/" + _Email, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				aSuccsecc(true, itemJson.ToString("message"));
			}
			else
				aSuccsecc(false, itemJson.ToString("message"));
		}

		public IEnumerator GetAllData(string _userID, string _userNAME, Action<UserData> aPlayerInfo, Action<List<fishInfo>> aFishInfo, Action<string[], int[]> ListGun, Action<List<infoDoiThe>> aDoiThe, Action<List<ListPromotion>> aListPromotion, Action<List<ListEvent>> aListEvent,

			Action<List<ListMission>> aListMission, Action<DailyBonus> aDaily, Action<QuaysoContent> aQuayso, Action<OnlineTimeCount> aOnlineTime, Action<AdminInfoMNG> aAdminInfo, Action<List<infoNap>> aNap)
		{
			string adress = APILinked.linkGetAllData + _userID + "/" + _userNAME + "/" + NGUIMessage.instance.Platform + "/" + NGUIMessage.instance.BC_GameVersion;

			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkGetAllData + _userID + "/" + _userNAME + "/" + NGUIMessage.instance.Platform + "/" + NGUIMessage.instance.BC_GameVersion, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);

			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				bool isFromServer = false;
				JSON[] allData = itemJson.ToArray<JSON>("data");
				Debug.Log("All data : " + allData.Length);
				// USER ID
				JSON[] useridData = allData[0].ToArray<JSON>("User");
				JSON[] fishData = allData[1].ToArray<JSON>("Fishinfo");
				JSON[] GunData = allData[2].ToArray<JSON>("Guninfo");
				string cloneFishvalue = "";
				string cloneGun = "";
				for (int i = 0; i < fishData.Length; i++)
				{
					cloneFishvalue = cloneFishvalue + fishData[i].ToString("value") + fishData[i].ToString("probability");
				}
				for (int i = 0; i < GunData.Length; i++)
				{
					cloneGun = cloneGun + GunData[i].ToString("value");
				}
				//cloneFishvalue = cloneFishvalue.Substring (0,cloneFishvalue.Length -1);
				//cloneGun = cloneGun.Substring (0,cloneGun.Length -1);
				//CHECK FROM SERVER
				string cloneKey = useridData[0].ToString("coin") + useridData[0].ToString("cash") + useridData[0].ToString("nbspin") + useridData[0].ToString("security") + useridData[0].ToString("exp")
					+ useridData[0].ToString("per_fish") + useridData[0].ToString("per_boss") + cloneFishvalue + cloneGun + "HDGAMES";
				string finalMD5 = NGUIMessage.instance.XuLiMd5Key(cloneKey);
				if (finalMD5 == itemJson.ToString("message"))
					isFromServer = true;
				else
					isFromServer = false;

				UserData infoData = new UserData()
				{
					UserID = useridData[0].ToString("id"),
					UserName = useridData[0].ToString("username"),
					UserCoins = useridData[0].ToInt("coin"),
					UserEXP = useridData[0].ToInt("exp"),
					UserSpinLeft = useridData[0].ToInt("nbspin"),
					UserCashs = useridData[0].ToInt("cash"),
					UserEmail = useridData[0].ToString("email"),
					UserPhone = useridData[0].ToInt("phone"),
					UserKey = useridData[0].ToString("security"),
					UserPerFish = useridData[0].ToFloat("per_fish"),
					UserPerBoss = useridData[0].ToFloat("per_boss"),

					UserElo_CT = useridData[0].ToString("elo_ct"),
					UserElo_CU = useridData[0].ToString("elo_cu"),
					UserElo_CL = useridData[0].ToString("elo_cl"),

					UserLevel_CT = useridData[0].ToString("level_ct"),
					UserLevel_CU = useridData[0].ToString("level_cu"),
					UserLevel_CL = useridData[0].ToString("level_cl"),

					UserTotalMatch_CT = useridData[0].ToString("totalmatch_ct"),
					UserTotalMatch_CU = useridData[0].ToString("totalmatch_cu"),
					UserTotalMatch_CL = useridData[0].ToString("totalmatch_cl"),

					UserWinMatch_CT = useridData[0].ToString("winmatch_ct"),
					UserWinMatch_CU = useridData[0].ToString("winmatch_cu"),
					UserWinMatch_CL = useridData[0].ToString("winmatch_cl"),

					UserRoomPlaying = useridData[0].ToString("roomplaying"),
					UserIsPlaying = useridData[0].ToString("isplaying")
				};
				if (isFromServer)
					aPlayerInfo(infoData);
				//GET FISH INFO
				JSON[] allfishData = allData[1].ToArray<JSON>("Fishinfo");
				if (allfishData.Length != 0)
				{
					List<fishInfo> listFish = new List<fishInfo>();
					for (int i = 0; i < allfishData.Length; i++)
					{
						fishInfo tempFish = new fishInfo()
						{
							_fishid = allfishData[i].ToString("id"),
							_fishname = allfishData[i].ToString("name"),
							_fishvalue = allfishData[i].ToString("value"),
							_fishscale = allfishData[i].ToString("scale"),
							_fishmovespeed = allfishData[i].ToString("movespeed"),
							_fishrotatespeed = allfishData[i].ToString("rotatespeed"),
							_fishprobability = allfishData[i].ToString("probability"),
							_fishfrequency = allfishData[i].ToString("frequency"),
							_fishdepth = allfishData[i].ToString("depth")
						};
						listFish.Add(tempFish);
					}
					if (isFromServer)
						aFishInfo(listFish);
				}
				//GUN INFO
				JSON[] allGunData = allData[2].ToArray<JSON>("Guninfo");
				if (allGunData.Length != 0)
				{
					string[] cloneList = new string[allGunData.Length];
					int[] cloneListpercent = new int[allGunData.Length];
					for (int i = 0; i < allGunData.Length; i++)
					{
						cloneList[i] = allGunData[i].ToString("value");
						cloneListpercent[i] = allGunData[i].ToInt("gunpercent");
					}
					if (isFromServer)
						ListGun(cloneList, cloneListpercent);
				}

				//PROMOTION
				JSON[] promotionData = allData[3].ToArray<JSON>("Promotion");
				if (promotionData.Length != 0)
				{
					List<ListPromotion> listPromotion = new List<ListPromotion>();
					for (int i = 0; i < promotionData.Length; i++)
					{
						ListPromotion tempPromotion = new ListPromotion()
						{
							promotion_id = int.Parse(promotionData[i].ToString("id")),
							promotion_title = promotionData[i].ToString("title"),
							promotion_decription = promotionData[i].ToString("description"),
						};
						listPromotion.Add(tempPromotion);
					}
					if (isFromServer)
						aListPromotion(listPromotion);
				}

				//EVENT
				JSON[] eventData = allData[4].ToArray<JSON>("Event");
				if (eventData.Length != 0)
				{
					List<ListEvent> listEvent = new List<ListEvent>();
					Debug.Log("Event data : " + eventData.Length);
					for (int i = 0; i < eventData.Length; i++)
					{
						ListEvent tempEvent = new ListEvent()
						{
							event_id = int.Parse(eventData[i].ToString("id")),
							event_title = eventData[i].ToString("title"),
							event_decription = eventData[i].ToString("description"),
							event_url = eventData[i].ToString("link")
						};
						if (isFromServer)
							listEvent.Add(tempEvent);
					}
					aListEvent(listEvent);
				}
				//MISSION
				JSON[] missionData = allData[5].ToArray<JSON>("Mission");
				if (missionData.Length != 0)
				{
					List<ListMission> listMission = new List<ListMission>();
					for (int i = 0; i < missionData.Length; i++)
					{
						ListMission tempMission = new ListMission()
						{
							mission_id = int.Parse(missionData[i].ToString("id")),
							mission_title = missionData[i].ToString("title"),
							mission_decription = missionData[i].ToString("description"),
						};
						listMission.Add(tempMission);
					}
					if (isFromServer)
						aListMission(listMission);
				}
				// DAILY BONUS
				JSON[] dailybonusData = allData[6].ToArray<JSON>("Dailybonus");
				if (dailybonusData.Length != 0)
				{
					DailyBonus infoDaily = new DailyBonus()
					{
						d1 = dailybonusData[0].ToString("1"),
						d2 = dailybonusData[0].ToString("2"),
						d3 = dailybonusData[0].ToString("3"),
						d4 = dailybonusData[0].ToString("4"),
						d5 = dailybonusData[0].ToString("5"),
						d6 = dailybonusData[0].ToString("6"),
						d7 = dailybonusData[0].ToString("7"),
						recieved_day = int.Parse(dailybonusData[0].ToString("continuous_login_day")),
						showed = dailybonusData[0].ToBoolean("showdailybonus")
					};
					if (isFromServer)
						aDaily(infoDaily);
				}
				//LUCKY WHEEL "Luckywheel"
				JSON[] luckywheelData = allData[7].ToArray<JSON>("Luckywheel");
				QuaysoContent contentData = new QuaysoContent()
				{
					c1 = luckywheelData[0].ToInt("1"),
					c2 = luckywheelData[0].ToInt("2"),
					c3 = luckywheelData[0].ToInt("3"),
					c4 = luckywheelData[0].ToInt("4"),
					c5 = luckywheelData[0].ToInt("5"),
					c6 = luckywheelData[0].ToInt("6"),
					c7 = luckywheelData[0].ToInt("7"),
					c8 = luckywheelData[0].ToInt("8"),
					c9 = luckywheelData[0].ToInt("9"),
					c10 = luckywheelData[0].ToInt("10"),
					c11 = luckywheelData[0].ToInt("11"),
					c12 = luckywheelData[0].ToInt("12")
				};
				if (isFromServer)
					aQuayso(contentData);
				//ONLINE CONTINUE "Onlinecontinuous"
				JSON[] onlineContinueData = allData[8].ToArray<JSON>("Onlinecontinuous");
				OnlineTimeCount onlineData = new OnlineTimeCount()
				{
					_5p = onlineContinueData[0].ToInt("5 minutes"),
					_30p = onlineContinueData[0].ToInt("30 minutes"),
					_60p = onlineContinueData[0].ToInt("60 minutes"),
					_120p = onlineContinueData[0].ToInt("120 minutes"),
				};
				if (isFromServer)
					aOnlineTime(onlineData);
				// ADMIN INFO "Admininfo"
				JSON[] admininfoData = allData[9].ToArray<JSON>("Admininfo");
				if (admininfoData.Length != 0)
				{
					string A = admininfoData[0].serialized;
					//A = JsonUtility.ToJson(admininfoData[0]);
					//Debug.Log(A);
					AdminInfo clonenew = new AdminInfo();
					string C = JsonUtility.ToJson(clonenew);

					AdminInfo hehe = JsonUtility.FromJson<AdminInfo>(A);
					//MainMenu.instance.Test_lb.text += "_NEW_PARSE_" + hehe.showexchange;

					AdminInfoMNG tempAdminInfo = new AdminInfoMNG
					{
						info = admininfoData[0].ToString("info"),
						info2 = admininfoData[0].ToString("info2"),
						telco = admininfoData[0].ToString("telco"),

						showexchange = admininfoData[0].ToString("showexchange"),
						showaddcoin_iap = admininfoData[0].ToString("showaddcoin_iap"),
						showaddcoin_sms = admininfoData[0].ToString("showaddcoin_sms"),
						showaddcoin_card = admininfoData[0].ToString("showaddcoin_card"),
						showaddcoin_paypal = admininfoData[0].ToString("showaddcoin_paypal"),

						nbfish1player = admininfoData[0].ToInt("nbfish1player"),
						nbfish2player = admininfoData[0].ToInt("nbfish2player"),
						nbfish3player = admininfoData[0].ToInt("nbfish3player"),
						nbfish4player = admininfoData[0].ToInt("nbfish4player")
					};

					if (isFromServer)
						aAdminInfo(tempAdminInfo);
				}
				//DOI THUONG INFO
				JSON[] allRewardData = allData[10].ToArray<JSON>("Exchange");
				if (allRewardData.Length != 0)
				{
					List<infoDoiThe> listDoiThe = new List<infoDoiThe>();
					for (int i = 0; i < allRewardData.Length; i++)
					{
						infoDoiThe tempDoithe = new infoDoiThe()
						{
							_value = allRewardData[i].ToString("value"),
							_type = allRewardData[i].ToString("type"),
							_minuscoins = allRewardData[i].ToString("minuscoin"),
							_minuscash = allRewardData[i].ToString("minuscash"),
							_show = allRewardData[i].ToString("show")
						};
						listDoiThe.Add(tempDoithe);
					}
					if (isFromServer)
						aDoiThe(listDoiThe);
				}
				// INFO NAP
				JSON[] allNapData = allData[11].ToArray<JSON>("Cashratio");
				if (allNapData.Length != 0)
				{
					List<infoNap> listNap = new List<infoNap>();
					for (int i = 0; i < allNapData.Length; i++)
					{
						infoNap tempNap = new infoNap()
						{
							_type = allNapData[i].ToString("type"),
							_coinplus = allNapData[i].ToString("coinplus"),
							_cashplus = allNapData[i].ToString("cashplus"),
							_showvnd = allNapData[i].ToString("showvnd"),
							_showdolla = allNapData[i].ToString("showdolla")
						};
						listNap.Add(tempNap);
					}
					if (isFromServer)
						aNap(listNap);
				}
			}
			else
			{
				Debug.Log(itemJson.ToString("message"));
				//StartCoroutine (NGUIMessage.instance.CALLALERT(itemJson.ToString("message")));
				NGUIMessage.instance.CALLBIGMESSAGE("Lỗi kết nối !!", itemJson.ToString("message"), GameObject.Find("UI Root (3D)").gameObject, "QuitGame");
			}
		}

		public IEnumerator GetUserInfo(string _userID, Action<bool,UserData> aSuccess)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkGetUserInfo + _userID, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				JSON[] allData = itemJson.ToArray<JSON>("data");
				// USER ID
				JSON[] useridData = allData[0].ToArray<JSON>("User");
				//Check MD5
				UserData infoData = new UserData()
				{
					UserID = useridData[0].ToString("id"),
					UserName = useridData[0].ToString("username"),
					UserCoins = useridData[0].ToFloat("coin"),
					UserEXP = useridData[0].ToInt("exp"),
					UserSpinLeft = useridData[0].ToInt("nbspin"),
					UserCashs = useridData[0].ToInt("cash"),
					UserEmail = useridData[0].ToString("email"),
					UserPhone = useridData[0].ToInt("phone"),
					UserKey = useridData[0].ToString("security"),
					UserPerFish = useridData[0].ToFloat("per_fish"),
					UserPerBoss = useridData[0].ToFloat("per_boss"),

					UserElo_CT = useridData[0].ToString("elo_ct"),
					UserElo_CU = useridData[0].ToString("elo_cu"),
					UserElo_CL = useridData[0].ToString("elo_cl"),

					UserLevel_CT = useridData[0].ToString("level_ct"),
					UserLevel_CU = useridData[0].ToString("level_cu"),
					UserLevel_CL = useridData[0].ToString("level_cl"),

					UserTotalMatch_CT = useridData[0].ToString("totalmatch_ct"),
					UserTotalMatch_CU = useridData[0].ToString("totalmatch_cu"),
					UserTotalMatch_CL = useridData[0].ToString("totalmatch_cl"),

					UserWinMatch_CT = useridData[0].ToString("winmatch_ct"),
					UserWinMatch_CU = useridData[0].ToString("winmatch_cu"),
					UserWinMatch_CL = useridData[0].ToString("winmatch_cl"),

					UserRoomPlaying = useridData[0].ToString("roomplaying"),
					UserIsPlaying = useridData[0].ToString("isplaying")
				};
				//MD5 = coin + cash + nbspin + resucity + exp + Key
				string cloneKeyString = useridData[0].ToInt("coin") + useridData[0].ToInt("cash") + useridData[0].ToInt("nbspin") + useridData[0].ToString("security") + useridData[0].ToInt("exp") + MainMenu.PUBLICKEY;
				string finalMd5 = NGUIMessage.instance.XuLiMd5Key(cloneKeyString);
				//if (finalMd5 == itemJson.ToString("message"))
					aSuccess(true,infoData);
				//else
					//aSuccess(false,null);
			}
		}
		// CONG TIEN IAP
		public IEnumerator IAPMoneyPlus(string _userID, string _menhgia, string _oderID, string _secureKey, Action<bool, string, string> aSuccess)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(_oderID);
			string encodedText = System.Convert.ToBase64String(bytesToEncode);
			string cloneKey = NGUIMessage.instance.XuLiMd5Key(_userID + _menhgia + encodedText + _secureKey + MainMenu.PUBLICKEY);
			WWW www = new WWW(APILinked.linkCongtiepIAP + _userID + "/" + _menhgia + "/" + encodedText + "/" + cloneKey, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				JSON[] allData = itemJson.ToArray<JSON>("data");
				// USER ID
				JSON[] useridData = allData[0].ToArray<JSON>("User");
				string cloneMoney = useridData[0].ToString("coin");
				string newKey = useridData[0].ToString("security");

				string cloneKey1 = useridData[0].ToString("coin") + useridData[0].ToString("cash") + useridData[0].ToString("nbspin") + useridData[0].ToString("security") + useridData[0].ToString("exp") + "HDGAMES";
				string finalMD5 = NGUIMessage.instance.XuLiMd5Key(cloneKey1);

				if (finalMD5 == itemJson.ToString("message"))
					aSuccess(true, cloneMoney, newKey);
				else
					aSuccess(false, "0", "HACK DETECTED !!");
			}
		}

		public IEnumerator Paypal(string _userID, Action<bool, string> aSuccess)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkPaypal + _userID, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				aSuccess(true, itemJson.ToString("message"));
			}
		}
		public IEnumerator GetBoss(int bossID, Action<string, string, string, string> aSuccess)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			WWW www = new WWW(APILinked.linkgetBoss + bossID, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				JSON[] allData = itemJson.ToArray<JSON>("data");
				JSON[] dataBoss = allData[0].ToArray<JSON>("Boss");
				string cloneMoney = dataBoss[0].ToString("value");
				string cloneTimestep = dataBoss[0].ToString("bosstimestep");
				string cloneduration = dataBoss[0].ToString("bossduration");
				string cloneMaxValue = dataBoss[0].ToString("bossmaxvalue");

				aSuccess(cloneMoney, cloneTimestep, cloneduration, cloneMaxValue);
			}
		}
		public IEnumerator SaveDataBoss(string _idmaster, string _idboss, string _bossvalue, Action<bool, string> aSuccess)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			string cloneSecu = NGUIMessage.instance.XuLiMd5Key(_idmaster + _idboss + _bossvalue + UserConfig.Data.UserKey + "HDGAMES");
			WWW www = new WWW(APILinked.linksaveDataBoss + _idmaster + "/" + _idboss + "/" + _bossvalue + "/" + cloneSecu, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				JSON[] allData = itemJson.ToArray<JSON>("data");
				JSON[] useridData = allData[0].ToArray<JSON>("User");
				string cloneS = useridData[0].ToString("security");

				string cloneKey = useridData[0].ToString("coin") + useridData[0].ToString("cash") + useridData[0].ToString("nbspin") + useridData[0].ToString("security") + useridData[0].ToString("exp") + "HDGAMES";
				string finalMD5 = NGUIMessage.instance.XuLiMd5Key(cloneKey);

				if (itemJson.ToString("message") == finalMD5)
				{
					aSuccess(true, cloneS);//SAVE IF EXACT
				}
			}
		}
		public IEnumerator KillTheBoss(string _idmaster, string _userId, string _bossID, string _dataTime, Action<bool, string, string> aSuccess)
		{
			var form = new WWWForm();
			string hash = "abcxyz";
			hash = hash.ToLower();
			form.AddField("secret_key", hash);
			string cloneSecu = NGUIMessage.instance.XuLiMd5Key(_idmaster + _userId + _bossID + _dataTime + UserConfig.Data.UserKey + "HDGAMES");
			WWW www = new WWW(APILinked.linkKilledboss + _idmaster + "/" + _userId + "/" + _bossID + "/" + _dataTime + "/" + cloneSecu, form);
			Debug.Log("link request = " + www.url);
			yield return www;
			Debug.Log("www.text : " + www.text);
			JSON itemJson = new JSON();
			itemJson.serialized = www.text;
			if (itemJson.ToBoolean("error") == false)
			{
				JSON[] allData = itemJson.ToArray<JSON>("data");
				JSON[] useridData = allData[0].ToArray<JSON>("User");
				JSON[] KilledData = allData[0].ToArray<JSON>("Killedplayer");
				string cloneS = useridData[0].ToString("security");
				string cloneN = KilledData[0].ToString("username");

				string cloneKey = useridData[0].ToString("coin") + useridData[0].ToString("cash") + useridData[0].ToString("nbspin") + useridData[0].ToString("security") + useridData[0].ToString("exp") + "HDGAMES";
				string finalMD5 = NGUIMessage.instance.XuLiMd5Key(cloneKey);

				if (itemJson.ToString("message") == finalMD5)
				{
					aSuccess(true, cloneN, cloneS);
				}
			}
		}
	}
}
