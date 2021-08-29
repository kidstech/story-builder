using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;

public class PageContainer : MonoBehaviour
{
    //
    public enum PAGE
    {
        PICTURE_TOP,
        PICTURE_BOTTOM,
        NO_PICTURE
    }

    //
    private float spacingWidth;
    private float pageWidth;

    //
    [Header("Prefabs")]
    public GameObject pagePrefabPictureTop;
    public GameObject pagePrefabPictureBottom;
    public GameObject pagePrefabNoPicture;

    [Header("Scene References")]
    public GameObject pageIconContainer;

    //
    [Header("Settings")]
    public int maxPageCount = 30;

    // Keep track of
    public int selectedPageGlobal = -1;
    private int currentPageCount = 0;

    //
    private void Start()
    {
        //
        GridLayoutGroup layout = GetComponent<GridLayoutGroup>();

        //
        spacingWidth = layout.spacing.x;
        pageWidth = layout.cellSize.x;
    }

    //
    public void AddPage(PAGE pageType)
    {
        GameObject newPage;

        switch (pageType)
        {
            case PAGE.PICTURE_TOP:
                newPage = Instantiate(pagePrefabPictureTop);
                break;

            case PAGE.PICTURE_BOTTOM:
                newPage = Instantiate(pagePrefabPictureBottom);
                break;

            case PAGE.NO_PICTURE:
                newPage = Instantiate(pagePrefabNoPicture);
                break;

            default:
                newPage = Instantiate(pagePrefabNoPicture);
                break;
        }

        //
        int pageNumber = selectedPageGlobal + 1;

        //
        currentPageCount++;

        // You might be able to make a VoiceSelectionHub prefab and then be able to associate that prefab with all newPage prefabs, and avoid using GameObject.Find here
        newPage.GetComponent<TextToSpeechHandler>().audio = GameObject.Find("StoryBuilderVoiceSelectionHub").GetComponent<AudioSource>();
        newPage.GetComponent<Page>().pageNumber = pageNumber;
        newPage.transform.SetParent(this.transform);
        newPage.transform.SetSiblingIndex(pageNumber);

        //
        AdjustOtherPages();

        //
        UpdateSelectedPage(pageNumber);
    }

    //
    public void RemovePage(int selectedPage)
    {
        //
        currentPageCount--;
        //
        Destroy(transform.GetChild(selectedPage).gameObject);

        //
        StartCoroutine(RemovePageCoroutine(selectedPage));
    }

    // We need to wait until the end of the frame after the Destroy resolve (it stays on screen until end of frame)
    private IEnumerator RemovePageCoroutine(int selectedPage)
    {
        yield return new WaitForEndOfFrame();

        AdjustOtherPages();

        // decrement to account for deleted index, unless we've deleted the first object
        if(selectedPageGlobal != 0) selectedPageGlobal--;

        UpdateSelectedPage(selectedPageGlobal);

    }

    //
    public void AdjustOtherPages()
    {
        /*
         *  Probably worth it to only update pages PAST the selected pages
         */

        if (selectedPageGlobal == -1) return;

        //
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Page>().pageNumber = i;
        }
    }

    //
    public void UpdateSelectedPage(int pageNumber)
    {
        if (pageNumber == currentPageCount)
        {
            // If it's the last thing in the list, do nothing
            if (transform.childCount != 0)
            {
                // Otherwise set the only thing in the list as focus
                selectedPageGlobal = 0;
                transform.GetChild(pageNumber - 1).gameObject.SetActive(false);
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        else if (pageNumber == currentPageCount + 1) // different conditional to distinguish between clicking left arrow to cycle to last page vs right arrow
        {
            if(transform.childCount != 0)
            {
                transform.GetChild(0).gameObject.SetActive(false); // deactivate first page
                selectedPageGlobal = currentPageCount -1; // get index of last page
                transform.GetChild(selectedPageGlobal).gameObject.SetActive(true); // activate last page
            }
        }
        else
        {
            //
            if (selectedPageGlobal != -1)
            {
                transform.GetChild(selectedPageGlobal).gameObject.SetActive(false);
            }

            selectedPageGlobal = pageNumber;

            if (selectedPageGlobal != -1)
            {
                //
                transform.GetChild(pageNumber).gameObject.SetActive(true);
            }
        }
    }

    public IEnumerator SpeakPage()
    {
        if (transform.childCount == 0) yield return null;

        //
        //string fullPage = "";
        Transform iteratedPagePrefab = null;
        string textToRead = null;
        float speechDuration = 0;

        //
        PAGE type = transform.GetChild(selectedPageGlobal).GetComponent<Page>().type;

        //
        if (type == PAGE.NO_PICTURE)
        {
            //
            for (int o = 0; o < transform.GetChild(selectedPageGlobal).childCount; o++)
            {
                // example:          PageContainer => PagePrefab => SentencePrefab
                iteratedPagePrefab = transform.GetChild(selectedPageGlobal).GetChild(o);
                textToRead = iteratedPagePrefab.GetComponentInChildren<SentenceTile>().textToDisplay.ToLower();
                speechDuration = Speaker.ApproximateSpeechLength(textToRead) * (1/TextToSpeechHandler.voiceRate);
                iteratedPagePrefab.GetComponent<SentenceTile>().ReadSentence();
                yield return new WaitForSeconds(speechDuration);
            }
        }
        else
        {
            //
            for (int o = 0; o < transform.GetChild(selectedPageGlobal).GetChild(0).childCount; o++)
            {
                // example:          PageContainer => PagePrefab => SentenceDropzone => SentencePrefab
                iteratedPagePrefab = transform.GetChild(selectedPageGlobal).GetChild(0).GetChild(o);
                textToRead = iteratedPagePrefab.GetComponentInChildren<SentenceTile>().textToDisplay.ToLower();
                speechDuration = Speaker.ApproximateSpeechLength(textToRead) * (1/TextToSpeechHandler.voiceRate);
                iteratedPagePrefab.GetComponent<SentenceTile>().ReadSentence();
                yield return new WaitForSeconds(speechDuration);
            }
        }

        //
        //fullPage = fullPage.Remove(fullPage.Length - 1, 1);

        //Speaker.Speak(fullPage);

        //
        //Debug.Log(fullPage);
    }

    //
    public List<SavedSentence> GetAllSentencesInPages()
    {
        List<SavedSentence> list = new List<SavedSentence>();

        // For every page
        for (int i = 0; i < transform.childCount; i++)
        {
            Page currentPage = transform.GetChild(i).GetComponent<Page>();

            if (currentPage.type == PAGE.NO_PICTURE)
            {
                // For every sentence in the page
                for (int o = 0; o < currentPage.transform.childCount; o++)
                {
                    // Add in the sentence
                    list.Add(currentPage.transform.GetChild(o).GetComponent<SentenceObject>().savedSentence);
                }
            }
            else
            {
                // For every sentence in the page
                for (int o = 0; o < currentPage.transform.GetChild(0).childCount; o++)
                {
                    // Add in the sentence
                    list.Add(currentPage.transform.GetChild(0).GetChild(o).GetComponent<SentenceObject>().savedSentence);
                }
            }
        }

        //
        return list;
    }
}
