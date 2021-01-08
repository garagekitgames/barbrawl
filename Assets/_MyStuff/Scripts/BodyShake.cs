using System;
using UnityEngine;

public class BodyShake : MonoBehaviour
{
	protected Rigidbody rigidbody;

    public float counter;

    public float shakeRate = 5f;

	public float startShakeRate = 5f;

	public float endShakeRate = 50f;

	public float shakeRateIncrease = 20f;

	public float shakeForce = 20f;

	public float startShakeForce = 5f;

	public float endShakeForce = 20f;

	public float shakeForceIncrease = 10f;

	private void Start()
	{
		this.rigidbody = base.GetComponent<Rigidbody>();
	}

	public void Shake()
	{
		base.enabled = true;
		this.shakeRate = this.startShakeRate;
		this.shakeForce = this.startShakeForce;
	}

	private void Update()
	{
		//this.shakeRate += this.shakeRateIncrease * Time.deltaTime;
		//this.shakeForce += this.shakeForceIncrease * Time.deltaTime;
		this.counter += this.shakeRate * Time.deltaTime;
		float num = Mathf.Sin(this.counter);
		this.rigidbody.AddTorque(0f, 0f, num * this.shakeForce, ForceMode.Force);
	}
}
