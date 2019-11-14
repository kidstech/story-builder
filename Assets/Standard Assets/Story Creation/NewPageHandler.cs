using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPageHandler : MonoBehaviour
{
    public GameObject normalPagePrefab;
    public GameObject picturePagePrefab;

    private int pageCounter = 0;

    public void AddNormalPage()
    {
        GameObject newPage = Instantiate(normalPagePrefab);

        newPage.name = "Page" + pageCounter;

        newPage.transform.SetParent(transform.parent, false);

        transform.parent.GetComponent<Pages>().UpdatePageCount();

        transform.SetAsLastSibling();

        pageCounter++;
    }

    public void AddPicturePage()
    {
        GameObject newPage = Instantiate(picturePagePrefab);

        newPage.transform.SetParent(transform.parent, false);

        transform.parent.GetComponent<Pages>().UpdatePageCount();

        transform.SetAsLastSibling();
    }
}
