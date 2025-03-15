using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRecieverScript : MonoBehaviour {
    [SerializeField] private GameObject doorConnected;
    [SerializeField] private int timeActive;
    [SerializeField] private int time;

    void Update() {
        time++;
        if (time > timeActive + 1) { //if it has been active for less time than the time then it must be deactivated
            setInactive();
            time = 0;
            timeActive = 0;
        }
    }

    public void setActive() {
        doorConnected.GetComponent<DoorScript>().open();
        timeActive++;
    }

    public void setInactive() {
        doorConnected.GetComponent<DoorScript>().close();
    }
}
