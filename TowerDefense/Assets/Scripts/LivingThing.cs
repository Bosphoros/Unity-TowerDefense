using UnityEngine;
using System.Collections;

public class LivingThing : MonoBehaviour {

    public int initialLife;
    public int life;


	// Use this for initialization
	void Awake () {
        life = initialLife;
	}
	
    public void Damage(int dmg) {
        life -= dmg;
        if(life <= 0)
        {
            ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
            if(ps != null)
            {
                ps.Play();
            }
            StartCoroutine("Despawn");
        }
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        life = initialLife;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
