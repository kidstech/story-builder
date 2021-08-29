using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordHolder : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject WordHolderPopupPrefab;
    [SerializeField]
    private Transform SentenceBuilderCanvas;

    public void OpenWordHolder(Word word)
    {
        GameObject popup = Instantiate(WordHolderPopupPrefab);
        popup.GetComponent<WordHolderPopup>().SetupWordHolderPopup(word);
        popup.transform.SetParent(SentenceBuilderCanvas, false);
    }
}
