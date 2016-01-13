using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class Menu : MonoBehaviour {

    public AudioMixer am;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

	// Use this for initialization
	void Start () {
        UIManager.GetInstance().HidePanel("Pause");
        UIManager.GetInstance().HidePanel("Options");
        UIManager.GetInstance().HidePanel("Hud");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause"))
        {
            OnPause();
        }
	}

    public void LoadNewGame()
    {
        Application.LoadLevel("Grided");
    }

    public void OnPause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            UIManager.GetInstance().HidePanel("Pause");
        }
        else
        {
            Time.timeScale = 0;
            UIManager.GetInstance().ShowPanel("Pause");
        }
    }

    public void CloseOptions()
    {
        UIManager.GetInstance().HidePanel("Options");
    }
    public void OpenOptions()
    {
        UIManager.GetInstance().ShowPanel("Options");
    }

    public void ChangeVolumeMaster(float volume)
    {
        am.SetFloat("masterVolume", volume);
    }

    public void ChangeVolumeSFX(float volume)
    {
        am.SetFloat("sfxVolume", volume);
    }

    public void ChangeVolumeAmbient(float volume)
    {
        am.SetFloat("ambientVolume", volume);
    }

    public void ChangeVolumeMusic(float volume)
    {
        am.SetFloat("musicVolume", volume);
    }
}
