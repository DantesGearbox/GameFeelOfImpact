using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFeelManager : MonoBehaviour {

	public bool disableScreenFreeze = false;
	public bool disableScreenShake = false;
	public bool disableParticles = false;
	public bool disableAnimations = false;
	public bool disableBlockStun = false;
	public bool disableSoundEffects = false;

	public Button But1;
	public Button But2;
	public Button But3;
	public Button But4;
	public Button But5;
	public Button But6;

	public Color disCol;
	public Color abColor;

	public static GameFeelManager instance;

	// Use this for initialization
	void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(this.gameObject);
			return;
		}
		DontDestroyOnLoad(this.gameObject);
	}



	public void ScreenFreeze() {
		if (disableScreenFreeze) {
			disableScreenFreeze = false;
			But1.GetComponent<Image>().color = abColor;
		} else {
			disableScreenFreeze = true;
			But1.GetComponent<Image>().color = disCol;
		}
	}
	public void ScreenShake() {
		if (disableScreenShake) {
			disableScreenShake = false;
			But2.GetComponent<Image>().color = abColor;
		} else {
			disableScreenShake = true;
			But2.GetComponent<Image>().color = disCol;
		}
	}
	public void Particles() {
		if (disableParticles) {
			disableParticles = false;
			But3.GetComponent<Image>().color = abColor;
		} else {
			disableParticles = true;
			But3.GetComponent<Image>().color = disCol;
		}
	}
	public void Animations() {
		if (disableAnimations) {
			disableAnimations = false;
			But4.GetComponent<Image>().color = abColor;
		} else {
			disableAnimations = true;
			But4.GetComponent<Image>().color = disCol;
		}
	}
	public void BlockStun() {
		if (disableBlockStun) {
			disableBlockStun = false;
			But5.GetComponent<Image>().color = abColor;
		} else {
			disableBlockStun = true;
			But5.GetComponent<Image>().color = disCol;
		}
	}
	public void SoundEffects() {
		if (disableSoundEffects) {
			disableSoundEffects = false;
			But6.GetComponent<Image>().color = abColor;
		} else {
			disableSoundEffects = true;
			But6.GetComponent<Image>().color = disCol;
		}
	}
}
