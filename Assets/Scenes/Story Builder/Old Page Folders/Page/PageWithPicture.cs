using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class PageWithPicture : MonoBehaviour
{
    //
    private RawImage image;

    //
    [Header("Buttons")]
    public GameObject addPictureButton;
    public GameObject deletePictureButton;

    //
    private void Start()
    {
        //
        image = GetComponent<RawImage>();
    }

    //
    public void LoadImage(string path)
    {
        if(path.Length == 0) return;

        // Size of the current image
        Texture2D picture = new Texture2D(1, 1);

        // Read in picture
        picture.LoadImage(File.ReadAllBytes(path));

        // Load picture and set as image
        image.texture = picture;

        // Disable other button
        addPictureButton.SetActive(false);
        deletePictureButton.SetActive(true);
    }

    //
    public void RemoveImage()
    {
        //
        image.texture = null;

        // Disable other button
        addPictureButton.SetActive(true);
        deletePictureButton.SetActive(false);
    }
}
