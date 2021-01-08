using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using garagekitgames;
public class OnJointBreakScript : MonoBehaviour
{
    bool once;
    Rigidbody myRB;
    public bool nonJointOnCollisionEffect;

    public UnityEvent onJointBreak;

    public bool jointBroken = false;

    // Start is called before the first frame update
    void Start()
    {

        myRB = this.GetComponent<Rigidbody>();

    }

    void OnJointBreak(float breakForce)
    {
        //Debug.Log("A joint has just been broken!, force: " + breakForce);
        //coroutine = LateCall(3);
        // StartCoroutine(coroutine);
        AudioManager.instance.Play("WoodCrack1");
        AudioManager.instance.Play("WoodBreak" + Random.Range(1, 6));
        //AudioManager.instance.Play("WoodBreak1");
        jointBroken = true;
        //if(onJointBreak)
        //{
            onJointBreak.Invoke();
        //}
            

    }
    private void OnCollisionEnter(Collision collision)
    {
        Transform collisiontransform1 = collision.transform;
        CharacterThinker cT = collisiontransform1.root.GetComponent<CharacterThinker>();
        CharacterHealth cH = collisiontransform1.root.GetComponent<CharacterHealth>();

        if(!cT || !cH)
        {
            return;
        }
        if (this.gameObject.tag.Contains("tutorialTarget"))
        {
            if (cT.amIMainPlayer && cT.attack)
            {
                cT.youHitMe = true;
                jointBroken = true;
            }
                
        }
        if(nonJointOnCollisionEffect)
        {
            Transform collisiontransform = collision.transform;
            if (transform.root != collisiontransform.root && !once)
            {
                once = true;
                /*if (collisiontransform.name.Contains("Foot"))
                {

                    EffectsController.Instance.PlayFootStepSound(collision.contacts[0].point, collision.impulse.sqrMagnitude / 10, collision.collider.tag);


                }*/
                if (collisiontransform.name.Contains("chest") || collisiontransform.name.Contains("hip") || collisiontransform.name.Contains("head"))
                {
                    AudioManager.instance.Play("WoodCrack1");
                    AudioManager.instance.Play("WoodBreak" + Random.Range(1, 6));
                    //EffectsController.Instance.CreateSmokeEffect(collision.contacts[0].point, collision.impulse.magnitude);
                    // AudioManager.instance.Play("WoodBreak"+ Random.Range(1, 6));
                }
                /*if (collisiontransform.CompareTag("Cash") || collisiontransform.name.Contains("CashFinal"))
                {
                    //EffectsController.Instance.CreateSmokeEffect(collision.contacts[0].point, collision.impulse.magnitude);
                    AudioManager.instance.Play("CoinDrop1");
                }*/
            }
        }
        
    }
    private void OnCollisionExit(Collision collision)
    {
        Transform collisiontransform = collision.transform;
        if (transform.root != collisiontransform.root && once)
        {
            once = false;

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
