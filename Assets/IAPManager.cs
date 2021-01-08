using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using EasyMobile;
using SO;
using garagekitgames;
using UnityEngine.Purchasing;
using System;

public class IAPManager : UnitySingletonPersistent<IAPManager>, IStoreListener
{

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    // Product identifiers for all products capable of being purchased: 
    // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
    // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
    // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

    // General product identifiers for the consumable, non-consumable, and subscription products.
    // Use these handles in the code to reference which product to purchase. Also use these values 
    // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
    // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
    // specific mapping to Unity Purchasing's AddProduct, below.
    public  string COINS_1000 = "coins_1000";
    public  string COINS_2500 = "coins_2500";

    public  string COINS_4500 = "coins_4500";

    public  string COINS_9000 = "coins_9000";

    public  string NO_ADS = "no_ads";

    
    public override void Awake()
    {
        base.Awake();
        //if (!RuntimeManager.IsInitialized())
        //    RuntimeManager.Init();

        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }


    }

    public bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Add a product to sell / restore by way of its identifier, associating the general identifier
        // with its store-specific identifiers.
        builder.AddProduct(COINS_1000, ProductType.Consumable);
        builder.AddProduct(COINS_2500, ProductType.Consumable);
        builder.AddProduct(COINS_4500, ProductType.Consumable);
        builder.AddProduct(COINS_9000, ProductType.Consumable);
        // Continue adding the non-consumable product.
        builder.AddProduct(NO_ADS, ProductType.NonConsumable);
        
        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }


    public IntVariable cashCollected;
    public BoolVariable removeAds;

    public bool isIAPInitialized;

    // Start is called before the first frame update
    void Start()
    {
        // Check if Unity IAP has been initialized
        //isIAPInitialized = InAppPurchasing.IsInitialized();
    }

    // Update is called once per frame
    void Update()
    {
        //isIAPInitialized = InAppPurchasing.IsInitialized();
    }

    // Subscribe to IAP purchase events
    void OnEnable()
    {
        //InAppPurchasing.PurchaseCompleted += PurchaseCompletedHandler;
        //InAppPurchasing.PurchaseFailed += PurchaseFailedHandler;
    }

    // Unsubscribe when the game object is disabled
    void OnDisable()
    {
        //InAppPurchasing.PurchaseCompleted -= PurchaseCompletedHandler;
        //InAppPurchasing.PurchaseFailed -= PurchaseFailedHandler;
    }

    //public void BuyConsumable()
    //{
    //    // Buy the consumable product using its general identifier. Expect a response either 
    //    // through ProcessPurchase or OnPurchaseFailed asynchronously.
    //    BuyProductID(kProductIDConsumable);
    //}


    //public void BuyNonConsumable()
    //{
    //    // Buy the non-consumable product using its general identifier. Expect a response either 
    //    // through ProcessPurchase or OnPurchaseFailed asynchronously.
    //    BuyProductID(kProductIDNonConsumable);
    //}

    // Purchase the sample product
    public void Purchase_1000_Coins()
    {
        // EM_IAPConstants.Sample_Product is the generated name constant of a product named "Sample Product"
        //InAppPurchasing.Purchase(EM_IAPConstants.Product_1000_Coins);
        BuyProductID(COINS_1000);
    }

    public void Purchase_2500_Coins()
    {
        // EM_IAPConstants.Sample_Product is the generated name constant of a product named "Sample Product"
        //InAppPurchasing.Purchase(EM_IAPConstants.Product_2500_Coins);
        BuyProductID(COINS_2500);
    }

    public void Purchase_4500_Coins()
    {
        // EM_IAPConstants.Sample_Product is the generated name constant of a product named "Sample Product"
        //InAppPurchasing.Purchase(EM_IAPConstants.Product_4500_Coins);
        BuyProductID(COINS_4500);
    }

    public void Purchase_9000_Coins()
    {
        // EM_IAPConstants.Sample_Product is the generated name constant of a product named "Sample Product"
        //InAppPurchasing.Purchase(EM_IAPConstants.Product_9000_Coins);
        BuyProductID(COINS_9000);
    }

    public void Purchase_Remove_Ads()
    {
        // EM_IAPConstants.Sample_Product is the generated name constant of a product named "Sample Product"
        //InAppPurchasing.Purchase(EM_IAPConstants.Product_Remove_Ads);
        BuyProductID(NO_ADS);
    }

    void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) => {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void AddCoins(int value)
    {
        cashCollected.Add(value);
        PersistableSO.Instance.Save();
        //cashCollectedThisRound.Add(value);
    }

    public void RemoveAds(bool value)
    {
        removeAds.value = value;
        PersistableSO.Instance.Save();
        //Advertising.RemoveAds();
        //cashCollectedThisRound.Add(value);
    }

    public string GetProductPriceFromStore(string id)
    {

        if(m_StoreController != null && m_StoreController.products != null)
        {
            return m_StoreController.products.WithID(id).metadata.localizedPriceString;
        }
        else
        {
            return "";
        }
        //ProductMetadata data = InAppPurchasing.GetProductLocalizedData(productName);

        //if (data != null)
        //{
        //    Debug.Log("Localized title: " + data.localizedTitle);
        //    Debug.Log("Localized description: " + data.localizedDescription);
        //    Debug.Log("Localized price string: " + data.localizedPriceString);
        //    return data.localizedPriceString;
        //}
        //else
        //{
        //    return "";
        //}


    }
    // Successful purchase handler
    //void PurchaseCompletedHandler(IAPProduct product)
    //{
    //    // Compare product name to the generated name constants to determine which product was bought
    //    switch (product.Name)
    //    {
    //        case EM_IAPConstants.Product_1000_Coins:
    //            Debug.Log(product.Name + "was purchased. The user should be granted it now.");
    //            //NativeUI.Alert("Purchase Success!", "The product " + product.Name + " has been purchased successfully!.");
    //            GameManager.Instance.purchaseEvent(product.Name, GetProductPriceFromStore(product.Name), "1");
    //            AddCoins(1000);
    //            break;
    //        case EM_IAPConstants.Product_2500_Coins:
    //            //NativeUI.Alert("Purchase Success!", "The product " + product.Name + " has been purchased successfully!.");
    //            GameManager.Instance.purchaseEvent(product.Name, GetProductPriceFromStore(product.Name), "1");
    //            AddCoins(2500);
    //            break;
    //        case EM_IAPConstants.Product_4500_Coins:
    //           // NativeUI.Alert("Purchase Success!", "The product " + product.Name + " has been purchased successfully!.");
    //            GameManager.Instance.purchaseEvent(product.Name, GetProductPriceFromStore(product.Name), "1");
    //            AddCoins(4500);
    //            break;
    //        case EM_IAPConstants.Product_9000_Coins:
    //            //NativeUI.Alert("Purchase Success!", "The product " + product.Name + " has been purchased successfully!.");
    //            GameManager.Instance.purchaseEvent(product.Name, GetProductPriceFromStore(product.Name), "1");
    //            AddCoins(9000);
    //            break;
    //        case EM_IAPConstants.Product_Remove_Ads:
    //            //NativeUI.Alert("Purchase Success!", "The product " + product.Name + " has been purchased successfully!.");
    //            GameManager.Instance.purchaseEvent(product.Name, GetProductPriceFromStore(product.Name), "1");
    //            //AddCoins(2500);
    //            RemoveAds(true);
    //            break;
    //            // More products here...
    //    }
    //}

    // Failed purchase handler
    //void PurchaseFailedHandler(IAPProduct product)
    //{
    //    Debug.Log("The purchase of product " + product.Name + " has failed.");
    //    //NativeUI.Alert("Purchase Failed!", "The purchase of product " + product.Name + " has failed.");
    //}

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // A consumable product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, COINS_1000, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            //NativeUI.Alert("Purchase Success!", "The product " + product.Name + " has been purchased successfully!.");
            //GameManager.Instance.purchaseEvent(product.Name, GetProductPriceFromStore(product.Name), "1");
            AddCoins(1000);

        }
        else if (String.Equals(args.purchasedProduct.definition.id, COINS_2500, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            //NativeUI.Alert("Purchase Success!", "The product " + product.Name + " has been purchased successfully!.");
            //GameManager.Instance.purchaseEvent(product.Name, GetProductPriceFromStore(product.Name), "1");
            AddCoins(2500);

        }
        else if (String.Equals(args.purchasedProduct.definition.id, COINS_4500, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            //NativeUI.Alert("Purchase Success!", "The product " + product.Name + " has been purchased successfully!.");
            //GameManager.Instance.purchaseEvent(product.Name, GetProductPriceFromStore(product.Name), "1");
            AddCoins(4500);

        }
        else if (String.Equals(args.purchasedProduct.definition.id, COINS_9000, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            //NativeUI.Alert("Purchase Success!", "The product " + product.Name + " has been purchased successfully!.");
            //GameManager.Instance.purchaseEvent(product.Name, GetProductPriceFromStore(product.Name), "1");
            AddCoins(9000);

        }
        // Or ... a non-consumable product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, NO_ADS, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            //NativeUI.Alert("Purchase Success!", "The product " + product.Name + " has been purchased successfully!.");
            //GameManager.Instance.purchaseEvent(product.Name, GetProductPriceFromStore(NO_ADS), "1");
            //AddCoins(2500);
            RemoveAds(true);
                
        }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
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
}
