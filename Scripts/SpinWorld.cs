using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWorld : MonoBehaviour {
    private static float spinSpeed = 1f;

    void Start() {
        Cursor.visible = false;
    }

    public static void rotateWorld(int direction, Vector3 pivot) {// direction is either 1 or -1
        float spinAmt = direction * spinSpeed * Time.deltaTime;
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>(); //finds all gameObjects
        foreach (GameObject obj in allObjects) { //loops through all gameObjects
            if (obj.layer != 3 && obj.layer != 5) { //ignores the object if it is layer of "Ignore rotation" or "UI"
                float relativeX = obj.transform.position.x - pivot.x; //gets the position of the object relative to the pivot
                float relativeY = obj.transform.position.y - pivot.y;
                float newRelativeX = relativeX * Mathf.Cos(spinAmt) - relativeY * Mathf.Sin(spinAmt); //uses rotation matrices to rotate the world
                float newRelativeY = relativeX * Mathf.Sin(spinAmt) + relativeY * Mathf.Cos(spinAmt);
                obj.transform.position = new Vector3(newRelativeX + pivot.x, newRelativeY + pivot.y, obj.transform.position.z); //sets position to new position
                obj.transform.localEulerAngles += new Vector3(0, 0, spinAmt * 180 / Mathf.PI); //rotates the object to make it face the right way
            }
        }
    }

    public float getSpinSpeed() {
        return spinSpeed;
    }
}
