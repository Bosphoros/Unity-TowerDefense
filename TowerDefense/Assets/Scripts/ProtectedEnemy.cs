using UnityEngine;
using System.Collections;

public class ProtectedEnemy : Poolable {

    public GameObject shield1;
    public GameObject shield2;
    public GameObject shield3;
    public GameObject shield4;

    public GameObject enemy;

    // Use this for initialization
    void Awake()
    {
        LivingThing lt = gameObject.GetComponent<LivingThing>();
        lt.life = enemy.GetComponent<LivingThing>().initialLife;
    }
	
    void OnEnable()
    {
        enemy.SetActive(true);
        shield1.SetActive(true);
        shield2.SetActive(true);
        shield3.SetActive(true);
        shield4.SetActive(true);
    }

	// Update is called once per frame
	void Update () {
	    if(enemy.activeInHierarchy == false)
        {
            gameObject.SetActive(false);
        }
	}

    void OnDisable()
    {
        enemy.SetActive(false);
        shield1.SetActive(false);
        shield2.SetActive(false);
        shield3.SetActive(false);
        shield4.SetActive(false);
    }
}
