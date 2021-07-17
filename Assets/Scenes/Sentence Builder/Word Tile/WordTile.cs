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
    private Color originalColor;
    private bool highlighted = false;
    public TextToSpeechHandler TTS;

    // When someone clicks a tile, speak the text on the tile and highlight the tile
    public void OnPointerClick(PointerEventData eventData)
    {
        string textToRead = this.textToDisplay.ToLower();

        // Highlight the word tile for approximately as long as it will take to say the text on the tile
        StartCoroutine(HighlightCoroutine(Speaker.ApproximateSpeechLength(textToRead)));
        // update word counts for the learner
        LearnerDataHandler.UpdateWordCount(textToRead);
        // store it locally
        LearnerDataHandler.StoreLearnerData();
        // update the server's copy
        StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
        // Speak the text on the tile using the correct voice
        TTS = GetComponentInParent<TextToSpeechHandler>();
        TTS.startSpeakingWordTile(textToRead);
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

    public IEnumerator HighlightCoroutine(float seconds)
    {
        Image image = GetComponent<Image>();
        Color previous = image.color;
        image.color = Color.yellow;

        yield return new WaitForSeconds(seconds);
        image.color = previous;
    }

    private IEnumerator HighlightCoroutine(float seconds, float delay)
    {
        // The visual aspect of this word tile
        Image image = GetComponent<Image>();

        // The usual color of this word tile
        Color previous = image.color;
        yield return new WaitForSeconds(delay);

        // Set the color of the word tile to the highlight color (currently yellow)
        image.color = Color.yellow;

        yield return new WaitForSeconds(seconds);
        image.color = previous;
    }

    public void SetUpTile(Word word)
    {
        this.word = word;
        this.name = word.word;
        this.transform.GetComponentInChildren<Text>().text = word.word;
        this.textToDisplay = word.word;
    }
}
