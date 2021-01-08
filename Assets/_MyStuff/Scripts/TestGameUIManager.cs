using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameUIManager : MonoBehaviour
{

    public GameObject[] players;

    // Use this for initialization
    void Start()
    {

        players[0].GetComponent<Punching>().enabled = false;
        players[1].GetComponent<Punching>().enabled = false;

        players[0].GetComponent<Punching2>().enabled = true;
        players[1].GetComponent<Punching2>().enabled = true;


    }

    public void SwitchStyle()
    {
        if (players[0].GetComponent<Punching>().enabled == true)
        {
            players[0].GetComponent<Punching>().enabled = false;
            players[0].GetComponent<Punching2>().enabled = true;

        }
        else if (players[0].GetComponent<Punching2>().enabled == true)
        {
            players[0].GetComponent<Punching>().enabled = true;
            players[0].GetComponent<Punching2>().enabled = false;
        }

        if (players[1].GetComponent<Punching>().enabled == true)
        {
            players[1].GetComponent<Punching>().enabled = false;
            players[1].GetComponent<Punching2>().enabled = true;

        }
        else if (players[1].GetComponent<Punching2>().enabled == true)
        {
            players[1].GetComponent<Punching>().enabled = true;
            players[1].GetComponent<Punching2>().enabled = false;
        }

    }
    // Update is called once per frame
    void Update()
    {

    }
}
