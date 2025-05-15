using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour {

    [SerializeField] private Dialogue[] dialogues;
    [SerializeField] private int indexConversation = -1;
    [SerializeField] private int indexSentence = -1;

    public void startConversation() {
        indexConversation = 0;
        indexSentence = 0;
    }

    public void goToNextDialogue() {
        indexConversation++;
        indexSentence = 0;
    }

    public void dispSentence(int indexConversation, int indexSentence) {
        Debug.Log(dialogues[indexConversation].getSentence(indexSentence));
    }

    void Start() {
        startConversation();
    }

    void Update() {
        dispSentence(indexConversation, indexSentence);
    }
}
