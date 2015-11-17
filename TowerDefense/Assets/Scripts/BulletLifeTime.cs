using UnityEngine;
using System.Collections;

public class BulletLifeTime : MonoBehaviour {

	private int lifeTime = 300;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime--;
		if (lifeTime <= 0) {
			Destroy(this.gameObject);
		}
	}
}
