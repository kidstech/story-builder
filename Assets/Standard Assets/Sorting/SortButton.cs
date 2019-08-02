using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortButton : MonoBehaviour
{
    public GameObject sortButtonObject;

    public GenerateSortButtons g;

    public Animator a;

    private Color c_green = new Color(0, 255, 0, 1);
    private Color c_red = new Color(255, 0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        sortButtonObject = GameObject.Find("SortButton");

        g = sortButtonObject.GetComponent<GenerateSortButtons>();

        a = sortButtonObject.GetComponent<Animator>();
    }

    public void toggleLetter()
    {
        g.updateSearchLetters(this.gameObject);
    }

    public void ToggleMenu()
    {
        bool isOpen = a.GetBool("open");

        if (isOpen)
        {
            this.gameObject.GetComponent<Image>().color = c_red;
            this.gameObject.GetComponentInChildren<Text>().text = "<";
        }
        else
        {
            this.gameObject.GetComponent<Image>().color = c_green;
            this.gameObject.GetComponentInChildren<Text>().text = ">";
        }

        a.SetBool("open", !isOpen);
    }


}
