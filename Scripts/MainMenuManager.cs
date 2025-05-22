using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    private GameObject continueButton;

    void Start() {
        continueButton = transform.Find("ContinueButton").gameObject;
    }

    void Update() {
        if (PlayerPrefs.GetInt("mostRecentSceneIndex") == 0) {
            continueButton.SetActive(false);
        } else {
            continueButton.SetActive(true);
        }
    }
}
