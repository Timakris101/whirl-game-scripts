using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {
    private int min = 1;
    [SerializeField] private int max = 100;
    void Update() {
        Camera camera = gameObject.GetComponent<Camera>();
        camera.orthographicSize -= Input.mouseScrollDelta.y;
        if (camera.orthographicSize > max) {
            camera.orthographicSize = max;
        }
        if (camera.orthographicSize < min) {
            camera.orthographicSize = min;
        }
    }
}
