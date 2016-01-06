using UnityEngine;
using System.Collections;

public class LaserTurret : MonoBehaviour {

    
    public float maxDistance;
    public GameObject crystal;
    public string targetName;
    public int fireRate;
    public int fireTime = 0;
    public int damage;

    private LineRenderer lr;
    private LivingThing target;

    // Use this for initialization
    void Start () {
        lr = crystal.GetComponent<LineRenderer>();
        lr.SetColors(new Color(1, 0.9f, 0.5f), new Color(0.95f, 0.39f, 0.39f));
        lr.SetPosition(0, crystal.transform.position);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        SearchTarget();

        if (target != null)
        {
            crystal.GetComponent<Levitation>().rotationSpeed = 90;
            if(fireTime >= fireRate*0.9)
            {
                lr.SetPosition(1, target.gameObject.transform.position);
                lr.enabled = true;
                ++fireTime;
            }
            else {
                lr.enabled = false;
                ++fireTime;
            }
            if (fireTime >= fireRate)
            {
                Vector3 dir = target.gameObject.transform.position - crystal.transform.position;
                Ray r = new Ray(crystal.transform.position, dir);
                RaycastHit hitInfo;
                Physics.Raycast(r, out hitInfo, maxDistance);
                Enemy enmy = hitInfo.collider.gameObject.GetComponent<Enemy>();
                if (enmy != null)
                {
                    LivingThing lt = enmy.body;
                    if (hitInfo.collider.gameObject.GetComponent<RegenShield>() != null)
                        ((RegenShield)lt).Damage(damage);
                    else
                        lt.Damage(damage);
                    if (lt.life <= 0)
                    {
                        Rigidbody rb = enmy.gameObject.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.AddForce(dir.normalized * 5, ForceMode.Impulse);
                        }
                    }
					AudioManager.GetInstance().Play ("L4z0R", true, false, AudioManager.spatialization.AUDIO_2D);
                }
                fireTime = 0;
            }
            
        }
        else
        {
            crystal.GetComponent<Levitation>().rotationSpeed = 0;
            lr.enabled = false;
        }
        

    }

    void SearchTarget()
    {
        
        if (target != null)
        {
            LivingThing lt = target.GetComponent<LivingThing>();
            if (Vector3.Distance(target.transform.position, crystal.transform.position) > maxDistance || !target.gameObject.activeInHierarchy || (lt != null && lt.life <= 0))
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
                if(ltt == null || ltt.life <= 0)
                {
                    t = null;
                }
                float distT = t == null ? float.PositiveInfinity : Vector3.Distance(crystal.transform.position, t.transform.position);
                for (int i = 1; i < enemies.Length; ++i)
                {
                    GameObject e = enemies[i];
                    LivingThing ltt2 = e.GetComponent<LivingThing>();
                    float distTmp = Vector3.Distance(crystal.transform.position, e.transform.position);
                    bool distance = distTmp < distT && distTmp <= maxDistance;
                    if (t == null && distance  || (ltt2 != null && ltt2.life > 0) && distance)
                    {
                        t = e;
                        distT = distTmp;
                    }
                }
                if (distT <= maxDistance)
                {
                    target = t.GetComponent<LivingThing>();
                }
            }
        }
    }
}
