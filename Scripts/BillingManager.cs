using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;


public class BillingManager : MonoBehaviour//, IStoreListener
{
	/*
	public static BillingManager instance;
	private static IStoreController m_StoreController;          // The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

	//Create string[] 5 product
	public static string PRODUCT_1 = "gameco.product.1";   
	public static string PRODUCT_2 = "gameco.product.2";
	public static string PRODUCT_5 = "gameco.product.5"; 
	public static string PRODUCT_10 = "gameco.product.10"; 
	public static string PRODUCT_25 = "gameco.product.25"; 
	 //Apple App Store-specific product identifier for the subscription product.
	private static string kProductNameAppleSubscription =  "com.unity3d.subscription.new";

	// Google Play Store-specific product identifier subscription product.
	private static string kProductNameGooglePlaySubscription =  "com.unity3d.subscription.original"; 

	void Awake(){
		instance = this;
	}

	void Start()
	{
		if (m_StoreController == null)
		{
			InitializePurchasing();
			DontDestroyOnLoad (this.gameObject);
		}
	}

	public void InitializePurchasing() 
	{
		if (IsInitialized())
		{
			return;
		}

		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
		#if UNITY_ANDROID
		builder.AddProduct(PRODUCT_1, ProductType.Consumable);
		builder.AddProduct(PRODUCT_2, ProductType.Consumable);
		builder.AddProduct(PRODUCT_5, ProductType.Consumable);
		builder.AddProduct(PRODUCT_10, ProductType.Consumable);
		builder.AddProduct(PRODUCT_25, ProductType.Consumable);
		#endif

		#if UNITY_IOS
		builder.AddProduct(PRODUCT_1 + ".ios", ProductType.Consumable);
		builder.AddProduct(PRODUCT_2 + ".ios", ProductType.Consumable);
		builder.AddProduct(PRODUCT_5 + ".ios", ProductType.Consumable);
		builder.AddProduct(PRODUCT_10 + ".ios", ProductType.Consumable);
		builder.AddProduct(PRODUCT_25 + ".ios", ProductType.Consumable);
		#endif

		//builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
		UnityPurchasing.Initialize(this, builder);
		Debug.Log ("INIT PURCHASE");
	}


	private bool IsInitialized()
	{
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	public void BuyP1(){
		BuyProductID (PRODUCT_1);
	}
	public void BuyP2(){
		BuyProductID (PRODUCT_2);
	}
	public void BuyP3(){
		BuyProductID (PRODUCT_5);
	}
	public void BuyP4(){
		BuyProductID (PRODUCT_10);
	}
	public void BuyP5(){
		BuyProductID (PRODUCT_25);
	}


	private void BuyProductID(string productId)
	{

		if (IsInitialized())
		{

			Product product = m_StoreController.products.WithID(productId);

			if (product != null && product.availableToPurchase)
			{
				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				m_StoreController.InitiatePurchase(product);
			}
			else
			{
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		else
		{
			Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}
		

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;
	}


	public void OnInitializeFailed(InitializationFailureReason error)
	{
		// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}


	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
	{
		// A consumable product has been purchased by this user.
		if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_1, StringComparison.Ordinal))
		{
			Debug.Log ("MUA 1$");
			StartCoroutine (BANCA.ReqAPI.Instance.IAPMoneyPlus(UserConfig.Data.UserID,"1",args.purchasedProduct.transactionID,UserConfig.Data.UserKey,(aBool,aString1,aString2) =>{
				if(aBool){
					UserConfig.Data.UserCoins = int.Parse(aString1);
					UserConfig.Data.UserKey = aString2;
					MainMenu.instance.ShowCoins();
				}
			}));
		}
		// Or ... a non-consumable product has been purchased by this user.
		else if (String.Equals(args.purchasedProduct.definition.id,PRODUCT_2, StringComparison.Ordinal))
		{
			Debug.Log ("MUA 2$");
			StartCoroutine (BANCA.ReqAPI.Instance.IAPMoneyPlus(UserConfig.Data.UserID, "2",args.purchasedProduct.transactionID, UserConfig.Data.UserKey, (aBool,aString1,aString2) =>{
				if(aBool){
					UserConfig.Data.UserCoins = int.Parse(aString1);
					UserConfig.Data.UserKey = aString2;
					MainMenu.instance.ShowCoins();
				}
			}));
		}
		// Or ... a subscription product has been purchased by this user.
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_5, StringComparison.Ordinal))
		{
			Debug.Log ("MUA 5$");
			StartCoroutine (BANCA.ReqAPI.Instance.IAPMoneyPlus(UserConfig.Data.UserID, "5",args.purchasedProduct.transactionID, UserConfig.Data.UserKey, (aBool,aString1,aString2) =>{
				if(aBool){
					UserConfig.Data.UserCoins = int.Parse(aString1);
					UserConfig.Data.UserKey = aString2;
					MainMenu.instance.ShowCoins();
				}
			}));
		}
		// Or ... an unknown product has been purchased by this user. Fill in additional products here....
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_10, StringComparison.Ordinal))
		{
			Debug.Log ("MUA 10$");
			StartCoroutine (BANCA.ReqAPI.Instance.IAPMoneyPlus(UserConfig.Data.UserID, "10",args.purchasedProduct.transactionID, UserConfig.Data.UserKey, (aBool,aString1,aString2) =>{
				if(aBool){
					UserConfig.Data.UserCoins = int.Parse(aString1);
					UserConfig.Data.UserKey = aString2;
					MainMenu.instance.ShowCoins();
				}
			}));
		}
		// Or ... an unknown product has been purchased by this user. Fill in additional products here....
		else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_25, StringComparison.Ordinal))
		{
			Debug.Log ("MUA 25$");
			StartCoroutine (BANCA.ReqAPI.Instance.IAPMoneyPlus(UserConfig.Data.UserID, "25",args.purchasedProduct.transactionID, UserConfig.Data.UserKey, (aBool,aString1,aString2) =>{
				if(aBool){
					UserConfig.Data.UserCoins = int.Parse(aString1);
					UserConfig.Data.UserKey = aString2;
					MainMenu.instance.ShowCoins();
				}
			}));
		}
		else 
		{
			Debug.Log ("Mua That Bai");
		}
		return PurchaseProcessingResult.Complete;
	}


	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		string A = string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason);
		StartCoroutine (NGUIMessage.instance.CALLALERT (A));
	}
	*/
}