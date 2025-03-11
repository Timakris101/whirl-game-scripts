using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {
    private int min = 1;
    [SerializeField] private int max = 100;
    void Update() {
        Camera camera = gameObject.GetComponent<Camera>();
        camera.orthographicSize -= Input.mouseScrollDelta.y; //adds mouse scroll to cam size
        if (camera.orthographicSize > max) { //makes cam size unable to go above max
            camera.orthographicSize = max;
        }
        if (camera.orthographicSize < min) { //makes cam size unable to go below min
            camera.orthographicSize = min;
        }
    }
}
