using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    void Start() {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update() {
        if (health <= 0) {
            if (transform.tag != "Player") {
                Destroy(gameObject);
            } else {
                //TODO: player kill
            }
        }
    }

    public void removeHealth(float amt) {
        health -= amt;
    }
}
