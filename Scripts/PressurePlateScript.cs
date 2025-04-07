using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour {
    [SerializeField] private bool on;

    void OnCollisionEnter2D(Collision2D col) {
        on = true;
    }

    void OnCollisionStay2D(Collision2D col) {
        on = true;
    }

    void OnCollisionExit2D(Collision2D col) {
        on = false;
    }

    void Update() {
        if (on) {
            GetComponent<RecieverScript>().setActive();
        } else {
            GetComponent<RecieverScript>().setInactive();
        }
    }
}
