using UnityEngine;
using System.Collections;

public class Player : LivingThing
{ 
    private float lifeTime;
    public float repulseCD;
    public Plane ground;
    private Grid grid;

    public GameObject laserTurretPrefab;
    public int laserTurrets = 0;
    public int maxLaserTurrets;
    public GameObject explosiveTurretPrefab;
    public int explosiveTurrets = 0;
    public int maxExplosiveTurrets;

    // Use this for initialization
    void Start () {
        lifeTime = 0;
        repulseCD = 50;
        grid = new Grid();
	}

    void Awake()
    {
        life = initialLife;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10,10,200,20), "Life : " + life);
        GUI.Label(new Rect(10, 30, 200, 20), "Time : " + ((int)(lifeTime*100.0f))/100.0f);
        GUI.Label(new Rect(10, 50, 200, 20), "Best : " + ((int)(PlayerPrefs.GetFloat("Time survived")*100.0f))/100.0f);

        GUI.Label(new Rect(200, 10, 200, 20), "Repulse : " + Mathf.Clamp((int)(20 - repulseCD), 0, 20));

        GUI.Label(new Rect(400, 10, 200, 20), "Laser turrets : " + laserTurrets + "/" + maxLaserTurrets);
        GUI.Label(new Rect(400, 30, 200, 20), "Explosive turrets : " + explosiveTurrets + "/" + maxExplosiveTurrets);
        if (life <= 0)
        {
            float prev = PlayerPrefs.GetFloat("Time survived");
            PlayerPrefs.SetFloat("Time survived", Mathf.Max(prev, lifeTime));
            GUI.Label(new Rect(200, 200, 500, 250), "Game Over");
            Time.timeScale = 0;
        }
    }

    bool GetPositionOnGround(out Vector3 pos)
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastTer;
        if (Physics.Raycast(r, out raycastTer))
        {
            if (raycastTer.collider.gameObject.tag.Equals("Ground"))
            {
                pos = raycastTer.point;
                return true;
            }
        }
        pos = Vector3.zero;
        return false;
    }

    void CastRepulse()
    {
        if (repulseCD > 20)
        {
            Vector3 pos;
            if (GetPositionOnGround(out pos))
            {
                RaycastHit[] rch = Physics.SphereCastAll(pos, 5, Vector3.up);
                foreach (RaycastHit rc in rch)
                {
                    GameObject o = rc.collider.gameObject;
                    float reduc = (5.0f / Vector3.Distance(gameObject.transform.position, o.transform.position));

                    LivingThing lt = rc.collider.gameObject.GetComponent<LivingThing>();
                    if (lt != null)
                    {
                        Rigidbody rb = lt.gameObject.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.AddForce((o.transform.position - pos).normalized * reduc * 200);
                        }
                    }
                }
                repulseCD = 0;
            }
        }
    }

    void BuildExplosiveTurret()
    {
        if (explosiveTurrets < maxExplosiveTurrets)
        {
            Vector3 pos;
            if (GetPositionOnGround(out pos))
            {
                pos = grid.GetCellCenter(pos);
                if (grid.IsFree(pos))
                {
                    GameObject turret = Instantiate(explosiveTurretPrefab);
                    turret.transform.position = pos;
                    grid.Take(pos);
                    explosiveTurrets++;
                }
            }
        }
    }

    void BuildLaserTurret()
    {
        if (laserTurrets < maxLaserTurrets)
        {
            Vector3 pos;
            if (GetPositionOnGround(out pos))
            {
                pos = grid.GetCellCenter(pos);
                if (grid.IsFree(pos))
                {
                    GameObject turret = Instantiate(laserTurretPrefab);
                    turret.transform.position = pos;
                    grid.Take(pos);
                    laserTurrets++;
                }
            }
        }
    }

	// Update is called once per frame
	void Update () {
        lifeTime += Time.deltaTime;
        repulseCD += Time.deltaTime;
	    

        if (Input.GetMouseButtonDown(1))
        {
            CastRepulse();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            BuildExplosiveTurret();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BuildLaserTurret();
        }
    }

}
