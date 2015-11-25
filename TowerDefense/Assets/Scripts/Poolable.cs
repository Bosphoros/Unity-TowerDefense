using UnityEngine;
using System.Collections;
using UnityEditor;

public class Poolable : MonoBehaviour {

    public float lifeTime;

	// Use this for initialization
	void OnEnable () {
        Invoke("Reset", lifeTime);
	}

    public void Reset() {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }


    public GameObject copy()
    {
        return Instantiate(gameObject);
    }

    void OnDisable() {
        CancelInvoke();
    }
}
