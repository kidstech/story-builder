using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;
using System.Linq;

public class SentenceBar : MonoBehaviour
{
    //
    private readonly int tileSize = 125;

    //
    private Vector2 originalSize;

    //
    private RectTransform r;

    //
    private void Start()
    {
        //
        r = GetComponent<RectTransform>();

        //
        originalSize = r.sizeDelta;
    }

    //
    public List<WordTile> GatherWordTiles()
    {
        //
        List<WordTile> results = new List<WordTile>();

        //
        for(int i = 0; i < transform.childCount; i++)
        {
            //
            results.Add(transform.GetChild(i).GetComponent<WordTile>());
        }

        //
        return results;
    }

    //
    public void ResizeSentence(int amountOfTileSpaceToAdd)
    {
        //
        if(GatherWordTiles().Count * tileSize < originalSize.x)
        {
            //
            return;
        }

        //
        float futureWidth = r.sizeDelta.x + (amountOfTileSpaceToAdd * tileSize);

        //
        if(futureWidth > originalSize.x)
        {
            //
            r.sizeDelta = new Vector2(futureWidth, originalSize.y);
        }
        else
        {
            //
            r.sizeDelta = originalSize;
        }
    }
}
