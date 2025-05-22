using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour {
    public void restart() {
        PlayerPrefs.SetInt("mostRecentSceneIndex", 0);
        load();
    }

    public void load() {
        SceneManager.LoadScene(PlayerPrefs.GetInt("mostRecentSceneIndex"));
    }

    void Update() {
        if (SceneManager.GetActiveScene().name != "MainMenu") { //this makes it so the only way your sceneindex can be at the mainmenu is victory
            if (PlayerPrefs.GetInt("mostRecentSceneIndex") != SceneManager.GetActiveScene().buildIndex) {
                PlayerPrefs.SetInt("mostRecentSceneIndex", SceneManager.GetActiveScene().buildIndex);
            } 
        }
    }
}
