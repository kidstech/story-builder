using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordHolder : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject WordHolderPopupPrefab;
    [SerializeField]
    private Transform SentenceBuilderCanvas;

    [SerializeField]
    private GameObject wordForms;
    public static Transform wordHolderDropZoneTransform;

    public void Start() {
        wordHolderDropZoneTransform = this.transform;
    }

    public void OpenWordHolder(Word word)
    {
        // GameObject popup = Instantiate(WordHolderPopupPrefab);
        // popup.GetComponent<WordHolderPopup>().SetupWordHolderPopup(word);
        NewWordHolder.word2 = word;
        wordForms.GetComponent<NewWordHolder>().setUpForms();
        // popup.transform.SetParent(SentenceBuilderCanvas, false);
         wordForms.SetActive(true);
    }
}
