using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Page : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //
    private float currentSize;
    private float maxSize;

    //
    private void Awake()
    {
        //
        currentSize = 0;

        //
        maxSize = transform.Find("SentenceSlotScrollviewPrefab").Find("SentenceSlot").GetComponent<RectTransform>().sizeDelta.y;
    }

    //
    public bool CheckFit(float size)
    {
        //
        if(currentSize + size <= maxSize)
        {
            //
            return true;
        }
        else
        {
            //
            return false;
        }
    }

    //
    public void UpdateFit(float size)
    {
        //
        currentSize -= size;
    }

    //
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<SavedSentenceObject>() != null)
        {
            eventData.pointerDrag.GetComponent<SavedSentenceObject>().heldOverStory = true;

            eventData.pointerDrag.GetComponent<SavedSentenceObject>().UpdatePage(this.gameObject);
        }
    }

    //
    public void OnPointerExit(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<SavedSentenceObject>() != null)
        {
            eventData.pointerDrag.GetComponent<SavedSentenceObject>().heldOverStory = false;

            eventData.pointerDrag.GetComponent<SavedSentenceObject>().UpdatePage(null);
        }
    }   
}