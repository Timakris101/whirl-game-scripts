using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour {
    private GameObject menuPanel;

    private GameObject dm;

    void Start() {
        if (GameObject.Find("DialogueManager")) {
            dm = GameObject.Find("DialogueManager");
        } else {
            dm = null;
        }
        menuPanel = transform.GetChild(0).Find("Panel").gameObject;
        menuPanel.SetActive(false);
    }

    public void openMenuButtonClicked() {
        if (dm != null) dm.SetActive(false);
        menuPanel.SetActive(true);
        GameObject.FindWithTag("MainCamera").GetComponent<CameraZoom>().enabled = false;
        transform.GetChild(0).Find("OpenMenuButton").gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    public void continueButtonClicked() {
        if (dm != null) dm.SetActive(true);
        menuPanel.SetActive(false);
        GameObject.FindWithTag("MainCamera").GetComponent<CameraZoom>().enabled = true;
        transform.GetChild(0).Find("OpenMenuButton").gameObject.SetActive(true);    
        Time.timeScale = 1;    
    }

    public void restartButtonClicked() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;    
    }

    public void mainMenuButtonClicked() {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;    
    }
}
