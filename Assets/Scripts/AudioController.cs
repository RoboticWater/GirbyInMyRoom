﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {

	public Slider master,music,sfx;

	// Use this for initialization
	void Start () {
		UpdateSlider();
		GetComponent<AudioSource>().volume = master.value * music.value;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateMaster() {
		SaveLoad.instance.masterVolume = master.value;
	}

	public void UpdateMusic() {
		SaveLoad.instance.musicVolume = music.value;
	}

	public void UpdateSFX() {
		SaveLoad.instance.sfxVolume = sfx.value;
	}
	public void UpdateVolumeInstance() {
		GetComponent<AudioSource>().volume = master.value * music.value;
		SaveLoad.Save();
		Debug.Log(SaveLoad.instance.musicVolume);
	}

	public void UpdateSlider() {
		master.value = Mathf.Clamp01(SaveLoad.instance.masterVolume);
		music.value = Mathf.Clamp01(SaveLoad.instance.musicVolume);
		sfx.value = Mathf.Clamp01(SaveLoad.instance.sfxVolume);
	}
}