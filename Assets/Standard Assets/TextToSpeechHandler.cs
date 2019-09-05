/// <summary>
/// Handles the text to speech system defined by RTVoice.
/// Can and should be extended upon in the future to account for different voices.
/// 
/// <author> antin006@morris.umn.edu </author>
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model.Event;
using Crosstales.RTVoice.Model;
using System.Linq;
using static Crosstales.RTVoice.Speaker;

public class TextToSpeechHandler : MonoBehaviour
{
    // ----------------- TTS SETTINGS ---------------

    // Speech button that changes from play to stop
    public TextToSpeechButton textToSpeechButton;

    // Is a phrase or word currently being spoken?
    public static bool isSpeaking = false;

    // Are there TTS voices available?
    public static bool voicesAvailable = true;

    // ----------------- SPEECH BEHAVIOR ---------------

    // Variable to tell us how to handle the upcoming speech.
    private SoundType type;

    // The object we are working with for getting word information from
    private GameObject source;

    // enums for behavior handling of the sound engine.
    public enum SoundType
    {
        NONE = -1,
        TILE = 0,
        SENTENCE = 1,
        SAVED_SENTENCE = 2
    }

    // ----------------- HIGHLIGHTING ---------------

    private void Start()
    {
        // Hook functions to run each time an event is triggered from Speaker, namely when we start and stop speaking, as well as while we are speaking.
        Speaker.OnSpeakNativeCurrentWord += SpeakNativeCurrentWord;
        Speaker.OnSpeakStart += speakStartMethod;
        Speaker.OnSpeakComplete += speakCompleteMethod;
        
        // Check if voices are available
        if (Speaker.Voices.Count <= 0)
        {
            voicesAvailable = false;
        }

        // Optional -- If commented out make sure that wordBank is assigned in the inspector.
    }

    private void OnDestroy()
    {
        Speaker.OnSpeakNativeCurrentWord -= SpeakNativeCurrentWord;
        Speaker.OnSpeakStart -= speakStartMethod;
        Speaker.OnSpeakComplete -= speakCompleteMethod;
    }

    // Starts a speech
    public void startSpeaking(string sentence, SoundType behaviorType, GameObject sourceObject)
    {
        // If we aren't currently speaking
        if (!isSpeaking && voicesAvailable)
        {
            // Set our behavior type
            type = behaviorType;

            // Set the object of where we are drawing information from
            source = sourceObject;

            // Then start speaking
            Speaker.SpeakNative(sentence, Speaker.VoiceForCulture("en"));
        }
    }

    // Stops an existing speech
    public void stopSpeaking()
    {
        // Stop speaking
        Speaker.Silence();
        isSpeaking = false;

        // Re-enable play option
        textToSpeechButton.showPlayOption();

        // Reset our behavior type
        this.type = SoundType.NONE;

        // Reset our source object
        this.source = null;
    }


    // Event hook for the start of a speech
    private void speakStartMethod(SpeakEventArgs e)
    {
        // Starting to speak
        isSpeaking = true;

        switch(type)
        {
            case SoundType.SENTENCE:
            case SoundType.TILE:

                // Allow user to stop speech
                textToSpeechButton.showStopOption();

                break;

            case SoundType.SAVED_SENTENCE:
                break;
        }
    }

    // Event hook for the finishing of a speech
    private void speakCompleteMethod(SpeakEventArgs e)
    {
        // No longer speaking
        isSpeaking = false;

        switch (type)
        {
            case SoundType.SENTENCE:

                // Allow user to use TTS again
                textToSpeechButton.showPlayOption();

                // Reset the last tile to be the normal color it was
                source.transform.GetChild(index - 1).GetComponent<Image>().color = previousColor;

                break;

            case SoundType.TILE:

                // Allow user to use TTS again
                textToSpeechButton.showPlayOption();

                // Change that tile back to normal
                source.GetComponent<Image>().color = previousColor;

                break;

            case SoundType.SAVED_SENTENCE:
                break;
        }
        
        // Reset Variables
        index = 0;
        tick = 0;
    }

    // TEMP
    private int tick = 0;
    private int index = 0;

    private Color previousColor;
    private Color highlightColor = new Color(255, 255, 0);

    // Event hook fired each time a new word is spoken.
    private void SpeakNativeCurrentWord(SpeakEventArgs e)
    {
       switch(type)
        {
            case SoundType.SENTENCE:

                // If this isn't the first word, set the previous child's color back to normal.
                if(index != 0 && tick == 0)
                {
                    source.transform.GetChild(index - 1).GetComponent<Image>().color = previousColor;
                }

                // Calculate the tick
                if(tick == 0)
                {
                    tick = source.transform.GetChild(index).GetChild(0).GetComponent<Text>().text.Split().Length;
                }

                // Save the previous color
                previousColor = source.transform.GetChild(index).GetComponent<Image>().color;

                // Set the current tile to be highlighted
                source.transform.GetChild(index).GetComponent<Image>().color = highlightColor;

                // Increment Variables
                index++;
                tick--;

                break;

            case SoundType.TILE:

                

                break;

            case SoundType.SAVED_SENTENCE:
                break;
        }
    }
}