using UnityEngine;
using System.Collections;

public class Player : LivingThing
{ 
    private float lifeTime;
    public float repulseCD;
    public float repulseWait = 20;
    public ParticleSystem repulseAnim;

    public float freezeCD;
    public float freezeWait = 20;
    public float freezeTime;
    public ParticleSystem freezeAnim;

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
        repulseCD = repulseWait;
        freezeCD = freezeWait;

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

        GUI.Label(new Rect(200, 10, 200, 20), "Repulse : " + Mathf.Clamp((int)(repulseWait - repulseCD), 0, 20));
        GUI.Label(new Rect(200, 30, 200, 20), "Freeze : " + Mathf.Clamp((int)(freezeWait - freezeCD), 0, 20));

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
        if (repulseCD >= repulseWait)
        {
            Vector3 pos;
            if (GetPositionOnGround(out pos))
            {
                RaycastHit[] rch = Physics.SphereCastAll(pos, 5, Vector3.up);
                foreach (RaycastHit rc in rch)
                {
                    GameObject o = rc.collider.gameObject;
                    float reduc = (5.0f / Vector3.Distance(gameObject.transform.position, o.transform.position));

                    Enemy enmy = o.GetComponent<Enemy>();

                    if (enmy != null)
                    {
                        Rigidbody rb = o.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.AddForce((o.transform.position - pos).normalized * reduc * 100, ForceMode.Impulse);
                        }
                    }

                }
                pos.y += 0.5f;
                repulseAnim.transform.position = pos;
                repulseAnim.Play();
                repulseCD = 0;
            }
        }
    }

    void CastFreeze()
    {
        if (freezeCD >= freezeWait)
        {
            Vector3 pos;
            if (GetPositionOnGround(out pos))
            {
                
                RaycastHit[] rch = Physics.SphereCastAll(pos, 5, Vector3.up);
                foreach (RaycastHit rc in rch)
                {
                    GameObject o = rc.collider.gameObject;

                    ForwardEnemy fe = o.GetComponent<ForwardEnemy>();

                    if (fe != null)
                    {
                        fe.freezeTime = freezeTime;
                    }
                }
                pos.y += 0.5f;
                freezeAnim.transform.position = pos;
                freezeAnim.Play();
                freezeCD = 0;
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
                    turret.transform.SetParent(gameObject.transform);
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
        freezeCD += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            CastFreeze();
        }

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


    void OnDrawGizmos()
    {
        float radius = gameObject.GetComponent<CapsuleCollider>().radius;
        Vector3 prec = new Vector3(Mathf.Cos(0) * radius, transform.position.y, Mathf.Sin(0) * radius) + gameObject.transform.position;
        for (int i = 1; i < 360; ++i)
        {
            Vector3 next = new Vector3(Mathf.Cos(i / (Mathf.PI * 2)) * radius, transform.position.y, Mathf.Sin(i / (Mathf.PI * 2)) * radius) + gameObject.transform.position;
            Gizmos.color = Color.white;
            Gizmos.DrawLine(prec, next);
            prec = next;
        }
    }
}
