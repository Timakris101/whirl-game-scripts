using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelScript : MonoBehaviour {
    private GameObject electricityCircle;
    private GameObject wireOutwards;
    private GameObject endWire;
    private GameObject player;
    private bool endLevel = false;
    [SerializeField] private float delay = 1f;
    private float timer;
    private Vector3 whereStop;


    void OnTriggerEnter2D(Collider2D col) {
        if (col.transform.tag == "Player") {
            endLevel = true;
            player = col.transform.gameObject;
            col.transform.gameObject.GetComponent<PlayerController>().enabled = false;
            whereStop = player.transform.position;
        }
    }

    void Start() {
        endWire = transform.Find("EndWire").gameObject;
        wireOutwards = transform.Find("WireOutwards").gameObject;
        electricityCircle = transform.Find("ElectricityCircle").gameObject;
    }

    void Update() {
        if (endLevel) {
            timer += Time.deltaTime;
            player.transform.position = whereStop;
            player.GetComponent<CircleCollider2D>().enabled = false;
            player.GetComponent<SpriteRenderer>().enabled = false;
            player.GetComponent<ParticleSystem>().Stop();
            if (timer > delay) {
                if (!(Vector3.Distance(electricityCircle.transform.position, endWire.transform.position) < .1f)) electricityCircle.transform.position += (endWire.transform.position - transform.position) * Time.deltaTime;
                if (!electricityCircle.GetComponent<ParticleSystem>().isPlaying) electricityCircle.GetComponent<ParticleSystem>().Play();
                if (Vector3.Distance(electricityCircle.transform.position, endWire.transform.position) < .1f) {
                    finishLevel();
                }
            }
        }
    }

    public static void finishLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        if (SceneManager.GetSceneByName("MainMenu").buildIndex == SceneManager.GetActiveScene().buildIndex + 1) PlayerPrefs.SetInt("mostRecentSceneIndex", 0); //if you win (beat level right before mainmenu) then you win the game and your savedscene resets
    }
}
