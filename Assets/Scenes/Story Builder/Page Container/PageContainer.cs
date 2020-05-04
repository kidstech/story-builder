using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageContainer : MonoBehaviour
{
    //
    private float spacingWidth;
    private float pageWidth;

    //
    [Header("Prefabs")]
    public GameObject pagePrefab;

    [Header("Scene References")]
    public GameObject pageIconContainer;

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
    public void AddPage()
    {
        //
        GameObject newPage = Instantiate(pagePrefab);

        //
        newPage.transform.SetParent(this.transform);
    }
}
