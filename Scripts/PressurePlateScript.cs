using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour {
    [SerializeField] private bool on;
    [SerializeField] private List<GameObject> contacts;

    void OnCollisionEnter2D(Collision2D col) {
        contacts.Add(col.gameObject);
    }

    void OnCollisionExit2D(Collision2D col) {
        contacts.Remove(col.gameObject);
    }

    void Update() {
        on = contacts.Count != 0;
        if (on) {
            GetComponent<RecieverScript>().setActive();
        } else {
            GetComponent<RecieverScript>().setInactive();
        }
    }
}
