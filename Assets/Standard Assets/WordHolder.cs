/// <summary>
/// Represents the Sentence that the user builds sentences with by dropping word tiles into.
/// 
/// <author> antin006@morris.umn.edu </author>
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // Useful for calling GameObject.Find() reliably
    public static string wordHolderGameObjectName;

    // Position of scrollbar at startup
    public Vector2 defaultScrolledPosition;

    /// <summary>
    /// Initalize object.
    /// </summary>
    void Start()
    {
        // Grab name for GameObject.Find()
        wordHolderGameObjectName = gameObject.name;

        // Save the startup scrolled position
        defaultScrolledPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    /// <summary>
    /// Raises the pointer enter event. 
    /// Helps to determine if a word tile is held within the sentence.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {

        // Grab the possible word tile
        GameObject wordTile = eventData.pointerDrag;

        // A dragged tile is held over the sentence
        if (wordTile != null && wordTile.GetComponent<WordTile>() != null)
        {
            wordTile.GetComponent<WordTile>().tileHeldOverWordHolder = true;
        }

    }

    /// <summary>
    /// Raises the pointer exit event. 
    /// Helps to determine if a word tile is held within the sentence.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerExit(PointerEventData eventData)
    {

        // Grab the possible word tile
        GameObject wordTile = eventData.pointerDrag;

        // A dragged tile is not held over the sentence
        if (wordTile != null && wordTile.GetComponent<WordTile>() != null)
        {
            wordTile.GetComponent<WordTile>().tileHeldOverWordHolder = false;
        }

    }

    /// <summary>
    /// Removes all word tiles from the this sentence.
    /// </summary>
    public void clear()
    {

        // Reset any scrolling of the transform
        resetScroll();

        // Remove all word tiles from the sentence
        foreach (Transform wordTile in this.transform)
            Destroy(wordTile.gameObject);

    }

    /// <summary>
    /// Resets the scroll bar to the default position.
    /// </summary>
    public void resetScroll()
    {
        GetComponent<RectTransform>().anchoredPosition = defaultScrolledPosition;
    }

}