using UnityEngine;
using System.Collections;

public class Cinematic : MonoBehaviour {

	int active;

	// Update is called once per frame
	void Update () {
	

		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			active = 0;
			CameraManager.GetInstance ().FadeTo ("0");
			Debug.Log ("0");
		}
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			active = 1;
			CameraManager.GetInstance ().FadeTo ("1");
			Debug.Log ("1");
		}
		if(active == 0 && CameraManager.GetInstance().IsStable())
			CameraManager.GetInstance ().AroundY (new Vector3 (150, 80, 25), .3f);
	}
}
