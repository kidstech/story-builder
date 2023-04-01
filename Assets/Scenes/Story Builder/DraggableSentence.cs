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
    public SentenceDropzone.Behavior heldOver;
    public SentenceDropzone.Behavior draggedFrom = SentenceDropzone.Behavior.SentenceBank;
    private Transform storyBuilderCanvas;

    private Vector2 offset = Vector2.zero;

    private CanvasGroup canvasGroup = null;


    private void Start()
    {
        storyBuilderCanvas = GameObject.Find("StoryBuilderCanvas").transform;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    { 
        offset = this.transform.position - Input.mousePosition;

        placeholder = new GameObject("Placeholder");
        placeholder.transform.SetParent(this.transform.parent);

        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.transform.parent.GetComponent<GridLayoutGroup>().cellSize.x;
        le.preferredHeight = this.transform.parent.GetComponent<GridLayoutGroup>().cellSize.y;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        parentToReturnTo = this.transform.parent;

        placeholderParent = parentToReturnTo;

        // Set this parent as the canvas
        this.transform.SetParent(storyBuilderCanvas);

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
         //
    
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        this.transform.position = Camera.main.ScreenToWorldPoint(mousePos);

        if(heldOver == SentenceDropzone.Behavior.SentenceBank)
        {
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
                    if (placeholder.transform.GetSiblingIndex() > newSiblingIndex)
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

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToReturnTo, false);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        RectTransform sentenceTileCoords = this.transform.GetComponent<RectTransform>();
        // give the sentence tile a draggable z position
        this.transform.GetComponent<RectTransform>().position = new Vector3(sentenceTileCoords.position.x, sentenceTileCoords.position.y, 0);
        canvasGroup.blocksRaycasts = true;

        Destroy(placeholder);
    }
}
