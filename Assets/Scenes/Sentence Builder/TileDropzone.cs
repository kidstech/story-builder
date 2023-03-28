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

    public SentenceBar sentenceToPlayNoise;
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
                    Destroy(droppedtile);
                    Destroy(d.placeholder);

                    break;

                //
                case Behavior.WordBank:
                    Destroy(droppedtile);
                    // fixes the New Game Objects that were being leftover when we dragged a tile from the wordbank to itself
                    Destroy(d.placeholder);

                    break;

                // All references to the Trash in other scripts have been removed, but for some reason, if you remove this case, it completely breaks the sentence bar
                // I have no idea why...
                case Behavior.Trash:
                
                    Destroy(eventData.pointerDrag);

                    break;

                // Only allow a max of 9 tiles in the sentence bar at a time
                case Behavior.Sentence:
                
                     if(GetComponent<SentenceBar>().GatherWordTiles().Count == 9) {
                        sentenceToPlayNoise.errorNoise.Play();
                     }
        
                    else if(GetComponent<SentenceBar>().GatherWordTiles().Count != 9) {
                        d.parentToReturnTo = this.transform;
                    }

                    break;

                //
                case Behavior.WordHolder:
                    //
                    d.parentToReturnTo = this.transform;

                    if(this.transform.childCount > 0)
                    {
                        //
                        Destroy(this.transform.GetChild(0).gameObject);
                    }
                    
                    //
                    Destroy(d.placeholder);

                    //
                    break;
            }
        }
    }
}
