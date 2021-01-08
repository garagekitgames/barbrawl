using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using garagekitgames;
//using EasyMobile;
using UnityEngine.SocialPlatforms;
using CloudOnce;

public class GPGSManager : UnitySingletonPersistent<GPGSManager>
{
    public bool isInitialized;

    public IntVariable highScore;
    public override void Awake()
    {
        base.Awake();

        //if (!RuntimeManager.IsInitialized())
        //    RuntimeManager.Init();


        
    }

    void OnEnable()
    {
        //GameServices.UserLoginSucceeded += OnUserLoginSucceeded;
       // GameServices.UserLoginFailed += OnUserLoginFailed;

        
        
    }

    // Unsubscribe
    void OnDisable()
    {
        //GameServices.UserLoginSucceeded -= OnUserLoginSucceeded;
       // GameServices.UserLoginFailed -= OnUserLoginFailed;

        Cloud.OnSignInFailed -= OnUserLoginFailed;
    }

    // Event handlers

    void OnInitializeSucceeded()
    {
        Cloud.OnInitializeComplete -= OnInitializeSucceeded;
        Debug.Log("User logged in successfully.");
    }


    void OnUserLoginSucceeded()
    {
        Debug.Log("User logged in successfully.");
    }

    void OnUserLoginFailed()
    {
        Debug.Log("User login failed.");
    }

    // Start is called before the first frame update
    void Start()
    {
        Cloud.Initialize(false, true);

        Cloud.OnInitializeComplete += OnInitializeSucceeded;
        Cloud.OnSignInFailed += OnUserLoginFailed;
        // isInitialized = GameServices.IsInitialized();
        //if (!GameServices.IsInitialized())
        // {
        //GameServices.Init();
        // }

        if (!Cloud.IsSignedIn)
        {
            Cloud.SignIn();
        }

    }

    // Update is called once per frame
    void Update()
    {
         //isInitialized = GameServices.IsInitialized();
    }

    public void ShowLeaderboardUI()
    {
        // Check for initialization before showing leaderboard UI
        /*if (GameServices.IsInitialized())
        {
            GameServices.ShowLeaderboardUI();
        }
        else
        {
#if UNITY_ANDROID
            GameServices.Init();    // start a new initialization process
#elif UNITY_IOS
    Debug.Log("Cannot show leaderboard UI: The user is not logged in to Game Center.");
#endif
        }*/
    }

    public void ShowAchievementsUI()
    {
        // Check for initialization before showing leaderboard UI
      /*  if (GameServices.IsInitialized())
        {
            GameServices.ShowAchievementsUI();
        }
        else
        {
#if UNITY_ANDROID
            GameServices.Init();    // start a new initialization process
#elif UNITY_IOS
    Debug.Log("Cannot show leaderboard UI: The user is not logged in to Game Center.");
#endif
        }*/
    }

    public void SubmitScoreToLeaderboard(int value)
    {

        if(Cloud.IsSignedIn)
        {
            //Leader
            //Leaderboards.HighScore.SubmitScore(highScore.value);
        }
        
        // Check for initialization before showing leaderboard UI
       /* if (GameServices.IsInitialized())
        {
            GameServices.ReportScore(highScore.value, EM_GameServicesConstants.Leaderboard_HighScore);
        }*/
        
    }

    public void LoadLocalUserScore()
    {
        // Check for initialization before showing leaderboard UI
        /*if (GameServices.IsInitialized())
        {
            GameServices.LoadLocalUserScore(EM_GameServicesConstants.Leaderboard_HighScore, OnLocalUserScoreLoaded);

        }
        else
        {
#if UNITY_ANDROID
            GameServices.Init();    // start a new initialization process
#elif UNITY_IOS
    Debug.Log("Cannot show leaderboard UI: The user is not logged in to Game Center.");
#endif
        }*/
    }


    // Score loaded callback
    void OnLocalUserScoreLoaded(string leaderboardName, IScore score)
    {
        /*if (score != null)
        {
            Debug.Log("Your score is: " + score.value);
        }
        else
        {
            Debug.Log("You don't have any score reported to leaderboard " + leaderboardName);
        }*/
    }

}
