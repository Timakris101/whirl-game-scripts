using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyLogic : MonoBehaviour {

    [SerializeField] private GameObject bullet;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float searchRad;
    [SerializeField] private float range;
    [SerializeField] private float inaccuracy;
    [SerializeField] private float coolDownTime;
    [SerializeField] private float maxHeat;
    private bool overHeated;
    private float heat;
    private float coolDownTimer;
    private float timer = 0;
    private Vector2 nonRBVel;
    private Vector2 prevPos;

    void Update() {
        nonRBVel = ((Vector2) transform.position - prevPos) / Time.deltaTime;
        timer += Time.deltaTime;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position + transform.right * transform.localScale.x, searchRad, transform.right * transform.localScale.x); //checks for the player in a wide range
        List<GameObject> objsHit = new List<GameObject>();
        foreach (RaycastHit2D hit in hits) {
            objsHit.Add(hit.transform.gameObject);
        }
        GameObject player = GameObject.Find("Player");
        bool playerDetected = objsHit.Contains(player);
        bool playerCanBeHit = false;
        if (playerDetected) {
            LayerMask mask =~ LayerMask.GetMask("Bullet"); //everything but bullets
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * transform.localScale.x * (GetComponent<BoxCollider2D>().size.x / 2 + bullet.GetComponent<CircleCollider2D>().radius * bullet.transform.localScale.x + 0.05f), player.transform.position - transform.position, range, mask); //checks if there is something in between it and the player
            if (hit) playerCanBeHit = hit.transform.gameObject == player;
        }
        if (timer > timeBetweenShots && playerCanBeHit && !overHeated) { //if timer do bullet things
            timer = 0; //reset timer
            spawnBullet();
        } else if (timer > timeBetweenShots) {
            if (heat > 0) heat--;
        }
        if (heat > maxHeat) {
            overHeated = true;
        }
        if (overHeated) {
            coolDownTimer += Time.deltaTime;
        }
        if (coolDownTimer > coolDownTime) {
            overHeated = false;
            coolDownTimer = 0;
            heat = 0;
        }
        prevPos = (Vector2) transform.position;
    }

    void spawnBullet() {
        GameObject newBullet = Instantiate(bullet, transform.position + transform.right * transform.localScale.x * (GetComponent<BoxCollider2D>().size.x / 2 + bullet.GetComponent<CircleCollider2D>().radius * bullet.transform.localScale.x + 0.05f), transform.rotation); //makes a new bullet at the gun launcher area
        newBullet.GetComponent<Rigidbody2D>().velocity = (Vector2) transform.right * transform.localScale.x * newBullet.GetComponent<BulletScript>().getBulletInitSpeed() + gameObject.GetComponent<Rigidbody2D>().velocity + (Vector2) transform.up * Random.Range(-inaccuracy, inaccuracy); //adds proper speed to the bullet
        heat++;
        coolDownTimer = 0;
    }
}
