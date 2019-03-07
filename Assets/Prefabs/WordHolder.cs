/// <summary>
/// Represents the Word Holder that the user can place a word tile into.
/// 
/// <author> antin006@morris.umn.edu </author>
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    
    // Useful for calling GameObject.Find() reliably
	//public static string wordHolderGameObjectName;
    

    /// <summary>
    /// Initalize object.
    /// </summary>
    void Start(){

		// Grab name for GameObject.Find()
		//wordHolderGameObjectName = gameObject.name;
        
    }
		
	/// <summary>
	/// Raises the pointer enter event. 
	/// Helps to determine if a word tile is held within the word holder.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerEnter (PointerEventData eventData) {

		// Grab the possible word tile
		GameObject wordTile = eventData.pointerDrag;

		// A dragged tile is held over the word holder
		if (wordTile != null && wordTile.GetComponent<WordTile>() != null) 
			wordTile.GetComponent<WordTile> ().tileHeldOverWordHolder = true;

	}

	/// <summary>
	/// Raises the pointer exit event. 
	/// Helps to determine if a word tile is held within the word holder.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerExit (PointerEventData eventData) {

		// Grab the possible word tile
		GameObject wordTile = eventData.pointerDrag;

		// A dragged tile is not held over the word holder
		if ( wordTile != null && wordTile.GetComponent<WordTile>() != null )
			wordTile.GetComponent<WordTile> ().tileHeldOverWordHolder = false;
		
	}

    /// <summary>
	/// Removes all word tiles from the this word holder.
	/// </summary>
	public void clear()
    {
        // Remove all word tiles from the word holder
        foreach (Transform wordTile in this.transform)
            Destroy(wordTile.gameObject);

    }

    /// <summary>
    /// Gets the word holder text.
    /// </summary>
    /// <returns>The word holder text.</returns>
    public string getWordHolderText(){
		string phrase = "";

		// Get all word tile text
		for(int i = 0; i < transform.childCount; i++)
			phrase += transform.GetChild (i).GetChild (0).GetComponent<Text> ().text + " ";
		
		return phrase;
	}

}
