using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private int selectedPage = -1;
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

        switch(pageType)
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
        int pageNumber = selectedPage + 1;

        //
        currentPageCount++;

        //
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

        UpdateSelectedPage(selectedPage);
    }

    //
    public void AdjustOtherPages()
    {
        /*
         *  Probably worth it to only update pages PAST the selected pages
         */

        if (selectedPage == -1) return;

        //
        for (int i = selectedPage; i < transform.childCount; i++)
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
                selectedPage = 0;
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        else
        {
            //
            if (selectedPage != -1)
            {
                transform.GetChild(selectedPage).gameObject.SetActive(false);
            }

            selectedPage = pageNumber;

            if (selectedPage != -1)
            {
                //
                transform.GetChild(pageNumber).gameObject.SetActive(true);
            }
        }
    }

    //
    public void SpeakPage()
    {
        if (transform.childCount == 0) return;

        //
        string fullPage = "";

        // For every page
        for(int i = 0; i < transform.childCount; i++)
        {
            //
            PAGE type = transform.GetChild(i).GetComponent<Page>().type;

            //
            if(type == PAGE.NO_PICTURE)
            {
                //
                for (int o = 0; o < transform.GetChild(0).childCount; o++)
                {
                    fullPage += transform.GetChild(i).GetChild(o).GetComponentInChildren<Text>().text + ". ";
                }
            }
            else
            {
                //
                for (int o = 0; o < transform.GetChild(0).GetChild(0).childCount; o++)
                {
                    fullPage += transform.GetChild(i).GetChild(0).GetChild(o).GetComponentInChildren<Text>().text + ". ";
                }
            }
        }

        //
        fullPage = fullPage.Remove(fullPage.Length - 1, 1);

        //
        Debug.Log(fullPage);
    }
}
