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
	public GameObject bullet;
	private int timeShoot = 0;

    public bool autoMode = false;

    public string targetName;
    private GameObject target;
    public Camera linkedCamera;

    public float maxDistance = 20;

	// Use this for initialization
	void Start () {

	}

    void Accelerate() {
        rotationSpeed = Mathf.Min(10, rotationSpeed + accelerationSpeed);
        cannon.transform.RotateAround(cannon.transform.position, cannon.transform.forward, rotationSpeed);
    }

    void Decelerate() {
        rotationSpeed = Mathf.Max(0, rotationSpeed - accelerationSpeed);
        cannon.transform.RotateAround(cannon.transform.position, cannon.transform.forward, rotationSpeed);
    }

    void Fire() {
        if (timeShoot > thresholdFire)
        {
            GameObject bul = Instantiate(bullet);
            bul.transform.position = cannon.transform.position;
            bul.transform.forward = cannon.transform.forward;
            bul.transform.Rotate(new Vector3(90, 0, 0));
            bul.GetComponent<Rigidbody>().AddForce(cannon.transform.forward * bulletSpeed);
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

    void UpdateCameraPosition() {
        Vector3 pos = turret.transform.position - turret.transform.forward * 1.5f + turret.transform.up;
        float speed = 2.0f * Time.deltaTime;
        linkedCamera.transform.position = Vector3.MoveTowards(linkedCamera.transform.position, pos, speed);
        Vector3 visor = turret.transform.position + turret.transform.up;
        linkedCamera.transform.LookAt(visor);
    }

    void SearchTarget () {
        if (target != null) {
            if (Vector3.Distance(target.transform.position, turret.transform.position) > maxDistance)
            {
                target = null;
            }
        }
        if(target == null)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetName);
            GameObject t = enemies[0];
            float distT = Vector3.Distance(turret.transform.position, t.transform.position);
            for(int i = 1; i < enemies.Length; ++i) {
                GameObject e = enemies[i];
                float distTmp = Vector3.Distance(turret.transform.position, e.transform.position);
                if (distTmp < distT && distTmp <= maxDistance) {
                    t = e;
                    distT = distTmp;
                }
            }
            if(distT <= maxDistance) {
                target = t;
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
		if (rotationSpeed > thresholdShoot) {
			if(!magicFiring.isPlaying)
				magicFiring.Play ();
			timeShoot++;
		} else {
			if(!magicFiring.isStopped)
				magicFiring.Stop ();
			timeShoot = 0;
		}

        SearchTarget();

        if (!autoMode) {
            ManualMode();
        }
        else {
            AutoMode(); 
        }

        UpdateCameraPosition();

        if (Input.GetKeyDown(KeyCode.A)) {
            autoMode = !autoMode;
        }
	}

}
