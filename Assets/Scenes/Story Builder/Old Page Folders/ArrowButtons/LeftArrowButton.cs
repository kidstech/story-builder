using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftArrowButton : MonoBehaviour
{

    private Button leftArrow;
    public PageIconContainer pageIconContainer;
    
    void Start()
    {
        leftArrow = GetComponent<Button>();
        leftArrow.onClick.AddListener(movePageFocusLeft);

    }
    // decrements the selected page as well as the pages to the left and right of it by one page number
    private void movePageFocusLeft() 
    {
        // uses the selected page to shift pages in view left by one
        pageIconContainer.movePageViewLeft(pageIconContainer.selectedPage);
    }
}
