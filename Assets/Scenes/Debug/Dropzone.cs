using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dropzone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    //
    public enum Behavior
    {
        Default,
        Trash,
        Sentence,
        WordHolder,
        WordBank
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
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

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
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

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
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

        //
        if(d != null)
        {
            //
            switch (behavior)
            {
                //
                case Behavior.Default:

                    //
                    Destroy(eventData.pointerDrag);

                    break;

                //
                case Behavior.WordBank:
                case Behavior.Trash:

                    //
                    Destroy(eventData.pointerDrag);

                    break;

                //
                case Behavior.Sentence:

                    //
                    d.parentToReturnTo = this.transform;

                    break;

                //
                case Behavior.WordHolder:

                    //
                    d.parentToReturnTo = this.transform;

                    //
                    Destroy(d.placeholder);

                    //
                    break;
            }

            //
            d.draggedFrom = behavior;
        }
    }
}
