using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileDropzone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
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
        DraggableTile d = eventData.pointerDrag.GetComponent<DraggableTile>();

        //
        if(d != null)
        {
            //
            d.placeholderParent = this.transform;

            //
            d.heldOver = behavior;

            //
            if(d.draggedFrom != Behavior.Sentence && behavior == Behavior.Sentence)
            {
                //
                GetComponent<SentenceBar>().ResizeSentence(1);
            }
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
        DraggableTile d = eventData.pointerDrag.GetComponent<DraggableTile>();

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

            //
            if (d.draggedFrom != Behavior.Sentence && behavior == Behavior.Sentence)
            {
                //
                GetComponent<SentenceBar>().ResizeSentence(-1);
            }
        }
    }

    //
    public void OnDrop(PointerEventData eventData)
    {
        //
        DraggableTile d = eventData.pointerDrag.GetComponent<DraggableTile>();

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

                    //
                    Destroy(eventData.pointerDrag);

                    break;

                case Behavior.Trash:

                    //
                    if(behavior == Behavior.Sentence)
                    {
                        //
                        GetComponent<SentenceBar>().ResizeSentence(-1);
                    }

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
                    if(this.transform.childCount > 0)
                    {
                        //
                        Destroy(this.transform.GetChild(0).gameObject);
                    }

                    //
                    GetComponent<WordHolder>().OpenWordHolder(eventData.pointerDrag.GetComponent<WordTile>().word);

                    //
                    Destroy(d.placeholder);

                    //
                    break;
            }
        }
    }
}
