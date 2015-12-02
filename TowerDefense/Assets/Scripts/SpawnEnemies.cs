using UnityEngine;
using System.Collections;

public class SpawnEnemies : MonoBehaviour {

    private Pool pool;
    public Poolable enemy;
    public GameObject target;
    public float radius;
    private bool doSpawn = true;

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

    IEnumerator Spawn() {

        while (doSpawn)
        {
            GameObject enmy = pool.getInactive();
            enmy.GetComponent<LivingThing>().Reset();
            float range = Random.Range(0, radius);
            Vector3 circle = Random.insideUnitCircle;
            Vector3 center = gameObject.transform.position;
            Vector3 spawnPosition = new Vector3(center.x + circle.x * range, center.y, center.z + circle.z * range);
            enmy.transform.position = spawnPosition;
            enmy.transform.LookAt(target.transform);
            yield return new WaitForSeconds(2.0f);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
