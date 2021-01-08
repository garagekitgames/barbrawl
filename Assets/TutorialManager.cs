using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using UnityEngine.Events;
using System;
using System.Linq;
using garagekitgames;
using TMPro;

public class TutorialManager : MonoBehaviour
{

    public enum FSMState
    {
        None,
        ShowTutorial_1,
        ShowTutorial_2,
        ShowTutorialContinueMessage,
        TutorialSuccess,
        DoNothing
    }

    public FSMState curState;
    public LevelData levelData;
    public GameObject Player;
    
    public CharacterRuntimeSet mainPlayerCharacterSet;
    public CharacterThinker playerCharacter;

    public BoolVariable showTutorialOnStart;
    //public BoolVariable tutorialSuccess_1;
   // public BoolVariable tutorialSuccess_2;
    public List<GameObject> tutorialTarget;
    public bool showingTutorial1 = false;
    //public bool showingTutorial2 = false;
    public GameObject currentTutorialObject;
    public UnityEvent DestroyTutorial;
    public UnityEvent OnTutorialSuccess;
    public UnityEvent OnShowTutorialStatus;
    public UnityEvent OnHideTutorialStatus;
    public UnityEvent OnShowTutorialContinueMessage;
    public UnityEvent OnHideTutorialContinueMessage;
    public UnityEvent OnShowSkipTutorial;
    public UnityEvent OnHideSkipTutorial;

    public UnityEvent OnHighlightInfo;
    public UnityEvent OnDeHighlightInfo;
    //private int selectedIndex = 0;
    public StringVariable tutorialStatus;
    public bool notInvoked = true;

    public float tutorialInactiveMessagetime = 2;
    public float tutorialInactiveMessagetimer;

    public float attackButtonClickTimer;

    public bool tutorialContinueMessageButtonClicked = false;

    public bool showedTutorialContinueDialogue = false;

    public Dictionary<int,string> randomTips;

    public bool windUp;
    public bool attack;
    public bool skipTutorial;


    public int selectedIndex;

    public List<GameObject> levelCharacters = new List<GameObject>();

    public TextMeshProUGUI textContent;
    public TextMeshProUGUI tutorialNumber;

    // Start is called before the first frame update
    void Start()
    {
        randomTips = new Dictionary<int, string>();
        SetupRandomTips();

        tutorialTarget = new List<GameObject>(GameObject.FindGameObjectsWithTag("tutorialTarget"));
        Player = levelData.Player;
        foreach (var character in mainPlayerCharacterSet.Items)
        {
            playerCharacter = character;
            //enemy.SetStopDoingShit(value);
            // var AI = enemy.GetComponent<CharacterBasicAI>();
            // AI.SetStopDoingShit(value);
        }
    }

    private void SetupRandomTips()
    {
        string[] tips = {
            "Attack When Enemy is within range !",
            "Tap and Hold to aim, and release to attack !",
            "Enemies that glow red, are about to attack you !",
            "Hold the tap longer for an accurate attack !",
            "Hold the tap longer for a stronger attack !",
            "You can turn the tutorial back on from options !",
            "If you miss two attack in a row, you will break your combo !",
            "Try to get a high Combo going !",
            "Higher Combo = Higher score !",
            "Stronger Attacks would make the enemies drop coins !",
            "The Arrow shows the direction of your attack !"
        };

        for (int i = 0; i < tips.Length; i++)
        {
            randomTips[i] = tips[i];
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (playerCharacter.input.PressLeftPunch())
        {
            attackButtonClickTimer = 0f;

            windUp = false;


        }

        else if (playerCharacter.input.HoldLeftPunch())
        {
            attackButtonClickTimer += Time.deltaTime;

            if (windUp == false && !attack)    // if the button is pressed for more than 0.2 seconds grab
            {
                windUp = true;
                //character.Remember("windUp", windUp);
                
            }

        }

        else if (playerCharacter.input.ReleaseLeftPunch())
        {

            if (!attack) // as long as key is held down increase time, this records how long the key is held down
            {
                //float attackPower = Mathf.Clamp(currentAttack.startAttackPower * (1 + attackButtonClickTimer * currentAttack.attackPowerIncreaseRate), currentAttack.minAttackPower, currentAttack.maxAttackPower);
                

                attack = true;
                attackButtonClickTimer = 0f;

               
            }

            windUp = false;

           




        }

        switch (curState)
        {
            case FSMState.None:
                Base();
                break;
            case FSMState.ShowTutorial_1:
                ShowTutorial_1();
                break;
            case FSMState.ShowTutorial_2:
                ShowTutorial_2();
                break;
            case FSMState.ShowTutorialContinueMessage:
                ShowTutorialContinueMessage();
                break;
            case FSMState.TutorialSuccess:
                TutorialSuccessState();
                break;
            case FSMState.DoNothing:
                OnDoNothing();
                break;
        }
    }

    private void OnDoNothing()
    {
        //OnTutorialSuccess.Invoke();
    }

    private void TutorialSuccessState()
    {
        OnHideSkipTutorial.Invoke();
        OnTutorialSuccess.Invoke();
        curState = FSMState.DoNothing;
    }

    private void ShowTutorialContinueMessage()
    {
        DestroyTutorial.Invoke();
        //Destroy Tutorial 1
        OnHideTutorialStatus.Invoke();

        OnHighlightInfo.Invoke();
        //Display the message to show tutorial next try ?, if yes set show tutorial on start to true else set it to false, also add it to save
        if (!showedTutorialContinueDialogue)
        {
            OnShowTutorialContinueMessage.Invoke();
            showedTutorialContinueDialogue = true;
        }

        OnHideSkipTutorial.Invoke();

        if (tutorialContinueMessageButtonClicked)
        {
            OnDeHighlightInfo.Invoke();
            curState = FSMState.TutorialSuccess;
        }
       
        //throw new NotImplementedException();
    }

    private void Base()
    {
        if (showTutorialOnStart.value)
        {
            currentTutorialObject = tutorialTarget.Where(obj => obj.GetComponent<OnJointBreakScript>().jointBroken == false).FirstOrDefault();
            //EffectsController.Instance.CreateFloatingTextEffectBasic(tutorialTarget[0].transform.position, "Tap on Target to attack", Color.white, tutorialTarget[0].transform, (Vector3.up * 2.5f), 0.5f);
            if(currentTutorialObject)
            {
                curState = FSMState.ShowTutorial_1;
            }
            OnShowSkipTutorial.Invoke();


        }
        else
        {
            StartCoroutine(showRandomTip());
            curState = FSMState.TutorialSuccess;
        }

    }

    private void ShowTutorial_1()
    {
        if(skipTutorial)
        {
            StartCoroutine(showRandomTip());
           // showingTutorial1 = false;
            DestroyTutorial.Invoke();
            //Destroy Tutorial 1
            OnHideTutorialStatus.Invoke();
            tutorialInactiveMessagetimer = 0;
            curState = FSMState.TutorialSuccess;
        }
        tutorialInactiveMessagetimer += Time.deltaTime; 
        //if (currentTutorialObject)
        //{
            if (!showingTutorial1)
            {
                EffectsController.Instance.CreateFloatingTextEffectBasic(Vector3.zero, "Tap and Hold anywhere to Aim !", Color.white, this.transform, (Vector3.up * 2.5f), 0.1f);
                showingTutorial1 = true;
                tutorialInactiveMessagetimer = 0;
            }
            tutorialStatus.value = "Tap anywhere on the screen to aim in that direction !";

            //Old tutorial, waits until dummy is broken
            
            /*
            foreach (var item in tutorialTarget)
            {
                if(item.GetComponent<OnJointBreakScript>().jointBroken == true)
                {
                   
                    currentTutorialObject = tutorialTarget.Where(obj => obj.GetComponent<OnJointBreakScript>().jointBroken == false).FirstOrDefault();
                    showingTutorial1 = false;
                    DestroyTutorial.Invoke();
                    //Destroy Tutorial 1
                    OnHideTutorialStatus.Invoke();
                    tutorialInactiveMessagetimer = 0;
                    curState = FSMState.ShowTutorial_2;
                    //tutorialTarget.Remove(item);
                }
                else
                {
                    if (notInvoked && (tutorialInactiveMessagetimer > tutorialInactiveMessagetime))
                    {
                        OnShowTutorialStatus.Invoke();
                        tutorialInactiveMessagetimer = 0;
                        notInvoked = false;

                    }

                }
            }*/

            
            

            if (windUp && (attackButtonClickTimer > 0.2f) )
            {
                currentTutorialObject = tutorialTarget.Where(obj => obj.GetComponent<OnJointBreakScript>().jointBroken == false).FirstOrDefault();
                showingTutorial1 = false;
                DestroyTutorial.Invoke();
                //Destroy Tutorial 1
                OnHideTutorialStatus.Invoke();
                tutorialInactiveMessagetimer = 0;
            attack = false;
            windUp = false;
            curState = FSMState.ShowTutorial_2;
               
            //tutorialTarget.Remove(item);

        }
        else
            {
                attack = false;
                //windUp = false;

                if (notInvoked && (tutorialInactiveMessagetimer > tutorialInactiveMessagetime))
                {
                    OnShowTutorialStatus.Invoke();
                    tutorialInactiveMessagetimer = 0;
                    notInvoked = false;

                }

            }
            
            /*if (currentTutorialObject.GetComponent<OnJointBreakScript>().jointBroken == true)
            {
                currentTutorialObject = tutorialTarget.Where(obj => obj.GetComponent<OnJointBreakScript>().jointBroken == false).FirstOrDefault();
                showingTutorial1 = false;
                DestroyTutorial.Invoke();
                //Destroy Tutorial 1
                curState = FSMState.ShowTutorial_2;
            }*/
        //}
        
    }

    private void ShowTutorial_2()
    {
        if (skipTutorial)
        {
            StartCoroutine(showRandomTip());
            //showingTutorial1 = false;
            DestroyTutorial.Invoke();
            //Destroy Tutorial 1
            OnHideTutorialStatus.Invoke();
            curState = FSMState.TutorialSuccess;
        }

        tutorialInactiveMessagetimer += Time.deltaTime;
        //if (currentTutorialObject)
        //{
            if (!showingTutorial1)
            {
                EffectsController.Instance.CreateFloatingTextEffectBasic(Vector3.zero, "Now Release to Attack !", Color.white, this.transform, (Vector3.up * 2.5f), 0.1f);
                showingTutorial1 = true;
                tutorialInactiveMessagetimer = 0;
                notInvoked = true;
            }
            /*foreach (var item in tutorialTarget)
            {
                if (item.GetComponent<OnJointBreakScript>().jointBroken == true)
                {
                   // currentTutorialObject = tutorialTarget.Where(obj => obj.GetComponent<OnJointBreakScript>().jointBroken == false).FirstOrDefault();
                    showingTutorial1 = false;
                    DestroyTutorial.Invoke();
                    //Destroy Tutorial 1
                    curState = FSMState.None;
                }
            }*/
            tutorialStatus.value = "Let go to attack !";
            if (attack)
            {
                //currentTutorialObject = tutorialTarget.Where(obj => obj.GetComponent<OnJointBreakScript>().jointBroken == false).FirstOrDefault();
                showingTutorial1 = false;
                DestroyTutorial.Invoke();
                //Destroy Tutorial 1
                OnHideTutorialStatus.Invoke();
            attack = false;
            windUp = false;
            curState = FSMState.ShowTutorialContinueMessage;
                

            }
            else
            {
                if (notInvoked && (tutorialInactiveMessagetimer > tutorialInactiveMessagetime))
                {
                    OnShowTutorialStatus.Invoke();
                    notInvoked = false;
                }

            }

            //Old tutorial 

            /*
            if (currentTutorialObject.GetComponent<OnJointBreakScript>().jointBroken == true)
            {
                currentTutorialObject = tutorialTarget.Where(obj => obj.GetComponent<OnJointBreakScript>().jointBroken == false).FirstOrDefault();
                showingTutorial1 = false;
                DestroyTutorial.Invoke();
                //Destroy Tutorial 1
                OnHideTutorialStatus.Invoke();
                curState = FSMState.ShowTutorialContinueMessage;


            }
            else
            {
                if(notInvoked && (tutorialInactiveMessagetimer > tutorialInactiveMessagetime))
                {
                    OnShowTutorialStatus.Invoke();
                    notInvoked = false;
                }
                
            }*/

            /*
            foreach (var item in tutorialTarget)
            {
                if (item.GetComponent<OnJointBreakScript>().jointBroken == true)
                {

                    currentTutorialObject = tutorialTarget.Where(obj => obj.GetComponent<OnJointBreakScript>().jointBroken == false).FirstOrDefault();
                    showingTutorial1 = false;
                    DestroyTutorial.Invoke();
                    //Destroy Tutorial 1
                    OnHideTutorialStatus.Invoke();
                    tutorialInactiveMessagetimer = 0;
                    curState = FSMState.ShowTutorial_2;
                    //tutorialTarget.Remove(item);
                }
                else
                {
                    if (notInvoked && (tutorialInactiveMessagetimer > tutorialInactiveMessagetime))
                    {
                        OnShowTutorialStatus.Invoke();
                        tutorialInactiveMessagetimer = 0;
                        notInvoked = false;

                    }

                }
            }*/

        //}
        

        
    }

    public void SetSkipTutorial(bool value)
    {
       //showingTutorial1 = false;
        DestroyTutorial.Invoke();
        //Destroy Tutorial 1
        OnHideTutorialStatus.Invoke();
        tutorialInactiveMessagetimer = 0;
        OnHideSkipTutorial.Invoke();
        this.skipTutorial = value;
    }

    public void ShowTutorialSettings()
    {
        OnShowTutorialContinueMessage.Invoke();
    }
    public void SetTutorialOnStartValue(bool value)
    {
        showTutorialOnStart.value = value;
        tutorialContinueMessageButtonClicked = true;
        OnHideTutorialContinueMessage.Invoke();
        PersistableSO.Instance.Save();
    }

    IEnumerator showRandomTip()
    {
        yield return new WaitForSeconds(0.2f);

        //string result;
        //randomTips.TryGetValue(randomTipKey, out result);
        // Debug.Log("Random Tip : Count : "+ randomTips.Count+" | Key : " + randomTipKey + " | Value : " + randomTips[randomTipKey]);
        int randomTipKey = UnityEngine.Random.Range(0, randomTips.Count);
        tutorialStatus.value = randomTips[randomTipKey];
            OnShowTutorialStatus.Invoke();
            yield return new WaitForSeconds(5f);
            OnHideTutorialStatus.Invoke();
        

        
    }


    public void ToggleLeft()
    {
        selectedIndex--;
        if (selectedIndex < 0)
        {
            selectedIndex = levelCharacters.Count - 1;
        }
        UpdateTutorialView(selectedIndex);
        //Call Update UI
        //Update the camera target
        //set the selected character // set currentLevel = selectedLevel
    }

    public void ToggleRight()
    {
        selectedIndex++;
        if (selectedIndex == levelCharacters.Count)
        {
            selectedIndex = 0;
        }
        UpdateTutorialView(selectedIndex);
        //Call Update UI
        //Update the camera target
        //set the selected character // set currentLevel = selectedLevel
    }

    public void UpdateTutorialView(int index)
    {
        

    }

    /*public void OnTutorialSuccess()
    {
        tutorialSuccess_1.value = true;

    }*/
}
