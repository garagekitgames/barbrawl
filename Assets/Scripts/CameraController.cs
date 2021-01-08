using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using System.Linq;
using garagekitgames;
public class CameraController : UnitySingleton<CameraController>
{

    public CinemachineBrain camBrain;
    public CinemachineVirtualCamera camVcam;
    public CinemachineConfiner camConfiner;
    public CinemachineTargetGroup camTargetGroup;

    public GameObject Player;
    public List<GameObject> Enemies;
    public float enemyWeight = 0.05f;

    public List<GameObject> camBounds;
    List<CinemachineTargetGroup.Target> enemyTargets = new List<CinemachineTargetGroup.Target>();


    // Start is called before the first frame update
    void Start()
    {
        camBrain = GameObject.FindObjectOfType<CinemachineBrain>();
        camVcam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        camConfiner = GameObject.FindObjectOfType<CinemachineConfiner>();
        camTargetGroup = GameObject.FindObjectOfType<CinemachineTargetGroup>();

        Player = GameObject.FindGameObjectWithTag("Player");//GameObject.FindGameObjectWithTag("CamTarget_Player");
                                                             //Enemies = GameObject.FindGameObjectsWithTag("CamTarget_Enemy").ToList<GameObject>();
                                                             //camBounds = GameObject.FindGameObjectsWithTag("CamBounds").ToList<GameObject>();

        //foreach (var item in Enemies)
        //{
        //    enemyTargets.Add(new CinemachineTargetGroup.Target(item.transform, enemyWeight, 0));
        //}

        SetupTarget(enemyTargets);
        //SetupCameraBounds(0);

        //InvokeRepeating("SetTargetWeights", 0.1f, 0.2f);
    }

    public void SetupTarget(List<CinemachineTargetGroup.Target> targets)
    {
        CinemachineTargetGroup.Target[] target = new CinemachineTargetGroup.Target[1];
        //targets.Add(new CinemachineTargetGroup.Target(Player.transform, 1, 0));
        camTargetGroup.m_Targets = target;
        camTargetGroup.m_Targets[0].target = Player.transform;
        camTargetGroup.m_Targets[0].radius = 0;
        camTargetGroup.m_Targets[0].weight = 1;
        //camTargetGroup.m_Targets.Clear();
        //camTargetGroup.m_Targets.Add(new CinemachineTargetGroup.Target(Player.transform, 1, 0));

        //foreach (var item in targets)
        //{
        //    camTargetGroup.m_Targets.Add(item);
        //}

        //camTargetGroup.m_Targets.Clear();
        //camTargetGroup.m_Targets.Insert(0, new CinemachineTargetGroup.Target(Player.transform, 1, 0));
        //camTargetGroup.m_Targets.Insert(1, new CinemachineTargetGroup.Target(ClosestEnemy().transform, enemyWeight, 0));


    }

    public void SetTargetWeights()
    {
        //float prevDist = 999;
        //for (int i = 0; i < camTargetGroup.m_Targets.Length; i++)
        //{
        //    if(!(Player.transform != camTargetGroup.m_Targets[i].target))
        //    {
        //        var dist = (Player.transform.position - camTargetGroup.m_Targets[0].target.position).sqrMagnitude;

        //        if(dist < prevDist)
        //        {
        //            camTargetGroup.m_Targets[i].weight = 1 / i + 1;
        //        }
        //        //if (((Player.transform.position - camTargetGroup.m_Targets[0].target.position).sqrMagnitude) > )
        //    }
        //}

        Array.Sort<CinemachineTargetGroup.Target>(camTargetGroup.m_Targets, (x, y) => Vector3.Distance(Player.transform.position, x.target.transform.position)
                                                                                    .CompareTo(Vector3.Distance(Player.transform.position, y.target.transform.position)));


        for (int i = 0; i < camTargetGroup.m_Targets.Length; i++)
        {
            if(i == 0)
            {
                camTargetGroup.m_Targets[i].weight = camTargetGroup.m_Targets.Length - 1;
            }
            
            else if(!camTargetGroup.m_Targets[i].target.transform.root.GetComponent<CharacterThinker>().health.alive)
            {
                camTargetGroup.m_Targets[i].weight = 0;
            }
            else
            {
                camTargetGroup.m_Targets[i].weight = (float)1 / (float)(i + 5);
            }
            
        }

    }

    public void SetupCameraBounds(int index)
    {
       // camConfiner.m_BoundingVolume = camBounds[index].GetComponent<Collider>();
    }

    public Transform ClosestEnemy()
    {
        Transform enemy;
        return enemy = Enemies.OrderBy(t => Vector3.Distance(Player.transform.position, t.transform.position)).FirstOrDefault().transform;
                                 

    }
    // Update is called once per frame
    void Update()
    {

        

    }

    private void LateUpdate()
    {
        
    }
}
