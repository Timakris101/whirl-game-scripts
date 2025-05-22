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
}
