/// <summary>
/// Handles the text to speech system defined by RTVoice.
/// Can and should be extended upon in the future to account for different voices.
/// 
/// <author> antin006@morris.umn.edu </author>
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model.Event;
using Crosstales.RTVoice.Model;
using UnityEngine.UI;
using Crosstales.RTVoice.Tool;

public class TextToSpeechHandler : MonoBehaviour
{
    // ----------------- TTS SETTINGS ---------------

    // Is a phrase or word currently being spoken?
    [HideInInspector]
    public static bool isSpeaking = false;

    // Are there TTS voices available?
    [HideInInspector]
    public static bool voicesAvailable = true;

    // need to eventually swap to MaryTTS for MacOS, voices are currently dependent on OS
    // variables are static so that they are globally changed. If this becomes an issue, an alternative to this is to create a game object with a global variables script
    // as outlined by https://answers.unity.com/questions/212090/sharing-variables-value-between-game-objects.html

    // To fix the OS dependency, it seems like there is a Speaker.Voices method that might just grab respective OS speakers and then we could just change though array indices?
    [Header("Speaker info")]
    public static string voiceName; // default voice dependent on OS

    // can be 0-3 where 1 is normal speed
    public static float voiceRate = 1f;

    public static float voicePitch = 1f;

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
        //Speaker.OnSpeakNativeCurrentWord += SpeakNativeCurrentWord; //An event triggered whenever a new word is spoken (native, Windows and iOS only)
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
        //Speaker.OnSpeakNativeCurrentWord -= SpeakNativeCurrentWord;
        Speaker.OnSpeakStart -= speakStartMethod;
        Speaker.OnSpeakComplete -= speakCompleteMethod;
    }

    // this will take the text stored in a button and set the speaker to that voice
    public void changeSpeaker() {
        voiceName = this.gameObject.GetComponentInChildren<Text>().text; // text is stored in child of child... may need double get call
        // demo the new voice so the user can know if they like it right away
        // livespeaker takes an array of args
        string[] args = new string[6];
        args[0] = "Hello, my name is " + voiceName;
        args[2] = voiceName;
        args[3] = voiceRate.ToString();
        args[5] = voicePitch.ToString();
        LiveSpeaker LS = new LiveSpeaker();
        Debug.Log("Voice pitch: " + args[5]);
        //Speaker.Speak("Hello, my name is " + voiceName, null,  Speaker.VoiceForName(voiceName), true, voiceRate, voicePitch);
        LS.Speak(args);
    }

    // keeping this because we want the sentence to be read more fluently once the sentence has been "built"
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
        Speaker.Speak(sentence, null, Speaker.VoiceForName(voiceName), true, voiceRate, 1f, null, voicePitch);
        
    }

    public void startSpeakingWordTile(string word){
        Speaker.Speak(word, null, Speaker.VoiceForName(voiceName), true, voiceRate, 1f, "", voicePitch);
        Debug.Log(voicePitch);
    }

    // grabs the slider value of an attached slider game object and sets it as the voice rate
    public void changeVoiceRate() {
        voiceRate = this.GetComponent<Slider>().value;
    }

    // grabs the slider value of an attached slider game object and sets it as the voice pitch
    public void changeVoicePitch() {
        voicePitch = this.GetComponent<Slider>().value;
    }

    // Slowly here means that TTS acts on each tile individually, rather than combining the text from the tiles into a sentence and reading that.
    // This will probably only be used on the TextToSpeech button so the kids can get more practice with identifying their soon to-be sentence.
    public IEnumerator startSpeakingSentenceSlowly(List<WordTile> wordTiles, bool highlight){
        this.wordTiles = wordTiles;
        this.highlight = highlight;
        // if TTS is already going, we will stop it from saying something else
        if(speakingSentence == true){
            stopSpeaking();
        }

        speakingSentence = true;

        // iterate through all the word tiles we have in the sentence and activate TTS and highlighting on each one individually
        foreach(WordTile wordTile in wordTiles){
            // store the text of the word tile
            string tileText = wordTile.GetComponentInChildren<Text>().text;

            // approx how long it takes TTS to speak the word
            float timeToSpeak = Speaker.ApproximateSpeechLength(tileText);

            StartCoroutine(wordTile.HighlightCoroutine(timeToSpeak));
            //Speaker.SpeakNative(tileText, Speaker.VoiceForCulture("en"));
            Speaker.Speak(tileText, null, Speaker.VoiceForName(voiceName), true, voiceRate, 1f, null, voicePitch);
            Debug.Log(voiceName);

            // Wait for TTS to go through the current word before saying the next.
            yield return new WaitForSeconds(timeToSpeak);
            
        }
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

            // not sure what the intention was here, but this seems to have been causing the last tile to stay highlighted after pushing the button.
            // if(highlight)
            // {
            //     //
            //     wordTiles.Last().Highlight();
            // }
            
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
                    wordTiles[index - 1].Highlight();
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