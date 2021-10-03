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
using Crosstales.RTVoice.Model;
using UnityEngine.UI;
using Crosstales.RTVoice.Tool;
using UnityEngine.SceneManagement;

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

    public AudioSource audio;

    // ----------------- HIGHLIGHTING ---------------

    // Do we want to highlight?
    private bool highlight = false;

    // Store all the word tile components we need for highlighting
    private List<WordTile> wordTiles;

    // In general, we are not currently speaking the sentence
    public bool speakingSentence = false;

    // A tick variable represents the number of words inside a single tile
    private int tick = 0;

    // The index is which tile we are on in the sentence
    private int index = -1;

    public Scrollbar sentenceScrollbar;

    private float scrollbarValueIncrement = 0;
    
    // max number of word tiles that can be in view at once, needs to be changed if the size of the sentence bar or its tiles change
    private int maxTilesPerSentence = 6;

    // uncomment the line below to use animatePipes
    //private SubmitSentenceButton submitSentenceButton;
    //
    private void Start()
    {
        // Hook functions to run each time an event is triggered from Speaker, namely when we start and stop speaking, as well as while we are speaking.
        //Speaker.OnSpeakNativeCurrentWord += SpeakNativeCurrentWord; //An event triggered whenever a new word is spoken (native, Windows and iOS only)
        // Speaker.OnSpeakStart += speakStartMethod;
        // Speaker.OnSpeakComplete += speakCompleteMethod;
        //Debug.Log("there are " + Speaker.Voices.Count + " voices on the system.");
        
        // Check if voices are available
        if (Speaker.Instance.Voices.Count <= 0)
        {
            voicesAvailable = false;
        }
        voiceName = Speaker.Instance.Voices[0].Name; // default voice assigned here so compiler stops whining

        if (ChangeScene.sceneState == ChangeScene.SceneType.SentenceBuilder)
        {
            sentenceScrollbar = GameObject.Find("SentenceScrollbar").GetComponent<Scrollbar>();
        }
        // uncomment the line below to use animatePipes
        //submitSentenceButton = GameObject.Find("SubmitSentenceButton").GetComponent<SubmitSentenceButton>();
    }

    //
    private void OnDestroy()
    {
        //Speaker.OnSpeakNativeCurrentWord -= SpeakNativeCurrentWord;
        // Speaker.OnSpeakStart -= speakStartMethod;
        // Speaker.OnSpeakComplete -= speakCompleteMethod;
    }

    // this will take the text stored in a button and set the speaker to that voice
    // change speaker will be associated with several buttons that each know the name of their voices
    public void changeSpeaker() {
        voiceName = this.gameObject.GetComponentInChildren<Text>().text;
        Speaker.Instance.Speak("Hello!", audio,  Speaker.Instance.VoiceForName(voiceName), true, voiceRate, voicePitch);
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
        Speaker.Instance.Speak(sentence, audio, Speaker.Instance.VoiceForName(voiceName), true, voiceRate, voicePitch);
        
    }

    public void startSpeakingWordTile(string word){
        Speaker.Instance.Speak(word, audio, Speaker.Instance.VoiceForName(voiceName), true, voiceRate, voicePitch);
        //Debug.Log(voicePitch);
    }

    // grabs the slider value of an attached slider game object and sets it as the voice rate
    public void changeVoiceRate() {
        voiceRate = this.GetComponent<Slider>().value;
    }

    // grabs the slider value of an attached slider game object and sets it as the voice pitch
    public void changeVoicePitch() {
        voicePitch = this.GetComponent<Slider>().value;
        audio.pitch = voicePitch;
    }
    /// <summary>
    /// Slowly here means that TTS acts on each tile individually, rather than combining the text from the tiles into a sentence and reading that.
    /// This makes each word more emphasized, with more time between each word.
    /// </summary>
    // This will probably only be used on the TextToSpeech button so the kids can get more practice with identifying their soon to-be sentence.
    public IEnumerator startSpeakingSentenceSlowly(List<WordTile> wordTiles, bool highlight){

        this.wordTiles = wordTiles;
        this.highlight = highlight;
        
        // if TTS is already going, we will stop it from saying something else
        if(speakingSentence == true){
            stopSpeaking();
        }

        speakingSentence = true;

        // 6 tiles fit in the sentence bar, so if we have more than that, we need to consider moving the scrollbar alongside the highlighting/TTS
        if(wordTiles.Count > 6) {
            // making sure the first tile is in view when TTS begins speaking
            // seems to be working, but gives the slider a weird initial value. Not sure why that is
            sentenceScrollbar.value = 0;
            
            // formula to calculate the value change needed to move the scrollbar one tile over is: (n) / (n)^2
            // where n = numTiles - 6 and comes from our knowledge that only 6 tiles will fit in the viewport at a time, 
            // so we only care about moving the scrollbar after things start getting out of view
            float n = wordTiles.Count - 6; // move scrollbar before reaching final word in viewport for context

            scrollbarValueIncrement = n/(n * n);
        }

        int loopCounter = 0;
        int incrementCounter = 0;

        // iterate through all the word tiles we have in the sentence and activate TTS and highlighting on each one individually
        foreach(WordTile wordTile in wordTiles){

            loopCounter++; // tracking when we need to move the scrollbar
            // store the text of the word tile
            string tileText = wordTile.GetComponentInChildren<Text>().text;

            // approx how long it takes TTS to speak the word
            float timeToSpeak = Speaker.Instance.ApproximateSpeechLength(tileText) / (voiceRate);
            int numberTilesReadBeforeScrolling = 4; // change this if you want begin scrolling earlier or later. (so if this were 6, the max number of tiles in view, scrolling wouldn't begin until TTS reached the last tile in the sentence bar)

            if (loopCounter > numberTilesReadBeforeScrolling) {  
                // move the scrollbar one tile over so they stay in view
                sentenceScrollbar.value += scrollbarValueIncrement;
                incrementCounter++;
            }
            // uncomment the line below to use animatePipes
            //StartCoroutine(animatePipes(timeToSpeak));
            StartCoroutine(wordTile.HighlightCoroutine(timeToSpeak));
            Speaker.Instance.Speak(tileText.ToLower(), audio, Speaker.Instance.VoiceForName(voiceName), true, voiceRate, voicePitch);
            //Debug.Log(voiceName);

            // Wait for TTS to go through the current word before saying the next.
            yield return new WaitForSeconds(timeToSpeak);
            
        }
        speakingSentence = false; 
    }

    // // Event hook for the start of a speech
    // private void speakStartMethod(SpeakEventArgs e)
    // {
    //     // Starting to speak
    //     isSpeaking = true;
    // }

    // // Event hook for the finishing of a speech
    // private void speakCompleteMethod(SpeakEventArgs e)
    // {
    //     // No longer speaking
    //     isSpeaking = false;
        
    //     // Reset Variables
    //     index = -1;
    //     tick = 0;

    //     //
    //     if(speakingSentence)
    //     {
    //         //
    //         speakingSentence = false;

    //         // not sure what the intention was here, but this seems to have been causing the last tile to stay highlighted after pushing the button.
    //         // if(highlight)
    //         // {
    //         //     //
    //         //     wordTiles.Last().Highlight();
    //         // }
            
    //     }
    // }

    // // Event hook fired each time a new word is spoken.
    // private void SpeakNativeCurrentWord(SpeakEventArgs e)
    // {
    //     //
    //     if (!highlight) return;

    //     //
    //     if(speakingSentence)
    //     {
    //         // The variable tick will be 0 when the previous tile is done
    //         // When that is the case...
    //         if (tick == 0)
    //         {
    //             // Progress to the next word tile (tracked by index)
    //             index++;

    //             // The text on this tile
    //             WordTile wt = wordTiles[index] as WordTile;
    //             string textToRead =  wt.textToDisplay;

    //             // Calculate the number of ticks this tile will get depending upon how many words are on the tile
    //             tick = textToRead.Split(' ').Length;

    //             //
    //             if (index > 0)
    //             {
    //                 //
    //                 wordTiles[index - 1].Highlight();
    //             }

    //             //
    //             wordTiles[index].Highlight();
    //         }

    //         //
    //         tick--;
    //     }
    // }
    // experimenting with basing animation duration/speed to match each word tile
    // still need to figure out how to modify animation speed to coincide with TTS modulation.
    // currently the animation speed is static, so while how long the animation plays changes, it will end on different frames, depending on TTS speed.
    // public IEnumerator animatePipes(float duration)
    // {
    //     submitSentenceButton.pipesAnimator.SetBool("ProcessingTile", true);
    //     yield return new WaitForSeconds(duration);
    //     submitSentenceButton.pipesAnimator.SetBool("ProcessingTile", false);
    // }

    // Stops an existing speech
    public void stopSpeaking()
    {
        // Stop speaking
        Speaker.Instance.Silence();

        //
        isSpeaking = false;
    }
    // returns an approximation of how long the speaker needs to read a sentence
    public float getApproxSpeechTime(List<WordTile> wordTiles){
        float speechDuration = 0;
        foreach(WordTile tile in wordTiles){
            speechDuration += Speaker.Instance.ApproximateSpeechLength(tile.word.baseWord);
        }
        speechDuration = speechDuration / voiceRate; // account for changing voice speed
        return speechDuration;
    }

    public float getVoiceRate() {
        return voiceRate;
    }
}