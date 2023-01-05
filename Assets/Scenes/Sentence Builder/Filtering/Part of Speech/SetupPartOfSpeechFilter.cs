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
        * Colors were created based on the RGB values of the word tile colors in the word bank.
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
                    Color nounColor = new Color32(91, 155, 213, 255);
                    cb.normalColor = nounColor;
                    cb.highlightedColor = Color.Lerp(nounColor, Color.black, 0.2f);
                    cb.pressedColor = Color.Lerp(nounColor, Color.black, 0.35f);
                    cb.selectedColor = Color.Lerp(nounColor, Color.black, 0.5f);
                    button.colors = cb;
                    break;

                case "Verb" :
                    Color verbColor = new Color32(112, 173, 71, 255);
                    cb.normalColor = verbColor;
                    cb.highlightedColor = Color.Lerp(verbColor, Color.black, 0.2f);
                    cb.pressedColor = Color.Lerp(verbColor, Color.black, 0.35f);
                    cb.selectedColor = Color.Lerp(verbColor, Color.black, 0.5f);
                    button.colors = cb;
                    break;

                case "Adjective" :
                    Color adjectiveColor = new Color32(237, 125, 49, 255);
                    cb.normalColor = adjectiveColor;
                    cb.highlightedColor = Color.Lerp(adjectiveColor, Color.black, 0.2f);
                    cb.pressedColor = Color.Lerp(adjectiveColor, Color.black, 0.35f);
                    cb.selectedColor = Color.Lerp(adjectiveColor, Color.black, 0.5f);
                    button.colors = cb;
                    break;

                case "Misc" :
                    Color miscColor = new Color32(255, 192, 0, 255);
                    cb.normalColor = miscColor;
                    cb.highlightedColor = Color.Lerp(miscColor, Color.black, 0.2f);
                    cb.pressedColor = Color.Lerp(miscColor, Color.black, 0.35f);
                    cb.selectedColor = Color.Lerp(miscColor, Color.black, 0.5f);
                    button.colors = cb;
                    break;
            }

            // parent is WordBankSortingPartOfSpeech
            sortButton.transform.SetParent(this.transform, true);
        }
    }
}