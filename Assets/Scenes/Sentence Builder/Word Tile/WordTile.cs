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
    public Color originalColor;
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
        image.color = originalColor;
        // Highlight the word tile for approximately as long as it will take to say the text on the tile
        StartCoroutine(HighlightCoroutine(Speaker.Instance.ApproximateSpeechLength(textToRead)));
        // update word counts for the learner
        LearnerDataHandler.UpdateWordCount(textToRead);
        // store it locally
        LearnerDataHandler.StoreLearnerData();
        // update the server's copy
        StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
        // Speak the text on the tile using the correct voice
        TTS = GetComponentInParent<TextToSpeechHandler>();
        TTS.startSpeakingWordTile(textToRead);


        if(this.gameObject.GetComponent<DraggableTile>().draggedFrom != TileDropzone.Behavior.WordHolder) {

            GameObject o = Instantiate(this.gameObject);
            o.GetComponent<WordTile>().word = this.gameObject.GetComponent<WordTile>().word;
            o.GetComponent<Image>().color = this.gameObject.GetComponent<WordTile>().originalColor;

            if(WordHolder.wordHolderDropZoneTransform.childCount > 0) {
                GameObject.Destroy(WordHolder.wordHolderDropZoneTransform.GetChild(0).gameObject);
                o.transform.SetParent(WordHolder.wordHolderDropZoneTransform, false);
                o.GetComponent<DraggableTile>().draggedFrom = TileDropzone.Behavior.WordHolder;
            }
            else {
                o.transform.SetParent(WordHolder.wordHolderDropZoneTransform, false);
                o.GetComponent<DraggableTile>().draggedFrom = TileDropzone.Behavior.WordHolder;
            }
        }
    }

    // public void Highlight()
    // {
    //     Image image = GetComponent<Image>();
    //     highlighted = true;

    //     if (highlighted)
    //     {
    //         originalColor = image.color;
    //         image.color = Color.yellow;
    //     }
    //     else
    //     {
    //         image.color = originalColor;
    //     }
    // }

    // public void Highlight(float seconds)
    // {
    //     StartCoroutine(HighlightCoroutine(seconds));
    // }

    // public void Highlight(float seconds, float delay)
    // {
    //     StartCoroutine(HighlightCoroutine(seconds, delay));
    // }
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

    // private IEnumerator HighlightCoroutine(float seconds, float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     image.color = Color.yellow;

    //     yield return new WaitForSeconds(seconds);
    //     image.color = originalColor;
    // }

    public void SetUpTile(Word word)
    {
        this.word = word;
        this.name = word.baseWord;
        this.transform.GetComponentInChildren<Text>().text = word.baseWord;
        this.textToDisplay = word.baseWord;
        this.contextPackId = word.contextPackId;
    }
}
