using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedSentenceBank : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject sentencePrefab;

    private Vector2 sentencePrefabSize;
    private string sentenceText;

    // saved sentence bank disabled when not in use => change scene sentencebuilder -> storybuilder needs to activate the
    void OnEnable()
    {
        Debug.Log(this.transform.name + " has been enabled");
        List<SavedSentence> sentences = LoadSavedSentences.LoadSentences();

        sentencePrefabSize = sentencePrefab.GetComponent<RectTransform>().sizeDelta;
        GetComponent<RectTransform>().sizeDelta = new Vector2(sentencePrefabSize.x, sentencePrefabSize.y * sentences.Count);

        for (int i = 0; i < sentences.Count; i++)
        {
            GameObject newSentence = Instantiate(sentencePrefab);

            newSentence.GetComponent<SentenceObject>().savedSentence = sentences[i];

            sentenceText = CompileSentence(sentences[i].words);
            newSentence.GetComponentInChildren<Text>().text = sentenceText;

            newSentence.transform.SetParent(this.transform, false);

            newSentence.GetComponent<SentenceTile>().textToDisplay = sentenceText; // update textToDisplay for SentenceTile script bc that's what it uses for tts
        }
    }
    void OnDisable()
    {
        Debug.Log(this.transform.name + " has been disabled");
        // empty the bank each time so we don't create duplicates
        foreach (Transform child in this.transform)
        {
            Debug.Log("destroying: " + child.gameObject.name);
            GameObject.Destroy(child.gameObject);
        }
    }

    public static string CompileSentence(List<Word> words)
    {
        string compiledSentence = "";

        for(int i = 0; i < words.Count; i++)
        {
            compiledSentence += words[i].word + " ";
        }

        compiledSentence = compiledSentence.Remove(compiledSentence.Length - 1, 1);

        return compiledSentence;
    }
}
