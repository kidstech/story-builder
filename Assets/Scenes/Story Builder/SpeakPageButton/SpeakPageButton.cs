using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakPageButton : MonoBehaviour
{
    [Header("Page Icon Containers")]
    public PageContainer pageContainer;
    private Button button;

    //
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SpeakPage);
    }

    private void SpeakPage()
    {
        StartCoroutine(pageContainer.SpeakPage());
        // get all the sentences in the container
        List<SavedSentence> savedSentences = pageContainer.GetAllSentencesInPages();
        // iterate through the sentences in the container
        foreach(SavedSentence sentence in savedSentences)
        {
            // iterate through all the words in a sentence
            foreach(Word word in sentence.words)
            {
                // update the word counts for each word
                WordCountHandler.UpdateWordCount(word.word);
            }
        }
        // store updated wordcounts locally
        WordCountHandler.StoreLearnerData();
        // update server with new word counts from speaking the page
        StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
    }
}
