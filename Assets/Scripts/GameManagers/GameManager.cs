using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public int pinataCount = 2;
    public int matchTimeInSeconds = 240;
    Timer matchTimer;
    int type = 0;

    public static GameManager instance = null;

	// Use this for initialization
	void Start () {
        instance = this; //only have one gameManager since no scene switching no need to persist and check for duplicates
        matchTimer = gameObject.AddComponent<Timer>();
	}
	
	// Update is called once per frame
	void Update () {
        switch (type)
        {
            case 0: //game start
                //Get Ready Go!
                matchTimer.Begin((float)matchTimeInSeconds);
                type = 1;
                break;
            case 1: //game play
                UpdatePinataCount();
                CheckTimer();
                break;
            case 2: //game over
                break;
            default:
                break;
        }
	}

    void UpdatePinataCount()
    {

    }
    
    void CheckTimer()
    {
        if (!matchTimer.isTimerRunning)
        {
            type = 2;
        }
    }

    public string MatchTimeLeft()
    {
        string s;
        TimeSpan timeSpan = TimeSpan.FromSeconds(matchTimer.timeLeft);
        if(matchTimer.timeLeft > 60f)
            s = string.Format("{0:D1}: {1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        else
            s = string.Format("{0:D2}.{1:D0}", timeSpan.Seconds,timeSpan.Milliseconds/100);
        return s;
    }
}
