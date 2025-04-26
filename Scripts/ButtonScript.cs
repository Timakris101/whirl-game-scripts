using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D col) {
        GetComponent<RecieverScript>().setActive();
    }
}
