using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
    [SerializeField] private float bulletInitSpeed = 10;
    [SerializeField] private float lifetime = 10;
    [SerializeField] private float timer = 0;
    [SerializeField] private float damage = 1;
    [SerializeField] private bool bouncy;
    [SerializeField] private int maxBounceAmt;
    [SerializeField] private GameObject bulletEffect;
    private int bounceAmt = 0;

    void OnCollisionEnter2D(Collision2D col) { //if bullet hits object damage object
        if (col.gameObject.GetComponent<Health>() != null) {
            col.gameObject.GetComponent<Health>().removeHealth(damage);
        }
        if (!bouncy) {
            killBullet();
        } else {
            bounceAmt++;
            if (bounceAmt > maxBounceAmt) {
                killBullet();
            }
        }
    }

    private void killBullet() {
        Instantiate(bulletEffect, gameObject.transform.position, Quaternion.identity);
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
