using UnityEngine;
using System.Collections;

public class LivingThing : MonoBehaviour {

    public int initialLife;
    public int life;

	// Use this for initialization
	void Start () {
        life = initialLife;
	}
	
    public void Damage(int dmg) {
        life -= dmg;
        if(life <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        life = initialLife;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
