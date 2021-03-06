﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageIcon : MonoBehaviour
{
    //
    public int pageNumber = -1;

    //
    private PageIconContainer pageIconContainer;
    public GameObject pageContainer;
    private Button button;

    //
    private void Start()
    {
        //
        pageContainer = GameObject.Find("PageContainer");
        pageIconContainer = transform.parent.GetComponent<PageIconContainer>();
        button = GetComponent<Button>();

        //
        button.onClick.AddListener(WorkAround);
    }

    //
    private void WorkAround()
    {
        //
        pageIconContainer.UpdateSelectedPage(pageNumber);
        pageContainer.transform.GetComponent<PageContainer>().UpdateSelectedPage(pageNumber);
    }

    //
    public void SetupPageIcon(int newPageNumber)
    {
        // Since pages technically start at 0
        pageNumber = newPageNumber;

        // 
        name = string.Concat("Page", (newPageNumber + 1));
        GetComponentInChildren<Text>().text = "Page\n" + (newPageNumber + 1);
    }
}
