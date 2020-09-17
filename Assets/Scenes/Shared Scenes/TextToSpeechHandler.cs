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
using Crosstales.RTVoice.Model;

public class TextToSpeechHandler : MonoBehaviour
{
    // ----------------- TTS SETTINGS ---------------

    // Is a phrase or word currently being spoken?
    [HideInInspector]
    public static bool isSpeaking = false;

    // Are there TTS voices available?
    [HideInInspector]
    public static bool voicesAvailable = true;

    // Default voice
    private Voice selectedVoice = Speaker.VoiceForCulture("en");

    // ----------------- HIGHLIGHTING ---------------

    // Do we want to highlight?
    private bool highlight = false;

    // Store all the word tile components we need for highlighting
    private List<WordTile> wordTiles;

    // In general, we are not currently speaking the sentence
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
            selectedVoice = null;
        }

        //==================
        // CONCEPT: Being able to change your selected voices
        // THIS IS NOT IMPLEMENTED CURRENTLY
        //==================

        // Select a Random Voice from the available voices
        // int voiceNum = Random.Range(0, Speaker.Voices.Count);

        // Assign that random voice
        // selectedVoice = Speaker.Voices[voiceNum];

        // OR YOU CAN ASSIGN VOICES MANUALLY BY CREATING SOME SORT OF MENU DROPDOWN SOMEWHERE
        // And give users the option to pick and choose -- probably best to allow for demo speaking
        // Put this in options and or the sentence builder scene
        /*
        foreach(Voice voice in Speaker.Voices)
        {
            //Generate Menu Option
                // Allow for demo of voice
                // Set voice on selection
        }
        */
    }

    //
    private void OnDestroy()
    {
        Speaker.OnSpeakNativeCurrentWord -= SpeakNativeCurrentWord;
        Speaker.OnSpeakStart -= speakStartMethod;
        Speaker.OnSpeakComplete -= speakCompleteMethod;
    }

    //
    public void startSpeakingSentence(List<WordTile> wordTiles, bool highlight)
    {
        this.wordTiles = wordTiles;
        this.highlight = highlight;

        speakingSentence = true;

        string sentence = "";

        foreach(WordTile wordTile in wordTiles)
        {
            //
            sentence += wordTile.textToDisplay + " ";
        }

        //
        sentence = sentence.Substring(0, sentence.Length - 1);

        //
        Speaker.SpeakNative(sentence, selectedVoice);
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
            if(highlight)
            {
                //
                wordTiles.Last().Highlight();
            }
            
        }
    }

    // Event hook fired each time a new word is spoken.
    private void SpeakNativeCurrentWord(SpeakEventArgs e)
    {
        //
        if (!highlight) return;

        //
        if(speakingSentence)
        {
            // The variable tick will be 0 when the previous tile is done
            // When that is the case...
            if (tick == 0)
            {
                // Progress to the next word tile (tracked by index)
                index++;

                // The text on this tile
                WordTile wt = wordTiles[index] as WordTile;
                string textToRead =  wt.textToDisplay;

                // Calculate the number of ticks this tile will get depending upon how many words are on the tile
                tick = textToRead.Split(' ').Length;

                //
                if (index > 0)
                {
                    //
                    wordTiles[index - 1].Highlight(Speaker.ApproximateSpeechLength(textToRead));
                }

                //
                wordTiles[index].Highlight();
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