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
    public GameObject pageContainer;

    [Header("Settings")]
    public int maxPageCount = 30;

    // Keeps track of what page we are 'focused' on
    private int selectedPage = -1;
    private int currentPageCount = 0;

    //
    private RectTransform rt;
    private Color yellow;

    //
    private void Start()
    {
        //
        rt = GetComponent<RectTransform>();
        yellow = new Color(255, 218, 0);
    }

    //
    public void AddPageIcon()
    {
        //
        if(currentPageCount >= maxPageCount)
        {
            return;
        }

        // Set page number equal to the currentPageCount, then increase by one
        int pageNumber = currentPageCount++;

        //
        pageContainer.GetComponent<PageContainer>().AddPage();

        //
        GameObject newPageIcon = Instantiate(pageIconPrefab);

        //
        newPageIcon.GetComponent<PageIcon>().SetupPageIcon(pageNumber);
        newPageIcon.transform.SetParent(this.transform);
        newPageIcon.transform.SetSiblingIndex(pageNumber);

        //
        rt.sizeDelta = rt.sizeDelta + new Vector2(64, 0);

        //
        UpdateSelectedPage(pageNumber);
    }

    //
    public void RemovePage()
    {
        //
        Transform pageToDelete = transform.GetChild(1 + selectedPage);

        //
        Destroy(pageToDelete);

        //
        for(int i = 1; i < transform.childCount - 1; i++)
        {
            //
            transform.GetChild(i).GetComponent<PageIcon>().UpdatePageNumber(i);
        }
    }

    //
    public void UpdateSelectedPage(int pageNumber)
    {
        // If the page is the same, do nothing.
        if (pageNumber == selectedPage)
        {
            return;
        }

        // Revert old icon color
        if(selectedPage != -1)
        {
            transform.GetChild(selectedPage).GetComponent<Image>().color = Color.white;

            pageContainer.transform.GetChild(selectedPage).gameObject.SetActive(false);
        }

        //
        selectedPage = pageNumber;

        // Set new icon
        transform.GetChild(selectedPage).GetComponent<Image>().color = yellow;
        pageContainer.transform.GetChild(selectedPage).gameObject.SetActive(true);
    }
}
