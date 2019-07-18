using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GenerateSortButtons : MonoBehaviour
{
    // button prefab
    public GameObject buttonPrefab;

    // Master WOrd LIst
    MasterWordList w;

    // Word Bank Script
    buildWordBank b;

    // --
    public List<string> searchLetters;

    // Start is called before the first frame update
    void Start()
    {
        w = LoadContextPacks.loadContextPacks();

        b = GameObject.Find("WordBank").GetComponent<buildWordBank>();

        buildButtons(w.masterWordList);
    }

    public void buildButtons(List<MasterWordList.Word> list)
    {
        // Start by generating the first 26 buttons A-Z
        List<string> alphabet = new List<string>(26) {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

        // For each letter
        for(int i = 0; i < 26; i++)
        {
            // Copy in a new game object
            GameObject o = Instantiate(buttonPrefab);

            // Change the display text
            o.GetComponentInChildren<Text>().text = alphabet[i];

            // Add it into the button view
            o.transform.SetParent(this.transform, false);

        }
    }

    public void updateSearchLetters(string newLetter)
    {
        // Test for the letter already being in our list
        var itemToUpdate = searchLetters.SingleOrDefault(r => r == newLetter);

        // If the item exists, remove it
        if(itemToUpdate != null)
        {
            searchLetters.Remove(itemToUpdate);
        }
        else
        {
            searchLetters.Add(newLetter);
        }

        List<MasterWordList.Word> newList = new List<MasterWordList.Word>();

        newList = w.getLetter(searchLetters);

        b.rebuildWordBank(newList);
    }
}
