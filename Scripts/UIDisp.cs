using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiDisp : MonoBehaviour {
    private Vector3 localPos;
    [SerializeField] private bool rotIsLocal;
    [SerializeField] private float scale;
    void Start() {
        localPos = transform.position - transform.parent.position;
    }
    void Update() {
        float scaleValP = -1 * Camera.main.transform.position.z * Mathf.Tan(Camera.main.fieldOfView * Mathf.PI / 180 / 2) * scale; //persp val to keep text the same size on the screen
        float scaleValO = Camera.main.GetComponent<Camera>().orthographicSize * scale; //othro val calculated to make the text stay the same size
        float scaleVal = (Camera.main.GetComponent<Camera>().orthographic ? scaleValO : scaleValP); //if cam is othro it uses othro val else it uses persp val
        transform.localScale = new Vector3(transform.parent.GetComponent<PlayerController>().getDirection() * scaleVal, scaleVal, 1); //scales the text such that it takes up the "scale" (i.e. 1/10) of the camera
        GetComponent<RectTransform>().localPosition = localPos; //keeps ui text certain amt away from the center of the camera
        if (!rotIsLocal) GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0); //makes it face down always
    }
}
