using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMatchingScript : MonoBehaviour {
    void Start() {
       gameObject.GetComponent<BoxCollider2D>().size = gameObject.GetComponent<SpriteRenderer>().size;
    }
}
