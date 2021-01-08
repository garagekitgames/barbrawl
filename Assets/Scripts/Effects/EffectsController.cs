using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using EZCameraShake;
using UnityEngine.SceneManagement;
using EZObjectPools;
using UnityEngine.UI;
using TMPro;
using SO;

using garagekitgames;
//
public class EffectsController : UnitySingletonPersistent<EffectsController>
{
    //
    public IntVariable currentScore;
    public IntVariable comboMultiplier;
    public SmokeObject smokePuff;

    public GameObject bloodPrefab;
    public GameObject cashPrefab;
    public GameObject alertPrefab;

    public GameObject lightHitFXPrefab;
    public GameObject heavyHitFXPrefab;

    public GameObject smokeFXPrefab;

    public GameObject windUpFXPrefab;
    public GameObject resurrectPrefab;

    public GameObject floatingTextPrefab;
    public GameObject scoreFloatingTextPrefab;
    public GameObject hintTextPrefab;

    public GameObject[] bgFxPrefabs;
    public int currentBgFx;

    public AudioClip[] clips;
    public AudioClip[] weaponclips;
    public AudioClip[] pcshshClips;
    public AudioClip[] windupclips;
    public AudioClip[] playerWindupclips;
    public AudioClip[] playerWeaponWindupclips;
    public AudioClip[] enemyWindupAlertclips;
    public AudioClip[] wooshclips;
    public AudioClip[] weaponWooshclips;
    public AudioClip[] crunchClips;
    public AudioClip[] groundImpactClips;

    public AudioClip[] groundFootStepClips;
    public AudioClip[] gruntClips;

    public AudioClip[] playerYellClips;

    public AudioClip[] pickupClip;
    //
    public float minVolume = 0.2f;
    public float maxVolume = 0.3f;
    //
    public float minVelocity = 1;
    public float maxVelocity = 10;
    //
    protected float lastImpactTime = 0.5f;
    public float minImpactDelay = 0.15f;
    //
    public float pitch = 1;

    // 
    public AudioMixerGroup mixerGroup;
    public Camera camTransform;

    public MultiFighterCamera mfCam;
    public Fighter3DCamera f3dCam;
    // How long the object should shake for.
    //public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    //


    public float Magnitude = 2f;
    public float Roughness = 10f;
    public float FadeOutTime = 5f;

    // public static EffectsController Instance;

    [Range(1.0f, 10f)]
    public float slowFactor = 1.8f;
    //the new time scale  
    //private float newTimeScale;
    private IEnumerator coroutine;
    EZObjectPool cashObjectPool = new EZObjectPool();
    EZObjectPool alertObjectPool = new EZObjectPool();
    EZObjectPool scoreObjectPool = new EZObjectPool();

    public bool bigHit = false;

    public bool damaged;
    public Image damageImage;
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.8f);     // The colour the damageImage is set to, to flash


    public bool enemyDamaged;
    public Image enemyDamageImage;
    public float enemyFlashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color enemyFlashColour = new Color(1f, 0f, 0f, 0.8f);     // The colour the damageImage is set to, to flash.

    public RipplePostProcessor camRippler;

    public LevelData currentLevel;

    public int currentSlowPriority = 0;
    public int previousSlowPriority = 0;

    public IntVariable currentPlayerLevel;

    private void Start()
    {
        Time.timeScale = 1;

        currentBgFx = 1;
    }

    public void ChangedBgFx(int i)
    {
        currentBgFx = i;
        //if (currentBgFx != 0)
        //{
            Instantiate(bgFxPrefabs[currentBgFx]);
        //}
    }


    public void setDamagedValue(bool value)
    {
        damaged = value;
    }
    public void setEnemyDamagedValue(bool value)
    {
        enemyDamaged = value;
    }
    private void Update()
    {
       
        if (damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        damaged = false;


        if (enemyDamaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            enemyDamageImage.color = enemyFlashColour;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            enemyDamageImage.color = Color.Lerp(enemyDamageImage.color, Color.clear, enemyFlashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        enemyDamaged = false;
    }


    public override void Awake()
    {
        base.Awake();
        //MathFL.SetupLookupTables();

        if (camTransform == null)
        {
            camTransform = Camera.main;
        }

        mfCam = camTransform.transform.root.GetComponent<MultiFighterCamera>();
        f3dCam = camTransform.transform.root.GetComponent<Fighter3DCamera>();
        cashObjectPool = EZObjectPool.CreateObjectPool(Instance.cashPrefab, cashPrefab.name, 10, true, true, true);
        alertObjectPool = EZObjectPool.CreateObjectPool(Instance.alertPrefab, alertPrefab.name, 10, true, true, true);
        //scoreObjectPool = EZObjectPool.CreateObjectPool(Instance.scoreFloatingTextPrefab, scoreFloatingTextPrefab.name, 10, true, true, true);
        camRippler = camTransform.transform.GetComponent<RipplePostProcessor>();
        Time.timeScale = 1;
        ResetTime(1);

    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);

        //reset the values  
        Time.timeScale = 1.0f;
        //Time.fixedDeltaTime = Time.fixedDeltaTime * slowFactorValue;
        Time.fixedDeltaTime = 0.01f;
        //Time.maximumDeltaTime = Time.maximumDeltaTime * slowFactorValue;
        Time.maximumDeltaTime = 0.3333333f;

        Instance.pitch = Time.timeScale;
        bigHit = false;
        previousSlowPriority = 0;

        if (scene.name.Contains("Level"))
        {
            if (camTransform == null)
            {
                camTransform = Camera.main;
            }

            mfCam = camTransform.transform.root.GetComponent<MultiFighterCamera>();
            f3dCam = camTransform.transform.root.GetComponent<Fighter3DCamera>();
            cashObjectPool = EZObjectPool.CreateObjectPool(Instance.cashPrefab, cashPrefab.name, 10, true, true, true);
            alertObjectPool = EZObjectPool.CreateObjectPool(Instance.alertPrefab, alertPrefab.name, 10, true, true, true);
           // scoreObjectPool = EZObjectPool.CreateObjectPool(Instance.scoreFloatingTextPrefab, scoreFloatingTextPrefab.name, 10, true, true, true);
            camRippler = camTransform.transform.GetComponent<RipplePostProcessor>();
            //reset the values  
            Time.timeScale = 1.0f;
            //Time.fixedDeltaTime = Time.fixedDeltaTime * slowFactorValue;
            Time.fixedDeltaTime = 0.01f;
            //Time.maximumDeltaTime = Time.maximumDeltaTime * slowFactorValue;
            Time.maximumDeltaTime = 0.3333333f;
            Instance.pitch = Time.timeScale;
            bigHit = false;
            previousSlowPriority = 0;
        }
        
    }


    public void slowDownTime(float slowFactorValue, float waitTime, int m)
    {

        /*else if(m == 1)
        {
            bigHit = false;
        }*/
        currentSlowPriority = m;
        if (currentSlowPriority >= previousSlowPriority)
        {
            coroutine = TimeSlow(slowFactorValue, waitTime, m);
            StartCoroutine(coroutine);
        }
       /* if(!bigHit)
        {
            coroutine = TimeSlow(slowFactorValue, waitTime, m);
            StartCoroutine(coroutine);
        }*/
        
    }
    private IEnumerator TimeSlow(float slowFactorValue, float waitTime, int m)
    {
        
        // Debug.Log("Actual Value: " + Time.maximumDeltaTime);
        SlowTime(slowFactorValue, m);
       // Debug.Log("Changed Value: " + Time.maximumDeltaTime);
        //Debug.Log("Supposed Value: " + timeScaleAmount * timePerFrame);
        yield return new WaitForSeconds(waitTime);

        ResetTime(slowFactorValue);
       // Debug.Log("After Reset Value: " + Time.maximumDeltaTime);
        //print("Coroutine ended: " + Time.time + " seconds");
    }

    public void SlowTime(float slowFactorValue, int m)
    {

        previousSlowPriority = currentSlowPriority;

        if (Time.timeScale == 1.0f)
        {
            if (m == 0)
            {
                bigHit = true;
            }
            float  newTimeScale = Time.timeScale / slowFactorValue;
            //assign the 'newTimeScale' to the current 'timeScale'  
            Time.timeScale = newTimeScale;
            //proportionally reduce the 'fixedDeltaTime', so that the Rigidbody simulation can react correctly  
            Time.fixedDeltaTime = Time.fixedDeltaTime / slowFactorValue;
            //The maximum amount of time of a single frame  
            Time.maximumDeltaTime = Time.maximumDeltaTime / slowFactorValue;

            Instance.pitch = 0.8f;
        }

    }

    public void ResetTime(float slowFactorValue)
    {
        if ((Time.timeScale != 1.0f) && (Time.fixedDeltaTime != 0.01f)) //the game is running in slow motion  
        {
            //reset the values  
            Time.timeScale = 1.0f;
            //Time.fixedDeltaTime = Time.fixedDeltaTime * slowFactorValue;
            Time.fixedDeltaTime = 0.01f;
            //Time.maximumDeltaTime = Time.maximumDeltaTime * slowFactorValue;
            Time.maximumDeltaTime = 0.3333333f;
            Instance.pitch = Time.timeScale;
            bigHit = false;
            previousSlowPriority = 0;
        }
    }

   /* public void SwitchTime()
    {

        //if the game is running normally  
        if (Time.timeScale == 1.0f)
        {
            //assign the 'newTimeScale' to the current 'timeScale'  
            Time.timeScale = newTimeScale;
            //proportionally reduce the 'fixedDeltaTime', so that the Rigidbody simulation can react correctly  
            Time.fixedDeltaTime = Time.fixedDeltaTime / slowFactor;
            //The maximum amount of time of a single frame  
            Time.maximumDeltaTime = Time.maximumDeltaTime / slowFactor;
        }
        else if (Time.timeScale == newTimeScale) //the game is running in slow motion  
        {
            //reset the values  
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = Time.fixedDeltaTime * slowFactor;
            Time.maximumDeltaTime = Time.maximumDeltaTime * slowFactor;
        }

    }*/


    //
    public void CreateSmokePuffs(int count, Vector3 position, float force, Vector3 baseVelocity, float startOffset)
    {
        if (Instance != null)
        {
            float startAngle = Random.value * 3;
            for (int i = 0; i < count; i++)
            {
                Vector2 direction = MathFL.Point2OnCircle(startAngle + Mathf.PI * 2 * (i / (float)count), i % 2 == 0 ? force * (0.8f + Random.value * 0.2f) : force * 0.99f * (0.9f + Random.value * 0.1f));
                SmokeObject smoke = Instantiate(Instance.smokePuff, position + new Vector3(direction.x * startOffset, 0, direction.y * startOffset), Quaternion.identity) as SmokeObject;
                smoke.deathOnGroundCollision = false;
                Vector3 rVelocity = Random.onUnitSphere * 0.2f;
                if (rVelocity.y < 0) rVelocity *= -1;
                rVelocity.y += force * 0.0f;
                smoke.Setup(new Vector3(direction.x, 0, direction.y) + rVelocity);
            }
        }
        else
        {
            Debug.LogError("No Effects Controller Instance in scene.");
        }
    }

    public  void CreateBloodEffect(Vector3 position)
    {
        GameObject blood = Instantiate(Instance.bloodPrefab, position, Quaternion.identity);
    }

    public void CreateWindupEffect(Vector3 position, Transform parent)
    {
        GameObject windup = Instantiate(Instance.windUpFXPrefab, position, Quaternion.identity, parent);
    }

    public void CreateResurrectEffect(Vector3 position)
    {
        GameObject resurrect = Instantiate(Instance.resurrectPrefab, position, Instance.resurrectPrefab.transform.rotation);
    }

    public void CreateCash(Vector3 position, int num)
    {
        for (int i = 0; i < num; i++)
        {
            //GameObject cash = Instantiate(Instance.cashPrefab, position, Quaternion.identity);

            GameObject cash;
            //Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            cashObjectPool.TryGetNextObject(position, Quaternion.identity, out cash);
        }
        //GameObject cash = Instantiate(Instance.cashPrefab, position, Quaternion.identity);
    }

    public void CreateAlert(Vector3 position)
    {
        //for (int i = 0; i < num; i++)
       // {
            //GameObject cash = Instantiate(Instance.cashPrefab, position, Quaternion.identity);

            GameObject alert;
            //Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            alertObjectPool.TryGetNextObject(position, Quaternion.identity, out alert);
        //}
        //GameObject cash = Instantiate(Instance.cashPrefab, position, Quaternion.identity);
    }

    public void CreateFloatingTextEffect(Vector3 position, float relativeVolocityMagnitude, Transform parent, int score, bool amIMainPlayer)
    {
        //if(relativeVolocityMagnitude <= 65)
        //{
        //    GameObject hitFX = Instantiate(Instance.floatingTextPrefab, position, Quaternion.identity, parent);
        //}
        //else
        //{
        GameObject floatingTextFX = Instantiate(Instance.floatingTextPrefab, position + (Vector3.up * 1.5f), Quaternion.identity);
        if(amIMainPlayer)
        {
            floatingTextFX.transform.GetChild(0).GetComponentInChildren<TextMeshPro>().text = "- " + Mathf.RoundToInt(score).ToString();
            floatingTextFX.transform.GetChild(0).GetComponentInChildren<TextMeshPro>().color = Color.red;
        }
        else
        {
            floatingTextFX.transform.GetChild(0).GetComponentInChildren<TextMeshPro>().text = Mathf.RoundToInt(score).ToString();
        }
        
        //}

    }

    public void CreateFloatingTextEffectBasic(Vector3 position, string textToDisplay, Color textColor, Transform parent, Vector3 offset, float showTextDuration)
    {
        /*
        GameObject floatingTextFX = Instantiate(Instance.hintTextPrefab, position + offset, Quaternion.identity);
        destroyMe textDestroy = floatingTextFX.GetComponentInChildren<destroyMe>();
        textDestroy.deathtimer = showTextDuration;
        
        floatingTextFX.transform.GetChild(0).GetComponentInChildren<TextMeshPro>().text = textToDisplay;
        floatingTextFX.transform.GetChild(0).GetComponentInChildren<TextMeshPro>().color = textColor;
        */

        Instance.StartCoroutine(Instance.createFloatingTextWithDelay( position,  textToDisplay,  textColor,  parent,  offset,  showTextDuration, 0.2f));

    }

    public void CreateFloatingTextWithDelayForScore(Vector3 position, string textToDisplay, Color textColor, Transform parent, Vector3 offset, float showTextDuration)
    {
        /*
        GameObject floatingTextFX = Instantiate(Instance.hintTextPrefab, position + offset, Quaternion.identity);
        destroyMe textDestroy = floatingTextFX.GetComponentInChildren<destroyMe>();
        textDestroy.deathtimer = showTextDuration;
        
        floatingTextFX.transform.GetChild(0).GetComponentInChildren<TextMeshPro>().text = textToDisplay;
        floatingTextFX.transform.GetChild(0).GetComponentInChildren<TextMeshPro>().color = textColor;
        */

        Instance.StartCoroutine(Instance.createFloatingTextWithDelayForScore(position, textToDisplay, textColor, parent, offset, showTextDuration, 0.1f));

    }


    IEnumerator createFloatingTextWithDelay(Vector3 position, string textToDisplay, Color textColor, Transform parent, Vector3 offset, float showTextDuration, float sec)
    {
        yield return new WaitForSeconds(sec);
        GameObject floatingTextFX = Instantiate(Instance.hintTextPrefab, position + offset, Quaternion.identity);
        destroyMe textDestroy = floatingTextFX.GetComponentInChildren<destroyMe>();
        textDestroy.deathtimer = showTextDuration;
        //if(amIMainPlayer)
        //{
        floatingTextFX.transform.GetChild(0).GetComponentInChildren<TextMeshPro>().text = textToDisplay;
        floatingTextFX.transform.GetChild(0).GetComponentInChildren<TextMeshPro>().color = textColor;

    }

    IEnumerator createFloatingTextWithDelayForScore(Vector3 position, string textToDisplay, Color textColor, Transform parent, Vector3 offset, float showTextDuration, float sec)
    {
        yield return new WaitForSeconds(sec);

        GameObject scoreTextGO;
        //Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        scoreObjectPool.TryGetNextObject(position + offset, Quaternion.identity, out scoreTextGO);

        //GameObject floatingTextFX = Instantiate(Instance.hintTextPrefab, position + offset, Quaternion.identity);
        destroyMe textDestroy = scoreTextGO.GetComponentInChildren<destroyMe>();
        textDestroy.deathtimer = showTextDuration;
        //if(amIMainPlayer)
        //{
        scoreTextGO.transform.GetChild(0).GetComponentInChildren<TextMeshPro>().text = textToDisplay;
        scoreTextGO.transform.GetChild(0).GetComponentInChildren<TextMeshPro>().color = textColor;

    }

    public void CreateFloatingTextEffectForScore(Vector3 position, float relativeVolocityMagnitude, Transform parent, int score, bool amIMainPlayer)
    {
        //if(relativeVolocityMagnitude <= 65)
        //{
        //    GameObject hitFX = Instantiate(Instance.floatingTextPrefab, position, Quaternion.identity, parent);
        //}
        //else
        //{
        GameObject floatingTextFX = Instantiate(Instance.floatingTextPrefab, position + (Vector3.up * 1.5f), Quaternion.identity);
        if (amIMainPlayer)
        {
            //floatingTextFX.transform.GetChild(0).GetComponentInChildren<TextMeshPro>().text = "- " + Mathf.RoundToInt(relativeVolocityMagnitude).ToString();
           // floatingTextFX.transform.GetChild(0).GetComponentInChildren<TextMeshPro>().color = Color.red;
        }
        else
        {
            floatingTextFX.transform.GetChild(0).GetComponentInChildren<TextMeshPro>().text = "+ " + Mathf.RoundToInt(comboMultiplier.value).ToString();
        }

        //}

    }


    public void CreateHitEffect(Vector3 position, float relativeVolocityMagnitude, Vector3 normal)
    {

        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, normal);
        //camRippler.RippleEffect(position);
        if (relativeVolocityMagnitude <= 65)
        {
            GameObject hitFX = Instantiate(Instance.lightHitFXPrefab, position, rot);
        }
        else
        {
            GameObject hitFX = Instantiate(Instance.heavyHitFXPrefab, position, rot);
        }

    }

    public void CreateSmokeEffect(Vector3 position, float relativeVolocityMagnitude)
    {
        GameObject smokeFX = Instantiate(Instance.smokeFXPrefab, position, Quaternion.identity);

        float m = Mathf.Clamp01((relativeVolocityMagnitude - Instance.minVelocity) / (Instance.maxVelocity - Instance.minVelocity));
        float volumeM = Instance.minVolume + (Instance.maxVolume - Instance.minVolume) * m;

        GameObject gO = new GameObject("OneShotAudio");
        gO.transform.position = position;
        AudioSource source = gO.AddComponent<AudioSource>();
        source.loop = false;
        source.dopplerLevel = 0.1f;
        source.volume = volumeM;
        source.pitch = Instance.pitch; //Random.Range(0.8f, 1.2f);//Instance.pitch;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.minDistance = 2000;
        source.maxDistance = 2010;
        source.spatialBlend = 0.5f;
        source.outputAudioMixerGroup = Instance.mixerGroup;
        int clipIndex = Mathf.FloorToInt(Random.Range(0, Instance.groundImpactClips.Length));
        if (clipIndex < Instance.groundImpactClips.Length && clipIndex >= 0)
        {
            source.clip = Instance.groundImpactClips[clipIndex]; // **** INDEX 0 to 1 less than number of clips ***
            //Debug.Log("groundImpactClips " + clipIndex + "  groundImpactClips.Length " + Instance.groundImpactClips.Length);
        }
        else
        {
           // Debug.LogError("groundImpactClips " + clipIndex + "  groundImpactClips.Length " + Instance.groundImpactClips.Length);
        }
        source.Play();
        //
        Destroy(gO, source.clip.length + 0.1f);

    }

    public  void PlayImpactSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
    {
        if (relativeVolocityMagnitude > Instance.minVelocity)
        {
            //
            float m = Mathf.Clamp01((relativeVolocityMagnitude - Instance.minVelocity) / (Instance.maxVelocity - Instance.minVelocity));
            float volumeM = Instance.minVolume + (Instance.maxVolume - Instance.minVolume) * m;
            //
            GameObject gO = new GameObject("OneShotAudio");
            gO.transform.position = collisionPoint;
            AudioSource source = gO.AddComponent<AudioSource>();
            source.loop = false;
            source.dopplerLevel = 0.1f;
            source.volume = volumeM;
            source.pitch = Random.Range(Instance.pitch - 0.1f, Instance.pitch + 0.1f); //Instance.pitch;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 2000;
            source.maxDistance = 2010;
            source.spatialBlend = 0.5f;
            source.outputAudioMixerGroup = Instance.mixerGroup;
            /*int clipIndex = Mathf.FloorToInt(m * 0.999f * Instance.clips.Length);
            if (clipIndex < Instance.clips.Length && clipIndex >= 0)
            {
                source.clip = Instance.clips[Mathf.FloorToInt(m * 0.999f * Instance.clips.Length)]; // **** INDEX 0 to 1 less than number of clips ***
               // Debug.Log("clipIndex " + clipIndex + "  clips.Length " + Instance.clips.Length);
            }
            else
            {
               // Debug.LogError("clipIndex " + clipIndex + "  clips.Length " + Instance.clips.Length);
            }*/

            if (tag.Contains("Wep"))
            {
                int clipIndex = Random.Range(0, Instance.weaponclips.Length);  //Mathf.FloorToInt(m * 0.999f * Instance.windupclips.Length);
                if (clipIndex < Instance.weaponclips.Length && clipIndex >= 0)
                {
                    source.clip = Instance.weaponclips[clipIndex]; // **** INDEX 0 to 1 less than number of clips ***
                                                                        //  Debug.Log("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
                }
                else
                {
                    //  Debug.LogError("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
                }
            }
            else
            {
                int clipIndex = Mathf.FloorToInt(m * 0.999f * Instance.clips.Length);  //Mathf.FloorToInt(m * 0.999f * Instance.windupclips.Length);
                if (clipIndex < Instance.clips.Length && clipIndex >= 0)
                {
                    source.clip = Instance.clips[clipIndex]; // **** INDEX 0 to 1 less than number of clips ***
                                                                  //  Debug.Log("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
                }
                else
                {
                    //  Debug.LogError("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
                }
            }

            source.Play();
            //
            Destroy(gO, source.clip.length + 0.1f);
            //
        }
    }

    public void PlayGruntSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
    {
        if (relativeVolocityMagnitude > Instance.minVelocity)
        {
            //
            float m = Mathf.Clamp01((relativeVolocityMagnitude - Instance.minVelocity) / (Instance.maxVelocity - Instance.minVelocity));
            float volumeM = Instance.minVolume + (Instance.maxVolume - Instance.minVolume) * m;
            //
            GameObject gO = new GameObject("OneShotAudio");
            gO.transform.position = collisionPoint;
            AudioSource source = gO.AddComponent<AudioSource>();
            source.loop = false;
            source.dopplerLevel = 0.1f;
            source.volume = volumeM;
            source.pitch = Random.Range(Instance.pitch - 0.3f, Instance.pitch + 0.3f); //Instance.pitch;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 2000;
            source.maxDistance = 2010;
            source.spatialBlend = 0.5f;
            source.outputAudioMixerGroup = Instance.mixerGroup;
            int clipIndex = Random.Range(0, Instance.gruntClips.Length); // Mathf.FloorToInt(m * 0.999f * Instance.gruntClips.Length);
            if (clipIndex < Instance.gruntClips.Length && clipIndex >= 0)
            {
                source.clip = Instance.gruntClips[clipIndex]; // **** INDEX 0 to 1 less than number of clips ***
               // Debug.Log("clipIndex " + clipIndex + "  clips.Length " + Instance.gruntClips.Length);
            }
            else
            {
              //  Debug.LogError("clipIndex " + clipIndex + "  clips.Length " + Instance.gruntClips.Length);
            }
            source.Play();
            //
            Destroy(gO, source.clip.length + 0.1f);
            //
        }
    }

    public void PlayDeathSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
    {
        if (relativeVolocityMagnitude > Instance.minVelocity)
        {
            //
            float m = Mathf.Clamp01((relativeVolocityMagnitude - Instance.minVelocity) / (Instance.maxVelocity - Instance.minVelocity));
            float volumeM = Instance.minVolume + (Instance.maxVolume - Instance.minVolume) * m;
            //
            GameObject gO = new GameObject("OneShotAudio");
            gO.transform.position = collisionPoint;
            AudioSource source = gO.AddComponent<AudioSource>();
            source.loop = false;
            source.dopplerLevel = 0.1f;
            source.volume = volumeM;
            source.pitch = Random.Range(Instance.pitch - 0.3f, Instance.pitch + 0.3f); //Instance.pitch;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 2000;
            source.maxDistance = 2010;
            source.spatialBlend = 0.5f;
            source.outputAudioMixerGroup = Instance.mixerGroup;
            int clipIndex = Random.Range(0, Instance.gruntClips.Length); // Mathf.FloorToInt(m * 0.999f * Instance.gruntClips.Length);
            if (clipIndex < Instance.gruntClips.Length && clipIndex >= 0)
            {
                source.clip = Instance.gruntClips[clipIndex]; // **** INDEX 0 to 1 less than number of clips ***
                                                              // Debug.Log("clipIndex " + clipIndex + "  clips.Length " + Instance.gruntClips.Length);
            }
            else
            {
                //  Debug.LogError("clipIndex " + clipIndex + "  clips.Length " + Instance.gruntClips.Length);
            }
            source.Play();
            //
            Destroy(gO, source.clip.length + 0.1f);
            //
        }
    }

    public  void PlayBreakSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
    {
        if (relativeVolocityMagnitude > Instance.minVelocity)
        {
            //
            float m = Mathf.Clamp01((relativeVolocityMagnitude - Instance.minVelocity) / (Instance.maxVelocity - Instance.minVelocity));
            float volumeM = Instance.minVolume + (Instance.maxVolume - Instance.minVolume) * m;
            //
            GameObject gO = new GameObject("OneShotAudio");
            gO.transform.position = collisionPoint;
            AudioSource source = gO.AddComponent<AudioSource>();
            source.loop = false;
            source.dopplerLevel = 0.1f;
            source.volume = volumeM;
            source.pitch = Random.Range(Instance.pitch - 0.3f, Instance.pitch + 0.3f);  //Instance.pitch;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 2000;
            source.maxDistance = 2010;
            source.spatialBlend = 0.5f;
            source.outputAudioMixerGroup = Instance.mixerGroup;
            int clipIndex = Mathf.FloorToInt(m * 0.999f * Instance.crunchClips.Length);
            if (clipIndex < Instance.crunchClips.Length && clipIndex >= 0)
            {
                source.clip = Instance.crunchClips[Mathf.FloorToInt(m * 0.999f * Instance.crunchClips.Length)]; // **** INDEX 0 to 1 less than number of clips ***
              //  Debug.Log("clipIndex " + clipIndex + "  clips.Length " + Instance.crunchClips.Length);
            }
            else
            {
              //  Debug.LogError("clipIndex " + clipIndex + "  clips.Length " + Instance.crunchClips.Length);
            }
            source.Play();
            //
            Destroy(gO, source.clip.length + 0.1f);
            //
        }
    }

    public  void Sleep(float x)
    {
        // sleep 
        Instance.StartCoroutine(Instance.SleepTime(x));
    }

    public IEnumerator SleepTime(float x)
    {
        float initialTimeScale = Time.timeScale;
        //####MAKE DEAD coroutine
        Time.timeScale = 0f;
        //print ("before pause : "+x+" : "+Time.time);
        yield return new WaitForSecondsRealtime(x);
        //print ("after pause : "+x+" : "+Time.time);
        Time.timeScale = initialTimeScale;

    }

    public  void PlayBloodSpurtSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
    {
        if (relativeVolocityMagnitude > Instance.minVelocity)
        {
            //
            float m = Mathf.Clamp01((relativeVolocityMagnitude - Instance.minVelocity) / (Instance.maxVelocity - Instance.minVelocity));
            float volumeM = Instance.minVolume + (Instance.maxVolume - Instance.minVolume) * m;
            //
            GameObject gO = new GameObject("OneShotAudio");
            gO.transform.position = collisionPoint;
            AudioSource source = gO.AddComponent<AudioSource>();
            source.loop = false;
            source.dopplerLevel = 0.1f;
            source.volume = volumeM;
            source.pitch = Random.Range(Instance.pitch - 0.3f, Instance.pitch + 0.3f);  //Instance.pitch;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 2000;
            source.maxDistance = 2010;
            source.spatialBlend = 0.5f;
            source.outputAudioMixerGroup = Instance.mixerGroup;
            int clipIndex = Mathf.FloorToInt(m * 0.999f * Instance.pcshshClips.Length);
            if (clipIndex < Instance.pcshshClips.Length && clipIndex >= 0)
            {
                source.clip = Instance.pcshshClips[Mathf.FloorToInt(m * 0.999f * Instance.pcshshClips.Length)]; // **** INDEX 0 to 1 less than number of clips ***
              //  Debug.Log("clipIndex " + clipIndex + "  clips.Length " + Instance.pcshshClips.Length);

            }
            else
            {
              //  Debug.LogError("clipIndex " + clipIndex + "  clips.Length " + Instance.pcshshClips.Length);
            }
            source.Play();
            //
            Destroy(gO, source.clip.length + 0.1f);
            //
        }
    }


    public void PlayFootStepSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
    {
        if (relativeVolocityMagnitude > Instance.minVelocity)
        {
            //
            float m = Mathf.Clamp01((relativeVolocityMagnitude - Instance.minVelocity) / (Instance.maxVelocity - Instance.minVelocity));
            float volumeM = Instance.minVolume + (Instance.maxVolume - Instance.minVolume) * m;
            //
            GameObject gO = new GameObject("OneShotAudio");
            gO.transform.position = collisionPoint;
            AudioSource source = gO.AddComponent<AudioSource>();
            source.loop = false;
            source.dopplerLevel = 0.1f;
            source.volume = volumeM * 0.5f;
            source.pitch = Random.Range(Instance.pitch - 0.3f, Instance.pitch + 0.3f);  //Instance.pitch;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 2000;
            source.maxDistance = 2010;
            source.spatialBlend = 0.5f;
            source.outputAudioMixerGroup = Instance.mixerGroup;
            int clipIndex = Mathf.FloorToInt(m * 0.999f * Instance.groundFootStepClips.Length);
            if (clipIndex < Instance.groundFootStepClips.Length && clipIndex >= 0)
            {
                source.clip = Instance.groundFootStepClips[Mathf.FloorToInt(m * 0.999f * Instance.groundFootStepClips.Length)]; // **** INDEX 0 to 1 less than number of clips ***
              //  Debug.Log("groundFootStepClips " + clipIndex + "  groundFootStepClips.Length " + Instance.groundFootStepClips.Length);

            }
            else
            {
               // Debug.LogError("groundFootStepClips " + clipIndex + "  groundFootStepClips.Length " + Instance.groundFootStepClips.Length);
            }
            source.Play();
            //
            Destroy(gO, source.clip.length + 0.1f);
            //
        }
    }


    public  void PlayWooshSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
    {
        if (relativeVolocityMagnitude > Instance.minVelocity)
        {
            //
            float m = Mathf.Clamp01((relativeVolocityMagnitude - Instance.minVelocity) / (Instance.maxVelocity - Instance.minVelocity));
            float volumeM = Instance.minVolume + (Instance.maxVolume - Instance.minVolume) * m;
            //
            GameObject gO = new GameObject("OneShotAudio");
            gO.transform.position = collisionPoint;
            AudioSource source = gO.AddComponent<AudioSource>();
            source.loop = false;
            source.dopplerLevel = 0.1f;
            source.volume = volumeM * 2;
            source.pitch = Instance.pitch;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 2000;
            source.maxDistance = 2010;
            source.spatialBlend = 0.5f;
            source.outputAudioMixerGroup = Instance.mixerGroup;
            /*int clipIndex = Mathf.FloorToInt(m * 0.999f * Instance.wooshclips.Length);
            if (clipIndex < Instance.wooshclips.Length && clipIndex >= 0)
            {
                source.clip = Instance.wooshclips[Mathf.FloorToInt(m * 0.999f * Instance.wooshclips.Length)]; // **** INDEX 0 to 1 less than number of clips ***
               // Debug.Log("clipIndex " + clipIndex + "  clips.Length " + Instance.wooshclips.Length);
            }
            else
            {
              //  Debug.LogError("clipIndex " + clipIndex + "  clips.Length " + Instance.wooshclips.Length);
            }*/

            if (tag.Contains("Wep"))
            {
                int clipIndex = Random.Range(0, Instance.weaponWooshclips.Length);  //Mathf.FloorToInt(m * 0.999f * Instance.windupclips.Length);
                if (clipIndex < Instance.weaponWooshclips.Length && clipIndex >= 0)
                {
                    source.clip = Instance.weaponWooshclips[clipIndex]; // **** INDEX 0 to 1 less than number of clips ***
                                                                               //  Debug.Log("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
                }
                else
                {
                    //  Debug.LogError("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
                }
            }
            else
            {
                int clipIndex = Random.Range(0, Instance.wooshclips.Length);  //Mathf.FloorToInt(m * 0.999f * Instance.windupclips.Length);
                if (clipIndex < Instance.wooshclips.Length && clipIndex >= 0)
                {
                    source.clip = Instance.wooshclips[clipIndex]; // **** INDEX 0 to 1 less than number of clips ***
                                                                         //  Debug.Log("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
                }
                else
                {
                    //  Debug.LogError("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
                }
            }

            source.Play();
            //
            Destroy(gO, source.clip.length + 0.1f);
            //
        }
    }



    public void PlayWindupSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
    {
        if (relativeVolocityMagnitude > Instance.minVelocity)
        {
            //
            float m = Mathf.Clamp01((relativeVolocityMagnitude - Instance.minVelocity) / (Instance.maxVelocity - Instance.minVelocity));
            float volumeM = Instance.minVolume + (Instance.maxVolume - Instance.minVolume) * m;
            //
            GameObject gO = new GameObject("OneShotAudio");
            gO.transform.position = collisionPoint;
            AudioSource source = gO.AddComponent<AudioSource>();
            source.loop = false;
            source.dopplerLevel = 0.1f;
            source.volume = volumeM * 2f;
            source.pitch = Random.Range(Instance.pitch - 0.1f, Instance.pitch + 0.1f);  //Instance.pitch;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 2000;
            source.maxDistance = 2010;
            source.spatialBlend = 0.5f;
            source.outputAudioMixerGroup = Instance.mixerGroup;
            int clipIndex = Random.Range(0, Instance.windupclips.Length);  //Mathf.FloorToInt(m * 0.999f * Instance.windupclips.Length);
            if (clipIndex < Instance.windupclips.Length && clipIndex >= 0)
            {
                source.clip = Instance.windupclips[clipIndex]; // **** INDEX 0 to 1 less than number of clips ***
              //  Debug.Log("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
            }
            else
            {
              //  Debug.LogError("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
            }
            source.Play();
            //
            Destroy(gO, source.clip.length + 0.1f);
            //
        }
    }

    public void PlayPlayerWindupSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
    {
        if (relativeVolocityMagnitude > Instance.minVelocity)
        {
            //
            float m = Mathf.Clamp01((relativeVolocityMagnitude - Instance.minVelocity) / (Instance.maxVelocity - Instance.minVelocity));
            float volumeM = Instance.minVolume + (Instance.maxVolume - Instance.minVolume) * m;
            //
            GameObject gO = new GameObject("OneShotAudio");
            gO.transform.position = collisionPoint;
            AudioSource source = gO.AddComponent<AudioSource>();
            source.loop = false;
            source.dopplerLevel = 0.1f;
            
            source.pitch = Random.Range(Instance.pitch - 0.1f, Instance.pitch + 0.1f);  //Instance.pitch;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 2000;
            source.maxDistance = 2010;
            source.spatialBlend = 0.5f;
            source.outputAudioMixerGroup = Instance.mixerGroup;
            if (tag.Contains("Wep"))
            {
                source.volume = volumeM * 2f;
                int clipIndex = Random.Range(0, Instance.playerWeaponWindupclips.Length);  //Mathf.FloorToInt(m * 0.999f * Instance.windupclips.Length);
                if (clipIndex < Instance.playerWeaponWindupclips.Length && clipIndex >= 0)
                {
                    source.clip = Instance.playerWeaponWindupclips[clipIndex]; // **** INDEX 0 to 1 less than number of clips ***
                                                                         //  Debug.Log("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
                }
                else
                {
                    //  Debug.LogError("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
                }
            }
            else
            {
                source.volume = volumeM *0.1f;
                int clipIndex = Random.Range(0, Instance.playerWindupclips.Length);  //Mathf.FloorToInt(m * 0.999f * Instance.windupclips.Length);
                if (clipIndex < Instance.playerWindupclips.Length && clipIndex >= 0)
                {
                    source.clip = Instance.playerWindupclips[clipIndex]; // **** INDEX 0 to 1 less than number of clips ***
                                                                         //  Debug.Log("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
                }
                else
                {
                    //  Debug.LogError("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
                }
            }
            source.Play();
            //
            Destroy(gO, source.clip.length + 0.1f);
            //
        }
    }

    public void PlayEnemyWindupAlertSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
    {
        if (relativeVolocityMagnitude > Instance.minVelocity)
        {
            //
            float m = Mathf.Clamp01((relativeVolocityMagnitude - Instance.minVelocity) / (Instance.maxVelocity - Instance.minVelocity));
            float volumeM = Instance.minVolume + (Instance.maxVolume - Instance.minVolume) * m;
            //
            GameObject gO = new GameObject("OneShotAudio");
            gO.transform.position = collisionPoint;
            AudioSource source = gO.AddComponent<AudioSource>();
            source.loop = false;
            source.dopplerLevel = 0.1f;
            source.volume = volumeM * 0.3f;
            source.pitch = Random.Range(Instance.pitch - 0.1f, Instance.pitch + 0.1f);  //Instance.pitch;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 2000;
            source.maxDistance = 2010;
            source.spatialBlend = 0.5f;
            source.outputAudioMixerGroup = Instance.mixerGroup;
            int clipIndex = Random.Range(0, Instance.enemyWindupAlertclips.Length);  //Mathf.FloorToInt(m * 0.999f * Instance.windupclips.Length);
            if (clipIndex < Instance.enemyWindupAlertclips.Length && clipIndex >= 0)
            {
                source.clip = Instance.enemyWindupAlertclips[clipIndex]; // **** INDEX 0 to 1 less than number of clips ***
                                                                     //  Debug.Log("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
            }
            else
            {
                //  Debug.LogError("windupclips " + clipIndex + "  windupclips.Length " + Instance.windupclips.Length);
            }
            source.Play();
            //
            Destroy(gO, source.clip.length + 0.1f);
            //
        }
    }

    public void PlayYellSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
    {
        if (relativeVolocityMagnitude > Instance.minVelocity)
        {
            //
            float m = Mathf.Clamp01((relativeVolocityMagnitude - Instance.minVelocity) / (Instance.maxVelocity - Instance.minVelocity));
            float volumeM = Instance.minVolume + (Instance.maxVolume - Instance.minVolume) * m;
            //
            GameObject gO = new GameObject("OneShotAudio");
            gO.transform.position = collisionPoint;
            AudioSource source = gO.AddComponent<AudioSource>();
            source.loop = false;
            source.dopplerLevel = 0.1f;
            source.volume = volumeM * 0.3f;
            source.pitch = Instance.pitch;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 2000;
            source.maxDistance = 2010;
            source.spatialBlend = 0.5f;
            source.outputAudioMixerGroup = Instance.mixerGroup;
            int clipIndex = Random.Range(0, Instance.playerYellClips.Length);  // Mathf.FloorToInt(m * 0.999f * Instance.playerYellClips.Length); //Random.Range(0, Instance.playerYellClips.Length);//
            if (clipIndex < Instance.playerYellClips.Length && clipIndex >= 0)
            {
                source.clip = Instance.playerYellClips[clipIndex]; // **** INDEX 0 to 1 less than number of clips ***
               // Debug.Log("clipIndex " + clipIndex + "  clips.Length " + Instance.playerYellClips.Length);
            }
            else
            {
              //  Debug.LogError("clipIndex " + clipIndex + "  clips.Length " + Instance.playerYellClips.Length);
            }
            source.Play();
            //
            Destroy(gO, source.clip.length + 0.1f);
            //
        }
    }

    public  void Shake(float amt, float length)
    {
       /* if (camTransform == null)
        {
            camTransform = Camera.main;
        }

        
        if (Instance.mfCam == null)
        {
            mfCam = camTransform.transform.root.GetComponent<MultiFighterCamera>();
        }*/

        if(Instance.mfCam.enabled)
        {
            amt = amt / 2000f;
            
        }
        /*if(Instance.f3dCam.enabled)
        {
            amt = amt / 350f;
           
        }*/
        Instance.shakeAmount = amt;

        Instance.InvokeRepeating("BeginShake", 0, 0.01f);
        Instance.Invoke("StopShake", length);
        //CameraShaker.Instance.ShakeOnce(Instance.Magnitude, Instance.Roughness, 0, Instance.FadeOutTime);


    }

    public void ShakeCam(float duration, float magnitude)
    {

        IEnumerator coroutine = ShakeCamCoRoutine(duration, magnitude);
        StartCoroutine(coroutine);
    }
    public IEnumerator ShakeCamCoRoutine(float duration, float magnitude)
    {
        Vector3 camPos = camTransform.transform.position;
        Vector3 velocity = Vector3.zero;
        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            //Lerp(enemyDamageImage.color, Color.clear, enemyFlashSpeed * Time.deltaTime);
            camTransform.transform.position = Vector3.SmoothDamp(camTransform.transform.position, new Vector3(camPos.x + x, camPos.y + y, camPos.z), ref velocity, (elapsed / duration) * Time.deltaTime);
            //camTransform.transform.position = Vector3.Lerp(camTransform.transform.position, new Vector3(camPos.x + x, camPos.y + y, camPos.z), (elapsed / duration));
            //camTransform.transform.position = new Vector3(camPos.x + x, camPos.y + y, camPos.z);

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        camTransform.transform.localPosition = Vector3.zero;

    }
    void BeginShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = camTransform.transform.position;

            float shakeAmtX = Random.value * shakeAmount * 2 - shakeAmount;
            float shakeAmtY = Random.value * shakeAmount * 2 - shakeAmount;

            camPos.x += shakeAmtX;
            camPos.y += shakeAmtY;

            camTransform.transform.position = camPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
        if (Instance.mfCam.enabled)
        {
            camTransform.transform.localPosition = Vector3.zero;
        }
        /*if (Instance.f3dCam.enabled)
        {

        }*/
    }

}
