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

    // The child number of the tile we are working with if SoundType is of type TILE
    /*
     * This ugliness is how we accomplish highlight tiles.
     * In unity, we can't store references (even static) to instantiated prefabs.
     * Passing the instantiated prefab into a function will cause it to only exist in the scope of that function
     * Assigning it to a variable makes it null outside of the function
     * So to get around this, we keep track of what column and what row the tile came from, and find it that way.
     */ 
    private static int tileNumber;
    private static int columnNumber;

    // enums for behavior handling of the sound engine.
    public enum SoundType
    {
        NONE = -1,
        TILE = 0,
        SENTENCE = 1,
        SENTENCE_SAVE = 2,
        SAVED_SENTENCE = 3
    }

    // ----------------- HIGHLIGHTING ---------------

    // A tick variable represents the number of words inside a single tile
    private int tick = 0;

    // The index is which tile we are on in the sentence
    private int index = 0;

    // Self explainatory
    private Color previousColor;
    private Color highlightColor = new Color(255, 255, 0);

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
            if(behaviorType == SoundType.TILE)
            {
                // If the object is a tile, record the child index number of that tile

                // transform.parent will get the wordTile slot
                tileNumber = sourceObject.transform.parent.GetSiblingIndex();

                // transform.parent.parent will get the wordColumn
                columnNumber = sourceObject.transform.parent.parent.GetSiblingIndex();
            }
            else
            {
                source = sourceObject;
            }
            
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
        source = null;
    }


    // Event hook for the start of a speech
    private void speakStartMethod(SpeakEventArgs e)
    {
        // Starting to speak
        isSpeaking = true;

        switch(type)
        {
            case SoundType.SENTENCE:
            case SoundType.SENTENCE_SAVE:
            case SoundType.TILE:
            case SoundType.SAVED_SENTENCE:

                // Allow user to stop speech
                textToSpeechButton.showStopOption();

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
                source.transform.GetChild(index - 1).Find("Highlight").GetComponent<Image>().color = previousColor;

                break;

            case SoundType.TILE:

                // Allow user to use TTS again
                textToSpeechButton.showPlayOption();

                // Store object for easier handling
                Image tileImage = GameObject.Find("WordBankContent").transform.GetChild(columnNumber).transform.GetChild(tileNumber).GetChild(0).Find("Highlight").GetComponent<Image>();

                // Save the previous color
                tileImage.color = previousColor;

                break;

            case SoundType.SENTENCE_SAVE:

                // Allow user to use TTS again
                textToSpeechButton.showPlayOption();

                break;

            case SoundType.SAVED_SENTENCE:
                break;
        }
        
        // Reset Variables
        index = 0;
        tick = 0;
    }

    // Event hook fired each time a new word is spoken.
    private void SpeakNativeCurrentWord(SpeakEventArgs e)
    {
       switch(type)
        {
            case SoundType.SENTENCE:

                // If this isn't the first word, set the previous child's color back to normal.
                if(index != 0 && tick == 0)
                {
                    // Reset the highlight border
                    source.transform.GetChild(index - 1).Find("Highlight").GetComponent<Image>().color = previousColor;
                }

                // Calculate the tick
                if(tick == 0)
                {
                    //Calculate the tick
                    tick = source.transform.GetChild(index).Find("Text").GetComponent<Text>().text.Split().Length;

                    // Save the previous color
                    previousColor = source.transform.GetChild(index).Find("Highlight").GetComponent<Image>().color;

                    // Set the current tile to be highlighted
                    source.transform.GetChild(index).Find("Highlight").GetComponent<Image>().color = highlightColor;
                }

                // Increment tick count
                tick--;

                // Check if our tick went down to zero
                if (tick == 0)
                {
                    // And if it did, progress our index forward
                    index++;
                }

                break;

            // When we are saving stuff
            case SoundType.SENTENCE_SAVE:

                // If this isn't the first word, set the previous child's color back to normal.
                if (index != 0 && tick == 0)
                {
                    
                }

                // Calculate the tick
                if (tick == 0)
                {
                    //Calculate the tick
                    tick = source.transform.GetChild(index).Find("Text").GetComponent<Text>().text.Split().Length;

                    // Enable the animator
                    source.transform.GetChild(index).GetComponent<Animator>().enabled = true;

                    // Change the animation boolean to play the animation
                    source.transform.GetChild(index).GetComponent<Animator>().SetBool("open", true);

                    source.transform.GetChild(index).SetParent(source.transform.parent.parent.parent);
                }

                // Increment tick count
                tick--;

                // Check if our tick went down to zero
                if (tick == 0)
                {
                    // Destroy(source.transform.GetChild(0).gameObject);
                }

                break;


            case SoundType.TILE:

                // Store object for easier handling
                // GetChild(0) will get the tile inside of the word holder
                Image tileImage = GameObject.Find("WordBankContent").transform.GetChild(columnNumber).transform.GetChild(tileNumber).GetChild(0).Find("Highlight").GetComponent<Image>();

                // Save the previous color
                previousColor = tileImage.color;

                // Highlight the tile
                tileImage.color = highlightColor;

                break;

            case SoundType.SAVED_SENTENCE:
                break;
        }
    }
}