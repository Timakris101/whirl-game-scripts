using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {
    private GameObject continueButton;

    void Start() {
        continueButton = transform.Find("ContinueButton").gameObject;
    }

    void Update() {
        if (PlayerPrefs.GetInt("maxSceneIndexThisSave") == 0) {
            continueButton.SetActive(false);
        } else {
            continueButton.SetActive(true);
        }
    }
}
