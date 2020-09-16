using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordHolder : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject WordHolderPopupPrefab;

    //
    public void OpenWordHolder(Word word)
    {
        //
        GameObject popup = Instantiate(WordHolderPopupPrefab);

        //
        popup.GetComponent<WordHolderPopup>().SetupWordHolderPopup(word);

        // Set as a child of the canvas (WordHolderDrop -> WordHolder -> Canvas)
        popup.transform.SetParent(this.transform.parent.parent.transform, false);
    }
}
