using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPageButton : MonoBehaviour
{
    //
    [Header("Page Container Scene Object")]
    public PageIconContainer pageContrainer;

    //
    private Button button;
    
    //
    private void Start()
    {
        //
        button = GetComponent<Button>();

        //
        button.onClick.AddListener(pageContrainer.AddPageIcon);

        //
        pageContrainer.AddPageIcon();
    }
}
