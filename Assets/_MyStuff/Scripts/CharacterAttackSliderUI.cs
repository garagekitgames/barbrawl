using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace garagekitgames
{
    public class CharacterAttackSliderUI : MonoBehaviour
    {

        //public Canvas playerCanvas;
       // public BodyPart playerRootPart;
        public CharacterThinker character;

        public Slider m_AimSlider;
        public Image m_FillImage;                           // The image component of the slider.
        public Color m_FullHealthColor = Color.red;       // The color the health bar will be when on full health.
        public Color m_ZeroHealthColor = Color.green;         // The color the health bar will be when on no health.
        // Use this for initialization
        void Start()
        {

            character = transform.GetComponent<CharacterThinker>();

            if(!m_AimSlider)
            {
                if (GameObject.FindGameObjectWithTag("AttackSlider") && character.amIMainPlayer)
                {
                    m_AimSlider = GameObject.FindGameObjectWithTag("AttackSlider").GetComponent<Slider>();
                }
            }
            m_FullHealthColor = Color.white;
            //m_FillImage = transform.GetComponentInChildren<Canvas>();
            //playerCanvas = transform.GetComponentInChildren<Canvas>();


        }

        // Update is called once per frame
        void Update()
        {
            if (!m_AimSlider)
                return;
            if (character.windUp)
            {
                AttackData currentAttack = character.currentAttack;
                float attackButtonClickTimer = character.attackButtonClickTimer;
                float attackPower = Mathf.Clamp(currentAttack.startAttackPower * (1 + attackButtonClickTimer * currentAttack.attackPowerIncreaseRate), currentAttack.minAttackPower, currentAttack.maxAttackPower);

                //float attackPower = character.Remember<float>("attackPower");
                float maxAttackPower = currentAttack.maxAttackPower;
                m_AimSlider.value = attackPower / maxAttackPower * 100;
                m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, attackPower / maxAttackPower);
            }
            else
            {
                m_AimSlider.value = 0;
                m_FillImage.color = m_ZeroHealthColor;
            }


            
        }
    }

}

