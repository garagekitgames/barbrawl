using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

public class KeepInFocus : MonoBehaviour
{

    // Array of targets
    public Transform[] focusTargets;

    // Current target
    public float focusTargetID;

    // Cache profile
   // PostProcessVolume postProfile;

    // Adjustable aperture - used in animations within Timeline
    [Range(0.1f, 20f)] public float aperture;


    void Start()
    {
        //// Load the post processing profile
        //postProfile = GetComponent<PostProcessingBehaviour>().profile;
    }

    void Update()
    {
        //// Get distance from camera and target
        //float dist = Vector3.Distance(transform.position, focusTargets[Mathf.FloorToInt(focusTargetID)].position);

        //// Get reference to the DoF settings
        //var dof = postProfile.depthOfField.settings;

        //// Set variables
        //dof.focusDistance = dist;
        //dof.aperture = aperture;

        //// Apply settings
        //postProfile.depthOfField.settings = dof;
    }
}