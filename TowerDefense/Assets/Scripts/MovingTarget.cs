using UnityEngine;
using System.Collections;

public class MovingTarget : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(target.transform.position, Vector3.up, 0.3f);
	}
}
