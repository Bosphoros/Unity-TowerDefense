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

    void OnDrawGizmosSelected()
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);
        Vector3 prec = new Vector3(Mathf.Cos(0)*dist, transform.position.y, Mathf.Sin(0) * dist);
        for(int i = 1; i < 360; ++i) {
            Vector3 next = new Vector3(Mathf.Cos(i/(Mathf.PI*2)) * dist, transform.position.y, Mathf.Sin(i / (Mathf.PI * 2)) * dist);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(prec, next);
            prec = next;
        }
       
    }
}
