using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;
using UnityEngine.EventSystems;

public class TextToSpeechPitchSlider : MonoBehaviour {
    public List<AudioSource> Sources = new List<AudioSource>();
    public Slider slider;
    public Text pitch;

    public void DisplayPitchValue() {
        string pitchValue = "Pitch value: " + slider.value;
        pitch.text = pitchValue;
    }
    public void RateChanged(float value) {
        foreach (AudioSource source in Sources) {
            source.pitch = value;
        }
    }
}