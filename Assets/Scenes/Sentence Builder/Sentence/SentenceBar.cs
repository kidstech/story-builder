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

    int animcount = 1;

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

    // ClearTiles will remove the tiles from the sentence bar one by one and wait for an animation to play
    // (animation not implemented yet)
    // public void ClearTiles()
    // {
    //     //
    //     foreach(Transform child in transform)
    //     {
    //         Debug.Log("Game object name: " + child.gameObject.name);
    //         StartCoroutine(animateTiles());
    //         animateTile(child);
    //     }
    // }
    public IEnumerator ClearTiles()
    {
        int length = this.transform.childCount;
        // this cannot be a for each loop. It turns out that destroying the game objects that the for each loop uses to
        // determine where it is in the loop causes very bad things to happen.
        for(int i = 0; i < length; i++)
        {
            animateTile(this.transform.GetChild(0));
            yield return new WaitForSeconds(1);
            // By destroying the first child, we change the indices of the child array
            // So if we were to use i here, we would destroy every other game object and eventually find ourselves outside of the array.
            Destroy(this.transform.GetChild(0).gameObject);
            // This is where we would put our conveyor belt animation.
            // we need this wait time so that the resize operation that occurs after destroying a game object can take place
            yield return new WaitForSeconds(.25f);
        }

    }
    
    // takes in a child of the sentence transform and animates it
    // just teleports the tile up 20 units for now
    public void animateTile(Transform child)
    {
            Debug.Log("animateTile has been called " + animcount + " times.");
            animcount++;
            // You can't actually directly edit the position of the game object so we have to copy the
            // position info to a variable, change it, and then set the position equal to the value of that changed variable.
            Vector3 tilePosition = child.gameObject.transform.position;
            // move tile up towards pipe
            tilePosition.y += 20;
            child.gameObject.transform.position = tilePosition;
    }
}
