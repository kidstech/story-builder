using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterController : MonoBehaviour
{
    // The prefab button for filtering
    public GameObject alphaFilterButton;
    public GameObject packFilterButton;

    // The object containing all the word tiles
    public Transform wordBankContentNew;

    //
    public Transform wordBankSortingAlphabet;
    public Transform wordBankSortingPacks;

    // The current letters to filter the search by
    private List<string> lettersToFilter = new List<string>();
    private List<string> packsToFilter = new List<string>();

    // The list of buttons that will be created and filtered by
    private List<string> filterByCharacters = new List<string>()
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
    private List<ContextPack> filterByPacks;

    //
    private void Start()
    {
        //
        filterByPacks = LoadContextPacks.loadContextPacks();

        //
        wordBankContentNew = GameObject.Find("WordBankContentNew").transform;

        // Begin creating the needed buttons
        for(int i = 0; i < filterByCharacters.Count; i++)
        {
            //
            GameObject sortButton = Instantiate(alphaFilterButton);

            //
            sortButton.name = filterByCharacters[i];

            //
            sortButton.GetComponentInChildren<Text>().text = filterByCharacters[i];

            //
            sortButton.transform.SetParent(wordBankSortingAlphabet.transform, false);
        }

        //
        for(int i = 0; i < filterByPacks.Count; i++)
        {
            //
            GameObject sortButton = Instantiate(packFilterButton);

            //
            sortButton.name = filterByPacks[i].contextPackName;

            //
            if(filterByPacks[i].contextPackIconPath != "")
            {
                //
            }
            else
            {
                //
                sortButton.GetComponentInChildren<Text>().text = filterByPacks[i].contextPackName;
            }

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
}
