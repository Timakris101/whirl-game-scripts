using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    private string bulletType;
    private float bulletInitSpeed = 10;
    private float lifetime = 10;
    private float timer = 0;
    private float damage = 1;

    void OnCollisionEnter2D(Collision2D col) { //if bullet hits object damage object
        if (col.gameObject.GetComponent<Health>() != null) {
            col.gameObject.GetComponent<Health>().removeHealth(damage);
        }
        Destroy(gameObject);
    }

    void Update() { //if bullet has outlived its lifetime it dies
        timer += Time.deltaTime;
        if (timer > lifetime) Destroy(gameObject);
    }

    public float getBulletInitSpeed() { //returns init speed factor
        return bulletInitSpeed;
    }
}
