﻿using System.Collections;
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
    private bool dragging;
    private GameObject sentence = null;
    private bool drop = false;
    
    public void Start()
    {
        sentence = GameObject.Find("Sentence");
    }    

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedObject = gameObject;
        startPosition = transform.position;
        startSibIndex = transform.GetSiblingIndex();
        transform.SetAsLastSibling();
        transform.GetComponent<Image>().raycastTarget = false; // stop the object from blocking raycasts
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // if we drag and release our sentence over the sentence bar, resubmit it
        if (eventData.hovered.Contains(sentence))
        {
            resubmitSentence(draggedObject.GetComponentInChildren<SaveSentenceTiles>().savedSentence);
        }
        draggedObject = null;
        transform.position = startPosition; // reset the submitted sentence object back to its original position
        transform.SetSiblingIndex(startSibIndex); // reset parent so dragged object is no longer on top of everything
        transform.GetComponent<Image>().raycastTarget = true; // allow raycasts to hit the object again so we can drag it later
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