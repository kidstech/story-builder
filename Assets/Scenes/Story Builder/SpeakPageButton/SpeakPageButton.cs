using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakPageButton : MonoBehaviour
{
    public SavedSentenceBank sentenceBank;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SpeakStory);
    }

    private void SpeakStory()
    {
        sentenceBank.ttsUpdateLearnerData();
    }
}
