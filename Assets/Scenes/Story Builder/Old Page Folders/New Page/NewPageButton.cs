using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPageButton : MonoBehaviour
{
    //
    [Header("Page Container Scene Object")]
    public PageIconContainer pageIconContrainer;

    //
    [Header("Page Type")]
    public PageContainer.PAGE pageType;

    //
    private Button button;
    
    //
    private void Start()
    {
        //
        button = GetComponent<Button>();

        //
        button.onClick.AddListener(WorkAround);
    }

    //
    private void WorkAround()
    {
        pageIconContrainer.AddPageIcon(pageType);
    }
}
