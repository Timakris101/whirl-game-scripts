using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void removeHealth(float amt) {
        health -= amt;
    }
}
