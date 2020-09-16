using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Crosstales.RTVoice;

public class WordTile : MonoBehaviour, IPointerClickHandler
{
    //
    //[HideInInspector]
    public Word word;

    //
    private Color originalColor;

    //
    private bool highlighted = false;

    //
    public void OnPointerClick(PointerEventData eventData)
    {
        //
        StartCoroutine(HighlightCoroutine(Speaker.ApproximateSpeechLength(this.GetComponentInChildren<Text>().text)));

        //
        Speaker.SpeakNative(this.GetComponentInChildren<Text>().text, Speaker.VoiceForCulture("en"));
    }

    //
    public void Highlight()
    {
        //
        Image image = GetComponent<Image>();

        //
        highlighted = !highlighted;

        //
        if (highlighted)
        {
            //
            originalColor = image.color;

            //
            image.color = Color.yellow;
        }
        else
        {
            //
            image.color = originalColor;
        }
    }

    //
    public void Highlight(float seconds)
    {
        //
        StartCoroutine(HighlightCoroutine(seconds));
    }

    //
    public void Highlight(float seconds, float delay)
    {
        //
        StartCoroutine(HighlightCoroutine(seconds, delay));
    }

    //
    private IEnumerator HighlightCoroutine(float seconds)
    {
        //
        Image image = GetComponent<Image>();

        //
        Color previous = image.color;

        //
        image.color = Color.yellow;

        //
        yield return new WaitForSeconds(seconds);

        //
        image.color = previous;
    }

    private IEnumerator HighlightCoroutine(float seconds, float delay)
    {
        //
        Image image = GetComponent<Image>();

        //
        Color previous = image.color;

        //
        yield return new WaitForSeconds(delay);

        //
        image.color = Color.yellow;

        //
        yield return new WaitForSeconds(seconds);

        //
        image.color = previous;
    }

    //
    public void SetUpTile(Word word)
    {
        //
        this.word = word;

        //
        this.name = word.word;

        //
        this.transform.GetComponentInChildren<Text>().text = word.word;
    }
}
