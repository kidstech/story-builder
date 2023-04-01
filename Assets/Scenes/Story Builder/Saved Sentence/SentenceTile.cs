using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Crosstales.RTVoice;

public class SentenceTile : MonoBehaviour, IPointerClickHandler
{
    public string textToDisplay;
    public Color originalColor;
    public TextToSpeechHandler TTS;
    public List<string> words;
    Image image = null;

    void Start()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
        // grab the actual word objects of the sentence for wordcount tracking
        words = this.GetComponent<SentenceObject>().savedSentence.selectedWordForms;
    }

    // When someone clicks a tile, speak the text on the tile and highlight the tile
    public void OnPointerClick(PointerEventData eventData)
    {
        string textToRead = this.textToDisplay.ToLower();
        
        // Highlight the word tile for approximately as long as it will take to say the text on the tile
        StartCoroutine(HighlightCoroutine(Speaker.Instance.ApproximateSpeechLength(textToRead) * 1/TextToSpeechHandler.voiceRate));

        // Speak the text on the tile using the correct voice
        TTS = GetComponentInParent<TextToSpeechHandler>();
        TTS.startSpeakingWordTile(textToRead);
        foreach(string selectedWord in words)
        {
            // update wordcounts for every word in the sentence
            LearnerDataHandler.UpdateWordCount(selectedWord);
        }
        // store the updated learnerdata
        LearnerDataHandler.StoreLearnerData();
        // send off the updated data to the server
        StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());

    }

    // This is used for the story construction side of things, to make speak page highlight the words as it goes
    // The method is a copy of OnPointerClick that exists so we don't have to pass eventData when we want to read a tile without it being clicked on
    public void ReadSentence()
    {
        string textToRead = this.textToDisplay.ToLower();
        
        // Highlight the word tile for approximately as long as it will take to say the text on the tile
        StartCoroutine(HighlightCoroutine(Speaker.Instance.ApproximateSpeechLength(textToRead) * 1/TextToSpeechHandler.voiceRate));

        // Speak the text on the tile using the correct voice
        TTS = GetComponentInParent<TextToSpeechHandler>();
        TTS.startSpeakingWordTile(textToRead);
    }



    public IEnumerator HighlightCoroutine(float seconds)
    {
        Image image = GetComponent<Image>();
        Color previous = image.color;
        image.color = Color.yellow;
        yield return new WaitForSeconds(seconds);
        image.color = originalColor;
    }

}
