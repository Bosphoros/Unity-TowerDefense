using UnityEngine;
using System.Collections;

public class MovingCamera : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
	
	}

    void UpdateCameraPosition()
    {
        Vector3 pos = target.transform.position - target.transform.forward * 1.5f + target.transform.up;
        float speed = 2.0f * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pos, speed);
        Vector3 visor = target.transform.position + target.transform.up;
        transform.LookAt(visor);
    }

    // Update is called once per frame
    void Update () {
        UpdateCameraPosition();
	}
}
