using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnityAdsChecker : MonoBehaviour
{
    public TextMeshProUGUI outputText;
    public Color textColor;
    public Color initColor;

    // Start is called before the first frame update
    void Start()
    {
        outputText = this.gameObject.GetComponent<TextMeshProUGUI>();
        initColor = outputText.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(UnityAdsManager.Instance.isRewardedAdReady)
        {
            outputText.color = textColor;

        }
        else
        {
            outputText.color = initColor;
        }
    }
}
