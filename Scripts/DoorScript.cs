using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {
    [SerializeField] private bool isOpen = false;

    public void open() {
        if (!isOpen) transform.position += transform.right * 10;
        isOpen = true;
    }

    public void close() {
        if (isOpen) transform.position -= transform.right * 10;
        isOpen = false;
    }
}
