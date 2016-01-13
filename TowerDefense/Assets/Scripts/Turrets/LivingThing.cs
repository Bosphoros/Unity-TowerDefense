using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LivingThing : MonoBehaviour {

    public int initialLife;
    public int life;

    public Slider lifeBar;

	// Use this for initialization
	void Awake () {
        life = initialLife;
        lifeBar.maxValue = initialLife;
	}

    void Update()
    {
        if(lifeBar != null)
        {
            lifeBar.value = life;
            if(life <= 0)
            {
                lifeBar.enabled = false;
            }
            else
            {
                lifeBar.enabled = true;
            }
        }
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
            Invoke("Despawn", 5);
        }
    }

    void Despawn()
    {
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        life = initialLife;
    }

    void OnDisable()
    {
        Reset();
    }
}
