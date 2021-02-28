using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedSentenceBank : MonoBehaviour
{
    //
    [Header("Prefabs")]
    public GameObject sentencePrefab;

    //
    private Vector2 sentencePrefabSize;

    //
    private void Start()
    {
        //
        List<SavedSentence> sentences = LoadSavedSentences.LoadSentences();

        //
        sentencePrefabSize = sentencePrefab.GetComponent<RectTransform>().sizeDelta;

        //
        GetComponent<RectTransform>().sizeDelta = new Vector2(sentencePrefabSize.x, sentencePrefabSize.y * sentences.Count);

        //
        for (int i = 0; i < sentences.Count; i++)
        {
            //
            GameObject newSentence = Instantiate(sentencePrefab);

            //
            newSentence.GetComponent<SentenceObject>().savedSentence = sentences[i];

            //
            newSentence.GetComponentInChildren<Text>().text = CompileSentence(sentences[i].words);

            //
            newSentence.transform.SetParent(this.transform, false);

            newSentence.AddComponent<SpeakSentence>();
        }
    }

    //
    private string CompileSentence(List<Word> words)
    {
        //
        string compiledSentence = "";

        //
        for(int i = 0; i < words.Count; i++)
        {
            //
            compiledSentence += words[i].word + " ";
        }

        //
        compiledSentence = compiledSentence.Remove(compiledSentence.Length - 1, 1);

        //
        return compiledSentence;
    }
}
