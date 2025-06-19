using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour {
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    private static float resetDelay = 1f;
    private float timer; //resets with new script

    [SerializeField] private GameObject deathEffect;

    void Start() {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update() {
        if (gameObject == Camera.main.gameObject && Camera.main.transform.parent == null) {
            handleSeperatedCam();

        } else {
            if (health <= 0) {
                if (deathEffect != null) Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity);
                if (gameObject.transform.tag != "Player") Destroy(gameObject);

                Camera.main.transform.SetParent(null, true);
                Camera.main.gameObject.AddComponent<Health>();
                Destroy(gameObject);
            }
            if (transform.position.y < -10000) health = 0; //kills obj if they are too low
        }
    }

    public void handleSeperatedCam() {
        timer += Time.deltaTime;
        if (timer > resetDelay) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void removeHealth(float amt) {
        health -= amt;
    }

    public float getHealth() {
        return health;
    }
}
