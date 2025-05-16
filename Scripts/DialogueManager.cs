using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour {

    [SerializeField] private Dialogue[] dialogues;
    [SerializeField] private int indexConversation = -1;
    [SerializeField] private int indexSentence = -1;
    [SerializeField] private float indexLetter = -1;
    [SerializeField] private float writingSpeed;
    [SerializeField] private bool conversationOn;

    [SerializeField] private GameObject dialogueTextBox;
    [SerializeField] private GameObject nameTextBox;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject spriteArea;

    public void startConversation() {
        indexConversation = 0;
        indexSentence = 0;
        indexLetter = 0;
        conversationOn = true;
    }

    public void endConversation() {
        dialogueTextBox.GetComponent<TextMeshProUGUI>().text = "";
        nameTextBox.GetComponent<TextMeshProUGUI>().text = "";
        spriteArea.SetActive(false);
        continueButton.SetActive(false);
        conversationOn = false;
    }

    public void goToNextDialogue() {
        indexConversation++;
        indexSentence = 0;
        indexLetter = 0;

        if (indexConversation >= dialogues.Length) {
            endConversation();
        }
    }

    public void goToNextSentence() {
        indexSentence++;
        indexLetter = 0;

        if (indexSentence >= dialogues[indexConversation].getSentences().Length) {
            goToNextDialogue();
        }
    }

    public void clickedContinue() {
        if (conversationOn) {
            if (sentenceComplete()) {
                goToNextSentence();
            } else {
                indexLetter = dialogues[indexConversation].getSentence(indexSentence).Length;
            }
        }
    }

    public void dispDialogue() {
        if (!sentenceComplete()) {
            indexLetter += Time.deltaTime * writingSpeed;
        }
        nameTextBox.GetComponent<TextMeshProUGUI>().text = dialogues[indexConversation].getName() + ": ";
        dialogueTextBox.GetComponent<TextMeshProUGUI>().text = dialogues[indexConversation].getSentence(indexSentence).Substring(0, (int) indexLetter);
        spriteArea.GetComponent<UnityEngine.UI.Image>().sprite = dialogues[indexConversation].getSprite();
    }

    private bool sentenceComplete() {
        return !(indexLetter < dialogues[indexConversation].getSentence(indexSentence).Length);
    }

    void Start() {
        startConversation();
    }

    void Update() {
        if (conversationOn) dispDialogue();
    }
}
