using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordHolderPopupButton : MonoBehaviour
{
    // The word this button represents;
    public string word;

    // Start is called before the first frame update
    void Start()
    {
        // Assigns a listener for clicking (onClick) and when it is fired, runs setWordTile after finding the required bits.
        // "WordHolderPopup(Clone)" is needed because it is Instantiated in, in WordHolderPopup
        gameObject.GetComponent<Button>().onClick.AddListener(() => GameObject.Find("WordHolderPopup(Clone)").GetComponent<WordHolderPopup>().setWordTile(gameObject.GetComponentInChildren<Text>().text));
    }
}
