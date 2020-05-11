using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    //
    public int pageNumber = -1;

    //
    public PageContainer.PAGE type;

    //
    public void UpdatePageNumber(int newPageNumber)
    {
        //
        pageNumber = newPageNumber;
    }
}
