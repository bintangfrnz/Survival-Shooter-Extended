using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BrightnessSetting : MonoBehaviour {

    // reference -> camera brightness
    Brightness brightness;
    // reference -> brighness slider
    public Slider brightnessSlider;

    void Awake() {
        brightness = Camera.main.GetComponent<Brightness>();
    }

    // atur nilai brightness
    public void SetBrightness() {
        brightness.brightness = brightnessSlider.value;
    }
}
