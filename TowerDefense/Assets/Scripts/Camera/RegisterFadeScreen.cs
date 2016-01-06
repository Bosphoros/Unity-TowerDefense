using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RegisterFadeScreen : MonoBehaviour {

	Image image;
	bool fadeIn;
	float remainingTime;

	void Start () {
		image = gameObject.transform.GetComponent<Image> ();
		fadeIn = true;
		remainingTime = 0;
		CameraManager.GetInstance ().SetFadeScreen (this);
	}

	void Update () {
		if (fadeIn) {
			FadeIn ();
		} else {
			FadeOut ();
		}
	}

	public void Reverse() {
		fadeIn = !fadeIn;
		remainingTime = 2;
	}

	void FadeIn() {
		float a = Mathf.Clamp01 (remainingTime / 2);
		Color c = new Color (image.color.r, image.color.g, image.color.b, a);
		image.color = c;
		remainingTime = Mathf.Clamp01 (remainingTime - Time.deltaTime);
	}

	void FadeOut() {
		float a = Mathf.Clamp01 (1 - remainingTime / 2);
		Color c = new Color (image.color.r, image.color.g, image.color.b, a);
		image.color = c;
		remainingTime = Mathf.Clamp01 (remainingTime - Time.deltaTime);
	}

	public void FadeTime() {
		Invoke ("FadeEnd", 2);
	}

	void FadeEnd() {
		Reverse ();
		CameraManager.GetInstance ().ActivateMain ();
	}
		
}
