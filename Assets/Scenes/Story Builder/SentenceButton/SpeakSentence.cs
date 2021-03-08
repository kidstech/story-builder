using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpeakSentence : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        SpeakSentenceText();
    }
    private void SpeakSentenceText()
    {
        Speaker.Silence(); // silence any previous speech
        Speaker.Speak(this.transform.GetComponentInChildren<Text>().text);
    }
}
