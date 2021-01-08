using UnityEngine;
using System.Collections;

public class ConfigJointRotation : MonoBehaviour {

	private Quaternion startRotation;
	public ConfigurableJoint myJoint;
	public Vector3 rotationValue;
	public Transform target;

	Quaternion localToJointSpace;
	Quaternion startLocalRotation;
	JointDrive jointDrive = new JointDrive();

	// The ranges are not set in stone. Feel free to extend the ranges
	[Range(0f, 10000f)] public float maxTorque = 10000f; // Limits the world space torque
	[Range(0f, 10000f)] public float maxForce = 10000f; // Limits the force
	[Range(0f, 10000f)] public float maxJointTorque = 10000f; // Limits the force
	[Range(0f, 10f)] public float jointDamping = .6f; // Limits the force

	public float[] maxTorqueProfile = {100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f}; // Individual limits per limb
	public float[] maxForceProfile = {1f, .2f, .2f, .2f, .2f, 1f, 1f, .2f, .2f, .2f, .2f, .2f};
	public float[] maxJointTorqueProfile = {1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f};
	public float[] jointDampingProfile = {1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f};



	[Range(0f, .64f)] public float PTorque = .16f; // For all limbs Torque strength
	[Range(0f, 160f)] public float PForce = 30f;

	[Range(0f, .008f)] public float DTorque = .002f; // Derivative multiplier to PD controller
	[Range(0f, .064f)] public float DForce = .01f;

	//	public float[] PTorqueProfile = {20f, 30f, 10f, 30f, 10f, 30f, 30f, 30f, 10f, 30f, 10f}; // Per limb world space torque strength
	public float[] PTorqueProfile = {20f, 30f, 10f, 30f, 10f, 30f, 30f, 30f, 30f, 10f, 30f, 10f}; // Per limb world space torque strength for EthanRagdoll_12 (twelve rigidbodies)
	public float[] PForceProfile = {1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f};



	public Vector3 rigidbodiesPosToCOM;

	[Range(0f, 340f)] public float angularDrag = 100f; // Rigidbodies angular drag. Unitys parameter
	[Range(0f, 2f)] public float drag = .5f; // Rigidbodies drag. Unitys parameter
	float maxAngularVelocity = 1000f; // Rigidbodies maxAngularVelocity. Unitys parameter

	float torqueAngle; // Återanvänds för localTorque, därför ingen variabel localTorqueAngle
	Vector3 torqueAxis;
	Vector3 torqueError;
	Vector3 torqueSignal;
	Vector3 torqueLastError ;
	Vector3 torqueVelError;
	[HideInInspector] public Vector3 totalTorqueError; // Total world space angular error of all limbs. This is a vector.

	Vector3 forceAxis;
	Vector3 forceSignal;
	Vector3 forceError;
	Vector3 forceLastError;
	Vector3 forceVelError;
	[HideInInspector] public Vector3 totalForceError; // Total world position error. a vector.
	public float[] forceErrorWeightProfile = {1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f};
	Rigidbody slaveRigidbodies;


	public float fixedDeltaTime = 0.01f; // If you choose to go to longer times you need to lower PTorque, PLocalTorque and PForce or the system gets unstable. Can be done, longer time is better performance but worse mimicking of master.
	float reciFixedDeltaTime; 

	public bool doingDimp = false;

	//new PD 

	public float frequency = 0f;
	public float damping = 0f;
	void SetMaxForce(float force)
	{
		this.maxForce = force;
	}

	// Use this for initialization
	void Start () {

		Time.fixedDeltaTime = fixedDeltaTime; // Set the physics loop update intervall
		//		Debug.Log("The script AnimFollow has set the fixedDeltaTime to " + fixedDeltaTime); // Remove this line if you don't need the "heads up"
		reciFixedDeltaTime = 1f / fixedDeltaTime; // Cache the reciprocal

		 startRotation = transform.localRotation;
		myJoint = GetComponent<ConfigurableJoint> ();

		Vector3 forward = Vector3.Cross (myJoint.axis, myJoint.secondaryAxis);
		Vector3 up = myJoint.secondaryAxis;

		localToJointSpace =  Quaternion.LookRotation(forward, up);

		startLocalRotation = transform.localRotation * localToJointSpace;

		jointDrive = myJoint.slerpDrive;
		jointDrive.mode = JointDriveMode.Position;
		myJoint.slerpDrive = jointDrive;



		transform.GetComponent<Rigidbody>().useGravity = true;
		transform.GetComponent<Rigidbody>().angularDrag = angularDrag;
		transform.GetComponent<Rigidbody>().drag = drag;
		transform.GetComponent<Rigidbody>().maxAngularVelocity = maxAngularVelocity;

		slaveRigidbodies = transform.GetComponent<Rigidbody> ();
	
	}
	
	// Update is called once per frame
	void Update () {

		totalTorqueError = Vector3.zero;
		totalForceError = Vector3.zero;


		rigidbodiesPosToCOM = Quaternion.Inverse(transform.rotation) * (transform.GetComponent<Rigidbody>().worldCenterOfMass - transform.position); 
		//myJoint.SetTargetRotationLocal (Quaternion.Euler (target.localRotation.eulerAngles), startRotation);

		myJoint.targetRotation = Quaternion.Inverse(localToJointSpace) * Quaternion.Inverse(target.localRotation) * startLocalRotation;



		slaveRigidbodies.angularDrag = angularDrag; // Set rigidbody drag and angular drag in real-time
		slaveRigidbodies.drag = drag;

		Quaternion targetRotation;

		targetRotation = target.rotation * Quaternion.Inverse(transform.rotation);
			targetRotation.ToAngleAxis(out torqueAngle, out torqueAxis);
			torqueError = FixEuler(torqueAngle) * torqueAxis;

			if(torqueAngle != 360f)
			{
				totalTorqueError += torqueError;
				PDControl (PTorque * PTorqueProfile[1], DTorque, out torqueSignal, torqueError, ref torqueLastError, reciFixedDeltaTime);
			}
			else
				torqueSignal = new Vector3(0f, 0f, 0f);

			torqueSignal = Vector3.ClampMagnitude(torqueSignal, maxTorque * maxTorqueProfile[1]);
			slaveRigidbodies.AddTorque(torqueSignal, ForceMode.VelocityChange); // Add torque to the limbs




		// Force error
		Vector3 masterRigidTransformsWCOM = target.position + target.rotation * rigidbodiesPosToCOM;
		forceError = masterRigidTransformsWCOM - transform.GetComponent<Rigidbody>().worldCenterOfMass; // Doesn't work if collider is trigger
		totalForceError += forceError * forceErrorWeightProfile[1];

		PDControl (PForce * PForceProfile[1], DForce, out forceSignal, forceError, ref forceLastError, reciFixedDeltaTime);
		forceSignal = Vector3.ClampMagnitude(forceSignal, maxForce * maxForceProfile[1]);
		slaveRigidbodies.AddForce(forceSignal, ForceMode.VelocityChange);


		//New PD
		/*float kp = (6f*frequency)*(6f*frequency)* 0.25f;
		float kd = 4.5f*frequency*damping;
		float dt = Time.fixedDeltaTime;
		float g = 1 / (1 + kd * dt + kp * dt * dt);
		float ksg = kp * g;
		float kdg = (kd + kp * dt) * g;
		Vector3 Pt0 = transform.position;
		Vector3 Vt0 = slaveRigidbodies.velocity;
		Vector3 F = forceError * ksg + (slaveRigidbodies.velocity - Vt0) * kdg;
		slaveRigidbodies.AddForce (F);*/

	
	}

	//If we are touching something (like the ground )
	void OnCollisionEnter(Collision other)
	{
		
		//This means we are on the ground
		//We want to know when we have left the ground (or anything else)
		//if (gameObject.tag == "Head") {
	/*	if ((other.gameObject.tag == "Kick" ) || (other.gameObject.tag == "Punch" )  ) {    //You can copy this if statement and make it "Vehicle" or something to jump off a car.
			
				slaveRigidbodies.AddForceAtPosition (other.impulse, other.contacts [0].normal,ForceMode.Impulse);

				maxForce = 0.0f;
				
					StartCoroutine(DimPlayerLight());

			}*/

		if (gameObject.tag == "Head" && (other.gameObject.tag == "Kick" ) || (other.gameObject.tag == "Punch" )  ) {    //You can copy this if statement and make it "Vehicle" or something to jump off a car.
			/*	Debug.Log ("Impulse force : " + other.impulse.magnitude);
				Debug.Log ("relative velocity Force : " + other.relativeVelocity.magnitude);
				Debug.Log ("contact point : " + other.contacts [0].point.magnitude);
*/
			//slaveRigidbodies.AddForceAtPosition (other.impulse, other.contacts [0].normal,ForceMode.Impulse);

		//	maxForce = 0.0f;
			ConfigJointRotation[] confgjoint = FindObjectsOfType<ConfigJointRotation> ();
			foreach (ConfigJointRotation cj in confgjoint) {
				//cj.gameObject.transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.name == "ActiveRagdollTestDummy"
				if (cj.gameObject.transform.root.name == "ActiveRagdollTestDummy") {
					cj.SetMaxForce (0.0f);

					//cj.jointDrive = cj.GetComponent<ConfigurableJoint> ().slerpDrive;
					//jointDrive.positionSpring = 0f;
					//cj.GetComponent<ConfigurableJoint> ().slerpDrive = jointDrive;


					//StartCoroutine(cj.Dimmer (100f, 0f, "maxforce"));
					//StartCoroutine(cj.Dimmer (30f, 0f, "pforce"));


					cj.jointDrive = cj.GetComponent<ConfigurableJoint> ().slerpDrive;
					jointDrive.positionSpring = 1f;
					cj.GetComponent<ConfigurableJoint> ().slerpDrive = jointDrive;

				}



			}
			//if(!doingDimp)
			//{ 
			//StartCoroutine(DimPlayerLight());

			/*this.maxForce =  0.0f;
				this.PForce =   0.0f;

				jointDrive = myJoint.slerpDrive;
				jointDrive.positionSpring =    0.0f;
				myJoint.slerpDrive = jointDrive;*/

			//}
			//	other.gameObject.GetComponent<ConfigJointRotation> ().SetMaxForce(0.0f);
		}
	//	}

	}

	//void Ienu

	void OnCollisionExit(Collision other)
	{
		//Debug.Log ("Jumped 1 in air : "+other.gameObject.name);
		//We want to know when we have left the ground (or anything else)
		if (other.gameObject.tag == "Head")    //You can copy this if statement and make it "Vehicle" or something to jump off a car.
		{
			//maxForce = 100f;
		}
	}


	IEnumerator DimPlayerLight()
	{
		doingDimp = true;

		float initialRange = 0f;//Or the value you want
		float finalRange = 100f;//Or the value you want

		float dimLightTime =2f;//How much time will it take to dim the light
		float currentTime = 0.0f;
		bool done = false;
		while (!done)
		{
			float percent = currentTime / dimLightTime;
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


			this.maxForce = Mathf.Lerp(initialRange, finalRange, percent);
			this.PForce =  Mathf.Lerp(initialRange, 30f, percent);

			jointDrive = myJoint.slerpDrive;
			jointDrive.positionSpring =   Mathf.Lerp(initialRange, 5000f, percent);
			myJoint.slerpDrive = jointDrive;

			//this.myJoint.slerpDrive.positionSpring =
			currentTime += Time.deltaTime;

			yield return new WaitForEndOfFrame();
		}

		doingDimp = false;
	}



	IEnumerator Dimmer(float _initialRange, float _finalRange, string area)
	{
		doingDimp = true;

		float initialRange =_initialRange;//Or the value you want
		float finalRange = _finalRange;//Or the value you want

		float dimLightTime =0.000000000001f;//How much time will it take to dim the light
		float currentTime = 0.0f;
		bool done = false;
		while (!done)
		{
			float percent = currentTime / dimLightTime;
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

			if(area == "maxforce")
				this.maxForce = Mathf.Lerp(initialRange, finalRange, percent);
			
			if(area == "pforce")
				this.PForce =  Mathf.Lerp(initialRange, 30f, percent);


			//jointDrive = myJoint.slerpDrive;
			//jointDrive.positionSpring =   Mathf.Lerp(initialRange, 5000f, percent);
			//myJoint.slerpDrive = jointDrive;


			currentTime += Time.deltaTime;

			yield return new WaitForEndOfFrame();
		}

		doingDimp = false;
	}



	private float FixEuler (float angle) // For the angle in angleAxis, to make the error a scalar
	{
		if (angle > 180f)
			return angle - 360f;
		else
			return angle;
	}

	public static void PDControl (float P, float D, out Vector3 signal, Vector3 error, ref Vector3 lastError, float reciDeltaTime) // A PD controller
	{
		// theSignal = P * (theError + D * theDerivative) This is the implemented algorithm.
		signal = P * (error + D * ( error - lastError ) * reciDeltaTime);
		lastError = error;
	}
}
