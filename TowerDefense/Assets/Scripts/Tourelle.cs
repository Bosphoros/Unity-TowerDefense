using UnityEngine;
using System.Collections;

public class Tourelle : MonoBehaviour {

	public GameObject baseTourelle;
	public GameObject tourelle;
	public GameObject canon;
	private bool inputUpDown = false;
	private bool inputLeftRight = false;
	private float rotationSpeed = 0;
	private LineRenderer lr;


	public ParticleSystem magicFiring;
	public GameObject bullet;
	private int tempsTir = 0;

	// Use this for initialization
	void Start () {
		lr = canon.GetComponent<LineRenderer> ();
		lr.SetWidth (0.1f, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
	
		if (rotationSpeed > 8) {
			if(!magicFiring.isPlaying)
				magicFiring.Play ();
			tempsTir++;
		} else {
			if(!magicFiring.isStopped)
				magicFiring.Stop ();
			tempsTir = 0;
		}

		/*lr.SetPosition (0, canon.transform.position);
		lr.SetPosition (1, canon.transform.position + canon.transform.forward * 100);*/

		if (Input.GetKey (KeyCode.Space)) {
			rotationSpeed = Mathf.Min (10, rotationSpeed + 0.05f);
			canon.transform.RotateAround (canon.transform.position, canon.transform.forward, rotationSpeed);
			if(tempsTir > 40) {
				GameObject bul = Instantiate(bullet);
				bul.transform.position = canon.transform.position;
				bul.transform.forward = canon.transform.forward;
				bul.transform.Rotate(new Vector3(90,0,0));
				bul.GetComponent<Rigidbody>().AddForce(canon.transform.forward * 500);
				tempsTir = 0;
			}
		} else {
			rotationSpeed = Mathf.Max (0, rotationSpeed - 0.05f);
			canon.transform.RotateAround (canon.transform.position, canon.transform.forward, rotationSpeed);
		}

		if(Input.GetKeyDown(KeyCode.LeftArrow) && !inputUpDown){
			inputLeftRight = true;
		}
		if(Input.GetKeyDown(KeyCode.RightArrow) && !inputUpDown){
			inputLeftRight = true;
		}
		if(Input.GetKeyDown(KeyCode.UpArrow) && !inputLeftRight){
			inputUpDown = true;
		}
		if(Input.GetKeyDown(KeyCode.DownArrow) && !inputLeftRight){
			inputUpDown = true;
		}

		if (Input.GetKey (KeyCode.LeftArrow) && !inputUpDown) {
			baseTourelle.transform.Rotate (Vector3.up);
		}
		if (Input.GetKey (KeyCode.RightArrow) && !inputUpDown) {
			baseTourelle.transform.Rotate (Vector3.down);
		}
		if (Input.GetKey (KeyCode.UpArrow) && !inputLeftRight) {
			if (Vector3.Dot(tourelle.transform.forward.normalized, baseTourelle.transform.up.normalized) < 0.8f)
				tourelle.transform.Rotate (Vector3.left);
		}
		if (Input.GetKey (KeyCode.DownArrow) && !inputLeftRight) {
			if (Vector3.Dot(tourelle.transform.forward.normalized, baseTourelle.transform.up.normalized) > -0.5f)
				tourelle.transform.Rotate (Vector3.right);
		}

		if(Input.GetKeyUp(KeyCode.LeftArrow)){
			inputLeftRight = false;
		}
		if(Input.GetKeyUp(KeyCode.RightArrow)){
			inputLeftRight = false;
		}
		if(Input.GetKeyUp(KeyCode.UpArrow)){
			inputUpDown = false;
		}
		if(Input.GetKeyUp(KeyCode.DownArrow)){
			inputUpDown = false;
		}

	}

}
