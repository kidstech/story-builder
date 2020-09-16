using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableTile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public Transform parentToReturnTo = null;

    [HideInInspector]
    public Transform placeholderParent = null;

    [HideInInspector]
    public GameObject placeholder = null;

    //
    public TileDropzone.Behavior heldOver;
    public TileDropzone.Behavior draggedFrom = TileDropzone.Behavior.WordBank;

    //
    private Transform canvas = null;

    //
    private Vector2 offset = Vector2.zero;

    //
    private CanvasGroup canvasGroup = null;

    //
    private void Start()
    {
        //
        canvas = GameObject.Find("Canvas").transform;

        //
        canvasGroup = GetComponent<CanvasGroup>();
    }

    //
    public void OnBeginDrag(PointerEventData eventData)
    { 
        //
        offset = this.transform.position - Input.mousePosition;

        //
        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);

        //
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.transform.parent.GetComponent<GridLayoutGroup>().cellSize.x;
        le.preferredHeight = this.transform.parent.GetComponent<GridLayoutGroup>().cellSize.y;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        //
        if(draggedFrom == TileDropzone.Behavior.WordBank)
        {
            //
            GameObject o = Instantiate(this.gameObject);

            //
            o.GetComponent<WordTile>().word = this.gameObject.GetComponent<WordTile>().word;

            //
            o.transform.SetParent(this.transform.parent, false);
            o.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

            //
            placeholder.transform.SetAsLastSibling();
        }
        else
        {
            //
            placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        }

        //
        parentToReturnTo = this.transform.parent;

        //
        placeholderParent = parentToReturnTo;

        // Set this parent as the canvas
        this.transform.SetParent(canvas);

        //
        canvasGroup.blocksRaycasts = false;
    }

    //
    public void OnDrag(PointerEventData eventData)
    {
        //
        this.transform.position = eventData.position + offset;

        //
        if(heldOver == TileDropzone.Behavior.Sentence)
        {
            //
            if (placeholder.transform.parent != placeholderParent)
            {
                //
                placeholder.transform.SetParent(placeholderParent, false);
            }

            //
            int newSiblingIndex = placeholderParent.childCount;

            //
            for (int i = 0; i < placeholderParent.childCount; i++)
            {
                //
                if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
                {
                    //
                    newSiblingIndex = i;

                    //
                    if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                    {
                        //
                        newSiblingIndex--;
                    }

                    //
                    break;
                }
            }

            //
            placeholder.transform.SetSiblingIndex(newSiblingIndex);
        }
        else
        {
            // Remove the placeholder from the wordbank
            if (placeholder.transform.parent != placeholderParent)
            {
                //
                placeholder.transform.SetParent(placeholderParent, false);
            }
        }
    }

    //
    public void OnEndDrag(PointerEventData eventData)
    {
        //
        this.transform.SetParent(parentToReturnTo, false);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

        //
        draggedFrom = parentToReturnTo.GetComponent<TileDropzone>().behavior;

        //
        canvasGroup.blocksRaycasts = true;

        //
        Destroy(placeholder);
    }
}
