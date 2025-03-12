using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGravDisp : MonoBehaviour {
    public float scale;
    void Update() {
        float scaleValP = -1 * Camera.main.transform.position.z * Mathf.Tan(Camera.main.fieldOfView * Mathf.PI / 180 / 2) * scale; //persp val to keep arrow the same size on the screen
        float scaleValO = Camera.main.GetComponent<Camera>().orthographicSize * scale; //othro val calculated to make the arrow stay the same size
        float scaleVal = (Camera.main.GetComponent<Camera>().orthographic ? scaleValO : scaleValP); //if cam is othro it uses othro val else it uses persp val
        transform.localScale = new Vector3(scaleVal, scaleVal, 1); //scales the "arrow" such that it takes up the "scale" (i.e. 1/10) of the camera
        GetComponent<RectTransform>().localPosition = new Vector3(0, -1, 0); //keeps ui "arrow" 1 below the center of the camera
        GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0); //makes it face down always
    }
}
