using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BANCA
{
	public class APILinked : MonoBehaviour
	{
		//public static string Linksv = "http://dautruongcotuong.com/service";
		//public static string Linksv = "https://gameco.online/service";
//#if UNITY_WEBGL
		public static string Linksv = "https://gameco.online/service";
//#endif
		//public static string Linksv = "http://dev.adventuremobilegames.com/service";

		public static string GetTimesTamp()
		{
			long ticks = DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
			ticks /= 10000000;
			return ticks.ToString();
		}
		public static string linkPostScore
		{
			get
			{
				return Linksv + "/savedata/";
			}
		}
		public static string linkSendLanguage
		{
			get
			{
				return Linksv + "/changelanguage/";
			}
		}

		public static string linkLeaderBoard
		{
			get
			{
				return Linksv + "/leaderboard/";
			}
		}
		public static string linkMailbox
		{
			get
			{
				return Linksv + "/allmailbox/";
			}
		}
		public static string linkOpenMail
		{
			get
			{
				return Linksv + "/mailboxrecieved/";
			}
		}
		public static string linkDeleteMail
		{
			get
			{
				return Linksv + "/deletemail/";
			}
		}
		public static string linkNhapGiftcode
		{
			get
			{
				return Linksv + "/usegiftcode/";
			}
		}
		public static string linkUserrate
		{
			get
			{
				return Linksv + "/userrate/";
			}
		}
		public static string linkGetAllData
		{
			get
			{
				return Linksv + "/getdata/";
			}
		}
		public static string linkOnlineReward
		{
			get
			{
				return Linksv + "/onlinereward/";
			}
		}
		public static string linkSpin
		{
			get
			{
				return Linksv + "/spin/";
			}
		}
		public static string linkMuaSpin
		{
			get
			{
				return Linksv + "/buynbspin/";
			}
		}
		public static string linkRename
		{
			get
			{
				return Linksv + "/changename/";
			}
		}
		public static string linkNapTheCao
		{
			get
			{
				return Linksv + "/cardmoney/";
			}
		}
		public static string linkSaveData
		{
			get
			{
				return Linksv + "/savedata/";
			}
		}
		public static string linkDoiThuong
		{
			get
			{
				return Linksv + "/exchange/";
			}
		}
		public static string linkUpdateinfo
		{
			get
			{
				return Linksv + "/updateuserinfo/";
			}
		}
		public static string linkGetUserInfo
		{
			get
			{
				return Linksv + "/getuserinfo/";
			}
		}
		public static string linkCongtiepIAP
		{
			get
			{
				return Linksv + "/congtieniap/";
			}
		}
		public static string linkPaypal
		{
			get
			{
				return Linksv + "/paypal/";
			}
		}
		public static string linkgetBoss
		{
			get
			{
				return Linksv + "/getboss/";
			}
		}
		public static string linksaveDataBoss
		{
			get
			{
				return Linksv + "/savedataboss/";
			}
		}
		public static string linkKilledboss
		{
			get
			{
				return Linksv + "/deadandresetboss/";
			}
		}
		public static string linkDangKi
        {
            get
            {
				return Linksv + "/DangKiID/";
            }
        }
		public static string linkLogin
        {
            get
            {
				return Linksv + "/LoginGame/";
            }
        }
		public static string linkChangePassword
        {
            get
            {
				return Linksv + "/ChangePassword/";
            }
        }
		public static string linkForgetPassword
		{
			get
			{
				return Linksv + "/ForgetPassword/";
			}
		}
	}
}
