using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupPartOfSpeechFilter : MonoBehaviour
{
    // The prefab button for filtering by  part of speech
    public GameObject partOfSpeechFilterButton;

    //
    private List<string> filterByPartOfSpeech = new List<string>()
    {
        "Noun",
        "Verb",
        "Adjective",
        "Misc"
    };

    /*
        * In this code chunk, we instantiate filter buttons to allow users to filter the word bank by their part of speech.
        * In the word bank, parts of speech are separated by colors: nouns are blue, verbs are green, adjectives are red, and misc is yellow.
        * Colors were created based on the colors identified in the BuildWordBankNew.cs script
    */
    private void Start()
    {

        for (int i = 0; i < filterByPartOfSpeech.Count; i++)
        {
            //
            GameObject sortButton = Instantiate(partOfSpeechFilterButton);

            //
            sortButton.name = filterByPartOfSpeech[i].ToString();

            Button button = sortButton.GetComponent<Button>();

            ColorBlock cb = button.colors;

            switch(filterByPartOfSpeech[i]) {

                case "Noun" :
                    Color nounColor = new Color(0.357f, 0.608f, 0.835f);
                    cb.normalColor = nounColor;
                    cb.highlightedColor = Color.Lerp(nounColor, Color.black, 0.2f);
                    cb.pressedColor = Color.Lerp(nounColor, Color.black, 0.35f);
                    cb.selectedColor = Color.Lerp(nounColor, Color.black, 0.5f);
                    button.colors = cb;
                    sortButton.AddComponent<PartOfSpeechFilterButton>().partOfSpeech = 0;
                    break;

                case "Verb" :
                    Color verbColor = new Color(0.439f, 0.678f, 0.278f);
                    cb.normalColor = verbColor;
                    cb.highlightedColor = Color.Lerp(verbColor, Color.black, 0.2f);
                    cb.pressedColor = Color.Lerp(verbColor, Color.black, 0.35f);
                    cb.selectedColor = Color.Lerp(verbColor, Color.black, 0.5f);
                    button.colors = cb;
                    sortButton.AddComponent<PartOfSpeechFilterButton>().partOfSpeech = 1;
                    break;

                case "Adjective" :
                    Color adjectiveColor = new Color(0.929f, 0.49f, 0.192f);
                    cb.normalColor = adjectiveColor;
                    cb.highlightedColor = Color.Lerp(adjectiveColor, Color.black, 0.2f);
                    cb.pressedColor = Color.Lerp(adjectiveColor, Color.black, 0.35f);
                    cb.selectedColor = Color.Lerp(adjectiveColor, Color.black, 0.5f);
                    button.colors = cb;
                    sortButton.AddComponent<PartOfSpeechFilterButton>().partOfSpeech = 2;
                    break;

                case "Misc" :
                    Color miscColor = new Color(1f, 0.753f, 0f);
                    cb.normalColor = miscColor;
                    cb.highlightedColor = Color.Lerp(miscColor, Color.black, 0.2f);
                    cb.pressedColor = Color.Lerp(miscColor, Color.black, 0.35f);
                    cb.selectedColor = Color.Lerp(miscColor, Color.black, 0.5f);
                    button.colors = cb;
                    sortButton.AddComponent<PartOfSpeechFilterButton>().partOfSpeech = 3;
                    break;
            }

            // parent is WordBankSortingPartOfSpeech
            sortButton.transform.SetParent(this.transform, true);
        }
    }
}