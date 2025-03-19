using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {
    [SerializeField] private LineRenderer lr;
    [SerializeField] private float noHitMaxDist = 10000f;
    [SerializeField] private float damage = 1f;
    void Start() {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }
    void Update() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right); //cast a ray to the right
        lr.SetPosition(0, transform.position); //sets lr pos
        lr.SetPosition(1, hit ? hit.point : transform.position + transform.right * noHitMaxDist);
        if (hit) {
            if (hit.transform.gameObject.GetComponent<RecieverScript>() != null) { //makes reciever active
                hit.transform.gameObject.GetComponent<RecieverScript>().setActive();
            }
            if (hit.transform.gameObject.GetComponent<Health>() != null) { //does damage
                hit.transform.gameObject.GetComponent<Health>().removeHealth(damage);
            }
        }
    }
}
