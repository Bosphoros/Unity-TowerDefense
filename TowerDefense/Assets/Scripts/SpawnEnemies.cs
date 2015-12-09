using UnityEngine;
using System.Collections;

public class SpawnEnemies : MonoBehaviour {

    private Pool pool;
    public Poolable enemy;
    public GameObject target;
    public float radius;
    private bool doSpawn = true;
    public float timeSpawn;

	// Use this for initialization
	void Start () {

        
    }

    void OnEnable() {
        pool = new Pool(enemy);
        doSpawn = true;
        StartCoroutine("Spawn");
    }

    void OnDisable() {
        doSpawn = false;
    }

    void OnDrawGizmos()
    {
        Vector3 prec = new Vector3(Mathf.Cos(0) * radius, transform.position.y, Mathf.Sin(0) * radius) + gameObject.transform.position;
        for (int i = 1; i < 360; ++i)
        {
            Vector3 next = new Vector3(Mathf.Cos(i / (Mathf.PI * 2)) * radius, transform.position.y, Mathf.Sin(i / (Mathf.PI * 2)) * radius) + gameObject.transform.position;
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(prec, next);
            prec = next;
        }
    }

    IEnumerator Spawn() {

        while (doSpawn)
        {
            GameObject enmy = pool.getInactive();
            if(enmy.GetComponent<LivingThing>() != null)
                enmy.GetComponent<LivingThing>().Reset();
            float range = Random.Range(0, radius);
            Vector3 circle = Random.insideUnitCircle;
            Vector3 center = gameObject.transform.position;
            Vector3 spawnPosition = new Vector3(center.x + circle.x * range, center.y, center.z + circle.y * range);
            enmy.transform.position = spawnPosition;
            enmy.transform.LookAt(target.transform);
            enmy.transform.SetParent(gameObject.transform);
            enmy.GetComponent<Rigidbody>().velocity = Vector3.zero;
            enmy.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            yield return new WaitForSeconds(timeSpawn);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
