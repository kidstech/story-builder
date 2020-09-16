using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterController : MonoBehaviour
{
    // The object containing all the word tiles
    public Transform wordBankContentNew;

    // The current letters to filter the search by
    private List<string> lettersToFilter = new List<string>();
    private List<int> packsToFilter = new List<int>();

    //
    private void Start()
    {
        //
        wordBankContentNew = GameObject.Find("WordBankContentNew").transform;
    }

    //
    public void FilterWordBank()
    {
        //
        int totalLetters = lettersToFilter.Count;
        int totalPacks = packsToFilter.Count;

        // Keep track of the number of available tiles for use in calculating WordBank size.
        int numEnabledTiles = 0;

        // For every word tile
        for (int i = 0; i < wordBankContentNew.childCount; i++)
        {
            // Get all relevant information
            Transform wordTile = wordBankContentNew.GetChild(i);
            Word word = wordTile.GetComponent<WordTile>().word;

            // By default, enable the tile
            wordTile.gameObject.SetActive(true);

            // Bools for processing combination of OR and AND filters
            bool matchesLetter = false;
            bool matchesPack = false;

            // For every letter we need to sort by
            for (int o = 0; o < totalLetters; o++)
            {
                //
                if (word.word.ToLower()[0] == lettersToFilter[o].ToLower()[0])
                {
                    //
                    matchesLetter = true;

                    //
                    break;
                }
            }

            // For every pack we need to sort by
            for (int o = 0; o < totalPacks; o++)
            {
                //
                if (word.contextPackId == packsToFilter[o])
                {
                    //
                    matchesPack = true;

                    //
                    break;
                }
            }

            //
            if (totalLetters == 0) matchesLetter = true;
            if (totalPacks == 0) matchesPack = true;

            // Determine if the tile should be enabled or not.
            if(matchesLetter && matchesPack)
            {
                //
                wordTile.gameObject.SetActive(true);

                //
                numEnabledTiles++;
            }
            else
            {
                //
                wordTile.gameObject.SetActive(false);
            }
            
        }

        //
        wordBankContentNew.GetComponent<WordBank>().SortWordBank();

        //
        wordBankContentNew.GetComponent<WordBank>().ResizeWordBank(numEnabledTiles);
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
    public void UpdatePackFilter(int packId, bool remove)
    {
        //
        if (remove)
        {
            //
            packsToFilter.Remove(packId);
        }
        else
        {
            //
            packsToFilter.Add(packId);
        }

        //
        FilterWordBank();
    }
}
