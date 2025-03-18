using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiveWallScript : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D col) {
        if (col.transform.tag != "Surface") {
            if (col.gameObject.GetComponent<Health>() != null) {
                col.gameObject.GetComponent<Health>().removeHealth(1);
            }
        }
    }

    void OnCollisionStay2D(Collision2D col) {
        if (col.transform.tag != "Surface") {
            if (col.gameObject.GetComponent<Health>() != null) {
                col.gameObject.GetComponent<Health>().removeHealth(1);
            }
        }
    }
}
