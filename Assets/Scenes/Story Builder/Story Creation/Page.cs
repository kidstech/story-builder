using Crosstales.RTVoice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Page : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //
    private float currentSize;
    private float maxSize;

    public int currentSentence;
    public int currentSentenceMax;

    public string[] storyFragments;

    //
    private void Awake()
    {
        //
        currentSize = 0;

        //
        currentSentence = 0;
        currentSentenceMax = 0;

        //
        maxSize = transform.Find("SentenceSlotScrollviewPrefab").Find("SentenceSlot").GetComponent<RectTransform>().sizeDelta.y;
    }

    //
    public bool CheckFit(float size)
    {
        //
        if(currentSize + size <= maxSize)
        {
            //
            return true;
        }
        else
        {
            //
            return false;
        }
    }

    //
    public void UpdateFit(float size)
    {
        //
        currentSize -= size;
    }

    //
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<SavedSentenceObject>() != null)
        {
            eventData.pointerDrag.GetComponent<SavedSentenceObject>().heldOverStory = true;

            eventData.pointerDrag.GetComponent<SavedSentenceObject>().UpdatePage(this.gameObject);
        }
    }

    //
    public void OnPointerExit(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<SavedSentenceObject>() != null)
        {
            eventData.pointerDrag.GetComponent<SavedSentenceObject>().heldOverStory = false;

            eventData.pointerDrag.GetComponent<SavedSentenceObject>().UpdatePage(null);
        }
    }   

    public void ReadStory()
    {
        //
        TextToSpeechHandler speak = GameObject.Find("TextToSpeechHandler").GetComponent<TextToSpeechHandler>();

        //
        Transform pageContext = transform.Find("SentenceSlotScrollviewPrefab").Find("SentenceSlot");

        //
        currentSentence = 0;
        currentSentenceMax = pageContext.childCount;

        //
        string story = "";

        if (pageContext.childCount > 0)
        {
            //
            storyFragments = new string[pageContext.childCount];

            //
            for (int i = 0; i < pageContext.childCount; i++)
            {
                //
                storyFragments[i] = pageContext.GetChild(i).Find("Text").GetComponent<Text>().text;

                //
                story += pageContext.GetChild(i).Find("Text").GetComponent<Text>().text + ".";
            }

            //
            //speak.startSpeaking(storyFragments[0], TextToSpeechHandler.SoundType.STORY, this.gameObject);

            //
            currentSentence++;
        }
        else
        {
            Debug.Log("Nothing to speak");
        }
    }

    //
    public string ContinueStory()
    {
        string result = "";

        //
        if(currentSentence < currentSentenceMax)
        {
            //
            result = storyFragments[currentSentence];

            //
            currentSentence++;
        }

        //
        return result;
    }
}