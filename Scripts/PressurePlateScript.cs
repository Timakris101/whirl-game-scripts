using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour {
    [SerializeField] private bool on;

    void OnCollisionStay2D(Collision2D col) {
        if (col.transform.tag != "Surface") {
            on = true;
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        if (col.transform.tag != "Surface") {
            on = false;
        }
    }

    void Update() {
        if (on) {
            GetComponent<RecieverScript>().setActive();
        } else {
            GetComponent<RecieverScript>().setInactive();
        }
    }
}
