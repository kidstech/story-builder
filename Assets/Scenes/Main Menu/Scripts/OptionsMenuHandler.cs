using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuHandler : MonoBehaviour
{
    [Header("GameObjectReferences")]
    public GameObject optionsPanel;
    // this button is not a child of the options panel so that it can remain at the top right in spite of layout group properties
    public GameObject closeMenuButton;
    public void openMenu()
    {
        closeMenuButton.SetActive(true);
        optionsPanel.SetActive(true);
    }
    public void closeMenu()
    {
        closeMenuButton.SetActive(false);
        optionsPanel.SetActive(false);
    }
}
