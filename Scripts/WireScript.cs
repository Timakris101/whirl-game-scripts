using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireScript : MonoBehaviour {
    [SerializeField] private bool isLit = false;
    [SerializeField] private Color litColor;
    [SerializeField] private Color unlitColor;

    void Start() {
        GetComponent<SpriteRenderer>().color = unlitColor;
    }

    new public void light() {
        if (!isLit) GetComponent<SpriteRenderer>().color = litColor;
        isLit = true;
    }

    public void unlight() {
        if (isLit) GetComponent<SpriteRenderer>().color = unlitColor;
        isLit = false;
    }
}
