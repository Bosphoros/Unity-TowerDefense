using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cutscene : MonoBehaviour {

	string active = "";
    bool isMute = false;
    public Camera ecran;
	public GameObject person;
	public List<GameObject> listLongueRue;
    
    void Start()
    {
        AudioManager.GetInstance().Play("Music", true);
		ecran.transform.LookAt(new Vector3(150, 87, 25), Vector3.up);
    }

	// Update is called once per frame
	void Update () {
		
		if(active.Equals(""))
        {
			Begin ();
        }

        if (CameraManager.GetInstance().IsStable()) {

			if (active.Equals("Tour"))
			    CameraManager.GetInstance ().AroundY (new Vector3 (150, 80, 25), .3f);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            isMute = !isMute;
            AudioManager.GetInstance().Mute(isMute);
        }

    }

	void Begin() {
		SurvolVille ();
		Invoke ("MarcheRues", 18);
		Invoke ("LongueRue", 28);
		Invoke ("TourneTour", 58);
	}

	void SurvolVille() {
		CameraManager.GetInstance ().FadeTo ("Survol");
		active = "Survol";
	}

	void MarcheRues() {
		CameraManager.GetInstance ().FadeTo ("Rue");
		person.GetComponent<CharacterCutscene> ().walk = true;
		active = "Rue";
	}

	void LongueRue() {
		CameraManager.GetInstance ().FadeTo ("LongueRue");
		foreach (GameObject go in listLongueRue) {
				go.GetComponent<CharacterCutscene> ().walk = true;
		}
		active = "LongueRue";
	}

	void TourneTour() {
		CameraManager.GetInstance ().FadeTo ("Tour");
		active = "Tour";
	}

    void OnDestroy()
    {
        AudioManager.GetInstance().FreeResources();
    }
}
