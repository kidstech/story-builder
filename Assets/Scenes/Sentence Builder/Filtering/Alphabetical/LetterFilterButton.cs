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

        //
        GetComponent<Button>().onClick.AddListener(UpdateFilter);
    }

    //
    private void UpdateFilter()
    {
        //
        if (state)
        {
            //
            image.color = Color.white;
        }
        else
        {
            //
            image.color = Color.green;
        }

        //
        fc.UpdateLetterFilter(letter, state);

        //
        state = !state;
    }
}
