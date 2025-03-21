using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravBridgeScript : MonoBehaviour {
    private Vector3 startAngles;
    [SerializeField] private GameObject wall;
    [SerializeField] private float wallThickness = .2f;

    void Start() {
        startAngles = transform.eulerAngles;
        wall = transform.GetChild(0).gameObject;
    }

    void Update() {
        transform.eulerAngles = startAngles;
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, transform.up);
        Vector3 upEndPoint = hitUp.point; //"up" end point of wall
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, -transform.up);
        Vector3 downEndPoint = hitDown.point; //"down" end point of wall
        Vector3 wallPos = (upEndPoint + downEndPoint) / 2; //avg between the two
        float wallLength = Vector3.Distance(upEndPoint, downEndPoint); //distance between up and down points
        wall.transform.position = wallPos;
        wall.transform.localScale = new Vector3(wallThickness, wallLength, 1);
    }
}
