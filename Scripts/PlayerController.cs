using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour {
    [SerializeField] private GameObject ground; //ground object
    private Vector3 contactPoint;
    [SerializeField] private int speed; //speed
    [SerializeField] private int jumpForce; //jump force
    private Vector3 normalVector; //vector of normal of contact point
    private bool ableToFlutter; //can the player flutter if not in coyote
    private int flutterCoef = 70; //how effective is the flutter
    private float coyoteTime; //coyote timer
    private float coyoteMax = .1f; //coyote max time
    private bool startCoyoteTime; //is the player in coyote time
    private bool inAir; //is the player in the air
    private int direction = 1; //which way is the player facing
    private float undirectedlocalScaleX; //place to save the local scale of the x even while it is rotated
    [SerializeField] private float playerSpinSpeed; //how fast the player rotates towards the alignment vector
    [SerializeField] private float worldSpinSpeed; //spins world at speed
    private GameObject grabbedObj;
    [SerializeField] private float grabRad;
    private Vector3 grabbedObjUndisturbedScale;

    [SerializeField] private bool defectiveTracks;


    void OnCollisionStay2D(Collision2D col) { //sets the normal vector to the normal of the ground at the point of contact 
        if (col.gameObject != gameObject) {
            ground = col.gameObject;
            normalVector = col.contacts[0].normal;
            contactPoint = col.contacts[0].point;
            startCoyoteTime = false;
            coyoteTime = 0;
            inAir = false;
        }
    }

    void OnCollisionExit2D(Collision2D col) { //sets ground to null and allows coyote time
        if (col.gameObject != gameObject) {
            ground = null;
            startCoyoteTime = true;
            coyoteTime = 0;
            inAir = true;
        }
    }

    void Start() { //saves the x scale to not cause issues when turning the player model
        undirectedlocalScaleX = transform.localScale.x;
    }

    void Update() {
        if (!defectiveTracks) {
            handleMovement();
        } else {
            GetComponent<Animator>().speed = 0; //no track mvmnt
        }
        handleJumping();
        handleAlignment();
        handleSticking();
        handleTurning();
        handleWorldRotation();
        handleGrabbing();
    }

    void handleGrabbing() {
        bool canGrab = false;
        int whichCanGrabIndex = -1;
        Transform grabArea = transform.Find("GrabArea");
        Collider2D[] cols = Physics2D.OverlapCircleAll((Vector2) grabArea.position, grabRad);
        for (int i = 0; i < cols.Length; i++) { //checks all objects around area and selects the grabbable one
            if (cols[i] != null) {
                if (cols[i].transform != grabArea && cols[i].transform.tag != "Ungrabbable" && cols[i].transform.tag != "Surface" && cols[i].gameObject != gameObject) { //checks for grabbability
                    canGrab = true;
                    whichCanGrabIndex = i;
                    break;
                }
            }
        }
        if (Input.GetMouseButtonDown(0)) {
            if (grabbedObj == null && canGrab) { //grabs obj
                grabbedObj = cols[whichCanGrabIndex].gameObject;
                grabbedObjUndisturbedScale = cols[whichCanGrabIndex].transform.localScale;
            } else { //ungrabs obj
                grabbedObj = null;
            }
        }
        if (grabbedObj != null) { //moves grabbed obj to position and rotation
            grabbedObj.transform.localScale = new Vector3(direction * grabbedObjUndisturbedScale.x, grabbedObjUndisturbedScale.y, grabbedObjUndisturbedScale.z);
            grabbedObj.transform.eulerAngles = grabArea.eulerAngles;
            grabbedObj.transform.position = grabArea.position;
            if (ground != null) {
                grabbedObj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            } else {
                grabbedObj.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            }
        }
    }

    void handleWorldRotation() {
        ParticleSystem reactor = GetComponent<ParticleSystem>(); //particle system that plays "reactor" effect
        bool worldRotating = false; //checks if there is need to ook at particlesystem
        if (Input.GetKey("q")) { //if q spin counter-clockwise around player
            SpinWorld.rotateWorld(1 * worldSpinSpeed * Time.deltaTime, transform.position);
            worldRotating = !worldRotating; //if input on only one of the keys then we want to look at particlesystem
        }
        if (Input.GetKey("e")) { //if e spin clockwise around player
            SpinWorld.rotateWorld(-1 * worldSpinSpeed * Time.deltaTime, transform.position);
            worldRotating = !worldRotating; //if input on only one of the keys then we want to look at particlesystem
        }
        if (worldRotating) { //if input then we want to look at particlesystem
            if (!reactor.isPlaying) reactor.Play(false); //if it isn't already playing play so it doesn't constantly reset
        } else {
            reactor.Stop(false);
        }
    }

    void handleTurning() { //makes the player face left or right depending on direction
        transform.localScale = new Vector3(direction * undirectedlocalScaleX, transform.localScale.y, transform.localScale.z);
    }

    void handleMovement() {
        GetComponent<Animator>().speed = 0; //makes the animation be stopped  unless there is an input below
        bool canMove = true;
        bool moved = false;
        if (ground != null && !aligned()) canMove = false; //if you are on ground and not aligned with vector you cant move
        if (Input.GetKey("d") && canMove) {
            moved = true;
            GetComponent<Transform>().position += transform.right * speed * Time.deltaTime; //moves right
            direction = 1; //makes the player face right
        }
        if (Input.GetKey("a") && canMove) {
            moved = true;
            GetComponent<Transform>().position += transform.right * speed * -1 * Time.deltaTime; //moves left
            direction = -1; //makes the player face left
        }
        if (moved) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y); //sets x speed lower so it doesn’t keep its speed and move improperly
            if (GetComponent<Animator>().speed == 0) {//makes the animation for the player play, if only a or d
                GetComponent<Animator>().speed = 1;  
            } else {
                GetComponent<Animator>().speed = 0;
            }
        }
    }

    void handleJumping() {
        if (!Input.GetKey("w") || GetComponent<Rigidbody2D>().velocity.y < -1) { //if w button unpressed or if the player is going downwards then flutter has to be stopped
            ableToFlutter = false;
        }
        if (Input.GetKey("w") && canFlutter()) { //adds an extra force if w button is being held
            GetComponent<Rigidbody2D>().AddForce(transform.up * jumpForce / flutterCoef, ForceMode2D.Force);
            ableToFlutter = true;
        }
        if (startCoyoteTime) { //if it is coyote time the coyote timer increases
            coyoteTime += Time.deltaTime;
        }
        if (Input.GetKeyDown("w") && canJump() && (!inAir || inCoyote())) { //if player can jump then they jump
            GetComponent<Rigidbody2D>().velocity += (Vector2) transform.up * jumpForce; //sets velocity in the “up” direction
            transform.position += (Vector3) GetComponent<Rigidbody2D>().velocity * Time.deltaTime; //moves the player off of the ground to prevent double jump bugs because the code could read that it is still on the ground when it shouldn't be
            ableToFlutter = true; //makes player able to flutter
            ground = null; //make sure it knows it is no longer on the ground
            coyoteTime = 0;  //if you jump you are no longer in coyote time
            startCoyoteTime = false;
        }
    }

    void addGrav() { //sets built in gravity scale  of player to 1
        GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    void removeGrav() { //removes built in gravity of player
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    void handleAlignment() { // if contact with the ground the player aligns to the normal vector of the collision point, else aligns to nothing
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        if (ground != null) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(transform.forward, normalVector), playerSpinSpeed * Time.deltaTime); //Rotates the transform towards the which Quaternion.LookRotation(forward, up) returns a transform that has the up facing the up value and the forward facing the forward value, the character is being set to look forward and its up vector to be aligned
        } else {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(transform.forward, Vector3.up), playerSpinSpeed * Time.deltaTime); //Rotates the transform to make its upwards be global up
        }
    }

    void handleSticking() { //makes velocity go towards the wall if it is in contact with it and removes inbuilt gravity until there is neither coyote time or ground contact
        if (ground != null) {
            removeGrav();
            if (ground.GetComponent<Rigidbody2D>() != null) {
                gameObject.GetComponent<Rigidbody2D>().velocity = ground.GetComponent<Rigidbody2D>().GetPointVelocity(contactPoint);
                ground.GetComponent<Rigidbody2D>().AddForceAtPosition(normalVector / ground.GetComponent<Rigidbody2D>().mass, contactPoint, ForceMode2D.Force);
                gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(-normalVector / GetComponent<Rigidbody2D>().mass, contactPoint, ForceMode2D.Force);
            } else {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        } else {
            addGrav();
        }
    }

    private bool inCoyote() { //is the player in coyote time
        return coyoteTime < coyoteMax && startCoyoteTime;
    }

    private bool canJump() { //can the player jump
        return (ground != null || inCoyote()) && aligned();
    }

    private bool aligned() {
        return transform.rotation == Quaternion.LookRotation(transform.forward, normalVector);
    }

    private bool canFlutter() { //can the player "flutter"
        return (ableToFlutter || inCoyote()) && inAir; //must be in air to flutter
    }

    public int getDirection() { //getDirection method so other classes can read the value
        return direction;
    }
}
