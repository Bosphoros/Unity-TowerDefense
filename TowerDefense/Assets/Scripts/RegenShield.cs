using UnityEngine;
using System.Collections;

public class RegenShield : LivingThing {

    public float regenTime;

    new public void Damage(int dmg)
    {
        life -= dmg;
        if (life <= 0)
        {
            ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }
            Disappear();
        }
    }

    void Disappear()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Reset();

        Invoke("Regen", regenTime);
    }

    void Regen()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}
