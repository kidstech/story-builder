using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SaveSentenceTiles : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject draggedObject;
    private Vector3 startPosition;
    public List<WordTile> savedSentence; // storage for the latest submitted word tiles in case the user wishes to modify their sentence
    // Start is called before the first frame update
    private bool dragging;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedObject = gameObject;
        startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject sentence = GameObject.Find("Sentence");
        // if we drag and release our sentence over the sentence bar, resubmit it
        if (eventData.hovered.Contains(sentence))
        {
            resubmitSentence(draggedObject.GetComponent<SaveSentenceTiles>().savedSentence);
        }
        // reset the submitted sentence object back to its original position
        draggedObject = null;
        transform.position = startPosition;
    }

    // note: with current implementation, the resubmitted word tiles cannot be placed in between word tiles in the sentence bar. They will always append themselves to the end.
    // Not sure if we want to be able to stuff the resubmitted sentence between words that have already been placed or not.
    public void resubmitSentence(List<WordTile> sentenceTiles)
    {
        GameObject sentence = GameObject.Find("Sentence");
        foreach(WordTile wordtile in sentenceTiles)
        {
            // copy the word tile object and make it a child of SentenceInTiles 
            GameObject wordTileCopy = Instantiate(wordtile.gameObject, this.transform);
            sentence.GetComponent<SentenceBar>().ResizeSentence(1);
            // move the copy word tile over to the sentence bar
            wordTileCopy.transform.SetParent(sentence.transform);
        }
    }


}
