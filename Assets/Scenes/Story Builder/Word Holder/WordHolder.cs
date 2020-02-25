using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WordHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // Useful for calling GameObject.Find() reliably
    public static string wordHolderGameObjectName;

    // Position of scrollbar at startup
    public Vector2 defaultScrolledPosition;

    // Prefab called "WordHolderPopup"
    public GameObject wordHolderPopupPrefab;

    void Start()
    {
        // Grab name for GameObject.Find()
        wordHolderGameObjectName = gameObject.name;

        // Save the startup scrolled position
        defaultScrolledPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    // When the mouse enters the wordHolder
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

    // When the mouse exits the wordHolder
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

    // Method to build the screen for changing word forms
    public void setupWordHolderPopup(List<string> wordForms)
    {
        // If there is more than 1 word forms, open the menu to select the other options.
        if(wordForms.Count > 1)
        {
            // Create the object
            GameObject wordHolderPopup = Instantiate(wordHolderPopupPrefab);

            // Set its parent to be the canvas
            wordHolderPopup.transform.SetParent(GameObject.Find("Canvas").transform, false);

            // Assign the wordForms to the object
            wordHolderPopup.GetComponent<WordHolderPopup>().wordForms = wordForms;
        }
    }
}