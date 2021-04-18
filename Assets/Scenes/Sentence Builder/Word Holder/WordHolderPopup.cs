using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordHolderPopup : MonoBehaviour
{
    //
    public GameObject WordHolderPopupPrefabButton;

    //
    public int index;
    public int previousIndex;
    public int midIndex;

    //
    private Word word;

    //
    public List<GameObject> buttons = new List<GameObject>();

    //
    public void SetupWordHolderPopup(Word word)
    {
        // Middle Word
        GameObject midButton = Instantiate(WordHolderPopupPrefabButton);
        float buttonWidth = midButton.GetComponent<RectTransform>().rect.width;
        float buttonHeight = midButton.GetComponent<RectTransform>().rect.height;

        //
        midButton.GetComponent<RectTransform>().localPosition = new Vector2(0, 2*buttonHeight); // top middle
        midButton.GetComponent<Image>().color = Color.yellow;
        midButton.GetComponent<Button>().interactable = false;
        midButton.GetComponentInChildren<Text>().text = word.word;

        //
        midButton.transform.SetParent(this.transform, false);

        //
        previousIndex = midButton.transform.GetSiblingIndex();
        midIndex = previousIndex;

        //
        buttons.Add(midButton);


        /**
        Note: All this colNumber and rowNumber stuff should probably just be refactored to use prefabs and a gridlayout group like the wordbank eventually if we end up keeping this layout
        **/
        int colNumber = 1; 
        int rowNumber = 1;
        float topRowPosition = midButton.GetComponent<RectTransform>().localPosition.y;
        //
        for (int i = 0; i < word.forms.Count; i++)
        {
            //
            GameObject button = Instantiate(WordHolderPopupPrefabButton);
            //
            button.GetComponentInChildren<Text>().text = word.forms[i];
            //
            button.transform.SetParent(this.transform, false);

            //
            buttons.Add(button);

            // positioning word form buttons in rows of 3 starting from the top left
            if (colNumber == 1)
            {
                button.GetComponent<RectTransform>().localPosition = new Vector2(-buttonWidth, topRowPosition - rowNumber * buttonHeight);
            }
            else if (colNumber == 2)
            {
                button.GetComponent<RectTransform>().localPosition = new Vector2(0, topRowPosition - rowNumber * buttonHeight);
            }
            else // col number is 3
            {
                button.GetComponent<RectTransform>().localPosition = new Vector2(buttonWidth, topRowPosition - rowNumber * buttonHeight);
            }

            /* cloud based word form list positioning
            float offset = i * 2 * Mathf.PI / word.forms.Count + Mathf.PI / 2;
            button.GetComponent<RectTransform>().localPosition = new Vector2(200 * Mathf.Cos(offset), 200 * Mathf.Sin(offset));
            */

            if (colNumber < 3) colNumber++;
            else // we've made 3 columns. switch to new row and reset column count
            {
                colNumber = 1; 
                rowNumber++;
            }
        }

        //
        this.word = word;
    }

    //
    public void SelectNewWord(int index)
    {
        //
        transform.GetChild(index).GetComponent<Image>().color = Color.yellow;

        //
        transform.GetChild(previousIndex).GetComponent<Image>().color = Color.white;

        //
        transform.GetChild(midIndex).GetComponentInChildren<Text>().text = transform.GetChild(index).GetComponentInChildren<Text>().text;

        //
        previousIndex = index;

    }

    //
    public void CloseWordHolderPopup()
    {
        // Both of the following lines seem to be needed at the moment, 
        // and that is probably a sign that there is a place we could simplify something.
        GameObject.Find("WordHolderDrop").GetComponentInChildren<Text>().text = transform.GetChild(midIndex).GetComponentInChildren<Text>().text;
        GameObject.Find("WordHolderDrop").GetComponentInChildren<WordTile>().textToDisplay = transform.GetChild(midIndex).GetComponentInChildren<Text>().text;
        //
        Destroy(this.gameObject);
    }
}
