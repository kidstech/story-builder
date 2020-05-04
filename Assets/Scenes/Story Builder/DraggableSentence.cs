using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableSentence : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public Transform parentToReturnTo = null;

    [HideInInspector]
    public Transform placeholderParent = null;

    [HideInInspector]
    public GameObject placeholder = null;

    //
    public SentenceDropzone.Behavior heldOver;
    public SentenceDropzone.Behavior draggedFrom = SentenceDropzone.Behavior.SentenceBank;

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
        if(draggedFrom == SentenceDropzone.Behavior.SentenceBank)
        {
            //
            GameObject o = Instantiate(this.gameObject);

            //
            o.GetComponent<SentenceObject>().savedSentence = this.gameObject.GetComponent<SentenceObject>().savedSentence;

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
        if (heldOver == SentenceDropzone.Behavior.Page)
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
                if (this.transform.position.y > placeholderParent.GetChild(i).position.y)
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
    }

    //
    public void OnEndDrag(PointerEventData eventData)
    {
        //
        this.transform.SetParent(parentToReturnTo, false);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

        //
        draggedFrom = parentToReturnTo.GetComponent<SentenceDropzone>().behavior;

        //
        canvasGroup.blocksRaycasts = true;

        //
        Destroy(placeholder);
    }
}
