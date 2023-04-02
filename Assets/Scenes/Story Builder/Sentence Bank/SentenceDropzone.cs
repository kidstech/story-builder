﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SentenceDropzone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum Behavior
    {
        Default,
        SentenceBank,
        Page
    }
    public Behavior behavior = Behavior.Default;

    private int sentenceNum = 0;
    public Page page;

    [SerializeField] 
    public AudioSource errorNoise;

    [SerializeField] 
    public SavedSentenceBank ssBank;

    public int maxSentences = 8;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(eventData.pointerDrag == null)
        {
            return;
        }

        DraggableSentence d = eventData.pointerDrag.GetComponent<DraggableSentence>();

        if(d != null)
        {
            d.placeholderParent = this.transform;

            d.heldOver = behavior;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }
        DraggableSentence d = eventData.pointerDrag.GetComponent<DraggableSentence>();

        if(d != null)
        {
            if (d.placeholderParent == this.transform)
            {
                d.placeholderParent = d.parentToReturnTo;
            }



            d.heldOver = Behavior.Default;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedSentence = null;
        DraggableSentence d = eventData.pointerDrag.GetComponent<DraggableSentence>();
        droppedSentence = eventData.pointerDrag;
        SentenceObject s = eventData.pointerDrag.GetComponent<SentenceObject>();

        if(d != null)
        {
            switch (behavior) {
                case Behavior.SentenceBank:
                    Destroy(d.placeholder);
                    break;

                case Behavior.Page:
                    if (sentenceNum < maxSentences)
                    {
                        d.parentToReturnTo = this.transform;
                        sentenceNum ++;
                    }
                   else {
                        Destroy(droppedSentence);
                        Destroy(d.placeholder);
                        errorNoise.Play();
                    }
                    break;

                default:
                    StartCoroutine(ServerRequestHandler.UpdateSentence(s.savedSentence.sentenceId));
                    Destroy(droppedSentence);
                    Destroy(d.placeholder);
                    Destroy(eventData.pointerDrag);
                    ssBank.sentenceIds.Add(s.savedSentence.sentenceId);
                    break;
            }
        }
    }
}