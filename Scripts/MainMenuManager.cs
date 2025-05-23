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
        continueButton.SetActive(PlayerPrefs.GetInt("mostRecentSceneIndex") == 1);
    }

    public void restart() {
        PlayerPrefs.SetInt("mostRecentSceneIndex", 0);
        load();
    }

    public void load() {
        SceneManager.LoadScene(PlayerPrefs.GetInt("mostRecentSceneIndex"));
    }

    public void quit() {
        Application.Quit();
    }
}
