using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuteScript : MonoBehaviour {
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float timer;
    [SerializeField] private GameObject thingToSpawn;
    [SerializeField] private float speedToSpawnWith;
    [SerializeField] private bool on = true;

    void Update() {
        timer += Time.deltaTime;
        if (timer > timeBetweenShots && on) {
            timer = 0;
            shoot();
        }
    }

    public void turnOff() {
        on = false;
    }

    private void shoot() {
        GameObject newThing = Instantiate(thingToSpawn, transform.position, transform.rotation);
        newThing.GetComponent<Rigidbody2D>().velocity = transform.right * speedToSpawnWith;
    }
}
