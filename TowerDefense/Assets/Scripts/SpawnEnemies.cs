using UnityEngine;
using System.Collections;

public class SpawnEnemies : MonoBehaviour {

    private Pool pool;
    public Poolable enemy;
    public GameObject target;
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
            float range = Random.Range(20, 30);
            Vector3 circle = Random.onUnitSphere;
            Vector3 spawnPosition = new Vector3(circle.x * range, target.transform.position.y, circle.z * range);
            enmy.transform.position = spawnPosition;
            enmy.transform.LookAt(target.transform);
            yield return new WaitForSeconds(2.0f);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
