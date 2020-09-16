using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordHolderPopupButton : MonoBehaviour
{
    //
    private void Start()
    {
        //
        GetComponent<Button>().onClick.AddListener(Select);
    }

    //
    private void Select()
    {
        //
        transform.parent.GetComponent<WordHolderPopup>().SelectNewWord(transform.GetSiblingIndex());
    }
}
