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

    private List<string> packsToFilter = new List<string>();
    
    private int partOfSpeechToFilter = -1;

    private LetterFilterButton[] letterButtons = new LetterFilterButton[25];
    private PartOfSpeechFilterButton[] partOfSpeechButtons = new PartOfSpeechFilterButton[4];

    //
    private void Start()
    {
        //
        wordBankContentNew = GameObject.Find("WordBankContentNew").transform;

        // array of all the LetterFilterButton game objects so we can toggle their filter states without them being clicked on.
        letterButtons = GameObject.FindObjectsOfType<LetterFilterButton>();
        
        partOfSpeechButtons = GameObject.FindObjectsOfType<PartOfSpeechFilterButton>();

    }

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
            bool matchesPartOfSpeech = false;

            // Check if the part of the speech of the current word matches the part of speech we want to filter by
            if(word.partOfSpeechId == partOfSpeechToFilter) {
                matchesPartOfSpeech = true;
            }

            // For every letter we need to sort by
            for (int o = 0; o < totalLetters; o++)
            {
                //
                if (word.baseWord.ToLower()[0] == lettersToFilter[o].ToLower()[0])
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

            if (totalLetters == 0) matchesLetter = true;
            if (totalPacks == 0) matchesPack = true;
            if(partOfSpeechToFilter == -1) matchesPartOfSpeech = true;

            // Determine if the tile should be enabled or not.
            if(matchesLetter && matchesPack && matchesPartOfSpeech)
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
            for (int i = 0; i < 26; i++)
            {   
                // if the image of the LetterFilterButton is green, and it's not the letter we actually want to add to the filter...
                if (letterButtons[i].GetComponent<LetterFilterButton>().image.color == Color.green && letterButtons[i].GetComponent<LetterFilterButton>().letter != letter)
                {   
                    // change it back to white
                    letterButtons[i].GetComponent<LetterFilterButton>().image.color = Color.white;
                    // and invert its state (so we don't try and change it to white again when the button is clicked next time)
                    letterButtons[i].GetComponent<LetterFilterButton>().state = !letterButtons[i].GetComponent<LetterFilterButton>();
                }
            }
            // reflect the changes in our letter filter 
            lettersToFilter.Clear();
            lettersToFilter.Add(letter);
        }

        //
        FilterWordBank();
    }

    //
    public void UpdatePackFilter(string packId, bool remove)
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


    public void UpdatePartOfSpeechFilter(int partOfSpeech, bool isSelected) {

        if(!isSelected) {
            partOfSpeechToFilter = -1;
        }
        else {

            // Check if other parts of speech buttons are currently selected.
            // If there is one that is already selected, then reset its colors to the default colors and change isSelected to false
            for(int i = 0; i < partOfSpeechButtons.Length; i++) {
                PartOfSpeechFilterButton x = partOfSpeechButtons[i].GetComponent<PartOfSpeechFilterButton>();
                if(x.partOfSpeech != partOfSpeech && x.filterButton.colors.normalColor == x.defaultCB.selectedColor) {
                    x.filterButton.colors = x.defaultCB;
                    x.isSelected = false;
                }
            }
            partOfSpeechToFilter = partOfSpeech;
        }
        FilterWordBank();
    }
}