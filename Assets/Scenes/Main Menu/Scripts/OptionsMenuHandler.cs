using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuHandler : MonoBehaviour
{
    public GameObject optionsPanel;
    public void openMenu()
    {
        optionsPanel.SetActive(true);
    }
    public void closeMenu()
    {
        optionsPanel.SetActive(false);
    }
}
