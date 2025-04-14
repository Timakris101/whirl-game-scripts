using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanScript : MonoBehaviour {

    [SerializeField] private float affectDist;
    [SerializeField] private float strength;

    void Start() {
        GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x, affectDist);
        GetComponent<BoxCollider2D>().offset = new Vector2(0, affectDist / 2);
    }

    void OnTriggerStay2D(Collider2D col) {
        if (col.transform.tag != "Surface" && col.transform.tag != "Ungrabbable") col.gameObject.GetComponent<Rigidbody2D>().velocity += (Vector2) transform.up * strength * Time.deltaTime;
    }
}
