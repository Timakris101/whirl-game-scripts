using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour {

    [SerializeField] private float explosiveRange;
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    void OnCollisionEnter2D(Collision2D col) { //if bullet hits object damage object
        Collider2D[] cols = Physics2D.OverlapCircleAll((Vector2) transform.position, explosiveRange);
        for (int i = 0; i < cols.Length; i++) {
            if (cols[i].gameObject.GetComponent<Health>() != null) {
                cols[i].gameObject.GetComponent<Health>().removeHealth(damage);
            }
        }
        Destroy(gameObject);
    }

    public float getSpeed() {
        return speed;
    }

    void Update() {
        transform.up = GetComponent<Rigidbody2D>().velocity.normalized;
    }
}
