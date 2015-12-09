using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager {

	public enum spatialization : int {AUDIO_2D = 0, AUDIO_3D = 1};

	private List<AudioSource> audiosources;
	private AudioManager instance;

	private AudioManager() {
		audiosources = new List<AudioSource>();
		for (int i = 0; i < 10; ++i) {
			AudioSource tmp = new AudioSource();
			audiosources.Add(tmp);
			tmp.
		}
	}


	public static AudioManager GetInstance() {
		if (instance == null) {
			instance = new AudioManager();
		}
		return instance;
	}

	public void Mute(bool m) {
		foreach (AudioSource a in audiosources) {
			a.mute = m;
		}
	}

	public void Play(bool fadein = false, spatialization space = 1) {

	}
}
