using UnityEngine;
using System.Collections;
using UnityEditor;
using garagekitgames;
using SO;
using System.Linq;
using DamageEffect;
using System;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterCreationScript : ScriptableWizard
{

    /*public Texture2D portraitTexture;
    public Color color = Color.white;
    public string nickname = "Default nickname";*/

    private garagekitgames.BodyPart Hip;
    private garagekitgames.BodyPart Chest;
    private garagekitgames.BodyPart Head;
    private garagekitgames.BodyPart Arm_L;
    private garagekitgames.BodyPart LArm_L;
    private garagekitgames.BodyPart Hand_L;
    private garagekitgames.BodyPart Arm_R;
    private garagekitgames.BodyPart LArm_R;
    private garagekitgames.BodyPart Hand_R;
    private garagekitgames.BodyPart Thigh_L;
    private garagekitgames.BodyPart Leg_L;
    private garagekitgames.BodyPart Foot_L;
    private garagekitgames.BodyPart Thigh_R;
    private garagekitgames.BodyPart Leg_R;
    private garagekitgames.BodyPart Foot_R;

    private string hipName = "hip";
    private string chestName = "chest";
    private string headName = "head";
    private string larmName = "Arm_L";
    private string lfarmName = "Elbow_L";
    private string rarmName = "Arm_R";
    private string rfarmName = "Elbow_R";
    private string lhandName = "Hand_L";
    private string rhandName = "Hand_R";
    private string lthighName = "Thigh_L";
    private string rthighName = "Thigh_R";
    private string llegName = "Leg_L";
    private string rlegName = "Leg_R";
    private string lfootName = "Foot_L";
    private string rfootName = "Foot_R";

    private CharacterRuntimeSet defaultPlayerRuntimeSet;

    private CharacterAction[] defaultLoosePlayerCharacterActions = new CharacterAction[8];
    private CharacterAction[] defaultTightPlayerCharacterActions = new CharacterAction[8];

    private AttackData[] defaultLoosePlayerCharacterAttacks = new AttackData[8];
    private AttackData[] defaultTightPlayerCharacterAttacks = new AttackData[8];

    private LegsConfig defaultLegConfig;

    public GameObject character;

    public GameObject healthIndicator;

    public FloatReference currenthealth;

    public FloatReference maxhealth;

    public float massMultiplier = 1;

    private TransformVariable targetPoint;
    private TransformVariable cashTargetPoint;

    public bool changeJointLimits;

    public GameObject windUpPrefab;
    public AudioClip[] windUpAudioClips;

    public Material damageEffectMaterial;

    public GameEvent playerDamaged;
    public GameEvent playerDead;
    public GameEvent playerHit;
    public GameEvent playerMiss;
    public GameEvent playerVanished;

    public GameObject aimUIGO;

    public Color fullAimColor;
    public Color startAimColor;

    private void OnEnable()
    {
        Hip = (garagekitgames.BodyPart)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/BodyPart/Hip.asset", typeof(garagekitgames.BodyPart));
        Chest = (garagekitgames.BodyPart)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/BodyPart/Chest.asset", typeof(garagekitgames.BodyPart));
        Head = (garagekitgames.BodyPart)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/BodyPart/Head.asset", typeof(garagekitgames.BodyPart));
        Arm_L = (garagekitgames.BodyPart)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/BodyPart/Arm_L.asset", typeof(garagekitgames.BodyPart));
        LArm_L = (garagekitgames.BodyPart)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/BodyPart/LArm_L.asset", typeof(garagekitgames.BodyPart));
        Hand_L = (garagekitgames.BodyPart)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/BodyPart/Hand_L.asset", typeof(garagekitgames.BodyPart));
        Arm_R = (garagekitgames.BodyPart)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/BodyPart/Arm_R.asset", typeof(garagekitgames.BodyPart));
        LArm_R = (garagekitgames.BodyPart)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/BodyPart/LArm_R.asset", typeof(garagekitgames.BodyPart));
        Hand_R = (garagekitgames.BodyPart)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/BodyPart/Hand_R.asset", typeof(garagekitgames.BodyPart));
        Thigh_L = (garagekitgames.BodyPart)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/BodyPart/Thigh_L.asset", typeof(garagekitgames.BodyPart));
        Leg_L = (garagekitgames.BodyPart)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/BodyPart/Leg_L.asset", typeof(garagekitgames.BodyPart));
        Foot_L = (garagekitgames.BodyPart)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/BodyPart/Foot_L.asset", typeof(garagekitgames.BodyPart));
        Thigh_R = (garagekitgames.BodyPart)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/BodyPart/Thigh_R.asset", typeof(garagekitgames.BodyPart));
        Leg_R = (garagekitgames.BodyPart)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/BodyPart/Leg_R.asset", typeof(garagekitgames.BodyPart));
        Foot_R = (garagekitgames.BodyPart)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/BodyPart/Foot_R.asset", typeof(garagekitgames.BodyPart));



        defaultPlayerRuntimeSet = (CharacterRuntimeSet)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Sets/PlayerCharacterRuntimeSet.asset", typeof(CharacterRuntimeSet));

        //Default Actions 
        defaultLoosePlayerCharacterActions[0] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerCharacterMoveActionInput.asset", typeof(CharacterAction));
        defaultLoosePlayerCharacterActions[1] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerCharacterMoveActionOutput.asset", typeof(CharacterAction));
        defaultLoosePlayerCharacterActions[2] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerCharacterLegsInput.asset", typeof(CharacterAction));
        defaultLoosePlayerCharacterActions[3] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerSetDirectionOutputLooseGuy.asset", typeof(CharacterAction));
        defaultLoosePlayerCharacterActions[4] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerCharacterAttackInput.asset", typeof(CharacterAction));
        defaultLoosePlayerCharacterActions[5] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerCharacterAttackOutput.asset", typeof(CharacterAction));
        defaultLoosePlayerCharacterActions[6] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerCharacterSetTargetTouchInput.asset", typeof(CharacterAction));
        defaultLoosePlayerCharacterActions[7] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerCharacterSetDirectionInput.asset", typeof(CharacterAction));

        defaultTightPlayerCharacterActions[0] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerCharacterMoveActionInput.asset", typeof(CharacterAction));
        defaultTightPlayerCharacterActions[1] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerCharacterMoveActionOutput.asset", typeof(CharacterAction));
        defaultTightPlayerCharacterActions[2] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerCharacterLegsInput.asset", typeof(CharacterAction));
        defaultTightPlayerCharacterActions[3] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerSetDirectionOutput.asset", typeof(CharacterAction));
        defaultTightPlayerCharacterActions[4] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerCharacterAttackInput.asset", typeof(CharacterAction));
        defaultTightPlayerCharacterActions[5] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerCharacterAttackOutput.asset", typeof(CharacterAction));
        defaultTightPlayerCharacterActions[6] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerCharacterSetTargetTouchInput.asset", typeof(CharacterAction));
        defaultTightPlayerCharacterActions[7] = (CharacterAction)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Actions/NewActions/PlayerCharacterSetDirectionInput.asset", typeof(CharacterAction));


        //Default Attacks
        defaultLoosePlayerCharacterAttacks[0] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/LooseGuy/BasicLeftKick 1.asset", typeof(AttackData));
        defaultLoosePlayerCharacterAttacks[1] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/LooseGuy/BasicLeftKickFront 1.asset", typeof(AttackData));
        defaultLoosePlayerCharacterAttacks[2] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/LooseGuy/BasicRightKick 1.asset", typeof(AttackData));
        defaultLoosePlayerCharacterAttacks[3] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/LooseGuy/BasicRightKickFront 1.asset", typeof(AttackData));
        defaultLoosePlayerCharacterAttacks[4] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/LooseGuy/BasicLeftPunch 1.asset", typeof(AttackData));
        defaultLoosePlayerCharacterAttacks[5] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/LooseGuy/BasicRightPunch 1.asset", typeof(AttackData));
        defaultLoosePlayerCharacterAttacks[6] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/LooseGuy/BasicLeftPunchFromHip 1.asset", typeof(AttackData));
        defaultLoosePlayerCharacterAttacks[7] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/LooseGuy/BasicRightPunchFromHip 1.asset", typeof(AttackData));

        defaultTightPlayerCharacterAttacks[0] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/FastTightGuy/BasicLeftKick.asset", typeof(AttackData));
        defaultTightPlayerCharacterAttacks[1] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/FastTightGuy/BasicLeftKickFront.asset", typeof(AttackData));
        defaultTightPlayerCharacterAttacks[2] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/FastTightGuy/BasicRightKick.asset", typeof(AttackData));
        defaultTightPlayerCharacterAttacks[3] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/FastTightGuy/BasicRightKickFront.asset", typeof(AttackData));
        defaultTightPlayerCharacterAttacks[4] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/FastTightGuy/BasicLeftPunch.asset", typeof(AttackData));
        defaultTightPlayerCharacterAttacks[5] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/FastTightGuy/BasicRightPunch.asset", typeof(AttackData));
        defaultTightPlayerCharacterAttacks[6] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/FastTightGuy/BasicLeftPunchFromHip.asset", typeof(AttackData));
        defaultTightPlayerCharacterAttacks[7] = (AttackData)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/AttackData/FastTightGuy/BasicRightPunchFromHip.asset", typeof(AttackData));

        //Leg Config
        defaultLegConfig = (LegsConfig)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/LegConfigs/LegsConfig20KG.asset", typeof(LegsConfig));


        //target point
        targetPoint = (TransformVariable)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Variables/PlayerTransform.asset", typeof(TransformVariable));

        //cash target point
        cashTargetPoint = (TransformVariable)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Variables/CashTarget.asset", typeof(TransformVariable));

        //health = (FloatVariable)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Variables/PlayerMaxHealth.asset", typeof(FloatVariable));
        //windUpPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/ParticleEffects/CFX4 Rays 1.prefab", typeof(GameObject));

        //C:\Users\RICHARD\Documents\barbrawl\Assets/EffectCore/packs/StylizedProjectilePack2/prefabs/Flashy/GoldFire/Small/Flashy_GoldFire_Small_Impact.prefab
        windUpPrefab = AssetDatabase.LoadAssetAtPath("Assets/EffectCore/packs/StylizedProjectilePack2/prefabs/Flashy/GoldFire/Small/Flashy_GoldFire_Small_Impact.prefab", (typeof(GameObject))) as GameObject;

        //_MyStuff\Fight SFX PAck\Fight SFX Pack\windup
        windUpAudioClips = new AudioClip[1] { (AudioClip)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Fight SFX PAck/Fight SFX Pack/windup/sound_design_effect_electricity_electric_arc.wav", typeof(AudioClip)) };
        //BurnBlink3D.mat nityProjects\PhysicsFighter\Physics Fighter\Assets\DamageEffect\Materials\3D
        damageEffectMaterial = (Material)AssetDatabase.LoadAssetAtPath("Assets/DamageEffect/Materials/3D/BurnBlink3D.mat", typeof(Material));

        playerDamaged = new GameEvent();
        playerDead = new GameEvent();
        playerHit = new GameEvent();
        playerMiss = new GameEvent();
        playerVanished = new GameEvent();

        playerDamaged = (GameEvent)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Event/PlayerDamaged.asset", typeof(GameEvent));
        playerDead = (GameEvent)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Event/PlayerDead.asset", typeof(GameEvent));
        playerHit = (GameEvent)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Event/PlayerHit.asset", typeof(GameEvent));
        playerMiss = (GameEvent)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Event/PlayerMiss.asset", typeof(GameEvent));
        playerVanished = (GameEvent)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Event/PlayerVanished.asset", typeof(GameEvent));
        /*public GameEvent playerDamaged;
   public GameEvent playerDead;
   public GameEvent playerHit;
   public GameEvent playerMiss;*/

        aimUIGO = AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Prefab/OtherUtilityItems/Canvas.prefab", (typeof(GameObject))) as GameObject;
        Color color;
        /*if (ColorUtility.TryParseHtmlString("#09FFB200", out color))
        { fullAimColor = color; }

        if (ColorUtility.TryParseHtmlString("#09FFFFFF", out color))
        { fullAimColor = color; }*/
        fullAimColor = new Color(1, 0.6994327f, 0, 1);
        startAimColor = new Color(1, 1, 1, 0);
    }


    [MenuItem("My Tools/Create Character Wizard")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<CharacterCreationScript>("Create Character", "Create Loose Player", "Create Tight Player");
    }

    void OnWizardCreate()
    {
        GameObject characterGO = character;


        var aimSliderGO = Instantiate(aimUIGO, characterGO.transform);

        Slider aimSlider = aimSliderGO.GetComponentInChildren<Slider>();
        Image aimImage = aimSliderGO.GetComponentInChildren<Image>();

        if (!characterGO.transform.GetComponent<CharacterAttackSliderUI>())
        {
            CharacterAttackSliderUI characterAttackSliderUI = characterGO.AddComponent<CharacterAttackSliderUI>() as CharacterAttackSliderUI;
            characterAttackSliderUI.m_AimSlider = aimSlider;
            characterAttackSliderUI.m_FillImage = aimImage;
            characterAttackSliderUI.m_FullHealthColor = fullAimColor;
            characterAttackSliderUI.m_ZeroHealthColor = startAimColor;

        }

        if (!characterGO.transform.GetComponent<CharacterUI>())
        {
            CharacterUI characterUI = characterGO.AddComponent<CharacterUI>() as CharacterUI;
            characterUI.playerRootPart = Hip;
            //characterAttackSliderUI.m_FillImage = aimImage;

        }


        CharacterJoint[] charJoints = characterGO.GetComponentsInChildren<CharacterJoint>();
        Transform[] childTransforms = characterGO.GetComponentsInChildren<Transform>();

        if (!characterGO.transform.GetComponent<CharacterInput>())
        {
            CharacterInput characterInput = characterGO.AddComponent<CharacterInput>() as CharacterInput;
            characterInput.controllerID = 0;
        }
        if (!characterGO.transform.GetComponent<CharacterThinker>())
        {
            CharacterThinker characterThinker = characterGO.AddComponent<CharacterThinker>() as CharacterThinker;
            characterThinker.canBeHurt = true;
            characterThinker.timeBetweenGettingHurt = 1;
            characterThinker.stopDoingShit = false;
            characterThinker.stopAttacking = false;
            characterThinker.stopLooking = false;
            characterThinker.targetting = true;
            characterThinker.enableDrag = true;
            characterThinker.teamID = 0;
            characterThinker.amIMainPlayer = true;
            characterThinker.RuntimeSet = defaultPlayerRuntimeSet;
            characterThinker.actions = defaultLoosePlayerCharacterActions.ToList<CharacterAction>();
            characterThinker.attacks = defaultLoosePlayerCharacterAttacks.ToList<AttackData>();
            characterThinker.legConfig = defaultLegConfig;
            


        }
        if (!characterGO.transform.GetComponent<CharacterBodyPartHolder>())
        {
            CharacterBodyPartHolder characterBPHolder = characterGO.AddComponent<CharacterBodyPartHolder>() as CharacterBodyPartHolder;
        }
        if (!characterGO.transform.GetComponent<CharacterLegsSimple1>())
        {
            CharacterLegsSimple1 characterLegs = characterGO.AddComponent<CharacterLegsSimple1>() as CharacterLegsSimple1;
        }

        

        if (!characterGO.transform.GetComponent<DamageEffectScript>())
        {
            DamageEffectScript damageEffectScript = characterGO.AddComponent<DamageEffectScript>() as DamageEffectScript;
            damageEffectScript._damageMaterial = damageEffectMaterial;
            //characterHealth.currentHP.UseConstant = false;
            //characterHealth.currentHP = currenthealth;
            //characterHealth.maxHP = maxhealth;
        }

        if (!characterGO.transform.GetComponent<CharacterLimpSetting>())
        {
            CharacterLimpSetting characterLimpSetting = characterGO.AddComponent<CharacterLimpSetting>() as CharacterLimpSetting;
            characterLimpSetting.bodyParts = new List<garagekitgames.BodyPart> () { Hip, Chest, Head };
            characterLimpSetting.bodyPartToFixConstraints = Hip;
            characterLimpSetting.looseCharacter = true; 
        }


        if (!characterGO.transform.GetComponent<DiableAfterXSeconds>())
        {
            DiableAfterXSeconds disableScript = characterGO.AddComponent<DiableAfterXSeconds>() as DiableAfterXSeconds;
            //disableScript.afterDeathEvent.AddListener(delegate { playerVanished.Raise(); });
            //UnityAction<GameObject> callback = new UnityAction<GameObject>(playerVanished.Raise);
           // UnityEditor.Events.UnityEventTools.AddPersistentListener(disableScript.afterDeathEvent, new UnityAction(playerVanished.Raise));

            /*characterLimpSetting.bodyParts = new List<garagekitgames.BodyPart>() { Hip, Chest, Head };
            characterLimpSetting.bodyPartToFixConstraints = Hip;
            characterLimpSetting.looseCharacter = true;*/
        }

        if (!characterGO.transform.GetComponent<ResetCharacterRoot>())
        {
            ResetCharacterRoot resetRoot = characterGO.AddComponent<ResetCharacterRoot>() as ResetCharacterRoot;
            resetRoot.rootPart = Hip;
            /*characterLimpSetting.bodyParts = new List<garagekitgames.BodyPart>() { Hip, Chest, Head };
            characterLimpSetting.bodyPartToFixConstraints = Hip;
            characterLimpSetting.looseCharacter = true;*/
        }

        if (!characterGO.transform.GetComponent<CharacterHitMissTracker>())
        {
            CharacterHitMissTracker hitMissTracker = characterGO.AddComponent<CharacterHitMissTracker>() as CharacterHitMissTracker;
            hitMissTracker.windUpPrefab = windUpPrefab;
            hitMissTracker.windupclips = windUpAudioClips;

            //hitMissTracker.playerHitEvent.AddListener(delegate { playerHit.Raise(); });
           // hitMissTracker.playerMissEvent.AddListener(delegate { playerMiss.Raise(); });
            /*characterLimpSetting.bodyParts = new List<garagekitgames.BodyPart>() { Hip, Chest, Head };
            characterLimpSetting.bodyPartToFixConstraints = Hip;
            characterLimpSetting.looseCharacter = true;*/
        }

        if (!characterGO.transform.GetComponent<CharacterHealth>())
        {
            CharacterHealth characterHealth = characterGO.AddComponent<CharacterHealth>() as CharacterHealth;
            CharacterThinker characterThinker = characterGO.transform.GetComponent<CharacterThinker>();
            CharacterLimpSetting characterLimpSetting = characterGO.GetComponent<CharacterLimpSetting>() as CharacterLimpSetting;
            if (!characterGO.transform.GetComponent<CharacterLimpSetting>())
            {
                characterLimpSetting = characterGO.AddComponent<CharacterLimpSetting>() as CharacterLimpSetting;
                characterLimpSetting.bodyParts = new List<garagekitgames.BodyPart>() { Hip, Chest, Head };
                characterLimpSetting.bodyPartToFixConstraints = Hip;
                characterLimpSetting.looseCharacter = true;
            }
            ResetCharacterRoot resetRoot = characterGO.GetComponent<ResetCharacterRoot>() as ResetCharacterRoot;
            //characterHealth.currentHP.UseConstant = false;
            characterHealth.currentHP = currenthealth;
            characterHealth.maxHP = maxhealth;
            characterHealth.alive = true;
            characterHealth.ResetHP = true;
            //characterHealth.DeathEvent.AddListener(delegate { characterThinker.SetStopDoingShit(true); });
            // characterHealth.DeathEvent.AddListener(delegate { playerDead.Raise(); });
            // characterHealth.DeathEvent.AddListener(delegate { characterLimpSetting.goLimp(); });
            //  characterHealth.DamageEvent.AddListener(delegate { playerDamaged.Raise(); });

            //UnityAction<CharacterLimpSetting> callback = new UnityAction<CharacterLimpSetting>(characterLimpSetting.goLimp);
            //UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(characterHealth.DeathEvent, new UnityAction(characterLimpSetting.goLimp));


            // characterHealth.OnEnableEvent.AddListener(delegate { characterThinker.SetStopDoingShit(false); });
            //  characterHealth.OnEnableEvent.AddListener(delegate { characterLimpSetting.resetLimp(); });
            //  characterHealth.OnEnableEvent.AddListener(delegate { resetRoot.resetRoot(); });
        }
        if (!characterGO.transform.GetComponent<CharacterAttackSliderUI>())
        {
            CharacterAttackSliderUI powMeter = characterGO.AddComponent<CharacterAttackSliderUI>() as CharacterAttackSliderUI;
            //resetRoot.rootPart = Hip;
            /*characterLimpSetting.bodyParts = new List<garagekitgames.BodyPart>() { Hip, Chest, Head };
            characterLimpSetting.bodyPartToFixConstraints = Hip;
            characterLimpSetting.looseCharacter = true;*/
        }
        //  BodyPart Setup
        foreach (Transform childTransform in childTransforms)
        {

            if (childTransform.name.ToLower().Contains(hipName.ToLower()))
            {
                childTransform.gameObject.layer = 9;
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Hip;
                }
                if (!childTransform.gameObject.GetComponent<CollisionCheck>())
                {
                    CollisionCheck cCheck = childTransform.gameObject.AddComponent<CollisionCheck>();
                    //cCheck.BodyPart = Hip;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 3.125f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }



            }
            if (childTransform.name.ToLower().Contains(chestName.ToLower()) )
            {

                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Chest;
                }
                if (!childTransform.gameObject.GetComponent<CollisionCheck>())
                {
                    CollisionCheck cCheck = childTransform.gameObject.AddComponent<CollisionCheck>();
                    //cCheck.BodyPart = Hip;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 3.125f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
                if (!childTransform.gameObject.GetComponent<CharacterFaceDirection>())
                {
                    CharacterFaceDirection cFD = childTransform.gameObject.AddComponent<CharacterFaceDirection>();
                    cFD.bodyForward.z = 1;
                    cFD.facingForce = 50;
                    cFD.leadTime = 0;
                    //cCheck.BodyPart = Hip;
                }
                if (!childTransform.gameObject.GetComponent<CharacterMaintainHeight>())
                {
                    CharacterMaintainHeight cMH = childTransform.gameObject.AddComponent<CharacterMaintainHeight>();
                    cMH.desiredHeight = 2;
                    cMH.pullUpForce = 800;
                    cMH.leadTime = 0.3f;
                    cMH.period = 1;
                    //cCheck.BodyPart = Hip;
                }
                if (!childTransform.gameObject.GetComponent<CharacterUpright>())
                {
                    CharacterUpright cUR = childTransform.gameObject.AddComponent<CharacterUpright>();
                    cUR.keepUpright = true;
                    cUR.uprightForce = 0;
                    cUR.uprightOffset = 2;
                    cUR.additionalUpwardForce = 0;
                    //cCheck.BodyPart = Hip;
                }
                if (!childTransform.gameObject.GetComponent<SetPlayerAsTarget>())
                {
                    SetPlayerAsTarget characterTargetPoint = childTransform.gameObject.AddComponent<SetPlayerAsTarget>() as SetPlayerAsTarget;
                    characterTargetPoint.target = cashTargetPoint;
                }
            }
            if (childTransform.name.ToLower().Contains(headName.ToLower()) )
            {
                childTransform.tag = "Player";
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Head;
                }
                if (!childTransform.gameObject.GetComponent<CollisionCheck>())
                {
                    CollisionCheck cCheck = childTransform.gameObject.AddComponent<CollisionCheck>();
                    //cCheck.BodyPart = Hip;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1.25f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
                if (!childTransform.gameObject.GetComponent<SetPlayerAsTarget>())
                {
                    SetPlayerAsTarget characterTargetPoint = childTransform.gameObject.AddComponent<SetPlayerAsTarget>() as SetPlayerAsTarget;
                    characterTargetPoint.target = targetPoint;
                }
            }
            if (childTransform.name.ToLower().Contains(larmName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Arm_L;
                }

                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(rarmName.ToLower()) )
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Arm_R;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(lfarmName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = LArm_L;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 0.8f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(rfarmName.ToLower()) )
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = LArm_R;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 0.8f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(lhandName.ToLower())  )
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Hand_L;
                }
                if (!childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.AddComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                else if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(rhandName.ToLower())   )
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Hand_R;
                }
                if (!childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.AddComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                else if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(lthighName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Thigh_L;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1.875f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(rthighName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Thigh_R;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1.875f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(llegName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Leg_L;
                }
                if (!childTransform.gameObject.GetComponent<CharacterMaintainHeight>())
                {
                    CharacterMaintainHeight cMH = childTransform.gameObject.AddComponent<CharacterMaintainHeight>();
                    cMH.desiredHeight = 0.55f;
                    cMH.pullUpForce = 120;
                    cMH.leadTime = 0.2f;
                    cMH.period = 1;
                    //cCheck.BodyPart = Hip;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1.875f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(rlegName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Leg_R;
                }
                if (!childTransform.gameObject.GetComponent<CharacterMaintainHeight>())
                {
                    CharacterMaintainHeight cMH = childTransform.gameObject.AddComponent<CharacterMaintainHeight>();
                    cMH.desiredHeight = 0.55f;
                    cMH.pullUpForce = 120;
                    cMH.leadTime = 0.2f;
                    cMH.period = 1;
                    //cCheck.BodyPart = Hip;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1.875f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(lfootName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Foot_L;
                }
                if (!childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.AddComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                else if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }

            }
            if (childTransform.name.ToLower().Contains(rfootName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Foot_R;
                }
                if (!childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.AddComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                else if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }

                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }

            }

        }


        foreach (CharacterJoint charJoint in charJoints)
        {
            ConfigurableJoint confJoint;
            if (!charJoint.transform.GetComponent<ConfigurableJoint>())
            {

                confJoint = charJoint.gameObject.AddComponent<ConfigurableJoint>() as ConfigurableJoint;
                //				confJoint.autoConfigureConnectedAnchor = false;
                confJoint.connectedBody = charJoint.connectedBody;
                confJoint.anchor = charJoint.anchor;
                confJoint.axis = charJoint.axis;
                //				confJoint.connectedAnchor = charJoint.connectedAnchor;
                confJoint.secondaryAxis = charJoint.swingAxis;
                confJoint.xMotion = ConfigurableJointMotion.Locked;
                confJoint.yMotion = ConfigurableJointMotion.Locked;
                confJoint.zMotion = ConfigurableJointMotion.Locked;
                confJoint.angularXMotion = ConfigurableJointMotion.Limited;
                confJoint.angularYMotion = ConfigurableJointMotion.Limited;
                confJoint.angularZMotion = ConfigurableJointMotion.Limited;
                confJoint.lowAngularXLimit = charJoint.lowTwistLimit;
                confJoint.highAngularXLimit = charJoint.highTwistLimit;
                confJoint.angularYLimit = charJoint.swing1Limit;
                confJoint.angularZLimit = charJoint.swing2Limit;
                confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                confJoint.enablePreprocessing = false;
                confJoint.projectionDistance = 0.1f;
                confJoint.projectionAngle = 180f;
                confJoint.projectionMode = JointProjectionMode.PositionAndRotation;
                //				JointDrive temp = confJoint.slerpDrive; // These are left here to remind us how to set the drive
                //				temp.mode = JointDriveMode.Position;
                //				temp.positionSpring = 0f;
                //				confJoint.slerpDrive = temp;
                //				confJoint.targetRotation = Quaternion.identity;
            }
            DestroyImmediate(charJoint);
        }

        foreach (Transform childTransform in childTransforms)
        {

            if (childTransform.name.ToLower().Contains(hipName.ToLower()))
            {
                /*if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Hip;
                }*/


            }
            if (childTransform.name.ToLower().Contains(chestName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -30f;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 30f;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 30f;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 30f;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z; 
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(headName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -50f;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 50f;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 50f;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 60f;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z; 
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 1000;
                    x.positionDamper = 100;
                    x.maximumForce = 10;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(larmName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -100;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 60;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 100;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 50;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z; 
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(rarmName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -60;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 100;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 100;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 50;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z; 
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(lfarmName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -150;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 20;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 20;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 30;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z; 
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(rfarmName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -20;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 150;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 20;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 30;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z; 
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(lhandName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = 0;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 0;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 0;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 0;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z; 
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(rhandName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = 0;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 0;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 0;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 0;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z; 
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(lthighName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -20;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 130;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 65;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 40;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z; 
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(rthighName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -20;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 130;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 65;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 40;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z; 
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(llegName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -177;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 0;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 5;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 5;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z; 
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(rlegName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -177;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 0;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 5;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 5;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z; 
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(lfootName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = 0;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 0;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 0;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 0;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z; 
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }

            }
            if (childTransform.name.ToLower().Contains(rfootName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = 0;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 0;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 0;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 0;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z; 
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }

            }

        }
        //Debug.Log("Replaced " + i + " CharacterJoints with ConfigurableJoints on " + this.name);


        /*Character characterComponent = characterGO.AddComponent<Character>();
        characterComponent.portrait = portraitTexture;
        characterComponent.color = color;
        characterComponent.nickname = nickname;

        PlayerMovement playerMovement = characterGO.AddComponent<PlayerMovement>();
        characterComponent.playerMovement = playerMovement;*/
    }

    void OnWizardOtherButton()
    {
        GameObject characterGO = character;

        var aimSliderGO = Instantiate(aimUIGO, characterGO.transform);

        Slider aimSlider = aimSliderGO.GetComponentInChildren<Slider>();
        Image aimImage = aimSliderGO.GetComponentInChildren<Image>();

        if (!characterGO.transform.GetComponent<CharacterAttackSliderUI>())
        {
            CharacterAttackSliderUI characterAttackSliderUI = characterGO.AddComponent<CharacterAttackSliderUI>() as CharacterAttackSliderUI;
            characterAttackSliderUI.m_AimSlider = aimSlider;
            characterAttackSliderUI.m_FillImage = aimImage;
            characterAttackSliderUI.m_FullHealthColor = fullAimColor;
            characterAttackSliderUI.m_ZeroHealthColor = startAimColor;

        }

        if (!characterGO.transform.GetComponent<CharacterUI>())
        {
            CharacterUI characterUI = characterGO.AddComponent<CharacterUI>() as CharacterUI;
            characterUI.playerRootPart = Hip;
            //characterAttackSliderUI.m_FillImage = aimImage;

        }

        CharacterJoint[] charJoints = characterGO.GetComponentsInChildren<CharacterJoint>();
        Transform[] childTransforms = characterGO.GetComponentsInChildren<Transform>();

        if (!characterGO.transform.GetComponent<CharacterInput>())
        {
            CharacterInput characterInput = characterGO.AddComponent<CharacterInput>() as CharacterInput;
            characterInput.controllerID = 0;
        }
        if (!characterGO.transform.GetComponent<CharacterThinker>())
        {
            CharacterThinker characterThinker = characterGO.AddComponent<CharacterThinker>() as CharacterThinker;
            characterThinker.canBeHurt = true;
            characterThinker.timeBetweenGettingHurt = 1;
            characterThinker.stopDoingShit = false;
            characterThinker.stopAttacking = false;
            characterThinker.stopLooking = false;
            characterThinker.targetting = true;
            characterThinker.enableDrag = true;
            characterThinker.teamID = 0;
            characterThinker.amIMainPlayer = true;
            characterThinker.RuntimeSet = defaultPlayerRuntimeSet;
            characterThinker.actions = defaultTightPlayerCharacterActions.ToList<CharacterAction>();
            characterThinker.attacks = defaultTightPlayerCharacterAttacks.ToList<AttackData>();
            characterThinker.legConfig = defaultLegConfig;



        }
        if (!characterGO.transform.GetComponent<CharacterBodyPartHolder>())
        {
            CharacterBodyPartHolder characterBPHolder = characterGO.AddComponent<CharacterBodyPartHolder>() as CharacterBodyPartHolder;
        }
        if (!characterGO.transform.GetComponent<CharacterLegsSimple1>())
        {
            CharacterLegsSimple1 characterLegs = characterGO.AddComponent<CharacterLegsSimple1>() as CharacterLegsSimple1;
        }



        if (!characterGO.transform.GetComponent<DamageEffectScript>())
        {
            DamageEffectScript damageEffectScript = characterGO.AddComponent<DamageEffectScript>() as DamageEffectScript;
            damageEffectScript._damageMaterial = damageEffectMaterial;
            //characterHealth.currentHP.UseConstant = false;
            //characterHealth.currentHP = currenthealth;
            //characterHealth.maxHP = maxhealth;
        }

        if (!characterGO.transform.GetComponent<CharacterLimpSetting>())
        {
            CharacterLimpSetting characterLimpSetting = characterGO.AddComponent<CharacterLimpSetting>() as CharacterLimpSetting;
            characterLimpSetting.bodyParts = new List<garagekitgames.BodyPart>() { Hip, Chest, Head };
            characterLimpSetting.bodyPartToFixConstraints = Hip;
            characterLimpSetting.resetHipRotation = true;
            characterLimpSetting.looseCharacter = false;
        }


        if (!characterGO.transform.GetComponent<DiableAfterXSeconds>())
        {
            DiableAfterXSeconds disableScript = characterGO.AddComponent<DiableAfterXSeconds>() as DiableAfterXSeconds;
            //disableScript.afterDeathEvent.AddListener(delegate { playerVanished.Raise(); });
            //UnityAction<GameObject> callback = new UnityAction<GameObject>(playerVanished.Raise);
            // UnityEditor.Events.UnityEventTools.AddPersistentListener(disableScript.afterDeathEvent, new UnityAction(playerVanished.Raise));

            /*characterLimpSetting.bodyParts = new List<garagekitgames.BodyPart>() { Hip, Chest, Head };
            characterLimpSetting.bodyPartToFixConstraints = Hip;
            characterLimpSetting.looseCharacter = true;*/
        }

        if (!characterGO.transform.GetComponent<ResetCharacterRoot>())
        {
            ResetCharacterRoot resetRoot = characterGO.AddComponent<ResetCharacterRoot>() as ResetCharacterRoot;
            resetRoot.rootPart = Hip;
            /*characterLimpSetting.bodyParts = new List<garagekitgames.BodyPart>() { Hip, Chest, Head };
            characterLimpSetting.bodyPartToFixConstraints = Hip;
            characterLimpSetting.looseCharacter = true;*/
        }

        if (!characterGO.transform.GetComponent<CharacterHitMissTracker>())
        {
            CharacterHitMissTracker hitMissTracker = characterGO.AddComponent<CharacterHitMissTracker>() as CharacterHitMissTracker;
            hitMissTracker.windUpPrefab = windUpPrefab;
            hitMissTracker.windupclips = windUpAudioClips;

            //hitMissTracker.playerHitEvent.AddListener(delegate { playerHit.Raise(); });
            // hitMissTracker.playerMissEvent.AddListener(delegate { playerMiss.Raise(); });
            /*characterLimpSetting.bodyParts = new List<garagekitgames.BodyPart>() { Hip, Chest, Head };
            characterLimpSetting.bodyPartToFixConstraints = Hip;
            characterLimpSetting.looseCharacter = true;*/
        }

        if (!characterGO.transform.GetComponent<CharacterHealth>())
        {
            CharacterHealth characterHealth = characterGO.AddComponent<CharacterHealth>() as CharacterHealth;
            CharacterThinker characterThinker = characterGO.transform.GetComponent<CharacterThinker>();
            CharacterLimpSetting characterLimpSetting = characterGO.GetComponent<CharacterLimpSetting>() as CharacterLimpSetting;
            if (!characterGO.transform.GetComponent<CharacterLimpSetting>())
            {
                characterLimpSetting = characterGO.AddComponent<CharacterLimpSetting>() as CharacterLimpSetting;
                characterLimpSetting.bodyParts = new List<garagekitgames.BodyPart>() { Hip, Chest, Head };
                characterLimpSetting.bodyPartToFixConstraints = Hip;
                characterLimpSetting.looseCharacter = true;
            }
            ResetCharacterRoot resetRoot = characterGO.GetComponent<ResetCharacterRoot>() as ResetCharacterRoot;
            //characterHealth.currentHP.UseConstant = false;
            characterHealth.currentHP = currenthealth;
            characterHealth.maxHP = maxhealth;
            characterHealth.alive = true;
            characterHealth.ResetHP = true;
            //characterHealth.DeathEvent.AddListener(delegate { characterThinker.SetStopDoingShit(true); });
            // characterHealth.DeathEvent.AddListener(delegate { playerDead.Raise(); });
            // characterHealth.DeathEvent.AddListener(delegate { characterLimpSetting.goLimp(); });
            //  characterHealth.DamageEvent.AddListener(delegate { playerDamaged.Raise(); });

            //UnityAction<CharacterLimpSetting> callback = new UnityAction<CharacterLimpSetting>(characterLimpSetting.goLimp);
            //UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(characterHealth.DeathEvent, new UnityAction(characterLimpSetting.goLimp));


            // characterHealth.OnEnableEvent.AddListener(delegate { characterThinker.SetStopDoingShit(false); });
            //  characterHealth.OnEnableEvent.AddListener(delegate { characterLimpSetting.resetLimp(); });
            //  characterHealth.OnEnableEvent.AddListener(delegate { resetRoot.resetRoot(); });
        }
        //  BodyPart Setup
        foreach (Transform childTransform in childTransforms)
        {

            if (childTransform.name.ToLower().Contains(hipName.ToLower()))
            {
                childTransform.gameObject.layer = 9;
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Hip;
                }
                if (!childTransform.gameObject.GetComponent<CollisionCheck>())
                {
                    CollisionCheck cCheck = childTransform.gameObject.AddComponent<CollisionCheck>();
                    //cCheck.BodyPart = Hip;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 3.125f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;

                    rBody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                }
                if (!childTransform.gameObject.GetComponent<QuadraticDrag>())
                {
                    QuadraticDrag qDrag = childTransform.gameObject.AddComponent<QuadraticDrag>();
                    qDrag.drag = 10;
                    //cCheck.BodyPart = Hip;
                }
                if (!childTransform.gameObject.GetComponent<CharacterFaceDirection>())
                {
                    CharacterFaceDirection cFD = childTransform.gameObject.AddComponent<CharacterFaceDirection>();
                    cFD.bodyForward.z = 2.6f;
                    cFD.facingForce = 200;
                    cFD.leadTime = 0;
                    //cCheck.BodyPart = Hip;
                }
                if (!childTransform.gameObject.GetComponent<CharacterMaintainHeight>())
                {
                    CharacterMaintainHeight cMH = childTransform.gameObject.AddComponent<CharacterMaintainHeight>();
                    cMH.desiredHeight = 1.2f;
                    cMH.pullUpForce = 1000;
                    cMH.leadTime = 0;
                    cMH.period = 1;
                    //cCheck.BodyPart = Hip;
                }
                if (!childTransform.gameObject.GetComponent<CharacterUpright>())
                {
                    CharacterUpright cUR = childTransform.gameObject.AddComponent<CharacterUpright>();
                    cUR.keepUpright = true;
                    cUR.uprightForce = 1000;
                    cUR.uprightOffset = 0;
                    cUR.additionalUpwardForce = 0;
                    //cCheck.BodyPart = Hip;
                }


            }
            if (childTransform.name.ToLower().Contains(chestName.ToLower()))
            {

                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Chest;
                }
                if (!childTransform.gameObject.GetComponent<CollisionCheck>())
                {
                    CollisionCheck cCheck = childTransform.gameObject.AddComponent<CollisionCheck>();
                    //cCheck.BodyPart = Hip;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 3.125f * massMultiplier;
                    rBody.drag = 0;
                    rBody.angularDrag = 0.05f;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
                if (!childTransform.gameObject.GetComponent<CharacterFaceDirection>())
                {
                    CharacterFaceDirection cFD = childTransform.gameObject.AddComponent<CharacterFaceDirection>();
                    cFD.bodyForward.z = 1;
                    cFD.facingForce = 50;
                    cFD.leadTime = 0;
                    //cCheck.BodyPart = Hip;
                }
                /*if (!childTransform.gameObject.GetComponent<CharacterMaintainHeight>())
                {
                    CharacterMaintainHeight cMH = childTransform.gameObject.AddComponent<CharacterMaintainHeight>();
                    cMH.desiredHeight = 2;
                    cMH.pullUpForce = 800;
                    cMH.leadTime = 0.3f;
                    cMH.period = 1;
                    //cCheck.BodyPart = Hip;
                }
                if (!childTransform.gameObject.GetComponent<CharacterUpright>())
                {
                    CharacterUpright cUR = childTransform.gameObject.AddComponent<CharacterUpright>();
                    cUR.keepUpright = true;
                    cUR.uprightForce = 0;
                    cUR.uprightOffset = 2;
                    cUR.additionalUpwardForce = 0;
                    //cCheck.BodyPart = Hip;
                }*/
                if (!childTransform.gameObject.GetComponent<SetPlayerAsTarget>())
                {
                    SetPlayerAsTarget characterTargetPoint = childTransform.gameObject.AddComponent<SetPlayerAsTarget>() as SetPlayerAsTarget;
                    characterTargetPoint.target = cashTargetPoint;
                }
            }
            if (childTransform.name.ToLower().Contains(headName.ToLower()))
            {
                childTransform.tag = "Player";
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Head;
                }
                if (!childTransform.gameObject.GetComponent<CharacterFaceDirection>())
                {
                    CharacterFaceDirection cFD = childTransform.gameObject.AddComponent<CharacterFaceDirection>();
                    cFD.bodyForward.z = 1;
                    cFD.facingForce = 450;
                    cFD.leadTime = 0;
                    //cCheck.BodyPart = Hip;
                }
                if (!childTransform.gameObject.GetComponent<CollisionCheck>())
                {
                    CollisionCheck cCheck = childTransform.gameObject.AddComponent<CollisionCheck>();
                    //cCheck.BodyPart = Hip;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1.25f * massMultiplier;
                    rBody.drag = 0;
                    rBody.angularDrag = 0.05f;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
                if (!childTransform.gameObject.GetComponent<SetPlayerAsTarget>())
                {
                    SetPlayerAsTarget characterTargetPoint = childTransform.gameObject.AddComponent<SetPlayerAsTarget>() as SetPlayerAsTarget;
                    characterTargetPoint.target = targetPoint;
                }
            }
            if (childTransform.name.ToLower().Contains(larmName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Arm_L;
                }

                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 0;
                    rBody.angularDrag = 0.05f;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(rarmName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Arm_R;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 0;
                    rBody.angularDrag = 0.05f;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(lfarmName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = LArm_L;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 0.8f * massMultiplier;
                    rBody.drag = 0;
                    rBody.angularDrag = 0.05f;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(rfarmName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = LArm_R;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 0.8f * massMultiplier;
                    rBody.drag = 0;
                    rBody.angularDrag = 0.05f;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(lhandName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Hand_L;
                }
                if (!childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.AddComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 0;
                    rBody.angularDrag = 0.05f;
                }
                else if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 0;
                    rBody.angularDrag = 0.05f;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(rhandName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Hand_R;
                }
                if (!childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.AddComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 0;
                    rBody.angularDrag = 0.05f;
                }
                else if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 0;
                    rBody.angularDrag = 0.05f;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(lthighName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Thigh_L;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1.875f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(rthighName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Thigh_R;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1.875f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(llegName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Leg_L;
                }
                if (!childTransform.gameObject.GetComponent<CharacterMaintainHeight>())
                {
                    CharacterMaintainHeight cMH = childTransform.gameObject.AddComponent<CharacterMaintainHeight>();
                    cMH.desiredHeight = 0.55f;
                    cMH.pullUpForce = 120;
                    cMH.leadTime = 0.2f;
                    cMH.period = 1;
                    //cCheck.BodyPart = Hip;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1.875f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(rlegName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Leg_R;
                }
                if (!childTransform.gameObject.GetComponent<CharacterMaintainHeight>())
                {
                    CharacterMaintainHeight cMH = childTransform.gameObject.AddComponent<CharacterMaintainHeight>();
                    cMH.desiredHeight = 0.55f;
                    cMH.pullUpForce = 120;
                    cMH.leadTime = 0.2f;
                    cMH.period = 1;
                    //cCheck.BodyPart = Hip;
                }
                if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1.875f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }
            }
            if (childTransform.name.ToLower().Contains(lfootName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Foot_L;
                }
                if (!childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.AddComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                else if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }

            }
            if (childTransform.name.ToLower().Contains(rfootName.ToLower()))
            {
                if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Foot_R;
                }
                if (!childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.AddComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1 * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }
                else if (childTransform.gameObject.GetComponent<Rigidbody>())
                {
                    Rigidbody rBody = childTransform.gameObject.GetComponent<Rigidbody>();
                    //cCheck.BodyPart = Hip;
                    rBody.mass = 1f * massMultiplier;
                    rBody.drag = 1;
                    rBody.angularDrag = 1;
                }

                if (!childTransform.gameObject.GetComponent<JointFix>())
                {
                    JointFix jointFix = childTransform.gameObject.AddComponent<JointFix>();
                    //cCheck.BodyPart = Hip;
                }

            }

        }


        foreach (CharacterJoint charJoint in charJoints)
        {
            ConfigurableJoint confJoint;
            if (!charJoint.transform.GetComponent<ConfigurableJoint>())
            {

                confJoint = charJoint.gameObject.AddComponent<ConfigurableJoint>() as ConfigurableJoint;
                //				confJoint.autoConfigureConnectedAnchor = false;
                confJoint.connectedBody = charJoint.connectedBody;
                confJoint.anchor = charJoint.anchor;
                confJoint.axis = charJoint.axis;
                //				confJoint.connectedAnchor = charJoint.connectedAnchor;
                confJoint.secondaryAxis = charJoint.swingAxis;
                confJoint.xMotion = ConfigurableJointMotion.Locked;
                confJoint.yMotion = ConfigurableJointMotion.Locked;
                confJoint.zMotion = ConfigurableJointMotion.Locked;
                confJoint.angularXMotion = ConfigurableJointMotion.Limited;
                confJoint.angularYMotion = ConfigurableJointMotion.Limited;
                confJoint.angularZMotion = ConfigurableJointMotion.Limited;
                confJoint.lowAngularXLimit = charJoint.lowTwistLimit;
                confJoint.highAngularXLimit = charJoint.highTwistLimit;
                confJoint.angularYLimit = charJoint.swing1Limit;
                confJoint.angularZLimit = charJoint.swing2Limit;
                confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                confJoint.enablePreprocessing = false;
                confJoint.projectionDistance = 0.1f;
                confJoint.projectionAngle = 180f;
                confJoint.projectionMode = JointProjectionMode.PositionAndRotation;
                //				JointDrive temp = confJoint.slerpDrive; // These are left here to remind us how to set the drive
                //				temp.mode = JointDriveMode.Position;
                //				temp.positionSpring = 0f;
                //				confJoint.slerpDrive = temp;
                //				confJoint.targetRotation = Quaternion.identity;
            }
            DestroyImmediate(charJoint);
        }

        foreach (Transform childTransform in childTransforms)
        {

            if (childTransform.name.ToLower().Contains(hipName.ToLower()))
            {
                /*if (!childTransform.gameObject.GetComponent<BodyPartMono>())
                {
                    BodyPartMono bpMono = childTransform.gameObject.AddComponent<BodyPartMono>();
                    bpMono.BodyPart = Hip;
                }*/


            }
            if (childTransform.name.ToLower().Contains(chestName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -30f;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 30f;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 30f;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 30f;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z;
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 1000;
                    x.positionDamper = 100;
                    x.maximumForce = 1000;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(headName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -50f;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 50f;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 50f;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 60f;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z;
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 100;
                    x.positionDamper = 0.6f;
                    x.maximumForce = Mathf.Infinity;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(larmName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -100;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 60;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 100;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 50;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z;
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 1000;
                    x.positionDamper = 100;
                    x.maximumForce = 1000;
                    confJoint.slerpDrive = x;

                    confJoint.targetAngularVelocity = new Vector3(-15, -10, 5);

                }
            }
            if (childTransform.name.ToLower().Contains(rarmName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -60;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 100;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 100;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 50;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z;
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 1000;
                    x.positionDamper = 100;
                    x.maximumForce = 1000;
                    confJoint.slerpDrive = x;

                    confJoint.targetAngularVelocity = new Vector3(15, 10, 5);

                }
            }
            if (childTransform.name.ToLower().Contains(lfarmName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -150;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 20;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 20;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 30;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z;
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 1000;
                    x.positionDamper = 100;
                    x.maximumForce = 1000;
                    confJoint.slerpDrive = x;

                    confJoint.targetAngularVelocity = new Vector3(-40, 4, 6);

                }
            }
            if (childTransform.name.ToLower().Contains(rfarmName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -20;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 150;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 20;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 30;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z;
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 1000;
                    x.positionDamper = 100;
                    x.maximumForce = 1000;
                    confJoint.slerpDrive = x;

                    confJoint.targetAngularVelocity = new Vector3(40, -4, 6);

                }
            }
            if (childTransform.name.ToLower().Contains(lhandName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = 0;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 0;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 0;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 0;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z;
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 1000;
                    x.positionDamper = 100;
                    x.maximumForce = 1000;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(rhandName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = 0;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 0;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 0;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 0;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z;
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 1000;
                    x.positionDamper = 100;
                    x.maximumForce = 1000;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(lthighName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -20;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 130;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 65;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 40;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z;
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(rthighName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -20;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 130;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 65;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 40;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z;
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(llegName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -177;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 0;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 5;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 5;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z;
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(rlegName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = -177;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 0;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 5;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 5;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z;
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }
            }
            if (childTransform.name.ToLower().Contains(lfootName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = 0;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 0;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 0;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 0;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z;
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }

            }
            if (childTransform.name.ToLower().Contains(rfootName.ToLower()))
            {
                if (childTransform.gameObject.GetComponent<ConfigurableJoint>())
                {
                    ConfigurableJoint confJoint = childTransform.gameObject.GetComponent<ConfigurableJoint>();
                    if (changeJointLimits)
                    {
                        SoftJointLimit LAX = new SoftJointLimit();
                        LAX.limit = 0;
                        SoftJointLimit HAX = new SoftJointLimit();
                        HAX.limit = 0;
                        SoftJointLimit Y = new SoftJointLimit();
                        Y.limit = 0;
                        SoftJointLimit Z = new SoftJointLimit();
                        Z.limit = 0;

                        confJoint.lowAngularXLimit = LAX;
                        confJoint.highAngularXLimit = HAX;
                        confJoint.angularYLimit = Y;
                        confJoint.angularZLimit = Z;
                    }
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;


                    JointDrive x = confJoint.slerpDrive;
                    x.positionSpring = 0;
                    x.positionDamper = 0;
                    x.maximumForce = 0;
                    confJoint.slerpDrive = x;

                }

            }

        }
        //Debug.Log("Replaced " + i + " CharacterJoints with ConfigurableJoints on " + this.name);


        /*Character characterComponent = characterGO.AddComponent<Character>();
        characterComponent.portrait = portraitTexture;
        characterComponent.color = color;
        characterComponent.nickname = nickname;

        PlayerMovement playerMovement = characterGO.AddComponent<PlayerMovement>();
        characterComponent.playerMovement = playerMovement;*/
    }

    void OnWizardUpdate()
    {
        helpString = "Enter character details";
    }

}
