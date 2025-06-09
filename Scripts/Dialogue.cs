using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour {
    [SerializeField] string[] sentences;
    [SerializeField] new private string name; 
    [SerializeField] private Sprite sprite;

    public string getSentence(int index) {
        return sentences[index];
    }

    public string[] getSentences() {
        return sentences;
    }

    public string getName() {
        return name;
    }

    public Sprite getSprite() {
        return sprite;
    }
}
