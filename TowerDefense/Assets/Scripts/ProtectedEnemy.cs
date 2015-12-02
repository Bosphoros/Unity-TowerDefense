using UnityEngine;
using System.Collections;

public class ProtectedEnemy : Poolable {

    public GameObject shields;

    public LivingThing enemy;

    // Use this for initialization
    void Awake()
    {
        /*LivingThing lt = gameObject.GetComponent<LivingThing>();
        lt.life = enemy.GetComponent<LivingThing>().initialLife;*/
    }
	
    void OnEnable()
    {
        enemy.gameObject.SetActive(true);
        enemy.gameObject.transform.position = gameObject.transform.position;
        shields.SetActive(true);
    }

	// Update is called once per frame
	void Update () {
        if(enemy.life == 0)
        {
            gameObject.GetComponent<ForwardEnemy>().freezeTime = 200;
        }
	    if(enemy.gameObject.activeInHierarchy == false)
        {
            gameObject.SetActive(false);
        }
        enemy.gameObject.transform.position = gameObject.transform.position;
    }

    void OnDisable()
    {
        enemy.gameObject.SetActive(false);
        shields.SetActive(false);
    }

}
