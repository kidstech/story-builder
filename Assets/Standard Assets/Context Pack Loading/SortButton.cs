using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortButton : MonoBehaviour
{
    public GameObject sortButtonObject;

    public GenerateSortButtons g;

    private Color oldColor = new Color(0, 255, 0);

    // Start is called before the first frame update
    void Start()
    {
        sortButtonObject = GameObject.Find("SortButton");

        g = sortButtonObject.GetComponent<GenerateSortButtons>();
    }

    public void toggleLetter()
    {
        g.updateSearchLetters(this.gameObject.GetComponentInChildren<Text>().text);

        Color tempCol = this.gameObject.GetComponent<Image>().color;

        this.gameObject.GetComponent<Image>().color = oldColor;

        oldColor = tempCol;
    }


}
