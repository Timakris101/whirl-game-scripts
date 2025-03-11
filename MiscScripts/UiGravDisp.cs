using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGravDisp : MonoBehaviour {
    public float scale;
    void Update() {
        transform.localScale = new Vector3(Camera.main.orthographicSize * scale, Camera.main.orthographicSize * scale, 1); //scales the "arrow" such that it takes up the "scale" (i.e. 1/10) of the camera
        GetComponent<RectTransform>().localPosition = new Vector3(0, -1, 0); //keeps ui "arrow" 1 below the centerof the camera
    }
}
