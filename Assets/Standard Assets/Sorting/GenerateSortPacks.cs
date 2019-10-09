using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class GenerateSortPacks : MonoBehaviour
{
    // button prefab
    public GameObject packPrefab;

    // Master WOrd LIst
    MasterWordList w;

    // Word Bank Script
    buildWordBank b;

    // Keep track of what was the last button clicked
    GameObject oldButton = null;

    // Keep track of what was the last color of the button
    private Color c_green = new Color(0, 255, 0, 1);
    private Color c_white = new Color(1, 1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        w = LoadContextPacks.loadContextPacks();

        b = GameObject.Find("WordBankContent").GetComponent<buildWordBank>();

        buildButtons(w.masterWordList);
    }

    public void buildButtons(List<MasterWordList.Word> list)
    {
        // Get the number of context packs
        int packCount = w.masterContextPackList.Count;

        // For each context pack
        for (int i = 0; i < packCount; i++)
        {
            // Copy in a new game object
            GameObject o = Instantiate(packPrefab);

            byte[] image = new byte[0];

            string[] fileTypes = new string[] { ".png", ".jpg", "jpeg" };

            for(int k = 0; k < fileTypes.Length; k++)
            {
                string path = w.masterContextPackList[i].contextPackIconPath + fileTypes[k];

                if (File.Exists(path))
                {
                    image = File.ReadAllBytes(path);

                    Texture2D texture = new Texture2D(88, 44, TextureFormat.ARGB32, false);

                    texture.LoadImage(image);

                    o.GetComponentInChildren<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                    // Change the display text
                    o.GetComponentInChildren<Text>().text = "";

                    break;
                }
            }

            // If our image was not loaded, set the text equal to the name
            if(image.Length == 0)
            {
                o.GetComponentInChildren<Text>().text = w.masterContextPackList[i].contextPackName;
            }

            // Change some values in the button
            o.GetComponent<SortPack>().contextPackId = w.masterContextPackList[i].contextPackId;
            o.GetComponent<SortPack>().contextPackName = w.masterContextPackList[i].contextPackName;
            o.GetComponent<SortPack>().contextPackIconPath = w.masterContextPackList[i].contextPackIconPath;

            // Add it into the button view
            o.transform.SetParent(this.transform, false);

        }
    }

    public void updateSearchPack(GameObject button)
    {
        // If this is the same button
        if(button == oldButton)
        {
            int pack = button.GetComponent<SortPack>().contextPackId;

            bool packOn = w.ToggleFilter(pack);

            if (packOn)
            {
                button.GetComponent<Image>().color = c_green;
            }
            else
            {
                button.GetComponent<Image>().color = c_white;
            }
        }
        // This is a different button
        else
        {
            if(oldButton != null)
            {
                oldButton.GetComponent<Image>().color = c_white;

                int oldPack = oldButton.GetComponent<SortPack>().contextPackId;

                w.ToggleFilter(oldPack);
            }

            int pack = button.GetComponent<SortPack>().contextPackId;

            bool packOn = w.ToggleFilter(pack);

            button.GetComponent<Image>().color = c_green;

            oldButton = button;
        }

        List<MasterWordList.Word> newList = new List<MasterWordList.Word>();

        newList = w.getFilteredWords(); //w.getPack(searchPacks);

        b.rebuildWordBank(newList);
    }
}
