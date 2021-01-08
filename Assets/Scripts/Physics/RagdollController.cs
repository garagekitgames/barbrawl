using UnityEngine;
using System.Collections;

public class RagdollController : MonoBehaviour {

	public RagdollDriver ragdollDriver;
	GameObject master;
	Transform ragdollRootBone;
	Transform masterRootBone;
	//PlayerControl2 pControl;
	Animator animator;



	//RD Values
	[Range(10f, 170f)] public float getupAngularDrag = 50f;		// Custom drag values during getup animations
	[Range(10f, 50f)] public float fallAngularDrag = 20f;		// Custom drag values during fall
	[Range(5f, 85f)] public float getupDrag = 25f;		// Custom drag values during getup animations

	[Range(.5f, 4.5f)] public float fallLerp = 1.5f;				// Determines how fast the character loses control after colliding
	[Range(0f, .2f)] public float residualTorque = 0f;		// The torque immediately after collision
	[Range(0f, .2f)] public float residualForce = .1f;
	[Range(0f, 120f)] public float residualJointTorque = 120f;
	[Range(0f, 1f)] public float residualIdleFactor = 0f; 
	/*float residualTorque;
	float residualForce;
	float residualJointTorque;
	float fallAngularDrag;*/
	float drag;								
	float angularDrag;

	float startdrag;								
	float startangularDrag;
	float settledSpeed = .1f;	
	Quaternion rootboneToForward;	

	//RC checks
	bool jointLimits = false;

	//States
	public bool falling = false;
	public bool gettingUp = false;			// Is in getUp state
	public bool orientated = false;
	bool orientate = false;	

	float maxTorque = 100f;					// The torque when not in contact with other colliders
	float maxForce = 100f;
	float maxJointTorque = 10000f;

	float contactTorque = 1f;				// The torque when in contact with other colliders
	float contactForce = 2f;
	float contactJointTorque = 1000f;

	float getupLerp1 = .15f;		// Determines the initial regaining of strength after the character fallen to ease the ragdoll to the masters pose
	float getupLerp2 = 2f;			// Determines the regaining of strength during the later part of the get up state
	//float fromContactLerp = 1f;		
	//float wakeUpStrength = .2f;	

	public float getupTime = 2f;
	public float contactTime = 0f;

	public bool getup = false;

	public bool doingDimp = false;
	public int maxKnockDowns = 3;
	public int curKnockDowns;

	void Awake()
	{

		ragdollDriver = GetComponent<RagdollDriver>();
		master = ragdollDriver.master;
		ragdollRootBone = GetComponentInChildren<Rigidbody>().transform;

		int i = 0;
		Transform[] transforms = GetComponentsInChildren<Transform>();
		foreach(Transform transformen in transforms)  // Find the masterRootBoone
		{
			if (transformen == ragdollRootBone)
			{
				masterRootBone = master.GetComponentsInChildren<Transform>()[i];
				break;
			}
			i++;
		}

		//pControl = master.GetComponent<PlayerControl2> ();
		animator = master.GetComponent<Animator> ();

		curKnockDowns = maxKnockDowns;

		startdrag = ragdollDriver.drag;								
		startangularDrag = ragdollDriver.angularDrag;


	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//DoRagdollControl ();
		if (curKnockDowns <= 0) {
			ragdollDriver.dead = true;
			//ragdollDriver.killed = true;
			StartCoroutine (KnockDown ());
		}
	
	}

	public void KnockDown1()
	{
		if (!falling) {
			if (curKnockDowns > 0) {
				curKnockDowns = curKnockDowns - 1;
				StartCoroutine (KnockDown ());
			}
			else if (curKnockDowns <= 0) {
				ragdollDriver.dead = true;
				//ragdollDriver.killed = true;
				StartCoroutine (KnockDown ());
						}
		}

	}

	public void DoRagdollControl()
	{
		rootboneToForward = Quaternion.Inverse(masterRootBone.rotation) * master.transform.rotation; // Relative orientation of ragdollRootBone to ragdoll transform
		if (ragdollDriver.knockDown && orientated) {

			if (!falling) {

				ragdollDriver.maxTorque = residualTorque;
				ragdollDriver.maxForce = residualForce;
				ragdollDriver.maxJointTorque = residualJointTorque;
				ragdollDriver.SetJointTorque (residualJointTorque); 

				ragdollDriver.EnableJointLimits (true);
				jointLimits = true;
				ragdollDriver.secondaryUpdate = 100;

				ragdollDriver.angularDrag = fallAngularDrag;
				ragdollDriver.drag = drag;

			}

			falling = true;
			gettingUp = false;
			orientated = false;
			ragdollDriver.knockDown = false;
			//pControl.canMove = false;


		} 
		else if (falling || gettingUp) {

			if (gettingUp) {

				if (orientate) {
					falling = false;
					//pControl.canMove = false;
					// Here the master gets reorientated to the ragdoll which could have ended its fall in any direction and position
					master.transform.rotation = ragdollRootBone.rotation * Quaternion.Inverse(masterRootBone.rotation) * master.transform.rotation;
					master.transform.rotation = Quaternion.LookRotation(new Vector3(master.transform.forward.x, 0f, master.transform.forward.z), Vector3.up); 
					master.transform.Translate(ragdollRootBone.position - masterRootBone.position, Space.World);

					orientate = false; // Orientation is now done
					orientated = true;

					ragdollDriver.angularDrag = getupAngularDrag;
					ragdollDriver.drag = getupDrag;
				}
				if (orientated) {

					master.transform.Translate((ragdollRootBone.position - masterRootBone.position) * .5f, Space.World);

					ragdollDriver.maxTorque = Mathf.Lerp(ragdollDriver.maxTorque, contactTorque, getupLerp2 * Time.fixedDeltaTime); // We now start lerping the strength back to the ragdoll. Do until strength is wakeUpStrength. Animation is running wery slowly
					ragdollDriver.maxForce = Mathf.Lerp(ragdollDriver.maxForce, contactForce, getupLerp2 * Time.fixedDeltaTime);
					ragdollDriver.maxJointTorque = Mathf.Lerp(ragdollDriver.maxJointTorque, contactJointTorque, getupLerp2 * Time.fixedDeltaTime);
					ragdollDriver.secondaryUpdate = 2;



					if (jointLimits)
					{
						ragdollDriver.EnableJointLimits(false);
						jointLimits = false;
					}
					gettingUp = false;
					getup = true;
					//pControl.canMove = true;
				}

			}

			 if(falling) //Falling
			{
				// Lerp force to zero from residual values
				ragdollDriver.maxTorque = Mathf.Lerp(ragdollDriver.maxTorque, 0f, fallLerp * Time.fixedDeltaTime);
				ragdollDriver.maxForce = Mathf.Lerp(ragdollDriver.maxForce, 0f, fallLerp * Time.fixedDeltaTime);
				ragdollDriver.maxJointTorque = Mathf.Lerp(ragdollDriver.maxJointTorque, 0f, fallLerp * Time.fixedDeltaTime);
				ragdollDriver.SetJointTorque (ragdollDriver.maxJointTorque); // Do not wait for animfollow.secondaryUpdate

				contactTime += Time.fixedDeltaTime;

				// Orientate master to ragdoll and start transition to getUp when settled on the ground. Falling is over, getting up commences
				if ((ragdollRootBone.GetComponent<Rigidbody>().velocity.magnitude < settledSpeed) && (contactTime > getupTime) ) // && contactTime + noContactTime > .4f)
				{
					gettingUp = true;
					orientate = true;
					contactTime = 0f;
					falling = false;
					//pControl.canMove = false;
					//animator.speed = masterFallAnimatorSpeedFactor * animatorSpeed; // Animation speed during transition to get up state

						ragdollDriver.maxTorque = 0f; // These strengths shold be zero to avoid twitching during orientation
						ragdollDriver.maxForce = 0f;
						ragdollDriver.maxJointTorque = 0f;

					//animator.SetFloat(hash.speedFloat, 0f, 0f, Time.fixedDeltaTime);

//					Vector3 rootBoneForward = ragdollRootBone.rotation * rootboneToForward * Vector3.forward;
//					if (Vector3.Dot(rootBoneForward, Vector3.down) >= 0f) // Check if ragdoll is lying on its back or front, then transition to getup animation
//					{
//						if (!animator.GetCurrentAnimatorStateInfo(0).fullPathHash.Equals(Animator.StringToHash("Base Layer.GetUpFace")))
//							animator.SetBool("GetupFrontTrigger", true);
//						
//					}
//					else
//					{
//						if (!animator.GetCurrentAnimatorStateInfo(0).fullPathHash.Equals(Animator.StringToHash("Base Layer.GetUpBack")))
//							animator.SetBool("GetupBackTrigger", true);
//						
//					}
				}

			}

		}

		///////////////////////////////////////////////////////////
		/// 
		if (getup)
		{
			getup = false;
			Vector3 rootBoneForward = ragdollRootBone.rotation * rootboneToForward * Vector3.forward;
			if (Vector3.Dot(rootBoneForward, Vector3.down) >= 0f) // Check if ragdoll is lying on its back or front, then transition to getup animation
			{
				if (!animator.GetCurrentAnimatorStateInfo(0).fullPathHash.Equals(Animator.StringToHash("Base Layer.GetUpFace")))
					animator.SetBool("GetupFrontTrigger", true);

			}
			else
			{
				if (!animator.GetCurrentAnimatorStateInfo(0).fullPathHash.Equals(Animator.StringToHash("Base Layer.GetUpBack")))
					animator.SetBool("GetupBackTrigger", true);

			}
			StartCoroutine(Getup());

		}
	}
	IEnumerator KnockDown()
	{
		falling = true;
		ragdollDriver.knockDown = true;
		ragdollDriver.maxTorque = residualTorque;
		ragdollDriver.maxForce = residualForce;
		ragdollDriver.maxJointTorque = residualJointTorque;
		ragdollDriver.SetJointTorque (residualJointTorque); 

		ragdollDriver.EnableJointLimits (true);
		jointLimits = true;
		ragdollDriver.secondaryUpdate = 100;

		ragdollDriver.angularDrag = fallAngularDrag;
		ragdollDriver.drag = drag;

	



		float currentTime = 0.0f;
		bool done = false;
		while (!done) {
			float percent = currentTime / fallLerp;
			if (percent >= 1.0f) {
				percent = 1f;
				done = true;
			}
			percent = Mathf.Sin (percent * Mathf.PI * 0.5f);

			ragdollDriver.maxTorque = Mathf.Lerp (ragdollDriver.maxTorque, 0f, percent);
			ragdollDriver.maxForce = Mathf.Lerp (ragdollDriver.maxForce, 0f, percent);
			ragdollDriver.maxJointTorque = Mathf.Lerp (ragdollDriver.maxJointTorque, 0f, percent);
			ragdollDriver.SetJointTorque (ragdollDriver.maxJointTorque); // Do not wait for animfollow.secondaryUpdate






			//this.myJoint.slerpDrive.positionSpring =
			currentTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
			
		//yield return new WaitForSeconds(getupTime);
		if (!ragdollDriver.dead)
			StartCoroutine (Orientate ());
		else
			ragdollDriver.killed = true;


	}

	IEnumerator Orientate()
	{
		yield return new WaitForSeconds(getupTime);
		float currentTime = 0.0f;
		bool done = false;
		currentTime += Time.deltaTime;
		float percent = currentTime / getupLerp2;

		master.transform.rotation = ragdollRootBone.rotation * Quaternion.Inverse(masterRootBone.rotation) * master.transform.rotation;
		master.transform.rotation = Quaternion.LookRotation(new Vector3(master.transform.forward.x, 0f, master.transform.forward.z), Vector3.up); 
		master.transform.Translate(ragdollRootBone.position - masterRootBone.position, Space.World);

		ragdollDriver.angularDrag = getupAngularDrag;
		ragdollDriver.drag = getupDrag;


		master.transform.Translate((ragdollRootBone.position - masterRootBone.position) * .5f, Space.World);
					ragdollDriver.maxTorque = Mathf.Lerp (ragdollDriver.maxTorque, contactTorque, percent);
					ragdollDriver.maxForce = Mathf.Lerp (ragdollDriver.maxForce, contactForce, percent);
					ragdollDriver.maxJointTorque = Mathf.Lerp (ragdollDriver.maxJointTorque, contactJointTorque, percent);
					ragdollDriver.SetJointTorque (ragdollDriver.maxJointTorque); // Do not wait for animfollow.secondaryUpdate
		ragdollDriver.secondaryUpdate = 2;

		if (jointLimits)
		{
			ragdollDriver.EnableJointLimits(false);
			jointLimits = false;
		}


//		while (!done) {
//			float percent = currentTime / getupLerp2;
//			if (percent >= 1.0f) {
//				percent = 1f;
//				done = true;
//			}
//			percent = Mathf.Sin (percent * Mathf.PI * 0.5f);
//
////			ragdollDriver.maxTorque = Mathf.Lerp (ragdollDriver.maxTorque, contactTorque, percent);
////			ragdollDriver.maxForce = Mathf.Lerp (ragdollDriver.maxForce, contactForce, percent);
////			ragdollDriver.maxJointTorque = Mathf.Lerp (ragdollDriver.maxJointTorque, contactJointTorque, percent);
////			ragdollDriver.SetJointTorque (ragdollDriver.maxJointTorque); // Do not wait for animfollow.secondaryUpdate
//
////			ragdollDriver.maxTorque = Mathf.Lerp(ragdollDriver.maxTorque, contactTorque, getupLerp2 * Time.fixedDeltaTime); // We now start lerping the strength back to the ragdoll. Do until strength is wakeUpStrength. Animation is running wery slowly
////			ragdollDriver.maxForce = Mathf.Lerp(ragdollDriver.maxForce, contactForce, getupLerp2 * Time.fixedDeltaTime);
////			ragdollDriver.maxJointTorque = Mathf.Lerp(ragdollDriver.maxJointTorque, contactJointTorque, getupLerp2 * Time.fixedDeltaTime);
//
//
//
//
//
//			//this.myJoint.slerpDrive.positionSpring =
//			currentTime += Time.deltaTime;
//			yield return new WaitForEndOfFrame();
//		}
			
		if(!ragdollDriver.dead)
			StartCoroutine(Getup());


	}

	IEnumerator Getup()
	{
		gettingUp = true;
		Vector3 rootBoneForward = ragdollRootBone.rotation * rootboneToForward * Vector3.forward;
		if (Vector3.Dot(rootBoneForward, Vector3.down) >= 0f) // Check if ragdoll is lying on its back or front, then transition to getup animation
		{
			if (!animator.GetCurrentAnimatorStateInfo(0).fullPathHash.Equals(Animator.StringToHash("Base Layer.GetUpFace")))
				animator.SetBool("GetupFrontTrigger", true);

		}
		else
		{
			if (!animator.GetCurrentAnimatorStateInfo(0).fullPathHash.Equals(Animator.StringToHash("Base Layer.GetUpBack")))
				animator.SetBool("GetupBackTrigger", true);

		}

		doingDimp = true;

		float initialRange = 0f;//Or the value you want
		float finalRange = 100f;//Or the value you want

		//float dimLightTime =2f;//How much time will it take to dim the light
		float currentTime = 0.0f;
		bool done = false;
		while (!done)
		{
			float percent = currentTime / getupTime;
			if (percent >= 1.0f)
			{
				percent = 1f;
				done = true;
			}

			//Ease out :
			//float t = currentLerpTime / lerpTime;
			percent = Mathf.Sin(percent * Mathf.PI * 0.5f);

			//Ease in : 
			//percent = 1f - Mathf.Cos(percent * Mathf.PI * 0.5f);

			//Exponential : 
			//percent = percent*percent;

			//Smoothstep : 
			//percent = percent*percent * (3f - 2f*percent);

			//Smootherstep : 
			//percent = percent*percent*percent * (percent * (6f*percent - 15f) + 10f);

			//Elastic
			//percent = Mathf.Pow(2f,-10f*percent) * Mathf.Sin((percent-0.3f/4f)*(2*Mathf.PI)/0.3f) + 1f;


		//	this.maxForce = Mathf.Lerp(initialRange, finalRange, percent);
		//	this.PForce =  Mathf.Lerp(initialRange, 30f, percent);
			float dmgPerc = ((float)curKnockDowns / (float)maxKnockDowns);

			float currentMaxForceValue = maxForce * dmgPerc;
			float currentMaxJointTorque = maxJointTorque * dmgPerc;
			//float currentmaxJointTorqueProfileValue = maxJointTorqueProf * dmgPerc;

			//Debug.Log ("hurt PErc currentPForceProfileValue : " +currentPForceProfileValue);
			//Debug.Log ("hurt PErc currentmaxJointTorqueProfileValue : " +currentmaxJointTorqueProfileValue);
			//rD.PForceProfile[identity] = Mathf.Lerp(rD.PForceProfile[identity], currentPForceProfileValue, percent);
			//rD.maxJointTorqueProfile [identity] = Mathf.Lerp(rD.PForceProfile[identity], currentmaxJointTorqueProfileValue, percent);

			ragdollDriver.topMaxForce = currentMaxForceValue;
			//rD.maxForce =  Mathf.Lerp(rD.maxForce, rD.topMaxForce, percent);
			ragdollDriver.topMaxJointTorque = currentMaxJointTorque;

			ragdollDriver.maxTorque = Mathf.Lerp(ragdollDriver.maxTorque, maxTorque, percent);
			ragdollDriver.maxForce = Mathf.Lerp(ragdollDriver.maxForce, ragdollDriver.topMaxForce, percent);
			ragdollDriver.maxJointTorque = Mathf.Lerp(ragdollDriver.maxJointTorque, ragdollDriver.topMaxJointTorque, percent);






			//this.myJoint.slerpDrive.positionSpring =
			currentTime += Time.deltaTime;

			yield return new WaitForEndOfFrame();
		}

		//if(done)
		falling = false;
		ragdollDriver.knockDown = false;

		//startdrag = ragdollDriver.drag;								
		//startangularDrag = ragdollDriver.angularDrag;

		ragdollDriver.angularDrag = startangularDrag;
		ragdollDriver.drag = startdrag;

		doingDimp = false;
		gettingUp = false;

	}

}
