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
    private GameObject sentenceBar;

    public void start() {
        sentenceBar = GameObject.Find("Sentence");
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
            if (d.draggedFrom == Behavior.Sentence && behavior == Behavior.Sentence)
            {
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

            if (d.draggedFrom == Behavior.Sentence && behavior == Behavior.Sentence)
            {
                GetComponent<SentenceBar>().ResizeSentence(-1);
            }
        }
    }

    //
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedtile = null;
        //
        DraggableTile d = eventData.pointerDrag.GetComponent<DraggableTile>();
        droppedtile = eventData.pointerDrag;

        //
        if(d != null)
        {
            //
            switch (behavior)
            {
                //
                case Behavior.Default:
                    //Debug.Log("You have place the tile in the default space.");

                    // pretty sure the eventData no longer knows what object it was dragging
                    //Destroy(eventData.pointerDrag);
                    Destroy(droppedtile);
                    Destroy(d.placeholder);

                    break;

                //
                case Behavior.WordBank:
                    //Debug.Log("You have placed the tile in the wordbank.");
                    Destroy(droppedtile);
                    // fixes the New Game Objects that were being leftover when we dragged a tile from the wordbank to itself
                    Destroy(d.placeholder);

                    break;

                case Behavior.Trash:
                    //Debug.Log("You have placed the tile in the trash.");
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
                //Debug.Log("You have placed a tile in the sentence zone.");              

                    //
                    d.parentToReturnTo = this.transform;

                    break;

                //
                case Behavior.WordHolder:
                //Debug.Log("You have placed a tile in the wordholder zone.");

                    //
                    d.parentToReturnTo = this.transform;

                    if(this.transform.childCount > 0)
                    {
                        //
                        Destroy(this.transform.GetChild(0).gameObject);
                    }
                    
                    GetComponent<WordHolder>().OpenWordHolder(eventData.pointerDrag.GetComponent<WordTile>().word);

                    //
                    Destroy(d.placeholder);

                    //
                    break;
            }
        }
    }
}
