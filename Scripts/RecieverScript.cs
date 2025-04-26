using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecieverScript : MonoBehaviour {
    [SerializeField] private GameObject doorConnected;
    [SerializeField] private List<GameObject> wiresConnected;

    public void setActive() {
        doorConnected.GetComponent<DoorScript>().open();
        foreach (GameObject wireConnected in wiresConnected) wireConnected.GetComponent<WireScript>().light();
    }

    public void setInactive() {
        doorConnected.GetComponent<DoorScript>().close();
        foreach (GameObject wireConnected in wiresConnected) wireConnected.GetComponent<WireScript>().unlight();
    }
}
