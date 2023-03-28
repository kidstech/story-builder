using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordHolder : MonoBehaviour
{
    [SerializeField]
    private GameObject wordForms;
    [SerializeField]
    private Button formDialButton;
    public static Transform wordHolderDropZoneTransform;

    /*
        * Sets up the word form dial button
    */
    public void Start() {
        wordHolderDropZoneTransform = this.transform;
        formDialButton.onClick.AddListener(()=> {
            if(WordHolder.wordHolderDropZoneTransform.childCount > 0 && !wordForms.activeSelf) {
                 OpenWordHolder(wordHolderDropZoneTransform.GetChild(0).GetComponent<WordTile>().word);
            }
            else {
                wordForms.SetActive(false);
            }
        });
    }

    /*
        * Passes the word to the NewWordHolderPopup and calls the setUpForms method and enables the popup window
    */
    public void OpenWordHolder(Word word)
    {
        NewWordHolderPopup.word = word;
        wordForms.GetComponent<NewWordHolderPopup>().setUpForms();
        wordForms.SetActive(true);
    }
}
