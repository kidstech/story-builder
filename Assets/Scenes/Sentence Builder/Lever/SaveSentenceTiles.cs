using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SaveSentenceTiles : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject draggedObject;
    int startSibIndex;
    private Vector3 startPosition;
    public List<WordTile> savedSentence; // storage for the latest submitted word tiles in case the user wishes to modify their sentence
    // Start is called before the first frame update
    private bool dragging;
    private GameObject sentence = null;
    private bool drop = false;
    
    public void Start()
    {
        sentence = GameObject.Find("Sentence");
    }    

    // seems like we'll need to stop blocking raycasts on the object we are dragging so that we can actually detect stuff beneath it
    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedObject = gameObject;
        startPosition = transform.position;
        startSibIndex = transform.GetSiblingIndex();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        transform.position = Input.mousePosition;
        // if(eventData.hovered.Contains(sentence)){
        //     drop = true; //hovering over the sentence view, we want to resubmit the sentence
        //     transform.SetAsLastSibling();// ensure the dragged object is over the sentence view
        // } else {
        //     drop = false;
        //     transform.SetSiblingIndex(startSibIndex); // drop the dragged object back below the sentence view so that eventData.hovered can still detect it
        // }
    }

    // There's a bit of a weird quirk of eventData.hovered here, in that it seems to only work in finding objects that are below what we are dragging in the hierarchy.
    // Because of this sentence resubmission will break if SentenceScrollview ever finds its way above MostRecentSavedSentence
    public void OnEndDrag(PointerEventData eventData)
    {
        // if we drag and release our sentence over the sentence bar, resubmit it
        if (eventData.hovered.Contains(sentence))
        {
            resubmitSentence(draggedObject.GetComponentInChildren<SaveSentenceTiles>().savedSentence);
        }
        // reset the submitted sentence object back to its original position
        draggedObject = null;
        transform.position = startPosition;
        transform.SetSiblingIndex(startSibIndex); // reset parent so dragged object is no longer on top of everything
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
