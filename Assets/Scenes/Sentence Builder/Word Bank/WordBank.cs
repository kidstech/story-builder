using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordBank : MonoBehaviour
{
    //
    public List<WordTile> words;

    // The number of tiles that can fit inside of one row of the word bank
    private readonly int tilesPerRow = 6;

    //
    private readonly int tileHeight = 50;

    public static bool canTouch;

    //
    void Start() {
        Input.multiTouchEnabled = false;
    }
    public void ResizeWordBank(int numEnabledTiles)
    {
        // casting numEnabledTiles to float so that integer division doesn't break the rounding here
        float newHeight = Mathf.CeilToInt((float)numEnabledTiles / tilesPerRow) * tileHeight;

        //
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, newHeight);
    }
    //
    public void SortWordBank()
    {
        //
        words = new List<WordTile>();

        //
        for (int i = 0; i < this.transform.childCount; i++)
        {
            //
            words.Add(this.transform.GetChild(i).GetComponent<WordTile>());
        }

        //
        words = words.OrderBy(wordTile => wordTile.word.baseWord).ToList();

        //
        for (int i = 0; i < words.Count; i++)
        {
            //
            words[i].transform.SetSiblingIndex(i);
        }
    }
}
