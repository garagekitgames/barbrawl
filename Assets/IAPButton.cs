using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SO;
using garagekitgames;
//using EasyMobile;

public class IAPButton : MonoBehaviour
{
    public enum ItemType
    {
        Coins_1000,
        Coins_2500,
        Coins_4500,
        Coins_9000,
        RemoveAds
    }

    public ItemType itemType = ItemType.Coins_1000;

    public Text priceText;

    public string defaultText;
    //public 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadPrice());
    }

    // Update is called once per frame
    void Update()
    {
        switch (itemType)
        {
            case ItemType.Coins_1000:
                //IAPManager.Instance.Purchase_1000_Coins();
                break;
            case ItemType.Coins_2500:
                //IAPManager.Instance.Purchase_2500_Coins();
                break;
            case ItemType.Coins_4500:
                // IAPManager.Instance.Purchase_4500_Coins();
                break;
            case ItemType.Coins_9000:
                // IAPManager.Instance.Purchase_9000_Coins();
                break;
            case ItemType.RemoveAds:
                // IAPManager.Instance.Purchase_Remove_Ads();
                if (IAPManager.Instance.removeAds.value)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    this.gameObject.SetActive(true);
                }
                break;
        }
    }

    public void ClickBuy()
    {
        switch (itemType)
        {
            case ItemType.Coins_1000:
                IAPManager.Instance.Purchase_1000_Coins();
                break;
            case ItemType.Coins_2500:
                IAPManager.Instance.Purchase_2500_Coins();
                break;
            case ItemType.Coins_4500:
                IAPManager.Instance.Purchase_4500_Coins();
                break;
            case ItemType.Coins_9000:
                IAPManager.Instance.Purchase_9000_Coins();
                break;
            case ItemType.RemoveAds:
                IAPManager.Instance.Purchase_Remove_Ads();
                if (IAPManager.Instance.removeAds.value)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    this.gameObject.SetActive(true);
                }
                break;
        }
    }

    public IEnumerator LoadPrice()
    {
        while (!IAPManager.Instance.IsInitialized())
        {
            yield return null;
        }

        string loadedPrice = "";

        switch (itemType)
        {
            case ItemType.Coins_1000:
                loadedPrice = IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.COINS_1000);
                break;
            case ItemType.Coins_2500:
                loadedPrice = IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.COINS_2500);
                break;
            case ItemType.Coins_4500:
                loadedPrice = IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.COINS_4500);
                break;
            case ItemType.Coins_9000:
                loadedPrice = IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.COINS_9000);
                break;
            case ItemType.RemoveAds:
                loadedPrice = IAPManager.Instance.GetProductPriceFromStore(IAPManager.Instance.NO_ADS);
                if (IAPManager.Instance.removeAds.value)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    this.gameObject.SetActive(true);
                }
                break;
        }

        priceText.text = loadedPrice;
    }
}
