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
    private List<SavedSentence> sentences = new List<SavedSentence>();

    // saved sentence bank disabled when not in use => change scene sentencebuilder -> storybuilder needs to activate the
    void OnEnable()
    {
        //Debug.Log(this.transform.name + " has been enabled");
        sentences = LoadSavedSentences.LoadSentences();

        sentencePrefabSize = sentencePrefab.GetComponent<RectTransform>().sizeDelta;
        GetComponent<RectTransform>().sizeDelta = new Vector2(sentencePrefabSize.x, sentencePrefabSize.y * sentences.Count);

        // create sentence game object for each sentence we have and populate its components
        Debug.Log(sentences.Count);
        for (int i = sentences.Count-1; i >= 0; i--)
        {
            Debug.Log("Hello there");
            GameObject newSentence = Instantiate(sentencePrefab);

            newSentence.GetComponent<SentenceObject>().savedSentence = sentences[i];
            newSentence.GetComponentInChildren<Text>().text = sentences[i].sentenceText;
            newSentence.transform.SetParent(this.transform, false);
            newSentence.GetComponent<SentenceTile>().textToDisplay = sentences[i].sentenceText; // update textToDisplay for SentenceTile script bc that's what it uses for tts
        }
    }
    void OnDisable()
    {
        //Debug.Log(this.transform.name + " has been disabled");
        // empty the bank each time so we don't create duplicates
        foreach (Transform child in this.transform)
        {
            Debug.Log("destroying: " + child.gameObject.name);
            GameObject.Destroy(child.gameObject);
        }
    }

    public static string CompileSentence(List<WordTile> tiles)
    {
        string compiledSentence = "";

        for(int i = 0; i < tiles.Count; i++)
        {
            // using textToDisplay here because that's what gets changed in the wordholder
            compiledSentence += tiles[i].textToDisplay + " ";
        }

        compiledSentence = compiledSentence.Remove(compiledSentence.Length - 1, 1);

        return compiledSentence;
    }
}
