using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageIconContainer : MonoBehaviour
{
    //
    [Header("Prefabs")]
    public GameObject pageIconPrefab;

    [Header("Scene References")]
    public PageContainer pageContainer;

    [Header("Settings")]
    public int maxPageCount = 30;

    // Keeps track of what page we are 'focused' on
    public int selectedPage = -1;

    //
    private RectTransform rt;
    private Color yellow;
    private int currentPageCount = 0;

    //
    private void Start()
    {
        //
        rt = GetComponent<RectTransform>();
        yellow = new Color(255, 218, 0);
    }

    //
    public void AddPageIcon(PageContainer.PAGE pageType)
    {
        //
        if(currentPageCount >= maxPageCount)
        {
            // this should probably notify the user that they have hit the max number of pages somehow
            return;
        }

        //
        int pageNumber = selectedPage + 1;

        //
        currentPageCount++;

        //
        pageContainer.AddPage(pageType);

        //
        GameObject newPageIcon = Instantiate(pageIconPrefab);

        //
        newPageIcon.GetComponent<PageIcon>().SetupPageIcon(pageNumber);
        newPageIcon.transform.SetParent(this.transform);
        newPageIcon.transform.SetSiblingIndex(pageNumber);

        //
        rt.sizeDelta = rt.sizeDelta + new Vector2(64, 0);

        //
        AdjustOtherPages();

        //
        UpdateSelectedPage(pageNumber);
    }

    //
    public void AdjustOtherPages()
    {
        if (selectedPage == -1) return;

        //
        for (int i = selectedPage; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<PageIcon>().SetupPageIcon(i);
        }
    }

    // shifts the 3 pages in focus left by one
    public void movePageViewLeft(int selectedPage) {
        int leftOfCurrentPage = selectedPage - 1;
        // change focused page to the page before it
        UpdateSelectedPage(leftOfCurrentPage);
        // update the active pages
        EnableIcons();
    }
    // shifts the 3 pages in focus right by one
    public void movePageViewRight(int selectedPage) {
        int rightOfCurrentPage = selectedPage + 1;
        // change focused page to the page after it
        UpdateSelectedPage(rightOfCurrentPage);
        // update active pages
        EnableIcons();
    }

    //
    public void RemovePage()
    {
        //
        if (currentPageCount <= 0) return;

        //
        currentPageCount--;

        //
        Destroy(transform.GetChild(selectedPage).gameObject);

        //
        pageContainer.RemovePage(selectedPage);

        //
        StartCoroutine(RemovePageCoroutine());
    }

    // We need to wait until the end of the frame after the Destroy resolve (it stays on screen until end of frame)
    private IEnumerator RemovePageCoroutine()
    {
        yield return new WaitForEndOfFrame();

        AdjustOtherPages();

        UpdateSelectedPage(selectedPage);
    }

    //
    public void UpdateSelectedPage(int pageNumber)
    {
        if (pageNumber == currentPageCount)
        {
            // If it's the last thing in the list, do nothing
            if(transform.childCount != 0)
            {
                // reset coloration of last page
                transform.GetChild(selectedPage).GetComponent<Image>().color = Color.white;
                // Otherwise set the only thing in the list as focus
                selectedPage = 0;
                transform.GetChild(0).GetComponent<Image>().color = yellow;
            }
        } else if (pageNumber == -1) {
            transform.GetChild(selectedPage).GetComponent<Image>().color = Color.white;
            selectedPage = currentPageCount - 1; // last page
            transform.GetChild(selectedPage).GetComponent<Image>().color = yellow;

        } else {
            
            // Revert old icon color
            if (selectedPage != -1)
            {
                transform.GetChild(selectedPage).GetComponent<Image>().color = Color.white;
            }

            // change selected page to the newly added page
            selectedPage = pageNumber;

            // Set new icon
            if (selectedPage != -1)
            {
                transform.GetChild(selectedPage).GetComponent<Image>().color = yellow;
            }

            //
            //pageContainer.UpdateSelectedPage(selectedPage);
        }

        EnableIcons();
    }

    private void EnableIcons()
    {
        if (currentPageCount == 0) return;

        // if we only have three pages or our focus is on the first page we want the first three pages displayed
        if(currentPageCount < 4 || selectedPage == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        // otherwise if we are focused on the last page and we have at least three pages, display the last three of them
        else if (currentPageCount >= 3 && (selectedPage == currentPageCount - 1)){
            for (int i = selectedPage - 2; i < currentPageCount; i++){
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                // keep only three active pages; one to the left of the focused page, the focused page,  and one to the right
                if(i >= selectedPage - 1 && i < selectedPage + 2)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    // all other page gameobjects will be inactive
                    transform.GetChild(i).gameObject.SetActive(false);
                } 
            }
        }
    }
}
