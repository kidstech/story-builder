/// <summary>
/// Handles the text to speech system defined by RTVoice.
/// Can and should be extended upon in the future to account for different voices.
/// 
/// <author> antin006@morris.umn.edu </author>
/// </summary>
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model.Event;
using System.Linq;

public class TextToSpeechHandler : MonoBehaviour
{
    // ----------------- TTS SETTINGS ---------------

    // Is a phrase or word currently being spoken?
    [HideInInspector]
    public static bool isSpeaking = false;

    // Are there TTS voices available?
    [HideInInspector]
    public static bool voicesAvailable = true;

    // ----------------- HIGHLIGHTING ---------------

    // Store all the word tile components we need for highlighting
    private List<WordTile> words;

    //
    private bool speakingSentence = false;

    // A tick variable represents the number of words inside a single tile
    private int tick = 0;

    // The index is which tile we are on in the sentence
    private int index = -1;

    //
    private void Start()
    {
        // Hook functions to run each time an event is triggered from Speaker, namely when we start and stop speaking, as well as while we are speaking.
        Speaker.OnSpeakNativeCurrentWord += SpeakNativeCurrentWord;
        Speaker.OnSpeakStart += speakStartMethod;
        Speaker.OnSpeakComplete += speakCompleteMethod;
        
        // Check if voices are available
        if (Speaker.Voices.Count <= 0)
        {
            //
            voicesAvailable = false;
        }
    }

    //
    private void OnDestroy()
    {
        Speaker.OnSpeakNativeCurrentWord -= SpeakNativeCurrentWord;
        Speaker.OnSpeakStart -= speakStartMethod;
        Speaker.OnSpeakComplete -= speakCompleteMethod;
    }

    // Starts a speech
    public float startSpeaking(string sentence)
    {
        // If we aren't currently speaking
        if (!isSpeaking && voicesAvailable)
        {
            // Then start speaking
            Speaker.SpeakNative(sentence, Speaker.VoiceForCulture("en"));

            //
            return Speaker.ApproximateSpeechLength(sentence);
        }

        //
        return -1;
    }

    //
    public void startSpeakingSentence(List<WordTile> words)
    {
        //
        this.words = words;

        //
        this.speakingSentence = true;

        //
        string sentence = "";

        //
        foreach(WordTile word in words)
        {
            //
            sentence += word.word.word + " ";
        }

        //
        sentence = sentence.Substring(0, sentence.Length - 1);

        //
        Speaker.SpeakNative(sentence, Speaker.VoiceForCulture("en"));
    }

    // Event hook for the start of a speech
    private void speakStartMethod(SpeakEventArgs e)
    {
        // Starting to speak
        isSpeaking = true;
    }

    // Event hook for the finishing of a speech
    private void speakCompleteMethod(SpeakEventArgs e)
    {
        // No longer speaking
        isSpeaking = false;
        
        // Reset Variables
        index = -1;
        tick = 0;

        //
        if(speakingSentence)
        {
            //
            speakingSentence = false;

            //
            words.Last().Highlight();
        }
    }

    // Event hook fired each time a new word is spoken.
    private void SpeakNativeCurrentWord(SpeakEventArgs e)
    {
        //
        if(speakingSentence)
        {
            //
            if (tick == 0)
            {
                //
                index++;

                //
                tick = words[index].word.word.Split(' ').Length;

                //
                if (index > 0)
                {
                    //
                    words[index - 1].Highlight();
                }

                //
                words[index].Highlight();
            }

            //
            tick--;
        }
    }

    // Stops an existing speech
    public void stopSpeaking()
    {
        // Stop speaking
        Speaker.Silence();

        //
        isSpeaking = false;
    }
}