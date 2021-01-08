using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using UnityEngine.UI;
using TMPro;
using System;
using Doozy.Engine.Progress;

namespace garagekitgames
{
    public class UpdateSliderValuePercentage : MonoBehaviour
    {

        public IntVariable currentValue;
        public IntVariable maxValue;

        public IntVariable Percentage;

        public Slider slider;
        public Progressor progress;

        // Start is called before the first frame update
        void Awake()
        {
            slider = this.gameObject.GetComponent<Slider>();
            if(slider)
            {
                slider.value = ((float)currentValue.value / (float)maxValue.value) * 100.0f;
            }
            
            Percentage.value = Mathf.RoundToInt(((float)currentValue.value / (float)maxValue.value) * 100.0f);
        }
        private void Start()
        {
            
                slider = this.gameObject.GetComponent<Slider>();
            if (slider)
            {
                slider.value = ((float)currentValue.value / (float)maxValue.value) * 100.0f;
            }
                
                Percentage.value = Mathf.RoundToInt(((float)currentValue.value / (float)maxValue.value) * 100.0f);
            
        }

        // Update is called once per frame
        void Update()
        {
            UpdateSliderInt();

        }

        public void UpdateSliderInt()
        {
            Percentage.value = Mathf.RoundToInt(((float)currentValue.value / (float)maxValue.value) * 100.0f);
            if(progress)
            {
                progress.SetProgress((float)currentValue.value / (float)maxValue.value);
            }
            
            if (!slider)
                return;

            slider.value = Mathf.RoundToInt(((float)currentValue.value / (float)maxValue.value) * 100.0f);

            
        }
    }
}

