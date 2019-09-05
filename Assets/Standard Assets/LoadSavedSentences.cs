using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LoadSavedSentences : MonoBehaviour
{
    // List of all the saved sentences
    private List<SavedSentence> allSavedSentences;

    // The SavedSentence prefab
    public Transform savedSentencePrefab;

    // The CompletedSentences GameObject in the scene.
    public Transform savedSentenceBox;

    // Size of the savedSentencePrefab's y cord
    public static readonly int SAVED_SENTENCE_PREFAB_HEIGHT = 50;

    void Start()
    {
        // Populate the saved sentences into the completed sentences scrollview.
        allSavedSentences = SaveSentenceHandler.LoadJson();

        // Set up the saved sentences
        setupSavedSentences();
    }

    public void setupSavedSentences()
    {
        RectTransform content = (RectTransform)savedSentenceBox.transform;

        content.sizeDelta = new Vector2(content.sizeDelta.x, SAVED_SENTENCE_PREFAB_HEIGHT * allSavedSentences.Count);

        // Begin Instantiating SavedSentence objects for every saved sentence in the list
        for(int i = 0; i < allSavedSentences.Count; i++)
        {
            // Load up a new object
            Transform obj = (Transform)Instantiate(savedSentencePrefab);

            // Change that object's text field to be the sentence
            obj.GetChild(1).GetComponent<Text>().text = allSavedSentences[i].sentence;

            // Add that sentence to the savedSentenceBox
            obj.transform.SetParent(savedSentenceBox, false);
        }
    }

    private void clear()
    {
        foreach(Transform t in this.transform)
        {
            Destroy(t.gameObject);
        }
    }

    public void refreshSavedSentences()
    {
        // Delete all previous entries
        clear();

        // Populate the saved sentences into the completed sentences scrollview.
        allSavedSentences = SaveSentenceHandler.LoadJson();

        // Set up the saved sentences
        setupSavedSentences();
    }
}
