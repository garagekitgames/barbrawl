using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using garagekitgames;
using UnityEngine.Events;

using UnityEngine.SceneManagement;

public class LevelManager : UnitySingletonPersistent<LevelManager>
{

    //current level
    public IntVariable currentLevel;
    //next Level
    public IntVariable nextLevel;
    //current exp amount
    public IntVariable currentExp;//= 0;
    //exp amount needed for lvl 1
    public IntVariable expBase;//= 10;
    //exp amount left to next levelup
    public IntVariable expLeft;//= 10;
    //modifier that increases needed exp each level
    public FloatVariable expModifier;//= 1.15f;

    public UnityEvent OnLevelUp;

    public IntVariable currentLevelReward;
    int previousExpLeft;

    public bool stopDoingShit =  false;

    public IntVariable cashCollected;
    public IntVariable cashCollectedThisRound;

    public string levelName;
    public string nextLevelName;

    public static string levelPrefix = "Level";

   

    public override void Awake()
    {
        base.Awake();

    }
    // Start is called before the first frame update
    void Start()
    {
        previousExpLeft = expLeft.value;
        levelName = levelPrefix + currentLevel.value;
        nextLevelName = levelPrefix + nextLevel.value;
    }

    private void OnLevelWasLoaded()
    {
        currentExp.value = 0;
    }
    //vvvvvv USED FOR TESTING FEEL FREE TO DELETE
    void Update()
    {

        nextLevel.value = currentLevel.value + 1;
        levelName = levelPrefix + currentLevel.value;
        /*if (Input.GetButtonDown("Jump"))
        {
            GainExp(3);
        }*/
    }
    //^^^^^^ USED FOR TESTING FEEL FREE TO DELETE

    public void StopGainingExp()
    {
        stopDoingShit = true;
    }

    public void StopGainingExpAfterXSec(float sec)
    {
        IEnumerator coroutine = SetStopDoingShit(sec, true);
        StartCoroutine(coroutine);
    }

    public void StartGainingExp()
    {
        stopDoingShit = false;
    }

    public void StartGainingExpAfterXSec(float sec)
    {
        //stopDoingShit = false;
        IEnumerator coroutine = SetStopDoingShit(sec, false);
        StartCoroutine(coroutine);
    }

    IEnumerator SetStopDoingShit(float sec, bool value)
    {
        yield return new WaitForSeconds(sec);
        stopDoingShit = value;
    }
    //leveling methods
    public void GainExp(int e)
    {
        if (stopDoingShit)
            return;

        currentExp.value += e;
        if (currentExp.value >= expLeft.value)
        {
            LvlUp();
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel.value;
    }

    public int GetCurrentExp()
    {
        return currentExp.value;
    }

    public int GetExpLeft()
    {
        return expLeft.value;
    }

    public void ResetExp()
    {
        currentExp.value = 0;
    }

    void LvlUp()
    {

        currentExp.value -= expLeft.value;
        currentLevel.value++;
        /*float t = Mathf.Pow(expModifier.value, currentLevel.value);
        expLeft.value = Mathf.Clamp((int)Mathf.Floor(expBase.value * t), (int)Mathf.Floor(expBase.value * t), 40);*/
        previousExpLeft = expLeft.value;
        //if((float)previousExpLeft * expModifier.value <= 40)
        //{
            int currentExpLeft = (int)Mathf.Clamp((float)previousExpLeft * expModifier.value, 10, 50);
            expLeft.value = currentExpLeft;
       // }
       // else
       // {
       //     int currentExpLeft = (int)Mathf.Clamp((float)previousExpLeft * expModifier.value, 10, currentLevel.value);
       //     expLeft.value = currentExpLeft;
       // }
        
        currentLevelReward.value = currentLevel.value * 2;
        IEnumerator coroutine2 = GiveLevelUpReward(1.5f);
        StartCoroutine(coroutine2);
        OnLevelUp.Invoke();
    }


    IEnumerator GiveLevelUpReward(float sec)
    {
        //SetStopDoingShit(true);
        StopGainingExp();
        StartGainingExpAfterXSec(sec);
        yield return new WaitForSeconds(sec);
        //EffectsController.Instance.CreateCash(Vector3.up * 2 + Random.insideUnitSphere, currentLevel.value * 2);
        AddCoins(currentLevel.value * 2);

        PersistableSO.Instance.Save();
        //Do Function here...
    }

    public void AddCoins(int value)
    {
        cashCollected.Add(value);
        cashCollectedThisRound.Add(value);
    }
}
