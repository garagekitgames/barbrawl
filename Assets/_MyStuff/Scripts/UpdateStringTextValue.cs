using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using UnityEngine.UI;
using TMPro;

namespace garagekitgames
{
    public class UpdateStringTextValue : MonoBehaviour
    {
        public StringVariable inputValue;
        public TextMeshProUGUI outputText;

        private void Awake()
        {
            outputText = this.gameObject.GetComponent<TextMeshProUGUI>();
        }
        // Use this for initialization
        void Start()
        {

        }


        // Update is called once per frame
        void Update()
        {
            UpdateText();
        }

        public void UpdateText()
        {
            //Color32[] colors = outputText.textInfo.meshInfo[0].colors32;

            //colors[0] = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
            outputText.text = inputValue.value;
           // outputText.mesh.colors32 = colors;
        }
    }
}

