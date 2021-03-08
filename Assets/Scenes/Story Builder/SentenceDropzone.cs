using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SentenceDropzone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    //
    public enum Behavior
    {
        Default,
        SentenceBank,
        Page,
        Trash,
    }

    //
    public Behavior behavior = Behavior.Default;

    //
    public void OnPointerEnter(PointerEventData eventData)
    {
        //
        if(eventData.pointerDrag == null)
        {
            //
            return;
        }

        //
        DraggableSentence d = eventData.pointerDrag.GetComponent<DraggableSentence>();

        //
        if(d != null)
        {
            //
            d.placeholderParent = this.transform;

            //
            d.heldOver = behavior;
        }
    }

    //
    public void OnPointerExit(PointerEventData eventData)
    {
        //
        if (eventData.pointerDrag == null)
        {
            //
            return;
        }

        //
        DraggableSentence d = eventData.pointerDrag.GetComponent<DraggableSentence>();

        //
        if(d != null)
        {
            //
            if (d.placeholderParent == this.transform)
            {
                //
                d.placeholderParent = d.parentToReturnTo;
            }



            //
            d.heldOver = Behavior.Default;
        }
    }

    //
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedSentence = null;
        //
        DraggableSentence d = eventData.pointerDrag.GetComponent<DraggableSentence>();
        droppedSentence = eventData.pointerDrag;

        if(d != null)
        {
            switch (behavior) {
                case Behavior.SentenceBank:
                    Destroy(droppedSentence);
                    Destroy(d.placeholder);
                    break;

                case Behavior.Trash:
                    Destroy(eventData.pointerDrag);
                    break;

                case Behavior.Page:
                    d.parentToReturnTo = this.transform;
                    break;

                default:
                    Destroy(droppedSentence);
                    Destroy(d.placeholder);
                    Destroy(eventData.pointerDrag);
                    break;
            }
        }
    }
}
