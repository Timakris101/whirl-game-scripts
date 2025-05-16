using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour {

    private Dialogue[] dialogues;
    [SerializeField] private bool startAtStartOfLevel;
    [SerializeField] private bool useForCutscene;
    private int indexConversation = -1;
    private int indexSentence = -1;
    private float indexLetter = -1;
    [SerializeField] private float writingSpeed = 10;
    private bool conversationOn;

    private GameObject dialogueTextBox;
    private GameObject nameTextBox;
    private GameObject continueButton;
    private GameObject spriteArea;

    public void startConversation() {
        indexConversation = 0;
        indexSentence = 0;
        indexLetter = 0;
        transform.GetChild(0).gameObject.SetActive(true);
        conversationOn = true;
    }

    private void clear() {
        dialogueTextBox.GetComponent<TextMeshProUGUI>().text = "";
        nameTextBox.GetComponent<TextMeshProUGUI>().text = "";
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void endConversation() {
        clear();
        conversationOn = false;
        if (useForCutscene) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void goToNextDialogue() {
        indexConversation++;
        indexSentence = 0;
        indexLetter = 0;

        if (indexConversation >= dialogues.Length) {
            endConversation();
        }
    }

    private void goToNextSentence() {
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

    private void dispDialogue() {
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
        dialogues = GetComponents<Dialogue>();
        dialogueTextBox = transform.GetChild(0).GetChild(0).Find("DialogueTextBox").gameObject;
        nameTextBox = transform.GetChild(0).GetChild(0).Find("NameTextBox").gameObject;
        continueButton = transform.GetChild(0).GetChild(0).Find("ContinueButton").gameObject;
        spriteArea = transform.GetChild(0).GetChild(0).Find("SpriteArea").gameObject;

        clear();
        if (startAtStartOfLevel) startConversation();
    }

    void Update() {
        if (conversationOn) dispDialogue();
    }
}
