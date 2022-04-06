using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordHolderPopup : MonoBehaviour
{
    public GameObject WordHolderPopupPrefabButton;
    public GameObject WordHolderDrop;

    public int index;
    public int previousIndex;
    public int midIndex;
    private Word word;

    public List<GameObject> buttons = new List<GameObject>();
    
    [SerializeField]
    private Button closeForms;
    [SerializeField]
    private GameObject newFormsPopUp;

    void Start()
    {
        WordHolderDrop = GameObject.Find("WordHolderDrop");
        closeForms.onClick.AddListener(()=> newFormsPopUp.SetActive(false));
    }
    public void SetupWordHolderPopup(Word word)
    {
        // Middle Word
        GameObject midButton = Instantiate(WordHolderPopupPrefabButton);
        midButton.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        midButton.GetComponent<Image>().color = Color.yellow;
        midButton.GetComponent<Button>().interactable = false;
        midButton.GetComponentInChildren<Text>().text = word.baseWord;
        midButton.transform.SetParent(this.transform, false);

        previousIndex = midButton.transform.GetSiblingIndex();
        midIndex = previousIndex;
        buttons.Add(midButton);
        for (int i = 0; i < word.forms.Count; i++)
        {
            if(word.forms.Count != 1) {
            GameObject button = Instantiate(WordHolderPopupPrefabButton);
            float offset = i * 2 * Mathf.PI / word.forms.Count + Mathf.PI / 2;
            button.GetComponent<RectTransform>().localPosition = new Vector2(200 * Mathf.Cos(offset), 200 * Mathf.Sin(offset));
            button.GetComponentInChildren<Text>().text = word.forms[i];
            button.transform.SetParent(this.transform, false);
            buttons.Add(button);
        }
        this.word = word;
        }
    }

    public void SelectNewWord(int index)
    {
        transform.GetChild(index).GetComponent<Image>().color = Color.yellow;
        transform.GetChild(previousIndex).GetComponent<Image>().color = Color.white;
        // set both the text component and word string to the text of the wordform button the user selected
        transform.GetChild(midIndex).GetComponentInChildren<Text>().text = transform.GetChild(index).GetComponentInChildren<Text>().text;
        previousIndex = index;

    }

    public void CloseWordHolderPopup()
    {
        // Both of the following lines seem to be needed at the moment, 
        // and that is probably a sign that there is a place we could simplify something.
        WordHolderDrop.GetComponentInChildren<Text>().text = transform.GetChild(midIndex).GetComponentInChildren<Text>().text;
        WordHolderDrop.GetComponentInChildren<WordTile>().textToDisplay = transform.GetChild(midIndex).GetComponentInChildren<Text>().text;
        Destroy(this.gameObject);
    }
}
