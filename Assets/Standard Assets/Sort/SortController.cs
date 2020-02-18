using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortController : MonoBehaviour
{
    // The prefab button for sorting
    public GameObject sortButtonPrefab;

    // The object containing all the word tiles
    public Transform wordBankContentNew;

    // The current letters to filter the search by
    private List<string> lettersToFilter = new List<string>();

    // The list of buttons that will be created and filtered by
    private List<string> sortByCharacters = new List<string>()
    {
        "a",
        "b",
        "c",
        "d",
        "e",
        "f",
        "g",
        "h",
        "i",
        "j",
        "k",
        "l",
        "m",
        "n",
        "o",
        "p",
        "q",
        "r",
        "s",
        "t",
        "u",
        "v",
        "w",
        "x",
        "y",
        "z"
    };

    //
    private void Start()
    {
        //
        wordBankContentNew = transform.parent.Find("Viewport").Find("WordBankContentNew");

        // Begin creating the needed buttons
        for(int i = 0; i < sortByCharacters.Count; i++)
        {
            //
            GameObject sortButton = Instantiate(sortButtonPrefab);

            //
            sortButton.name = sortByCharacters[i];

            //
            sortButton.GetComponentInChildren<Text>().text = sortByCharacters[i];

            //
            sortButton.transform.SetParent(this.transform, false);
        }
    }

    //
    public void UpdateLetterFilter(string letter, bool remove)
    {
        //
        if (remove)
        {
            //
            lettersToFilter.Remove(letter);
        }
        else
        {
            //
            lettersToFilter.Add(letter);
        }

        //
        FilterWordBank();
    }

    //
    public void FilterWordBank()
    {
        //
        int totalLetters = lettersToFilter.Count;

        // For every word tile
        for (int i = 0; i < wordBankContentNew.childCount; i++)
        {

            // If the total letters is zero, that means we just removed a letter and we are filtering nothing, so reenable all tiles.
            if(totalLetters == 0)
            {
                //
                wordBankContentNew.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                // For every letter we need to sort by
                for (int o = 0; o < totalLetters; o++)
                {
                    //
                    Transform wordTile = wordBankContentNew.GetChild(i);

                    //
                    string word = wordTile.GetComponentInChildren<Text>().text;

                    //
                    if (word[0].Equals(lettersToFilter[o][0]))
                    {
                        //
                        wordTile.gameObject.SetActive(true);

                        //
                        break;
                    }
                    else
                    {
                        //
                        wordTile.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    //
    private void SortWordBank()
    {
        
    }
}
