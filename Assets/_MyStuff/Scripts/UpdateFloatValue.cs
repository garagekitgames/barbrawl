using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using UnityEngine.UI;

namespace garagekitgames
{
    public class UpdateFloatValue : MonoBehaviour
    {
        public FloatVariable inputValue;
        public Text outputText;

        // Use this for initialization
        void Start()
        {
            outputText = this.gameObject.GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            outputText.text = inputValue.value.ToString("F2");

        }
    }
}