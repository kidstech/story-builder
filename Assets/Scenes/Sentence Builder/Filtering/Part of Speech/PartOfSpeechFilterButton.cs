using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartOfSpeechFilterButton : MonoBehaviour
{
    private FilterController fc;

    // Starts of false as the button starts unselected
    public bool isSelected= false;

    // For words, the part of speech of the word is represented by an integer: 0 for nouns, 1 for verbs, 2 for adjectives, and 3 for misc
    // See Word.cs 
    [HideInInspector]
    public int partOfSpeech;

    public ColorBlock defaultCB;
    public ColorBlock swappedCB;

    public Button filterButton;


    // Start is called before the first frame update
    void Start()
    {
        //
        fc = GameObject.Find("WordBankSortingToggle").GetComponent<FilterController>();

        filterButton = GetComponent<Button>();
        filterButton.onClick.AddListener(UpdateFilter);

        defaultCB = filterButton.colors;
        swappedCB = defaultCB;

        
    }


    /*
        * This code chunk is called whenever a filter by part of speech button is selected.
        * If the button is selected, then we set the default color of the button to be its selected color. 
            This is done because of how default Unity Buttons cannot be selected, stay selected, and then be deselected by pressing on the button again.
        * If the button is not selected, then we set the buttons colors to the default colors that the button was instantiated with
         
    */
    private void UpdateFilter()
    {   
        isSelected = !isSelected;

        if(isSelected) {
            swappedCB.normalColor = defaultCB.selectedColor;
            filterButton.colors = swappedCB;
        }

        else {
            filterButton.colors = defaultCB;
        }

        fc.UpdatePartOfSpeechFilter(partOfSpeech, isSelected);
    }
}
