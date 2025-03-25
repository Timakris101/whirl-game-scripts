using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelScript : MonoBehaviour {
    private GameObject electricityCircle;
    private GameObject wireOutwards;
    private GameObject endWire;
    [SerializeField] private GameObject player;
    private bool endLevel = false;

    void OnTriggerEnter2D(Collider2D col) {
        if (col.transform.tag == "Player") {
            endLevel = true;
            player = col.transform.gameObject;
            col.transform.gameObject.GetComponent<PlayerController>().enabled = false;
        }
    }

    void Start() {
        endWire = transform.Find("EndWire").gameObject;
        wireOutwards = transform.Find("WireOutwards").gameObject;
        electricityCircle = transform.Find("ElectricityCircle").gameObject;
    }

    void Update() {
        if (endLevel) {
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            player.transform.position += (transform.position - player.transform.position) * Time.deltaTime;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 1f);
            if (!(Vector3.Distance(electricityCircle.transform.position, endWire.transform.position) < .1f)) electricityCircle.transform.position += (endWire.transform.position - transform.position) * Time.deltaTime;
            if (!electricityCircle.GetComponent<ParticleSystem>().isPlaying) electricityCircle.GetComponent<ParticleSystem>().Play();
            if (Vector3.Distance(electricityCircle.transform.position, endWire.transform.position) < .1f) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
