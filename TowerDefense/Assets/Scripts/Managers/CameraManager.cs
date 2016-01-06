using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager {

	private Dictionary<string, GameObject> cameras;
	static private CameraManager instance;
	private Camera active;
	RegisterFadeScreen fadescreen;

	string nextCamera;
	bool isStable = false;

	private CameraManager() {
		cameras = new Dictionary<string, GameObject> ();
	}

	public static CameraManager GetInstance() {
		if (instance == null) {
			instance = new CameraManager ();
		}
		return instance;
	}

	public bool RegisterCamera (GameObject c, string name) {
		if (!cameras.ContainsKey (name)) {
			cameras.Add (name, c);
			if (active == null) {
				active = c.GetComponent<Camera> ();
				Debug.Log (name);
				active.enabled = true;
			} else {
				c.GetComponent<Camera> ().enabled = false;
			}
			return true;
		}
		return false;
	}

	public void SetFadeScreen(RegisterFadeScreen screen) {
		fadescreen = screen;
	}

	public void AroundY(Vector3 center, float speed) {
		active.transform.LookAt (center, Vector3.up);
		active.transform.RotateAround (center, Vector3.up, speed * Time.timeScale);
	}

	public void FadeTo(string cameraName) {
		if (cameras.ContainsKey (cameraName)) {
			isStable = false;
			fadescreen.Reverse ();
			nextCamera = cameraName;
			fadescreen.FadeTime ();
		}
		else
			Debug.Log ("Doesn't exist");
	}

	public void ActivateMain(){
		active.enabled = false;
		active = cameras [nextCamera].GetComponent<Camera> ();
		active.enabled = true;
		nextCamera = "";
		isStable = true;
	}

	public bool IsStable() {
		return isStable;
	}

}
