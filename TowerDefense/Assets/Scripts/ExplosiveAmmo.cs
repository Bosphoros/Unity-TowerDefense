using UnityEngine;
using System.Collections;

public class ExplosiveAmmo : Poolable {

    public ParticleSystem ps;
    public float radius;
    public int damage;

    void Start()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        Invoke("Explode", lifeTime*0.8f);
        Invoke("Reset", lifeTime);
    }

    void Explode()
    {
        ps.Play();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        RaycastHit[] rch = Physics.SphereCastAll(gameObject.transform.position, radius, gameObject.transform.forward);
        foreach (RaycastHit r in rch)
        {
            GameObject o = r.collider.gameObject;
            float reduc = Mathf.Clamp((radius / Vector3.Distance(gameObject.transform.position, o.transform.position)), 0, 1);
            int reducDamages = (int)(damage * reduc);
            Enemy enmy = o.GetComponent<Enemy>();
            if(enmy != null)
            {
                LivingThing lt = enmy.body;
                lt.Damage(reducDamages);
                if(lt.life <= 0)
                {
                    Rigidbody rb = o.GetComponent<Rigidbody>();
                    if(rb != null)
                    {
                        rb.AddForce((o.transform.position - gameObject.transform.position).normalized * reduc * 50, ForceMode.Impulse);
                    }
                }
            }
        }
		AudioManager.GetInstance ().Play ("Energybomb", true, false, AudioManager.spatialization.AUDIO_2D);
    }

}
