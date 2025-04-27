using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {
    private int minO = 1; //min for ortho
    [SerializeField] private int maxO = 100; //max for ortho
    [SerializeField] private int minP = 10; //min for perspective
    [SerializeField] private int maxP = 90; //max for perspective

    void Start() {
        Camera.main.fieldOfView = PlayerPrefs.GetFloat("fov"); //makes the fov at start of scene how it was before
    }

    void Update() {
        Camera camera = gameObject.GetComponent<Camera>();
        PlayerPrefs.SetFloat("fov", camera.fieldOfView); //saves camera zoom in playerpref
        if (camera.orthographic) {
            camera.orthographicSize -= Input.mouseScrollDelta.y; //adds mouse scroll to cam size
            if (camera.orthographicSize > maxO) { //makes cam size unable to go above max
                camera.orthographicSize = maxO;
            }
            if (camera.orthographicSize < minO) { //makes cam size unable to go below min
                camera.orthographicSize = minO;
            }
        } else {
            camera.fieldOfView -= 2 * Mathf.Atan(Input.mouseScrollDelta.y); //adds mouse scroll to cam fov, tangent for smooth cam movement
            if (camera.fieldOfView > maxP) { //makes cam size unable to go above max
                camera.fieldOfView = maxP;
            }
            if (camera.fieldOfView < minP) { //makes cam size unable to go below min
                camera.fieldOfView = minP;
            }
        }
        if (!Input.GetKey("c")) { //if c is pressed it makes the camera face the local down and if not the global down of the object it is attached to
            transform.eulerAngles = new Vector3(0, 0, 0);
        } else {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
}
