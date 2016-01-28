using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    public Slider playerLifeBar;
    public Slider repulseCDBar;
    public Slider freezeCDBar;
	public Text timerText;

    // Use this for initialization
    void Start () {
        lifeTime = 0;
        repulseCD = repulseWait;
        freezeCD = freezeWait;

        grid = new Grid();
		AudioManager.GetInstance ().Play ("Ambient", true, AudioManager.spatialization.AUDIO_2D);

        UIManager.GetInstance().ShowPanel("Hud");

        playerLifeBar = GameObject.Find("SliderLife").GetComponent<Slider>();
        repulseCDBar = GameObject.Find("SliderCooldownRepulse").GetComponent<Slider>();
        freezeCDBar = GameObject.Find("SliderCooldownFreeze").GetComponent<Slider>();
    }

    void Awake()
    {
        life = initialLife;
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

		if (life <= 0) {
			GameOver ();
		} else {

			playerLifeBar.value = (float)life / (float)initialLife;
			repulseCDBar.value = Mathf.Clamp (repulseCD, 0, repulseWait) / repulseWait;
			freezeCDBar.value = Mathf.Clamp (freezeCD, 0, freezeWait) / freezeWait;

			lifeTime += Time.deltaTime;
			repulseCD += Time.deltaTime;
			freezeCD += Time.deltaTime;

			if (Time.timeScale > 0) {
				if (Input.GetKeyDown (KeyCode.Alpha1)) {
					CastFreeze ();
				}

				if (Input.GetKeyDown (KeyCode.Alpha2)) {
					CastRepulse ();
				}


				if (Input.GetKeyDown (KeyCode.Alpha3)) {
					BuildExplosiveTurret ();
				}

				if (Input.GetKeyDown (KeyCode.Alpha4)) {
					BuildLaserTurret ();
				}
			}
			if (Input.GetButtonDown ("Cancel")) {
				OnPause ();
			}
		}
    }

    public void OnPause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            UIManager.GetInstance().HidePanel("Pause");
        }
        else
        {
            Time.timeScale = 0;
            UIManager.GetInstance().ShowPanel("Pause");
        }
    }

	private void GameOver(){
		float prev = PlayerPrefs.GetFloat("Time survived");
		PlayerPrefs.SetFloat("Time survived", Mathf.Max(prev, lifeTime));
		UIManager.GetInstance ().HideAll ();
		UIManager.GetInstance ().FadeInPanel ("GameOver");
		if (timerText == null) {
			timerText = GameObject.Find ("TimerTextGameOver").GetComponent<Text> ();
		}
		timerText.text = "" + (float)(((int)(lifeTime*100))/100);
		AudioManager.GetInstance().Mute(true);
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
