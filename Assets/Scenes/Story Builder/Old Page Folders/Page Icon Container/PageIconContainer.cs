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
        if (leftOfCurrentPage == -1)
        {
            pageContainer.UpdateSelectedPage(currentPageCount + 1);
        }
        else pageContainer.UpdateSelectedPage(leftOfCurrentPage);
    }
    // shifts the 3 pages in focus right by one
    public void movePageViewRight(int selectedPage) {
        int rightOfCurrentPage = selectedPage + 1;
        // change focused page to the page after it
        UpdateSelectedPage(rightOfCurrentPage);
        pageContainer.UpdateSelectedPage(rightOfCurrentPage);
    }

    //
    public void RemovePage()
    {
        //
        if (currentPageCount <= 0) return;
        
        //
        Destroy(transform.GetChild(selectedPage).gameObject);
        
        //
        pageContainer.RemovePage(selectedPage);

        //
        StartCoroutine(RemovePageCoroutine());

        //
        currentPageCount--;
    }

    // We need to wait until the end of the frame after the Destroy resolve (it stays on screen until end of frame)
    private IEnumerator RemovePageCoroutine()
    {
        yield return new WaitForEndOfFrame();

        AdjustOtherPages();

        if (selectedPage != 0) selectedPage--; // decrement index to account for page deletion, if we haven't deleted the first page

        UpdateSelectedPage(selectedPage);
    }

    //
    public void UpdateSelectedPage(int pageNumber)
    {
        // if last page in list...
        if (pageNumber == currentPageCount)
        {
            // last page in populated list...
            if(transform.childCount > 1)
            {
                // reset coloration of last page
                transform.GetChild(selectedPage).GetComponent<Image>().color = Color.white;
                // change selected view to first page (roll over from end to beginning)
                selectedPage = 0;
                transform.GetChild(0).GetComponent<Image>().color = yellow;
            }
            // if we have gone left of start, roll over to end
        } else if (pageNumber == -1) {
            transform.GetChild(selectedPage).GetComponent<Image>().color = Color.white;
            selectedPage = currentPageCount + 1; // set selected page to one after last in list
            transform.GetChild(currentPageCount - 1).GetComponent<Image>().color = yellow;

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

        // if first page...
        if(selectedPage == 0)
        {
            // and we have 3+ pages...
            if (currentPageCount >= 3) 
            {
                // we have enough pages, show the first three
                for (int i = 0; i <= 2; i++) transform.GetChild(i).gameObject.SetActive(true);
            }
            else 
            {
                // we don't have enough pages to simply show the first three, so we show all the pages we have
                ShowAllPages();
            }

        }
        // logic for reaching last page from cycling right (CR)
        else if (selectedPage == currentPageCount - 1){

            // and we don't have enough pages to get two pages behind the last...
            if(currentPageCount < 3)
            {
                ShowAllPages();
            }
            else
            {
                for (int i = selectedPage - 2; i < currentPageCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        // end CR
        
        // logic for cycling left from first to last page (CL)
        else if (selectedPage == currentPageCount + 1)
        {
            selectedPage = currentPageCount - 1; // reset to usable last page value
            if (currentPageCount >= 3) //enough pages to show three
            {
                for (int i = 0; i < 3; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                Debug.Log("enableicons has reached the setactive logic");
                for (int i = selectedPage - 2; i < currentPageCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            else ShowAllPages();
        }
        //end CL

        else // show 3 pages, centered on selected page
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                // keep only three active pages; one to the left of the focused page, the focused page, and one to the right
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
    private void ShowAllPages() {
        for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(true);
    }
}
