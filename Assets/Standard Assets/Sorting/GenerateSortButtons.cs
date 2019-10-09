using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GenerateSortButtons : MonoBehaviour
{
    // button prefab
    public GameObject buttonPrefab;

    // back button prefab
    public GameObject backButtonPrefab;

    // Master WOrd LIst
    MasterWordList w;

    // Word Bank Script
    buildWordBank b;

    // --
    public List<string> searchLetters;

    // Keep track of what was the last button clicked
    GameObject oldButton = null;

    // Keep track of what was the last color of the button
    private Color c_green = new Color(0, 255, 0, 1);
    private Color c_white = new Color(1, 1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        w = LoadContextPacks.loadContextPacks();

        b = GameObject.Find("WordBankContent").GetComponent<buildWordBank>();

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

        // Add in the button to close the menu
        GameObject backButton = Instantiate(backButtonPrefab);

        backButton.transform.SetParent(this.transform, false);

        backButton.name = "SortButtonPrefabBack";
    }

    public void updateSearchLetters(GameObject button)
    {
        // If this is the same button
        if(button == oldButton)
        {
            string letter = button.GetComponentInChildren<Text>().text;

            bool letterOn = w.ToggleFilter(letter);

            if(letterOn)
            {
                button.GetComponent<Image>().color = c_green;
            }
            else
            {
                button.GetComponent<Image>().color = c_white;
            }
        }
        // This is a different button
        else
        {
            if(oldButton != null)
            {
                oldButton.GetComponent<Image>().color = c_white;

                string oldLetter = oldButton.GetComponentInChildren<Text>().text;

                w.ToggleFilter(oldLetter);
            }

            string letter = button.GetComponentInChildren<Text>().text;

            bool letterOn = w.ToggleFilter(letter);

            button.GetComponent<Image>().color = c_green;

            oldButton = button;
        }

        List<MasterWordList.Word> newList = new List<MasterWordList.Word>();

        newList = w.getFilteredWords(); //w.getLetter(searchLetters);

        b.rebuildWordBank(newList);
    }
}
