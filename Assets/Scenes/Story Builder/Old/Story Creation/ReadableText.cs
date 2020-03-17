using Crosstales.RTVoice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadableText : MonoBehaviour
{
    //
    public void ReadText()
    {
        //
        string textToSpeech = transform.Find("Text").GetComponent<Text>().text;

        //
        Speaker.SpeakNative(textToSpeech, Speaker.VoiceForCulture("en"));

        //
        float length = Speaker.ApproximateSpeechLength(textToSpeech);

        //
        StartCoroutine(Highlight(length));
    }

    //
    private IEnumerator Highlight(float highlightTime)
    {
        //
        Color previousColor;
        Color highlightColor = new Color(255, 255, 0);

        Text t = transform.Find("Text").GetComponent<Text>();

        previousColor = t.color;
        t.color = highlightColor;

        yield return new WaitForSeconds(highlightTime);

        t.color = previousColor;
    }
}
