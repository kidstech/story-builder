using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeletePageButton : MonoBehaviour
{
    [Header("Page Icon Containers")]
    public PageIconContainer pageIconContainer;
    private Button button;

    //
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(DeletePage);
    }

    private void DeletePage()
    {
        pageIconContainer.RemovePage();
    }
}
