using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyLogic : MonoBehaviour {

    [SerializeField] private GameObject bullet;
    [SerializeField] private float timeBetweenShots;
    private float timer = 0;
    private Vector2 nonRBVel;
    private Vector2 prevPos;

    void Update() {
        nonRBVel = ((Vector2) transform.position - prevPos) / Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > timeBetweenShots) { //if timer do bullet things
            timer = 0; //reset timer
            spawnBullet();
        }
        prevPos = (Vector2) transform.position;
    }

    void spawnBullet() {
        GameObject newBullet = Instantiate(bullet, transform.position + transform.right * transform.localScale.x, transform.rotation); //makes a new bullet at the gun launcher area
        newBullet.GetComponent<Rigidbody2D>().velocity = (Vector2) transform.right * transform.localScale.x * newBullet.GetComponent<BulletScript>().getBulletInitSpeed() + gameObject.GetComponent<Rigidbody2D>().velocity; //adds proper speed to the bullet
    }
}
