using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPageMenuToggle : MonoBehaviour
{
    //
    private Button button;

    //
    public GameObject newPageMenu;

    //
    private void Start()
    {
        //
        button = GetComponent<Button>();

        //
        button.onClick.AddListener(TogglePageMenu);
    }

    //
    private void TogglePageMenu()
    {
        //
        newPageMenu.SetActive(!newPageMenu.activeInHierarchy);
    }
}
