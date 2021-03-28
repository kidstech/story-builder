using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Crosstales.RTVoice;

public class SentenceTile : MonoBehaviour, IPointerClickHandler
{
    //[HideInInspector]
    public string textToDisplay;
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
        StartCoroutine(HighlightCoroutine(Speaker.ApproximateSpeechLength(textToRead) * 1/TextToSpeechHandler.voiceRate));

        // Speak the text on the tile using the correct voice
        TTS = GetComponentInParent<TextToSpeechHandler>();
        TTS.startSpeakingWordTile(textToRead);
    }

    // This is used for the story construction side of things, to make speak page highlight the words as it goes
    // The method is a copy of OnPointerClick that exists so we don't have to pass eventData when we want to read a tile without it being clicked on
    public void ReadSentence()
    {
        string textToRead = this.textToDisplay.ToLower();
        
        // Highlight the word tile for approximately as long as it will take to say the text on the tile
        StartCoroutine(HighlightCoroutine(Speaker.ApproximateSpeechLength(textToRead) * 1/TextToSpeechHandler.voiceRate));

        // Speak the text on the tile using the correct voice
        TTS = GetComponentInParent<TextToSpeechHandler>();
        TTS.startSpeakingWordTile(textToRead);
    }

    public IEnumerator HighlightCoroutine(float seconds)
    {
        image.color = Color.yellow;
        yield return new WaitForSeconds(seconds);
        image.color = originalColor;
    }

}
