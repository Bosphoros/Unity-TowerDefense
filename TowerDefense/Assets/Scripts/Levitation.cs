using UnityEngine;
using System.Collections;

public class Levitation : MonoBehaviour {

	public int decalageTime ;
	private int time = 0 ;
	public enum rotationDirection : int {RIGHT = -1, NONE = 0, LEFT = 1};
	public rotationDirection rotation;
	public float rotationSpeed;
	public float levitationHeight;

	// Use this for initialization
	void Start () {
		time += decalageTime;
	}
	
	// Update is called once per frame
	void Update () {
		time++;
		this.transform.Rotate (Time.fixedDeltaTime * Vector3.up * rotationSpeed * (int) rotation);
		transform.Translate (Time.fixedDeltaTime * Mathf.Cos (time/60) * Vector3.up * levitationHeight);
	}
}
