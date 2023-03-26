using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupLetterFilter : MonoBehaviour
{
    // The prefab button for filtering
    public GameObject letterFilterButton;

    //
    private List<string> filterByLetters = new List<string>()
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

        for (int i = 0; i < filterByLetters.Count; i++)
        {
            //
            GameObject sortButton = Instantiate(letterFilterButton);

            //
            sortButton.name = filterByLetters[i].ToString();

            //
            sortButton.GetComponentInChildren<Text>().text = filterByLetters[i].ToString();

            //
            sortButton.AddComponent<LetterFilterButton>().letter = filterByLetters[i];

            // parent is wordbanksortingalphabet
            sortButton.transform.SetParent(this.transform, true);
        }
    }
}
