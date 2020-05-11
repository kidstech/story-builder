using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class PageWithPictureButton : MonoBehaviour
{
    //
    public enum MODE
    {
        OPEN_PICTURE,
        DELETE_PICTURE
    }

    //
    [Header("Button Mode")]
    public MODE mode;

    //
    private Button button;
    private PageWithPicture pageWithPicture;

    //
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);

        pageWithPicture = transform.parent.GetComponent<PageWithPicture>();
    }

    //
    private void HandleClick()
    {
        switch (mode)
        {
            case MODE.OPEN_PICTURE:

                string path = EditorUtility.OpenFilePanel("Select Picture", "", "png");

                pageWithPicture.LoadImage(path);

                break;

            case MODE.DELETE_PICTURE:

                pageWithPicture.RemoveImage();

                break;
        }
    }
}
