using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPageHandler : MonoBehaviour
{
    public GameObject normalPagePrefab;
    public GameObject picturePagePrefab;

    public void AddNormalPage()
    {
        Debug.Log("Adding new page");

        GameObject newPage = Instantiate(normalPagePrefab);

        newPage.transform.SetParent(transform.parent, false);

        transform.parent.GetComponent<Pages>().UpdatePageCount();

        transform.SetAsLastSibling();
    }

    public void AddPicturePage()
    {
        GameObject newPage = Instantiate(picturePagePrefab);

        newPage.transform.SetParent(transform.parent, false);

        transform.parent.GetComponent<Pages>().UpdatePageCount();

        transform.SetAsLastSibling();
    }
}
