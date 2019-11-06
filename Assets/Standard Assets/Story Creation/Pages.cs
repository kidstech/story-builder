using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pages : MonoBehaviour
{
    //
    private readonly int pageWidth = 450;
    private readonly int pageHeight = 600;

    //
    private void Start()
    {
        //
        GetComponent<RectTransform>().sizeDelta = new Vector2(transform.childCount * (pageWidth + 10), pageHeight);
    }

    //
    public void UpdatePageCount()
    {
        //
        GetComponent<RectTransform>().sizeDelta = new Vector2(transform.childCount * pageWidth, pageHeight);
    }
}
