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

        //and the letter that was toggled is thrown into the filterwordbank() which activates/deactivates word tiles based on whether the selected letter matches the first
        //character of the first word in the word bank (which then recurses through the other letters it has)
        fc.UpdatePartOfSpeechFilter(partOfSpeech, isSelected);
    }
}
