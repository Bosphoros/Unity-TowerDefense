using UnityEngine;
using System.Collections;

public class Cinematic : MonoBehaviour {

	int active = -1;
    bool isMute = false;
    public Camera ecran;
    
    void Start()
    {
        AudioManager.GetInstance().Play("Music", true);
    }

	// Update is called once per frame
	void Update () {
        

        if(active == -1)
        {
            active = 1;
            CameraManager.GetInstance().FadeTo("0");
        }
        if (CameraManager.GetInstance().IsStable()) {

            if (active == 1)
			    CameraManager.GetInstance ().AroundY (new Vector3 (150, 80, 25), .3f);
            /*if (active == 2)
                CameraManager.GetInstance().MoveForward(2);*/

		    if (Input.GetKeyDown (KeyCode.Alpha1)) {
			    active = 1;
			    CameraManager.GetInstance ().FadeTo ("0");
		    }
		    if (Input.GetKeyDown (KeyCode.Alpha2)) {
			    active = 2;
			    CameraManager.GetInstance ().FadeTo ("1");
		    }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                active = 3;
                CameraManager.GetInstance().FadeTo("Head");
            }

        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMute = !isMute;
            AudioManager.GetInstance().Mute(isMute);
        }
        ecran.transform.LookAt(new Vector3(150, 87, 25), Vector3.up);
        /*ecran.transform.RotateAround(new Vector3(150, 80, 25), Vector3.up, 0.3f * Time.timeScale);*/
    }

    void OnDestroy()
    {
        AudioManager.GetInstance().FreeResources();
    }
}
