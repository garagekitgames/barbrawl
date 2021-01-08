using UnityEngine;
using System.Collections;

public class destroyMe : MonoBehaviour{

    //float timer;
    public float deathtimer = 10;
    public bool disableOnly;
    public bool amIParticle = false;


	// Use this for initialization
	void Start () {

        
        


    }

    private void OnEnable()
    {
        if (amIParticle)
        {
            DestroyMe();
        }
    }

    // Update is called once per frame


    public void DestroyMe()
    {
      
        if(deathtimer == 0)
        {
            //dontDestroy

        }
        else
        {
            IEnumerator coroutine = LateCall(deathtimer);
            StartCoroutine(coroutine);
        }
        
    }

    IEnumerator LateCall(float sec)
    {

        yield return new WaitForSeconds(sec);
        if(disableOnly)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
        

        //Do Function here...
    }
}
