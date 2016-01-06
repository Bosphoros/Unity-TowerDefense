using UnityEngine;
using System.Collections;

public class RegisterCamera : MonoBehaviour {

	public string cameraName;

	// Use this for initialization
	void Start () {
		CameraManager.GetInstance ().RegisterCamera (gameObject, cameraName);
	}
}
