﻿using System;
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
    public void AddPageIcon(PageContainer.PAGE pageType)
    {
        //
        if(currentPageCount >= maxPageCount)
        {
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
                // Otherwise set the only thing in the list as focus
                selectedPage = 0;
                transform.GetChild(0).GetComponent<Image>().color = yellow;
            }
        }
        else
        {
            // Revert old icon color
            if (selectedPage != -1)
            {
                transform.GetChild(selectedPage).GetComponent<Image>().color = Color.white;
            }

            //
            selectedPage = pageNumber;

            // Set new icon
            if (selectedPage != -1)
            {
                transform.GetChild(selectedPage).GetComponent<Image>().color = yellow;
            }

            //
            pageContainer.UpdateSelectedPage(selectedPage);
        }

        EnableIcons();
    }

    private void EnableIcons()
    {
        if (currentPageCount == 0) return;

        if(currentPageCount < 4)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                if(i >= selectedPage - 1 && i < selectedPage + 2)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                } 
            }
        }
    }
}