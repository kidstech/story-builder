using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject optionsMenu;

    public void openMenu()
    {
       optionsMenu.SetActive(true);
    }
    public void closeMenu()
    {
        optionsMenu.SetActive(false);
    }
        
}
