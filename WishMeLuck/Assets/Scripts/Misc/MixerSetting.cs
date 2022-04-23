using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class MixerSetting : MonoBehaviour {

    // reference -> slider
    public Slider musicSlider;
	public Slider effectsSlider;

    // reference -> audio mixer
    public AudioMixer masterMixer;

    void Start() {
        masterMixer.GetFloat("sfxVol", out float sfx_val);
		effectsSlider.value = sfx_val;
        masterMixer.GetFloat("musicVol", out float music_val);
		musicSlider.value = music_val;
    }

    // dipanggil saat slider digerakkan
	public void SetMusicVolume() {
		masterMixer.SetFloat ("musicVol", musicSlider.value);
	}
	// dipanggil saat slider digerakkan
	public void SetSfxVolume() {
		masterMixer.SetFloat ("sfxVol", effectsSlider.value);
	}
}
