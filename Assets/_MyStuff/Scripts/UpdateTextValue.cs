using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace garagekitgames
{
    public class UpdateTextValue : MonoBehaviour
    {

        public IntVariable inputValue;
        public TextMeshProUGUI outputText;

        private int previousValue;

        public UnityEvent OnValueChange;
        public UnityEvent OnValueIncrease;
        public UnityEvent OnValueDecrease;
        // Use this for initialization

        void Awake()
        {

            outputText = this.gameObject.GetComponent<TextMeshProUGUI>();
            outputText.text = inputValue.value.ToString();

        }

        // Update is called once per frame
        void Update()
        {
            UpdateText();
            outputText.text = inputValue.value.ToString();
        }

        public void UpdateText()
        {
            if(inputValue.value != previousValue)
            {
                OnValueChange.Invoke();
                if (inputValue.value > previousValue)
                {
                    OnValueIncrease.Invoke();
                    outputText.text = inputValue.value.ToString();
                    previousValue = inputValue.value;
                }
                else if (inputValue.value < previousValue)
                {
                    OnValueDecrease.Invoke();
                    outputText.text = inputValue.value.ToString();
                    previousValue = inputValue.value;
                }
            }
            
            
        }

    }
}

