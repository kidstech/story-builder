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

public class TextToSpeechHandler : MonoBehaviour
{


    // Speech button that changes from play to stop
    public TextToSpeechButton textToSpeechButton;

    // Is a phrase or word currently being spoken?
    public static bool isSpeaking = false;

    // Are there TTS voices available?
    public static bool voicesAvailable = true;


    // ----------------- HIGHLIGHTING ---------------

    // Keep track of all the words that WILL be currently spoken.
    public Transform[] words = new Transform[32];

    // Keep track of the last color the button was
    public Color prevColor = new Color(255, 255, 0, 100);

    // Keep track on what word in the list we are on
    public int currentWord = 0;

    // Keep track of the words in a word tile
    public int tick = 0;

    // Get methods from the sentence box
    public Sentence sentence;

    /// <summary>
    /// Start this instance.
    /// Checks for voices and initializes event hooks for RTVoice.
    /// </summary>
    /// 

    private void OnEnable()
    {
        Speaker.OnSpeakNativeCurrentWord += SpeakNativeCurrentWord;
        Speaker.OnSpeakStart += speakStartMethod;
        Speaker.OnSpeakComplete += speakCompleteMethod;
    }

    void Start()
    {

        // Check if voices are available
        if (Speaker.Voices.Count <= 0)
            voicesAvailable = false;

        // Initialize TTS event hooks

    }

    /// <summary>
    /// Starts the speaking of the provided phrase.
    /// </summary>
    /// <param name="phrase">Phrase to speak.</param>
    public void startSpeaking(string phrase)
    {

        // If we aren't currently speaking
        if (!isSpeaking && voicesAvailable)
        {

            // Then start speaking
            isSpeaking = true;
            Speaker.SpeakNative(phrase, Speaker.VoiceForCulture("en"));
        }

    }

    /// <summary>
    /// Stops the speaking of any speech.
    /// </summary>
    public void stopSpeaking()
    {

        // Stop speaking
        Speaker.Silence();
        isSpeaking = false;

        // Re-enable play option
        textToSpeechButton.showPlayOption();

        // ------HIGHLIGHTING-------
        // Reset Color
        words[currentWord - 1].GetComponent<Image>().color = prevColor;

        // Reset Variables
        currentWord = 0;
        tick = 0;
    }


    /// <summary>
    /// Event hook for the start of speaking.
    /// </summary>
    /// <param name="e">E.</param>
    private void speakStartMethod(SpeakEventArgs e)
    {
        // Allow user to stop speech
        textToSpeechButton.showStopOption();


        // ------HIGHLIGHTING-------
        // Reset word counter
        currentWord = 0;

        // Populate the array of Words
        words = sentence.getAllTiles();
    }

    /// <summary>
    /// Event hook for the completion of speaking.
    /// </summary>
    /// <param name="e">E.</param>
    private void speakCompleteMethod(SpeakEventArgs e)
    {

        // No longer speaking
        isSpeaking = false;

        // Allow user to use TTS again
        textToSpeechButton.showPlayOption();

        // ------HIGHLIGHTING-------

        // Set the last word back to its original color
        if(currentWord > 0)
        {
            words[currentWord - 1].GetComponent<Image>().color = prevColor;
        }
        else
        {
            words[0].GetComponent<Image>().color = prevColor;
        }

        // Reset Word Counter
        currentWord = 0;
    }

    /// <summary>
	/// Event hook for speaking current word.
	/// </summary>
	/// <param name="e">E.</param>
    private void SpeakNativeCurrentWord(SpeakEventArgs e)
    {
        // ------HIGHLIGHTING-------

        // 'Tick' represents the number words in a tile.
        if (tick == 0)
        {
            // Get the text in the tile and count the number of spaces in it.
            tick = words[currentWord].GetComponentInChildren<Text>().text.Count(char.IsWhiteSpace);

            // Set the previous tile back to the color it was
            if (currentWord > 0)
            {
                words[currentWord - 1].GetComponent<Image>().color = prevColor;
            }

            // Save the color of the n'th tile
            prevColor = words[currentWord].GetComponent<Image>().color;

            // Change the color of the n'th tile to the highlighted color
            words[currentWord].GetComponent<Image>().color = new Color32(255, 255, 0, 100);

            // If the tick is still equal to 0, then that means there is no spaces or anything, and is a single word. We can move to the next word safely.
            if(tick == 0)
            {
                currentWord++;
            }

        }
        else
        {
            // Incement Word if tick is going to become zero
            if (tick == 1)
            {
                currentWord++;
            }

            // Count down a tick
            tick--;
        }


    }

}