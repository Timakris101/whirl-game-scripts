using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    private string bulletType;
    private float bulletInitSpeed = 10;

    

    void Start() {
        GetComponent<Rigidbody2D>().velocity = (Vector2) transform.right * bulletInitSpeed;
    }
}
