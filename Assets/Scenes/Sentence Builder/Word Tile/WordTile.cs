using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Crosstales.RTVoice;
using System;

public class WordTile : MonoBehaviour, IPointerClickHandler
{
    //[HideInInspector]
    public Word word;
    public string textToDisplay;
    public string contextPackId;
    private Color originalColor;
    private bool highlighted = false;
    public TextToSpeechHandler TTS;
    private Image image = null;


    private void Start()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
        
    }

    // When someone clicks a tile, speak the text on the tile and highlight the tile
    public void OnPointerClick(PointerEventData eventData)
    {
        string textToRead = this.textToDisplay.ToLower();

        // Highlight the word tile for approximately as long as it will take to say the text on the tile
        StartCoroutine(HighlightCoroutine(Speaker.ApproximateSpeechLength(textToRead)));
        // Speak the text on the tile using the correct voice
        TTS = GetComponentInParent<TextToSpeechHandler>();
        TTS.startSpeakingWordTile(textToRead);
        // update word counts for the learner
        LearnerDataHandler.UpdateWordCount(textToRead);
        // store it locally
        LearnerDataHandler.StoreLearnerData();
        // update the server's copy
        StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
    }

    public void Highlight()
    {
        Image image = GetComponent<Image>();
        highlighted = !highlighted;

        if (highlighted)
        {
            originalColor = image.color;
            image.color = Color.yellow;
        }
        else
        {
            image.color = originalColor;
        }
    }

    public void Highlight(float seconds)
    {
        StartCoroutine(HighlightCoroutine(seconds));
    }

    public void Highlight(float seconds, float delay)
    {
        StartCoroutine(HighlightCoroutine(seconds, delay));
    }
    // this one
    // remove this comment when comitting
    public IEnumerator HighlightCoroutine(float seconds)
    {
        Image image = GetComponent<Image>();
        Color previous = image.color;
        image.color = Color.yellow;
        yield return new WaitForSeconds(seconds);
        image.color = originalColor;
    }

    private IEnumerator HighlightCoroutine(float seconds, float delay)
    {
        yield return new WaitForSeconds(delay);
        image.color = Color.yellow;

        yield return new WaitForSeconds(seconds);
        image.color = originalColor;
    }

    public void SetUpTile(Word word)
    {
        this.word = word;
        this.name = word.word;
        this.transform.GetComponentInChildren<Text>().text = word.word;
        this.textToDisplay = word.word;
        this.contextPackId = word.contextPackId; 
    }
}
