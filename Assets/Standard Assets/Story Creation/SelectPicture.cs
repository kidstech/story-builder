using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

public class SelectPicture : MonoBehaviour
{
    private string picturePath;

    private string[] options = { "PNG", "png", "JPG", "jpg" };

    private void SetPicture()
    {
        byte[] rawImage = new byte[0];

        rawImage = File.ReadAllBytes(picturePath);

        Texture2D texture = new Texture2D(400, 225, TextureFormat.ARGB32, false);

        texture.LoadImage(rawImage);

        transform.parent.Find("Picture").GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        Destroy(this.gameObject);
    }

    public void PickPicture()
    {
        picturePath = EditorUtility.OpenFilePanelWithFilters("Select a picture", "", options);

        SetPicture();
    }
}
