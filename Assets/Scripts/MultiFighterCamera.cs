using UnityEngine;
using System.Collections;

public class MultiFighterCamera : MonoBehaviour
{
    public float updateFrequency = 0.2f;

    public Vector3 offset;

    public Vector3 deathOffset;

    Vector3 startingOffset;

    private Vector3 lastOffset;

    public Vector3 targetsCenter;

    public float maxZoom = 10f;

    public float zoom = 1f;

    public Transform[] targets;

    private Transform thisTransform;

    private Vector3 lastLookAtPosition;

    private Vector3 velocity1;

    private Vector3 velocity2;

    public float smoothTime = 1f;

    public bool orbit;

    public float orbitSpeed = 6f;

    private bool toggle;

    //public GameObject controlsScreen;

    public Vector3 lookAtPosition;

    public Vector3 cameraPosition;

    public bool initialized;

    private void Start()
    {
        this.thisTransform = base.transform;
        this.RefreshCameraTargets();
        if (this.targets.Length > 0)
        {
            this.UpdateCenter();
        }
        this.lastLookAtPosition = this.targetsCenter;
        startingOffset = offset;
    }

    private void LateUpdate()
    {
        
            this.UpdateCamera();
       
    }

    public void OnDeath()
    {
        offset = deathOffset;
    }

    public void OnRevive()
    {
        offset = startingOffset;
    }

    private void UpdateCamera()
    {
        this.RefreshCameraTargets();
        if (this.targets.Length > 0)
        {
            this.UpdateCenter();
            this.UpdateZoom();
            if (this.initialized)
            {
                this.UpdatePosition();
            }
            else
            {
                this.SetPosition();
            }
        }
    }

    private void RefreshCameraTargets()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] array1 = GameObject.FindGameObjectsWithTag("Enemy");
        this.targets = new Transform[0];
        bool flag = false;
        /*GameObject[] array2 = array;
        for (int i = 0; i < array2.Length; i++)
        {
            GameObject gameObject = array2[i];
            Actor component = gameObject.transform.root.GetComponent<Actor>();
            if (component != null && component.isLocalPlayer)
            {
                this.targets = new Transform[1];
                this.targets[0] = gameObject.transform;
                flag = true;
            }
        }*/
        if (!flag && array.Length > 0)
        {
            this.targets = new Transform[array.Length];
            for (int j = 0; j < array.Length; j++)
            {
                this.targets[j] = array[j].transform;
            }
        }
    }

    private void UpdateCenter()
    {
        this.targetsCenter = Vector3.zero;
        if (this.targets.Length > 1)
        {
            for (int i = 0; i < this.targets.Length; i++)
            {
                this.targetsCenter += this.targets[i].position;
            }
            this.targetsCenter /= (float)this.targets.Length;
        }
        else
        {
            this.targetsCenter = this.targets[0].position;
        }
    }

    private void UpdateZoom()
    {
        float num = 1f;
        float num2 = 1f;
        if (this.targets.Length > 1)
        {
            for (int i = 0; i < this.targets.Length; i++)
            {
                num2 = Vector3.Distance(this.targets[i].position, this.targetsCenter);
                if (num2 > num)
                {
                    num = num2;
                }
            }
        }
        this.zoom = Mathf.Clamp(num2 / 5f, 1f, this.maxZoom);
    }

    private void UpdatePosition()
    {
        Vector3 a = Vector3.Lerp(this.lastOffset, this.offset, this.smoothTime);
        Vector3 target = this.targetsCenter + a * this.zoom;
        this.lookAtPosition = Vector3.SmoothDamp(this.lastLookAtPosition, this.targetsCenter, ref this.velocity1, this.smoothTime);
        this.cameraPosition = Vector3.SmoothDamp(this.thisTransform.position, target, ref this.velocity2, this.smoothTime);
        if (!this.orbit)
        {
            this.thisTransform.position = this.cameraPosition;
        }
        else
        {
            this.thisTransform.RotateAround(this.lookAtPosition, Vector3.up, this.orbitSpeed * Time.deltaTime);
            Vector3 position = new Vector3(this.thisTransform.position.x, this.cameraPosition.y, this.thisTransform.position.z);
            this.thisTransform.position = position;
        }
        base.transform.LookAt(this.lookAtPosition);
        this.lastLookAtPosition = this.lookAtPosition;
        this.lastOffset = a;
    }

    private void SetPosition()
    {
        Vector3 a = this.offset;
        Vector3 vector = this.targetsCenter + a * this.zoom;
        this.lookAtPosition = this.targetsCenter;
        this.cameraPosition = vector;
        if (!this.orbit)
        {
            this.thisTransform.position = this.cameraPosition;
        }
        else
        {
            this.thisTransform.RotateAround(this.lookAtPosition, Vector3.up, this.orbitSpeed);
            Vector3 position = new Vector3(this.thisTransform.position.x, this.cameraPosition.y, this.thisTransform.position.z);
            this.thisTransform.position = position;
        }
        base.transform.LookAt(this.lookAtPosition);
        this.lastLookAtPosition = this.lookAtPosition;
        this.lastOffset = a;
        this.initialized = true;
    }
}

