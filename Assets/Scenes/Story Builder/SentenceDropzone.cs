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
        Trash
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
        //
        DraggableSentence d = eventData.pointerDrag.GetComponent<DraggableSentence>();

        //
        if(d != null)
        {
            //
            switch (behavior)
            {
                //
                case Behavior.Default:
                case Behavior.Trash:
                case Behavior.SentenceBank:

                    //
                    Destroy(eventData.pointerDrag);

                    break;

                //
                case Behavior.Page:

                    //
                    d.parentToReturnTo = this.transform;

                    break;
            }
        }
    }
}
