using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour {
    public void restart() {
        PlayerPrefs.SetInt("maxSceneIndexThisSave", 0);
        load();
    }

    public void load() {
        SceneManager.LoadScene(PlayerPrefs.GetInt("maxSceneIndexThisSave"));
    }

    void Update() {
        if (SceneManager.GetActiveScene().name != "MainMenu") {
            if (PlayerPrefs.GetInt("maxSceneIndexThisSave") < SceneManager.GetActiveScene().buildIndex) {
                PlayerPrefs.SetInt("maxSceneIndexThisSave", SceneManager.GetActiveScene().buildIndex);
            } 
        }
    }
}
