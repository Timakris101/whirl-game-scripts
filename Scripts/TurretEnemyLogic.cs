using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyLogic : MonoBehaviour {

    [SerializeField] private GameObject bullet;
    [SerializeField] private float timeBetweenShots;
    private float timer = 0;

    void Update() {
        timer += Time.deltaTime;
        if (timer > timeBetweenShots) { //if timer do bullet things
            timer = 0; //reset timer
            spawnBullet();
        }
    }

    void spawnBullet() {
        GameObject newBullet = Instantiate(bullet, transform.position + transform.right * transform.localScale.x, transform.rotation); //makes a new bullet at the gun launcher area
        newBullet.GetComponent<Rigidbody2D>().velocity = (Vector2) transform.right * transform.localScale.x * newBullet.GetComponent<BulletScript>().getBulletInitSpeed() + GetComponent<Rigidbody2D>().velocity; //adds proper speed to the bullet
    }
}
