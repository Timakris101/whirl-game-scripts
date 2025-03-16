using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {
    [SerializeField] private bool isOpen = false;
    [SerializeField] private Vector3 unOpenedScale;

    void Start() {
        unOpenedScale = transform.localScale;
    }

    public void open() {
        if (!isOpen) transform.localScale = new Vector3(0, 0, 0);
        isOpen = true;
    }

    public void close() {
        if (isOpen) transform.localScale = unOpenedScale;
        isOpen = false;
    }
}
