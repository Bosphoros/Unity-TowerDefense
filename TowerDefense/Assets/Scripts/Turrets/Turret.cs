using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	public GameObject baseTurret;
	public GameObject turret;
	public GameObject cannon;
    public float accelerationSpeed = 0.05f;
    public float bulletSpeed = 500;
    public float thresholdShoot = 8;
    public int thresholdFire = 40;
	private bool inputUpDown = false;
	private bool inputLeftRight = false;
	private float rotationSpeed = 0;


	public ParticleSystem magicFiring;
	public Poolable bullet;
	private float timeShoot = 0;

    public bool autoMode = false;

    public string targetName;
    private GameObject target;

    public float maxDistance = 20;

    private Pool pool;

	// Use this for initialization
	void Start () {
        pool = new Pool(bullet);
	}

    void Accelerate() {
        rotationSpeed = Mathf.Min(10, rotationSpeed + accelerationSpeed * Time.timeScale);
        cannon.transform.RotateAround(cannon.transform.position, cannon.transform.forward, rotationSpeed * Time.timeScale);
    }

    void Decelerate() {
        rotationSpeed = Mathf.Max(0, rotationSpeed - accelerationSpeed * Time.timeScale);
        cannon.transform.RotateAround(cannon.transform.position, cannon.transform.forward, rotationSpeed * Time.timeScale);
    }

    void Fire() {
        if (timeShoot > thresholdFire)
        {
            GameObject bul = pool.getInactive();
            bul.transform.position = cannon.transform.position;
            bul.transform.forward = cannon.transform.forward;
            bul.transform.Rotate(new Vector3(90, 0, 0));
            bul.GetComponent<Rigidbody>().AddForce(cannon.transform.forward * bulletSpeed);
            bul.transform.SetParent(gameObject.transform);
            timeShoot = 0;
        }
    }

    void ManualMode() {
        if (Input.GetKey(KeyCode.Space))
        {
            Accelerate();
            Fire();
        }
        else
        {
            Decelerate();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && !inputUpDown)
        {
            inputLeftRight = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && !inputUpDown)
        {
            inputLeftRight = true;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && !inputLeftRight)
        {
            inputUpDown = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && !inputLeftRight)
        {
            inputUpDown = true;
        }

        if (Input.GetKey(KeyCode.LeftArrow) && !inputUpDown)
        {
            baseTurret.transform.Rotate(Vector3.up);
        }
        if (Input.GetKey(KeyCode.RightArrow) && !inputUpDown)
        {
            baseTurret.transform.Rotate(Vector3.down);
        }
        if (Input.GetKey(KeyCode.UpArrow) && !inputLeftRight)
        {
            if (Vector3.Dot(turret.transform.forward.normalized, baseTurret.transform.up.normalized) < 0.8f)
                turret.transform.Rotate(Vector3.left);
        }
        if (Input.GetKey(KeyCode.DownArrow) && !inputLeftRight)
        {
            if (Vector3.Dot(turret.transform.forward.normalized, baseTurret.transform.up.normalized) > -0.5f)
                turret.transform.Rotate(Vector3.right);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            inputLeftRight = false;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            inputLeftRight = false;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            inputUpDown = false;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            inputUpDown = false;
        }
    }

    void AutoMode() {
        if (target == null)
        {
            Decelerate();
            return;
        }

        /*Debug.DrawLine(turet.transform.position, target.transform.position, Color.red, 0f);
        Debug.DrawLine(baseTuret.transform.position, baseTuret.transform.position + 4 * baseTuret.transform.up, Color.blue, 0f);
        Debug.DrawLine(turet.transform.position, turet.transform.position + 4 * turet.transform.forward, Color.green, 0f);//*/

        float step = 2 * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(turret.transform.forward, (target.transform.position - turret.transform.position).normalized, step, 0.0f);

        float dot = Vector3.Dot(newDir, baseTurret.transform.up.normalized);
        if (dot < 0.8f && dot > -0.5f)
        {
            turret.transform.rotation = Quaternion.LookRotation(newDir);
        }

        float dotCannonTarget = Vector3.Dot(turret.transform.forward, (target.transform.position - turret.transform.position).normalized);

        if (Mathf.Abs(dotCannonTarget) > 0.95)
        {
            Accelerate();
            Fire();
        }
        else
        {
            Decelerate();
        }
    }



    void SearchTarget()
    {

        if (target != null)
        {
            LivingThing lt = target.GetComponent<LivingThing>();
            if (Vector3.Distance(target.transform.position, turret.transform.position) > maxDistance || !target.gameObject.activeInHierarchy || (lt != null && lt.life <= 0))
            {
                target = null;
            }
        }
        if (target == null)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetName);
            if (enemies.Length >= 1)
            {
                GameObject t = enemies[0];
                LivingThing ltt = t.GetComponent<LivingThing>();
                if (ltt == null || ltt.life <= 0)
                {
                    t = null;
                }
                float distT = t == null ? float.PositiveInfinity : Vector3.Distance(turret.transform.position, t.transform.position);
                for (int i = 1; i < enemies.Length; ++i)
                {
                    GameObject e = enemies[i];
                    LivingThing ltt2 = e.GetComponent<LivingThing>();
                    float distTmp = Vector3.Distance(turret.transform.position, e.transform.position);
                    bool distance = distTmp < distT && distTmp <= maxDistance;
                    if (t == null && distance || (ltt2 != null && ltt2.life > 0) && distance && ltt2.life > 0)
                    {
                        t = e;
                        distT = distTmp;
                    }
                }
                if (distT <= maxDistance)
                {
                    target = t;
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {

	    SearchTarget();

		if (rotationSpeed > thresholdShoot) {
			if(!magicFiring.isPlaying)
				magicFiring.Play ();
			timeShoot += 1*Time.timeScale;
		} else {
			if(!magicFiring.isStopped)
				magicFiring.Stop ();
			timeShoot = 0;
		}

        if (!autoMode) {
            ManualMode();
        }
        else {
            AutoMode(); 
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            autoMode = !autoMode;
        }
	}

}
