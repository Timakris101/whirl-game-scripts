using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyLogic : MonoBehaviour {

    [SerializeField] private GameObject bullet;
    [SerializeField] private float timeBetweenShots;
    private float timer = 0;

    void Update() {
        timer += Time.deltaTime;
        if (timer > timeBetweenShots) {
            timer = 0;
            GameObject newBullet = Instantiate(bullet, transform.position + transform.right * 1, transform.rotation);
        }
    }
}
