using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private GameObject ground; //ground object
    [SerializeField] private int speed; //speed
    [SerializeField] private int wallStickiness; //force attracting to wall
    [SerializeField] private int jumpForce; //jump force
    private Vector3 normalVector; //vector of normal of contact point
    private bool ableToFlutter; //can the player flutter if not in coyote
    [SerializeField] private int flutterCoef; //how effective is the flutter
    private float coyoteTime; //coyote timer
    [SerializeField] private float coyoteMax; //coyote max time
    private bool startCoyoteTime; //is the player in coyote time
    private bool inAir; //is the player in the air
    [SerializeField] private int direction = 1; //which way is the player facing
    private float undirectedlocalScaleX; //place to save the local scale of the x even while it is rotated

    void OnCollisionStay2D(Collision2D col) { //sets the normal vector to the normal of the ground at the point of contact 
        if (col.gameObject != gameObject && col.gameObject.tag.Equals("Surface")) {
            ground = col.gameObject;
            normalVector = col.contacts[col.contacts.Length - 1].normal;
            startCoyoteTime = false;
            coyoteTime = 0;
            inAir = false;
        }
    }

    void OnCollisionExit2D(Collision2D col) { //sets ground to null and allows coyote time
        if (col.gameObject != gameObject && col.gameObject.tag.Equals("Surface")) {
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
        alignToWall();
        stickToWall();
        handleMovement();
        handleJumping();
        handleTurning();
        handleWorldRotation();
    }

    void handleWorldRotation() {
        SpinWorld whirlBallScript = GetComponent<SpinWorld>();
        if (Input.GetKey("q")) { //if q spin counter-clockwise around player
            whirlBallScript.rotateWorld(-whirlBallScript.getSpinSpeed() * Time.deltaTime, transform.position);
        }
        if (Input.GetKey("e")) { //if e spin clockwise around player
            whirlBallScript.rotateWorld(whirlBallScript.getSpinSpeed() * Time.deltaTime, transform.position);
        }
    }

    void handleTurning() { //makes the player face left or right depending on direction
        transform.localScale = new Vector3(direction * undirectedlocalScaleX, transform.localScale.y, transform.localScale.z);
    }

    void handleMovement() {
        GetComponent<Animator>().speed = 0; //makes the animation be stopped  unless there is an input below
        if (Input.GetKey("d")) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y); //sets x speed to zero so it doesn’t keep its speed and move improperly
            GetComponent<Transform>().position += transform.right * speed * Time.deltaTime; //moves right
            GetComponent<Animator>().speed = 1;  //makes the animation for the player play
            direction = 1; //makes the player face right
        }
        if (Input.GetKey("a")) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y); //sets x speed to zero so it doesn’t keep its speed and move improperly
            GetComponent<Transform>().position += transform.right * speed * -1 * Time.deltaTime; //moves left
            GetComponent<Animator>().speed = 1; //makes the animation for the player play
            direction = -1; //makes the player face left
        }
    }

    void handleJumping() { //if w button unpressed or if the player is going downwards  then flutter has to be stopped
        if (!Input.GetKey("w") || GetComponent<Rigidbody2D>().velocity.y < -1) {
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
            GetComponent<Rigidbody2D>().velocity = (Vector2) normalVector * jumpForce; //sets velocity in the “up” direction
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

    void alignToWall() { // if contact with the ground the player aligns to the normal vector of the collision point, else aligns to nothing
        if (ground != null) {
            transform.rotation = Quaternion.LookRotation(transform.forward, normalVector); //Quaternion.LookRotation(forward, up) returns a transform that has the up facing the up value and the forward facing the forward value, the character is being set to look forward and its up vector to be aligned
        } else {
            transform.up = Vector3.up; //makes the transform upwards be global up
        }
    }

    void stickToWall() { //makes velocity go towards the wall if it is in contact with it and removes inbuilt gravity until there is neither coyote time or ground contact
        if (canJump()) {
            removeGrav();
            if (ground != null) {
                GetComponent<Rigidbody2D>().velocity = normalVector * -wallStickiness * Time.deltaTime;
            }
        } else {
            addGrav();
        }
    }

    private bool inCoyote() { //is the player in coyote time
        return coyoteTime < coyoteMax && startCoyoteTime;
    }

    private bool canJump() { //can the player jump
        return ground != null || inCoyote();
    }

    private bool canFlutter() { //can the player "flutter"
        return ableToFlutter || inCoyote();
    }

    public int getDirection() { //getDirection method so other classes can read the value
        return direction;
    }
}
