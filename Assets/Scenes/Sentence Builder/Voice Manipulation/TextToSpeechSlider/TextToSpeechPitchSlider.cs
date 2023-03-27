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


    // void Start() {
    //     pitch = GetComponentInChildren<Text>();
    //     PitchChanged(slider.value);
    //     DisplayPitchValue();
    // }

    public void DisplayPitchValue() {
        string pitchValue = "Pitch value: " + slider.value;
        pitch.text = pitchValue;
    }
    // public void ChangePitch(float value)
    //     {
    //         Source.pitch = value;
    //         pitch.text = value.ToString("0.00");
    //     }
        public void RateChanged(float value)
        {
            foreach (AudioSource source in Sources)
            {
                source.pitch = value;
            }
            //pitch.text = value.ToString("0.00");
        }
}