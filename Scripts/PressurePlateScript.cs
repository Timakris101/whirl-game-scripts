using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour {
    [SerializeField] private bool on;
    [SerializeField] private List<GameObject> contacts;

    void OnCollisionEnter2D(Collision2D col) {
        on = true;
        contacts.Add(col.gameObject);
    }

    void OnCollisionStay2D(Collision2D col) {
        on = true;
    }

    void OnCollisionExit2D(Collision2D col) {
        contacts.Remove(col.gameObject);
        if (contacts.Count == 0) on = false;
    }

    void Update() {
        if (on) {
            GetComponent<RecieverScript>().setActive();
        } else {
            GetComponent<RecieverScript>().setInactive();
        }
    }
}
