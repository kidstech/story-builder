using System.Collections;
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

        if(d != null)
        {
            switch (behavior) {
                case Behavior.SentenceBank:
                    //Debug.Log("sentencebank");
                    Destroy(droppedSentence);
                    Destroy(d.placeholder);
                    break;

                case Behavior.Page:
                    //Debug.Log("page");
                    d.parentToReturnTo = this.transform;
                    break;

                default:
                    //Debug.Log("default");
                    Destroy(droppedSentence);
                    Destroy(d.placeholder);
                    Destroy(eventData.pointerDrag);
                    break;
            }
        }
    }
}
