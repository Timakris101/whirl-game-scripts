using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGravDisp : MonoBehaviour {
    public float scale;
    void Update() {
        transform.localScale = new Vector3(Camera.main.orthographicSize * scale, Camera.main.orthographicSize * scale, 1);
        GetComponent<RectTransform>().localPosition = new Vector3(0, -1, 0);
    }
}
