using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileAction : Action {
    private float maxMissileCount = 2;
    private GameObject missile;
    private float missileInaccuracy = .1f;

    public MissileAction(GameObject boss) : base(boss) {
        missile = boss.GetComponent<BossScript>().getMissile();

        delay = 1f;
        maxCount = maxMissileCount;
    }

    public override void run() {
        GameObject newMissile = GameObject.Instantiate(missile, boss.transform.Find("MissileSpawn").position, boss.transform.rotation); //makes a new missile at the launcher area
        newMissile.GetComponent<Rigidbody2D>().velocity = (Vector2) (-boss.transform.up) * newMissile.GetComponent<MissileScript>().getSpeed() + (Vector2) boss.transform.right * Random.Range(-missileInaccuracy, missileInaccuracy); //adds proper speed to the missile
    }

    public override void increment() {
        counter++;
    }
}

public class LaserAction : Action {
    private float maxLaserTime = 1;
    private float laserWidth = .5f;
    private float laserDamage = 1;

    public LaserAction(GameObject boss) : base(boss) {
        delay = 0f;
        maxCount = maxLaserTime;
    }

    public override void run() {
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
    }

    public override void endAction() {
        base.endAction();
        boss.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
        boss.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
    }

    public override void increment() {
        counter += Time.deltaTime;
    }
}

public class BulletAction : Action {
    private float maxBulletCount = 10;
    private GameObject bullet;
    private float bulletInaccuracy = .5f;

    public BulletAction(GameObject boss) : base(boss) {
        bullet = boss.GetComponent<BossScript>().getBullet();

        delay = .1f;
        maxCount = maxBulletCount;
    }

    public override void run() {
        GameObject newBullet = GameObject.Instantiate(bullet, boss.transform.Find("BulletSpawn").position, boss.transform.rotation); //makes a new bullet at the gun launcher area
        newBullet.GetComponent<Rigidbody2D>().velocity = (Vector2) (-boss.transform.up) * newBullet.GetComponent<BulletScript>().getBulletInitSpeed() + (Vector2) boss.transform.right * Random.Range(-bulletInaccuracy, bulletInaccuracy); //adds proper speed to the bullet
    }

    public override void increment() {
        counter++;
    }
}

public class TurretAction : Action {
    protected float maxTurretCount = 1;

    public TurretAction(GameObject boss) : base(boss) {
        delay = 1f;
        maxCount = maxTurretCount;
    }

    public override void run() {
        boss.transform.Find("TurretChute").gameObject.GetComponent<ChuteScript>().shoot();
    }

    public override void increment() {
        counter++;
    }
}

public abstract class Action {
    private bool running;

    protected float counter;
    protected float maxCount;

    private float timer;
    protected float delay;

    protected GameObject boss;

    public Action(GameObject boss) {
        this.boss = boss;
        running = false;
        counter = 0;
        timer = 0;
    }

    public void runAction() {
        if (running) {
            timer += Time.deltaTime;
            if (timer > delay) {
                timer = 0;
                run();
                increment();
                if (counter > maxCount) {
                    endAction();
                }
            }
        }
    }

    public virtual void increment() {}

    public virtual void run() {}

    public virtual void endAction() {
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
    private float timer;
    [SerializeField] private float delay;
    [SerializeField] private int maxActions;
    [SerializeField] private Sprite leftFacing;
    [SerializeField] private Sprite midFacing;
    [SerializeField] private Sprite rightFacing;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject missile;

    private Direction[] directions = new Direction[3];
    private Direction dirFacing;

    void Start() {
        actions[0] = new MissileAction(gameObject);
        actions[1] = new LaserAction(gameObject);
        actions[2] = new BulletAction(gameObject);
        actions[3] = new TurretAction(gameObject);

        directions[0] = new Direction("left", gameObject);
        directions[1] = new Direction("middle", gameObject);
        directions[2] = new Direction("right", gameObject);
    }
    
    void Update() {
        if (GameObject.Find("DialogueManager").GetComponent<DialogueManager>().conversationOver()) {
            handleDirection();
            handleActions();
        }
    }

    public void handleActions() {
        timer += Time.deltaTime;
        if (timer > delay) {
            timer = 0;
            if (!enoughActionsRunning()) {
                actions[Random.Range(0, 4)].startAction();
            }
        }
        foreach (Action a in actions) {
            a.runAction();
        }
    }

    public bool enoughActionsRunning() {
        int actionsRunning = 0;
        foreach (Action a in actions) {
            if (a.getRunning()) {
                actionsRunning++;
            }
        }
        return actionsRunning >= maxActions;
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
