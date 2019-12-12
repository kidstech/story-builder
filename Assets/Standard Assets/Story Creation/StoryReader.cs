using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryReader : MonoBehaviour
{
    //
    public Story currentStory;
    public int currentPage = 0;
    public int maxPageNumber;

    //
    public int currentRow = 0;
    public float currentRowFill = 0;
    public float maxRows = 16;
    public float rowLength;

    //
    public GameObject textPrefab;
    public GameObject rowTextPrefab;

    //
    public string[] pageContent;

    //
    public void StartBuildingStory(Story story)
    {
        //
        currentStory = story;

        //
        pageContent = currentStory.story.Split('.');

        //
        maxPageNumber = (pageContent.Length - 1);

        //
        rowLength = rowTextPrefab.GetComponent<RectTransform>().sizeDelta.x;

        //
        StartCoroutine(DisplayPages(currentPage));
    }

    //
    public void PageForward()
    {
        //
        if(currentPage + 2 < maxPageNumber)
        {
            //
            currentPage += 2;

            //
            CleanPages();

            //
            StartCoroutine(DisplayPages(currentPage));
        }
    }

    //
    public void PageBackwards()
    {
        //
        if(currentPage > 0)
        {
            //
            currentPage -= 2;

            //
            CleanPages();

            //
            StartCoroutine(DisplayPages(currentPage));
        }
    }

    //
    private IEnumerator DisplayPages(int startPage)
    {
        //
        string[] tinyFragments;

        //
        currentRow = 0;
        currentRowFill = 0;

        //
        tinyFragments = pageContent[startPage].Split(' ');

        //
        for (int o = 0; o < tinyFragments.Length; o++)
        {
            if(o == 0)
            {
                // TODO first letter of every sentence capitalized.
            }

            if(o == tinyFragments.Length)
            {
                // TODO add period at the end
            }

            if (tinyFragments[o] != " " && tinyFragments[o] != string.Empty)
            {
                //
                GameObject newWord = Instantiate(textPrefab);

                //
                newWord.GetComponentInChildren<Text>().text = tinyFragments[o];

                //
                yield return StartCoroutine(UpdateSize(newWord));

                //
                if (!CanFit(newWord.GetComponent<RectTransform>().sizeDelta.x))
                {
                    //
                    GameObject newRow = Instantiate(rowTextPrefab);

                    //
                    newRow.transform.SetParent(this.transform.Find("PageLeft"));

                    //
                    currentRowFill = 0;

                    //
                    currentRow++;
                }

                //
                newWord.transform.SetParent(this.transform.Find("PageLeft").GetChild(currentRow), false);

                //
                currentRowFill += newWord.GetComponent<RectTransform>().sizeDelta.x;
            }
        }

        //
        currentRow = 0;
        currentRowFill = 0;

        // THIS PART
        if (maxPageNumber >= startPage + 1)
        {
            //
            tinyFragments = pageContent[startPage + 1].Split(' ');

            //
            for (int o = 0; o < tinyFragments.Length; o++)
            {
                if(tinyFragments[o] != " " && tinyFragments[o] != string.Empty)
                {
                    //
                    GameObject newWord = Instantiate(textPrefab);

                    //
                    newWord.GetComponentInChildren<Text>().text = tinyFragments[o];

                    //
                    yield return StartCoroutine(UpdateSize(newWord));

                    //
                    if (!CanFit(newWord.GetComponent<RectTransform>().sizeDelta.x))
                    {
                        // 
                        if(currentRow + 1 <= maxRows)
                        {
                            //
                            GameObject newRow = Instantiate(rowTextPrefab);

                            //
                            newRow.transform.SetParent(this.transform.Find("PageRight"));

                            //
                            currentRowFill = 0;

                            //
                            currentRow++;
                        }
                        else
                        {
                            Debug.LogWarning("EXCEEDED MAXIMUM ROWS");
                        }
                        
                    }

                    //
                    newWord.transform.SetParent(this.transform.Find("PageRight").GetChild(currentRow), false);

                    //
                    currentRowFill += newWord.GetComponent<RectTransform>().sizeDelta.x;
                }
            }
        }

        
    }

    //
    private IEnumerator UpdateSize(GameObject newWord)
    {
        //
        yield return new WaitForEndOfFrame();

        //
        newWord.GetComponent<RectTransform>().sizeDelta = newWord.transform.Find("Text").GetComponent<RectTransform>().sizeDelta + new Vector2(10, 10);
    }

    //
    private bool CanFit(float size)
    {
        //
        if(currentRowFill + size <= rowLength)
        {
            //
            return true;
        }
        else
        {
            //
            return false;
        }
    }

    //
    private void CleanPages()
    {
        for(int i = 0; i < transform.Find("PageLeft").childCount; i++)
        {
            Destroy(transform.Find("PageLeft").GetChild(i).gameObject);
        }

        for (int i = 0; i < transform.Find("PageRight").childCount; i++)
        {
            Destroy(transform.Find("PageRight").GetChild(i).gameObject);
        }

        //
        GameObject newRow = Instantiate(rowTextPrefab);

        //
        newRow.transform.SetParent(this.transform.Find("PageLeft"));

        //
        GameObject newRow2 = Instantiate(rowTextPrefab);

        //
        newRow2.transform.SetParent(this.transform.Find("PageRight"));

        currentRow = 0;
        currentRowFill = 0;
    }
}
