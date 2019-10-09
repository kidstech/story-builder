using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Crosstales.RTVoice;
using System;

public class SavedSentenceObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Transform canvas;

    private bool inSentenceSlot = false;

    StoryCreationHandler storyCreationHandler;

    private void Start()
    {
        canvas = GameObject.Find("Canvas").transform;

        storyCreationHandler = GameObject.Find("StoryCreationHandler").GetComponent<StoryCreationHandler>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(storyCreationHandler.inStoryMode)
        {
            if (!inSentenceSlot)
            {
                GameObject clonedSaveSentence = Instantiate(this.gameObject);

                clonedSaveSentence.transform.SetParent(transform.parent, false);
            }

            transform.SetParent(canvas);

            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject objectUnderneath = eventData.pointerCurrentRaycast.gameObject;

        // Check if the place we want to drag it to is a valid slot
        if (objectUnderneath != null)
        {
            if (objectUnderneath.name == "SentenceSlot")
            {
                // If it is, set it as a child
                transform.SetParent(objectUnderneath.transform);

                GetComponent<CanvasGroup>().blocksRaycasts = true;

                // Mark it is as nolong in the sentence slot
                inSentenceSlot = true;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
}
