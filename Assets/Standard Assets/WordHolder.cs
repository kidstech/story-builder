using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Boolean for keeping track if a tile is in the word holder or not.
    public bool tileInHolder = false;


    // Do something on start
    void Start()
    {
        
    }

   // Fires when the cursor enters the word holder
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Get a possible wordTile under our cursor.
        GameObject wordTile = eventData.pointerDrag;

        // Check if we are holding a word tile while over the wordholder
        if (wordTile != null && wordTile.GetComponent<WordTile>() != null)
        {
            // Flag the tile as being held over the word holder
            wordTile.GetComponent<WordTile>().tileHeldOverWordHolder = true;
        }
    }

    // Fired when the cursor leaves the word holder
    public void OnPointerExit(PointerEventData eventData)
    {

        // Grab the possible word tile
        GameObject wordTile = eventData.pointerDrag;

        // A dragged tile is not held over the sentence
        if (wordTile != null && wordTile.GetComponent<WordTile>() != null)
            wordTile.GetComponent<WordTile>().tileHeldOverSentence = false;

    }

    // Remove all tiles from the word holder
    public void clear()
    {
        // In case of more tiles
        foreach (Transform wordTile in this.transform)
            Destroy(wordTile.gameObject);

    }
}
