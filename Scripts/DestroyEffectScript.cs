using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffectScript : MonoBehaviour {

    [SerializeField] private float delay;

    void Update() {
        Destroy(gameObject, delay);
    }
}
