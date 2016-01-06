using UnityEngine;
using System.Collections;

public class AmmunitionCollide : MonoBehaviour {

    public string target;
    public float collideDistance;
    public int damage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	GameObject[] targets = GameObject.FindGameObjectsWithTag(target);
        if(target.Length > 0)
        {
            foreach (GameObject obj in targets)
            {
                if(Vector3.Distance(transform.position, obj.transform.position) <= collideDistance)
                {
                    obj.GetComponent<LivingThing>().Damage(damage);
                    GetComponent<Poolable>().Reset();
                    break;
                }
            }
        }
	}

}
