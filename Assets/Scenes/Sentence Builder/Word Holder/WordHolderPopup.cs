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

        //
        midButton.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
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

        //
        for (int i = 0; i < word.forms.Count; i++)
        {
            //
            GameObject button = Instantiate(WordHolderPopupPrefabButton);

            //
            float offset = i * 2 * Mathf.PI / word.forms.Count + Mathf.PI / 2;

            //
            button.GetComponent<RectTransform>().localPosition = new Vector2(200 * Mathf.Cos(offset), 200 * Mathf.Sin(offset));

            //
            button.GetComponentInChildren<Text>().text = word.forms[i];

            //
            button.transform.SetParent(this.transform, false);

            //
            buttons.Add(button);
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
