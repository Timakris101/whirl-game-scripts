using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceiverScript : MonoBehaviour {
    [SerializeField] private int timeActive;
    [SerializeField] private int time;

    void Update() {
        time++;
        if (time > timeActive) { //if it has been active for less time than the time then it must be deactivated
            GetComponent<RecieverScript>().setInactive();
            time = 0;
            timeActive = 0;
        } else {
            GetComponent<RecieverScript>().setActive();
        }
    }

    public void hitWithLaser() {
        timeActive++;
    }
}
