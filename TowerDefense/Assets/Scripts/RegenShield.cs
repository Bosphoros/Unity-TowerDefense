using UnityEngine;
using System.Collections;

public class RegenShield : LivingThing {

    public float regenTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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
            StartCoroutine("Regen");
        }
    }

    IEnumerator Regen()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(regenTime);
        gameObject.SetActive(true);
    }
}
