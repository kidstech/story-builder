using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Crosstales.RTVoice;
using System;

public class SavedSentenceObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    //
    Transform canvas;

    //
    StoryCreationHandler storyCreationHandler;

    //
    public Page AnyPage;

    //
    GameObject SentenceSlot;

    //
    GameObject placeholder = null;

    //
    private bool draggedFromSentenceList = true;
    private bool draggedFromStory = false;
    public bool heldOverStory = false;

    //
    public readonly int characterThreshhold = 40;

    //
    private void Awake()
    {
        //
        canvas = GameObject.Find("Canvas").transform;

        //
        storyCreationHandler = GameObject.Find("StoryCreationHandler").GetComponent<StoryCreationHandler>();

        //
        SentenceSlot = GameObject.Find("SentenceSlot");
    }

    //
    private void activatePlaceholder(int index)
    {
        if(AnyPage.CheckFit(GetComponent<RectTransform>().sizeDelta.y))
        {
            //
            placeholder.transform.SetParent(SentenceSlot.transform);

            //
            placeholder.transform.SetSiblingIndex(index);

            //
            placeholder.SetActive(true);
        }
        else
        {
            //
            return;
        }
    }

    //
    public void UpdatePage(GameObject newPage)
    {
        if(newPage != null)
        {
            //
            AnyPage = newPage.GetComponent<Page>();

            //
            if (placeholder != null)
            {
                //
                placeholder.transform.SetParent(newPage.transform);
            }

            //
            SentenceSlot = AnyPage.gameObject.transform.Find("SentenceSlotScrollviewPrefab").Find("SentenceSlot").gameObject;
        }
        else
        {
            AnyPage = null;
        }
    }

    //
    private void deactivatePlaceholder()
    {
        //
        placeholder.transform.SetParent(null);

        //
        placeholder.SetActive(false);
    }

    private void setInSentence()
    {
        //
        if (AnyPage.CheckFit(GetComponent<RectTransform>().sizeDelta.y))
        {
            //
            AnyPage.UpdateFit(-1 * GetComponent<RectTransform>().sizeDelta.y);

            //
            transform.SetParent(SentenceSlot.transform);

            //
            transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        }
        else
        {
            //
            Destroy(this.gameObject);
        }
    }

    //
    private GameObject BuildPlaceholder()
    {

        //
        GameObject go = new GameObject();

        //
        Rect rect = go.AddComponent<RectTransform>().rect;
        Rect draggedSentence = ((RectTransform)this.transform).rect;
        rect.height = draggedSentence.height;
        rect.width = draggedSentence.width;

        //
        go.SetActive(false);

        //
        return go;
    }

    //
    public void OnBeginDrag(PointerEventData eventData)
    {
        //
        if(storyCreationHandler.inStoryMode)
        {
            //
            if (draggedFromSentenceList)
            {
                //
                GameObject clonedSaveSentence = Instantiate(this.gameObject);

                //
                clonedSaveSentence.transform.SetParent(transform.parent, false);

                //
                int sizeMultiplier;

                //
                if (transform.Find("Text").GetComponent<Text>().text.Length < characterThreshhold)
                {
                    sizeMultiplier = 1;
                }
                else
                {
                    sizeMultiplier = 2 + transform.Find("Text").GetComponent<Text>().text.Length / characterThreshhold;
                }

                //
                Vector2 outSize = new Vector2(400, 50 * sizeMultiplier);
                Vector2 inSize = new Vector2(outSize.x - 10, outSize.y - 10);

                // Change the sizes of the objects
                gameObject.name = "Sentence";
                GetComponent<RectTransform>().sizeDelta = outSize;
                transform.Find("Background").GetComponent<RectTransform>().sizeDelta = outSize;
                transform.Find("Edge").GetComponent<RectTransform>().sizeDelta = inSize;
                transform.Find("Text").GetComponent<RectTransform>().sizeDelta = inSize;

                //
                placeholder = BuildPlaceholder();
            }

            //
            if(draggedFromStory)
            {
                //
                AnyPage.UpdateFit(GetComponent<RectTransform>().sizeDelta.y);

                //
                activatePlaceholder(transform.GetSiblingIndex());
            }

            //
            transform.SetParent(canvas);

            //
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else
        {
            //
            eventData.pointerDrag = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //
        this.transform.position = eventData.position;

        //
        if(heldOverStory && draggedFromSentenceList)
        {
            //
            activatePlaceholder(SentenceSlot.transform.childCount);
        }
        else if(!heldOverStory && draggedFromSentenceList)
        {
            //
            deactivatePlaceholder();
        }

        //
        if(heldOverStory && SentenceSlot.transform.childCount >= 1)
        {
            //
            int newSiblingIndex = SentenceSlot.transform.childCount;

            //
            for (int i = 0; i < SentenceSlot.transform.childCount; i++)
            {
                //
                if(transform.position.y > SentenceSlot.transform.GetChild(i).position.y)
                {
                    //
                    newSiblingIndex = i;

                    //
                    if(placeholder.transform.GetSiblingIndex() < newSiblingIndex)
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
        //
        if ((draggedFromSentenceList || draggedFromStory) && !heldOverStory)
        {
            //
            Destroy(this.gameObject);
        }
        //
        else if(draggedFromSentenceList && heldOverStory)
        {
            //
            setInSentence();

            //
            draggedFromSentenceList = false;
            draggedFromStory = true;
        }
        else if(draggedFromStory && heldOverStory)
        {
            //
            setInSentence();

            //
            draggedFromSentenceList = false;
            draggedFromStory = true;
        }
        else if(draggedFromSentenceList)
        {
            //
            //setInSentence();

            Debug.Log("Shouldn't get here!");
        }

        //
        deactivatePlaceholder();

        //
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
}
