using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravBridgeScript : MonoBehaviour {
    private Vector3 startAngles;
    [SerializeField] private GameObject wall;
    [SerializeField] private float wallThickness = .2f;
    [SerializeField] private float maxDist = 1000f;

    void Start() {
        startAngles = transform.eulerAngles;
        wall = transform.GetChild(0).gameObject;
    }

    void Update() {
        transform.eulerAngles = startAngles;

        Vector3 upEndPoint = positionOfHitInDir(1);
        Vector3 downEndPoint = positionOfHitInDir(-1);

        Vector3 wallPos = (upEndPoint + downEndPoint) / 2; //avg between the two
        float wallLength = Vector3.Distance(upEndPoint, downEndPoint); //distance between up and down points
        wall.transform.position = wallPos;
        wall.transform.localScale = new Vector3(wallThickness, wallLength, 1);
    }

    private Vector3 positionOfHitInDir(int dir) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir * transform.up); //checks all raycasts in a dir
        foreach(RaycastHit2D hit in hits) {
            if (hit.transform.gameObject != transform.GetChild(0).gameObject) {
                if (hit.transform.tag == "Surface") { //if its not a surface dont stop the bridge
                    return hit.point;
                }
            }
        }
        return transform.position + transform.up * maxDist * dir; //if it hits nothing it goes veeerrryyy far
    }
}
