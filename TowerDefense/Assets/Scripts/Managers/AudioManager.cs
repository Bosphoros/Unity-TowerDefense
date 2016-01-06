using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager {

	public enum spatialization : int {AUDIO_2D = 0, AUDIO_3D = 1};


	private Dictionary<string, AudioData> sounds;
	private List<GameObject> audiosources;
	static private AudioManager instance;

	private AudioManager() {
		audiosources = new List<GameObject>();
		sounds = new Dictionary<string, AudioData> ();
		for (int i = 0; i < 50; ++i) {
			GameObject obj = new GameObject();
			obj.AddComponent<AudioSource>();
			audiosources.Add(obj);
            obj.transform.name = "Audiosource";
		}
	}

	public static AudioManager GetInstance() {
		if (instance == null) {
			instance = new AudioManager();
		}
		return instance;
	}

	public void Mute(bool m) {
		foreach (GameObject a in audiosources) {
			a.GetComponent<AudioSource>().mute = m;
		}
	}

	public bool Register(string name, AudioData data) {
		if (sounds.ContainsKey (name)) {
			return false;
		}
		sounds.Add (name, data);
		return true;
	}

	public bool Withdraw(string name) {
		if (!sounds.ContainsKey (name)) {
			return false;
		}
		sounds.Remove (name);
		return true;
	}

	private AudioSource GetFreeAudioSource(int priority) {

		int least = int.MinValue;
		int leastId = -1;
		AudioSource auSo;
		for (int i = 0; i < audiosources.Count; ++i) {
			auSo = audiosources[i].GetComponent<AudioSource>();
			if( !auSo.isPlaying) {
				return auSo;
			}
			if(auSo.priority >= least && auSo.priority >= priority) {
				least = auSo.priority;
				leastId = i;
			}
		}
		if (leastId != -1) {
			auSo = audiosources [leastId].GetComponent<AudioSource> ();
			auSo.Stop ();
			return auSo;
		}
		return null;
	}

	public void Play(string soundName) {
		Play (soundName, false, spatialization.AUDIO_2D);
	}

	public void Play(string soundName, bool loop, spatialization space = spatialization.AUDIO_2D, float distMin = 0, float distMax = 0) {
		Play (soundName, false, loop, space, distMin, distMax);
	}

	public void Play(string soundName, bool randPitch, bool loop, spatialization space = spatialization.AUDIO_2D, float distMin = 0, float distMax = 0) {
		Play (soundName, randPitch, loop, space, Vector3.zero, distMin, distMax);
	}

	public void Play(string soundName, bool randPitch, bool loop, spatialization space, Vector3 pos, float distMin = 0, float distMax = 0) {
        if (sounds.ContainsKey(soundName))
        {
            AudioData ad = sounds[soundName];
            if (ad != null)
            {
                AudioSource auSo = GetFreeAudioSource(ad.priority);
                auSo.clip = ad.GetClip();
                auSo.outputAudioMixerGroup = ad.group;
                auSo.spatialBlend = (float)space;
                auSo.gameObject.transform.position = pos;
                auSo.minDistance = distMin;
                auSo.maxDistance = distMax;
                if (randPitch)
                {
                    auSo.pitch = Random.Range(ad.pitchMin, ad.pitchMax);
                }
                else
                {
                    auSo.pitch = 1;
                }
                auSo.Play();
            }
        }
	}

    public void FreeResources()
    {
        foreach(GameObject go in audiosources)
        {
            UnityEngine.Object.Destroy(go);
        }
    }

}
