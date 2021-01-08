using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
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

[ExecuteInEditMode]
[RequireComponent(typeof(CharacterHealth))]
[RequireComponent(typeof(CharacterHitMissTracker))]
[RequireComponent(typeof(CharacterThinker))]
[RequireComponent(typeof(CharacterLimpSetting))]
[RequireComponent(typeof(DiableAfterXSeconds))]
public class AddUnityEventsScript : MonoBehaviour {

    //public AudioSource audioSrc;
    [SerializeField]
    public UnityEvent events;
    UnityAction methodDelegate;
    //bool eventAdded = false;

    public CharacterThinker characterThinker;
    public CharacterHealth characterHealth;
    public CharacterHitMissTracker characterHitMissTracker;
    public CharacterLimpSetting characterLimpSetting;
    public DiableAfterXSeconds characterDisable;

    public GameEvent playerDamaged;
    public GameEvent playerDead;
    public GameEvent playerHit;
    public GameEvent playerMiss;
    public GameEvent playerVanished;

    void OnEnable()
    {
        UpdateInspector();
    }
    void UpdateInspector()
    {

        playerDamaged = (GameEvent)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Event/PlayerDamaged.asset", typeof(GameEvent));
        playerDead = (GameEvent)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Event/PlayerDead.asset", typeof(GameEvent));
        playerHit = (GameEvent)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Event/PlayerHit.asset", typeof(GameEvent));
        playerMiss = (GameEvent)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Event/PlayerMiss.asset", typeof(GameEvent));
        playerVanished = (GameEvent)AssetDatabase.LoadAssetAtPath("Assets/_MyStuff/Scripts/Scriptables/Event/PlayerVanished.asset", typeof(GameEvent));

        characterThinker = GetComponent<CharacterThinker>();
        characterHitMissTracker = GetComponent<CharacterHitMissTracker>();
        characterHealth = GetComponent<CharacterHealth>();
        characterLimpSetting = GetComponent<CharacterLimpSetting>();
        characterDisable = GetComponent<DiableAfterXSeconds>();
        ResetCharacterRoot resetRoot = GetComponent<ResetCharacterRoot>() as ResetCharacterRoot;

        characterDisable.afterDeathEvent.AddListener(delegate { playerVanished.Raise(); });

        //hitMissTracker.playerHitEvent.AddListener(delegate { playerHit.Raise(); });
        // hitMissTracker.playerMissEvent.AddListener(delegate { playerMiss.Raise(); });


        //characterHealth.DeathEvent.AddListener(delegate { characterThinker.SetStopDoingShit(true); });
        // characterHealth.DeathEvent.AddListener(delegate { playerDead.Raise(); });
        // characterHealth.DeathEvent.AddListener(delegate { characterLimpSetting.goLimp(); });
        //  characterHealth.DamageEvent.AddListener(delegate { playerDamaged.Raise(); });

        //UnityAction<CharacterLimpSetting> callback = new UnityAction<CharacterLimpSetting>(characterLimpSetting.goLimp);
        //UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(characterHealth.DeathEvent, new UnityAction(characterLimpSetting.goLimp));


        // characterHealth.OnEnableEvent.AddListener(delegate { characterThinker.SetStopDoingShit(false); });
        //  characterHealth.OnEnableEvent.AddListener(delegate { characterLimpSetting.resetLimp(); });
        //  characterHealth.OnEnableEvent.AddListener(delegate { resetRoot.resetRoot(); });

        //////////////////////////////////////////////////
        characterDisable.afterDeathEvent = new UnityEvent();
        UnityAction methodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction), playerVanished, "Raise") as UnityAction;
        UnityEditor.Events.UnityEventTools.AddPersistentListener(characterDisable.afterDeathEvent, methodDelegate);

        characterHitMissTracker.playerHitEvent = new UnityEvent();
        UnityAction methodDelegatehit = System.Delegate.CreateDelegate(typeof(UnityAction), playerHit, "Raise") as UnityAction;
        UnityEditor.Events.UnityEventTools.AddPersistentListener(characterHitMissTracker.playerHitEvent, methodDelegatehit);

        characterHitMissTracker.playerMissEvent = new UnityEvent();
        UnityAction methodDelegateMiss = System.Delegate.CreateDelegate(typeof(UnityAction), playerMiss, "Raise") as UnityAction;
        UnityEditor.Events.UnityEventTools.AddPersistentListener(characterHitMissTracker.playerMissEvent, methodDelegateMiss);


        characterHealth.DamageEvent = new UnityEvent();
        UnityAction methodDelegateDamage = System.Delegate.CreateDelegate(typeof(UnityAction), playerDamaged, "Raise") as UnityAction;
        UnityEditor.Events.UnityEventTools.AddPersistentListener(characterHealth.DamageEvent, methodDelegateDamage);

        UnityAction methodDelegateDamageHealthUI = System.Delegate.CreateDelegate(typeof(UnityAction), characterHealth, "SetHealthUI") as UnityAction;
        UnityEditor.Events.UnityEventTools.AddPersistentListener(characterHealth.DamageEvent, methodDelegateDamageHealthUI);

        characterHealth.DeathEvent = new UnityEvent();
        UnityAction methodDelegateDeath = System.Delegate.CreateDelegate(typeof(UnityAction), playerDead, "Raise") as UnityAction;
        UnityEditor.Events.UnityEventTools.AddPersistentListener(characterHealth.DeathEvent, methodDelegateDeath);

        characterHealth.OnEnableEvent = new UnityEvent();
        UnityAction<bool> methodDelegateSetStopDoing2 = System.Delegate.CreateDelegate(typeof(UnityAction<bool>), characterThinker, "SetStopDoingShit") as UnityAction<bool>;
        UnityEditor.Events.UnityEventTools.AddBoolPersistentListener(characterHealth.OnEnableEvent, methodDelegateSetStopDoing2, false);

        //script.Events = new UnityEvent();
        UnityAction<bool> methodDelegateSetStopDoing = System.Delegate.CreateDelegate(typeof(UnityAction<bool>), characterThinker, "SetStopDoingShit") as UnityAction<bool>;
        UnityEditor.Events.UnityEventTools.AddBoolPersistentListener(characterHealth.DeathEvent, methodDelegateSetStopDoing, true);

        UnityAction methodDelegategoLimp = System.Delegate.CreateDelegate(typeof(UnityAction), characterLimpSetting, "goLimp") as UnityAction;
        UnityEditor.Events.UnityEventTools.AddPersistentListener(characterHealth.DeathEvent, methodDelegategoLimp);


        UnityAction methodDelegateresetLimp = System.Delegate.CreateDelegate(typeof(UnityAction), characterLimpSetting, "resetLimp") as UnityAction;
        UnityEditor.Events.UnityEventTools.AddPersistentListener(characterHealth.OnEnableEvent, methodDelegateresetLimp);


        UnityAction methodDelegateresetRoot = System.Delegate.CreateDelegate(typeof(UnityAction), resetRoot, "resetRoot") as UnityAction;
        UnityEditor.Events.UnityEventTools.AddPersistentListener(characterHealth.OnEnableEvent, methodDelegateresetRoot);


        //if (!eventAdded)
        //{
        //audioSrc = GetComponent<AudioSource>();

        /////////////////////////
        /* events = new UnityEvent();
             methodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction), audioSrc, "Play") as UnityAction;
             UnityEditor.Events.UnityEventTools.AddPersistentListener(events, methodDelegate);

         script.Events = new UnityEvent();
         UnityAction methodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction), audioSource, "Play") as UnityAction;
         UnityEventTools.AddPersistentListener(script.Events, methodDelegate);

         // animator.Play(string) - its only add this animator
         script.Events = new UnityEvent();
         UnityAction<string> methodDelegateString = System.Delegate.CreateDelegate(typeof(UnityAction<string>), Selection.activeGameObject.GetComponent<Animator>(), "Play") as UnityAction<string>;
         UnityEventTools.AddStringPersistentListener(script.Events, methodDelegateString, "lala");*/
        ///////////////////////
        //eventAdded = true;
        //}
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
