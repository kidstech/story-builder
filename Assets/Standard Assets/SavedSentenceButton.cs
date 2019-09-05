using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedSentenceButton : MonoBehaviour
{
    public TextToSpeechHandler tts;

    private void Awake()
    {
        tts = GameObject.Find("TextToSpeechHandler").GetComponent<TextToSpeechHandler>();
    }

    public void PlaySentence()
    {
       // tts.startSpeaking(this.gameObject.GetComponentInChildren<Text>().text);
    }
}
