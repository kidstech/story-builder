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

    void Start()
    {
        // Populate the saved sentences into the completed sentences scrollview.
        allSavedSentences = SaveSentenceHandler.LoadJson();

        // Set up the saved sentences
        setupSavedSentences();
    }

    public void setupSavedSentences()
    {
        Debug.Log("Setting up " + allSavedSentences.Count + " new sentences.");

        // Begin Instantiating SavedSentence objects for every saved sentence in the list
        for(int i = 0; i < allSavedSentences.Count; i++)
        {
            // Load up a new object
            Transform obj = (Transform)Instantiate(savedSentencePrefab);

            // Change that object's text field to be the sentence
            obj.GetChild(1).GetComponent<Text>().text = allSavedSentences[i].sentence;

            // Add that sentence to the savedSentenceBox
            obj.transform.SetParent(savedSentenceBox);
        }
    }

    public void refreshSavedSentences()
    {

    }
}
