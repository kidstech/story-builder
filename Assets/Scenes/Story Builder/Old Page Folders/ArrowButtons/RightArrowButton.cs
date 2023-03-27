using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RightArrowButton : MonoBehaviour
{

    private Button rightArrow;
    public PageIconContainer pageIconContainer;
    
    void Start()
    {
        rightArrow = GetComponent<Button>();
        rightArrow.onClick.AddListener(movePageFocusRight);
    }

    public void movePageFocusRight() {
        // uses the selected page to shift pages in view right by one
        pageIconContainer.movePageViewRight(pageIconContainer.selectedPage);
    }
}
