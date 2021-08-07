using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class DeletePageButton : MonoBehaviour
{
    [Header("Page Icon Containers")]
    public PageIconContainer pageIconContainer;
    private Button button;

    //
    private void Start()
    {
        button = GetComponent<Button>();
        //button.onClick.AddListener(ShowDialog);
    }

    private void DeletePage()
    {
        pageIconContainer.RemovePage();
    }

    // pop up confirmation window to confirm deletion
    // private void ShowDialog() 
    // {
    //     if (EditorUtility.DisplayDialog("Confirm Deletion", "Are you sure you want to delete this page?", "Delete", "Cancel") == true)
    //     {
    //         DeletePage();
    //     }
    // }
}
