using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {

    private string type;
    private bool running;
    private float counter;
    private float timer;

    private float maxMissileCount = 2;
    private GameObject missile;
    private float missileInaccuracy = .1f;

    private float maxLaserTime = 1;
    private float laserWidth = .5f;
    private float laserDamage = 1;

    private float maxBulletCount = 10;
    private GameObject bullet;
    private float bulletInaccuracy = .5f;

    private float maxTurretCount = 2;

    private float delay;

    private GameObject boss;

    public Action(string type, GameObject boss) {
        this.type = type;
        this.boss = boss;
        bullet = boss.GetComponent<BossScript>().getBullet();
        missile = boss.GetComponent<BossScript>().getMissile();
        running = false;
        counter = 0;

        if (type.Equals("missile")) delay = 1f;
        if (type.Equals("laser")) delay = 0f;
        if (type.Equals("bullet")) delay = .1f;
        if (type.Equals("turret")) delay = 1f;
    }

    public void runAction() {
        if (running) {
            timer += Time.deltaTime;
            if (timer > delay) {
                timer = 0;
                if (type == "missile") {
                    GameObject newMissile = GameObject.Instantiate(missile, boss.transform.Find("MissileSpawn").position, boss.transform.rotation); //makes a new missile at the launcher area
                    newMissile.GetComponent<Rigidbody2D>().velocity = (Vector2) (-boss.transform.up) * newMissile.GetComponent<MissileScript>().getSpeed() + (Vector2) boss.transform.right * Random.Range(-missileInaccuracy, missileInaccuracy); //adds proper speed to the missile
                    counter++;
                    if (counter > maxMissileCount) {
                        endAction();
                    }
                }
                if (type == "laser") {
                    Direction dir = boss.GetComponent<BossScript>().getDirFacing();
                    Vector2 positionLaserStart = boss.transform.Find(dir.getName() + "LaserSpawn").position;
                    RaycastHit2D hit = Physics2D.CircleCast(positionLaserStart + dir.getVectorInDir() * laserWidth, laserWidth, dir.getVectorInDir()); //cast a ray to the right
                    boss.GetComponent<LineRenderer>().SetPosition(0, positionLaserStart); //sets lr pos
                    boss.GetComponent<LineRenderer>().SetPosition(1, hit.point);
                    boss.GetComponent<LineRenderer>().SetWidth(laserWidth, laserWidth);
                    if (hit) {
                        if (hit.transform.gameObject.GetComponent<Health>() != null) { //does damage
                            hit.transform.gameObject.GetComponent<Health>().removeHealth(laserDamage);
                        }
                    }

                    counter += Time.deltaTime;
                    if (counter > maxLaserTime) {
                        boss.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
                        boss.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
                        endAction();
                    }
                }
                if (type == "bullet") {
                    GameObject newBullet = GameObject.Instantiate(bullet, boss.transform.Find("BulletSpawn").position, boss.transform.rotation); //makes a new bullet at the gun launcher area
                    newBullet.GetComponent<Rigidbody2D>().velocity = (Vector2) (-boss.transform.up) * newBullet.GetComponent<BulletScript>().getBulletInitSpeed() + (Vector2) boss.transform.right * Random.Range(-bulletInaccuracy, bulletInaccuracy); //adds proper speed to the bullet
                    counter++;
                    if (counter > maxBulletCount) {
                        endAction();
                    }
                }
                if (type == "turret") {
                    GameObject.Find("TurretChute").GetComponent<ChuteScript>().shoot();
                    counter++;
                    if (counter > maxTurretCount) {
                        endAction();
                    }
                }
            }
        }
    }

    public void endAction() {
        running = false;
        counter = 0;
    }

    public void startAction() {
        running = true;
        counter = 0;
    }

    public bool getRunning() {
        return running;
    }
}

public class Direction {
    private string name;
    private float angle;
    private GameObject boss;

    public Direction(string name, GameObject boss) {
        this.boss = boss;
        this.name = name;
        angle = getAngleFromName(name);
    }

    public float getAngleFromName(string name) {
        if (name.Equals("left")) {
            return boss.transform.eulerAngles.z + 270 - 45;
        }
        if (name.Equals("middle")) {
            return boss.transform.eulerAngles.z + 270;
        }
        if (name.Equals("right")) {
            return boss.transform.eulerAngles.z + 270 + 45;
        }
        return 0;
    }

    public void resetAngle() {
        angle = getAngleFromName(name);
    }

    public Vector2 getVectorInDir() {
        return new Vector2(Mathf.Cos(angle / 180 * 3.14f), Mathf.Sin(angle / 180 * 3.14f));
    }

    public float distToOtherVector2(Vector2 other) {
        return Vector2.Distance(getVectorInDir(), other);
    }

    public string getName() {
        return name;
    }
}

public class BossScript : MonoBehaviour {
    private Action[] actions = new Action[4];
    [SerializeField] private int maxActions;
    [SerializeField] private Sprite leftFacing;
    [SerializeField] private Sprite midFacing;
    [SerializeField] private Sprite rightFacing;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject missile;

    private Direction[] directions = new Direction[3];
    private Direction dirFacing;

    void Start() {
        actions[0] = new Action("missile", gameObject);
        actions[1] = new Action("laser", gameObject);
        actions[2] = new Action("bullet", gameObject);
        actions[3] = new Action("turret", gameObject);

        directions[0] = new Direction("left", gameObject);
        directions[1] = new Direction("middle", gameObject);
        directions[2] = new Direction("right", gameObject);
    }
    
    void Update() {
        if (GameObject.Find("DialogueManager").GetComponent<DialogueManager>().conversationOver()) {
            handleDirection();
            bool enoughActionsRunning = false;
            int actionsRunning = 0;
            foreach (Action a in actions) {
                if (a.getRunning()) {
                    actionsRunning++;
                }
            }
            enoughActionsRunning = actionsRunning >= maxActions;
            if (!enoughActionsRunning) {
                actions[Random.Range(0, 4)].startAction();
            }
            foreach (Action a in actions) {
                a.runAction();
            }
        }
    }

    public Direction getDirFacing() {
        return dirFacing;
    }

    private void handleDirection() {
        GameObject player = GameObject.Find("Player");
        Vector2 vectorTowardsPlayer = player.transform.position - transform.position;

        foreach (Direction dir in directions) {
            dir.resetAngle();
        }


        Direction closest = directions[0];

        foreach (Direction dir in directions) {
            if (dir.distToOtherVector2(vectorTowardsPlayer) < closest.distToOtherVector2(vectorTowardsPlayer)) {
                closest = dir;
            }
        }

        dirFacing = closest;

        if (dirFacing.getName().Equals("left")) GetComponent<SpriteRenderer>().sprite = leftFacing;
        if (dirFacing.getName().Equals("middle")) GetComponent<SpriteRenderer>().sprite = midFacing;
        if (dirFacing.getName().Equals("right")) GetComponent<SpriteRenderer>().sprite = rightFacing;
    }

    public GameObject getBullet() {
        return bullet;
    }

    public GameObject getMissile() {
        return missile;
    }
}
