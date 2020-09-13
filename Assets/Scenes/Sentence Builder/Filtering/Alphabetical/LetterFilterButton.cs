using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterFilterButton : MonoBehaviour
{
    //
    private FilterController fc;
    private Image image;

    //
    private bool state = false;

    [HideInInspector]
    public string letter;

    // 
    void Start()
    {
        //
        fc = GameObject.Find("WordBankSortingToggle").GetComponent<FilterController>();

        //
        image = GetComponent<Image>();

        //make it so that when you click on a letter filter button, it calls UpdateFilter
        GetComponent<Button>().onClick.AddListener(UpdateFilter);
        
    }

    private void UpdateFilter()
    {   


        //state starts at false, so when this function is called (upon initially clicking a letter tile), it toggles its color to green
        if (state)
        {
            //
            image.color = Color.white;
        }
        else
        {
            image.color = Color.green;
        }

        //and the letter that was toggled is thrown into the filterwordbank() which activates/deactivates word tiles based on whether the selected letter matches the first
        //character of the first word in the word bank (which then recurses through the other letters it has)
        fc.UpdateLetterFilter(letter, state);

        //
        state = !state;
    }
}
