using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortButton : MonoBehaviour
{
    public GameObject sortButtonObject;

    public GenerateSortButtons g;

    public Animator a;

    public GameObject az;

    // Start is called before the first frame update
    void Start()
    {
        sortButtonObject = GameObject.Find("SortButton");

        g = sortButtonObject.GetComponent<GenerateSortButtons>();

        a = sortButtonObject.GetComponent<Animator>();

        az = GameObject.Find("AZDrawerButton");
    }

    public void toggleLetter()
    {
        g.updateSearchLetters(this.gameObject);
    }

    public void closeMenu()
    {
        a.SetBool("open", true);

        az.SetActive(true);
    }


}
