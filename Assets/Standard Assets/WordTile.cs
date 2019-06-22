/// <summary>
/// Represents a draggable word tile.
/// 
/// <author> antin006@morris.umn.edu </author>
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Crosstales.RTVoice;

public class WordTile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {

	// Is the tile a part of the sentence?
	public bool tileHeldOverSentence = false;

    // Is the tile a part of the word holder?
    public bool tileHeldOverWordHolder = false; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

	// Was the tile dragged from the wordbank?
	public bool draggedFromWordBank = true;

	// Was the tile dragged from the sentence?
	public bool draggedFromSentence = false;

    // Was the tile dragged from the word holder?
    public bool draggedFromWordHolder = false; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

	// Should this tile be deleted ASAP?
	public bool flaggedForDeletion = false;

	// Handles all text to speech operations
	public TextToSpeechHandler textToSpeechHandler;

	// A fake placeholder tile to create tile whitespace when rearranging sentence tiles
	private static Transform placeholderTile = null;

	// When a word bank tile is dragged a clone must be made to replace it
	public Transform clonedWordBankTile;

	// Sentence formed by individual word tiles
	private static Transform sentence;

	// Canvas holding all components
	private static Transform canvas;

    // Space holding dragged word
    public Transform wordHolder;





	/// <summary>
	/// Start this instance.
	/// Sets up some initial object references to be used throughout the application's lifetime.
	/// </summary>
	void Start(){

		// Grab a static reference for the sentence
		if (sentence == null) 
			sentence = GameObject.Find (Sentence.sentenceGameObjectName).transform;

		// Grab a static reference for the canvas
		if (canvas == null)
			// Hierarchy from word bank tile to top-level canvas
			canvas = transform.parent.parent.parent.parent.parent;

		// Build a placeholder tile to be used when rearranging sentence tiles
		if (placeholderTile == null)
			placeholderTile = buildPlaceHolder ();

	}

	/// <summary>
	/// Raises the pointer click event.
	/// When a word tile is clicked it should have its text spoken by TTS.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerClick (PointerEventData eventData) {

		// Speak the word on the tile
		textToSpeechHandler.startSpeaking (eventData.pointerPress.GetComponent<WordTile> ().getText ());

	}

	/// <summary>
	/// Raises the begin drag event.
	/// Occurs when a tile has just begun to be dragged.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnBeginDrag(PointerEventData eventData){


		// Word tile began drag from the word bank
		if (draggedFromWordBank) {
			// Create a clone of the dragged word tile
			clonedWordBankTile = Instantiate (clonedWordBankTile);

			// Set cloned tile's parent to the dragged tile's word bank slot
			clonedWordBankTile.transform.SetParent (transform.parent, false);
		} 

		// Word tile began drag from the sentence 
		else if (draggedFromSentence || draggedFromWordHolder) //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 
			// Add the placeholder to the sentence
			activatePlaceholder (transform.GetSiblingIndex());

		// Set dragged tile's parent to canvas for global visibilty
		transform.SetParent (canvas);

		// The dragged tile "blocks" cursor events from being registered by blocking rays cast from the cursor
		// Tell the tile to not block ray casts so cursor events can still be registered
		GetComponent<CanvasGroup> ().blocksRaycasts = false;

	}

	/// <summary>
	/// Raises the drag event.
	/// Occurs while a word tile is being dragged.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnDrag(PointerEventData eventData){

		// Tile follows cursor
		this.transform.position = eventData.position;

		// Activate placeholder for tiles dragged from the word bank

		// A dragged word bank tile is held over the sentence 
		if (tileHeldOverSentence && draggedFromWordBank) 
			// Setup a placeholder for the dragged word bank tile
			activatePlaceholder (sentence.childCount);

        // A dragged word bank tile is held over the word holder
        else if (tileHeldOverWordHolder && draggedFromWordBank)
            activatePlaceholder (wordHolder.childCount);     //!!!!!

        // A dragged word bank tile is not held over the sentence
        else if (!tileHeldOverSentence && !tileHeldOverWordHolder && draggedFromWordBank)
            // Remove the sentence placeholder
            deactivatePlaceholder();


		// Rearrange sentence tiles when a tile is dragged

		// Rearrange tiles when dragged tile is over the sentence and tiles exist in the sentence
		if (tileHeldOverSentence && sentence.childCount >= 1) {

			// Assume added tile ends up on right most index
			int newSiblingIndex = sentence.childCount;

			// Check every sentence tile against the dragged tile
			for (int i = 0; i < sentence.childCount; i++) {

				// Dragged tile is to the left of a word tile
				if (transform.position.x < sentence.GetChild (i).position.x) {

					// Get the new index for the placeholder
					newSiblingIndex = i;

					// Placeholder is to the left of the new sibling index
					if (placeholderTile.transform.GetSiblingIndex () < newSiblingIndex)
						// "Ignore" the placeholder and decrement the new index
						newSiblingIndex--;

					// Stop checking sentence children
					break;
				}
			}

			// Move the rearranged placeholder to the new index
			placeholderTile.SetSiblingIndex (newSiblingIndex);
		} 

	}

	/// <summary>
	/// Raises the end drag event.
	/// Occurs when a word tile has stopped being dragged and is dropped.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnEndDrag(PointerEventData eventData){

        // Dragged from word bank and not held over sentence, or, should be
        if (draggedFromWordBank && !tileHeldOverSentence && !tileHeldOverWordHolder || flaggedForDeletion) //!!!!!!!!!!!!!!!!!!!!!!
            Destroy(this.gameObject);

        /*// Dragged from word bank and not held over word holder, or should be
        else if (draggedFromWordBank && !tileHeldOverWordHolder || flaggedForDeletion)                  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Destroy(this.gameObject); */

        // Dragged from word bank and tile held over the sentence
        else if (draggedFromWordBank && tileHeldOverSentence)
        {

            // Add the tile to the sentence
            setTileInSentence();

            // Tile will be dragged from sentence in future drags
            draggedFromWordBank = false;
            draggedFromSentence = true;
        }

        // Dragged from word bank and tile held over word holder
        else if (draggedFromWordBank && tileHeldOverWordHolder && wordHolder.childCount < 1)                  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        {

            // Add the tile to the word holder
            setTileInWordHolder();

            // Tile will be dragged from word holder in future drags
            draggedFromWordBank = false;
            draggedFromWordHolder = true;
        }

        // Dragged from sentence and tile held over word holder
        else if (draggedFromSentence && tileHeldOverWordHolder && wordHolder.childCount < 1)
        {

            // Add the tile to the word holder
            setTileInWordHolder();

            // Tile will be dragged from word holder in future drags
            draggedFromSentence = false;
            draggedFromWordHolder = true;
        }

        //Destroys current tile in word holder before placing currently dragged tile in word holder
        else if (draggedFromWordBank && tileHeldOverWordHolder && wordHolder.childCount > 0)
        {

            foreach (Transform child in wordHolder.transform) {
                GameObject.Destroy(child.gameObject);
            }

            setTileInWordHolder();

            draggedFromWordBank = false;
            draggedFromWordHolder = true;

        }

        else if (draggedFromSentence && tileHeldOverWordHolder && wordHolder.childCount > 0)
        {
            setTileInSentence();
        }

        // Dragged from word holder and tile held over sentence
        else if (draggedFromWordHolder && tileHeldOverSentence)
        {

            // Add the tile to the sentence
            setTileInSentence();

            // Tile will be dragged from sentence in future drags
            draggedFromWordHolder = false;
            draggedFromSentence = true;
        }

        // Dragged from sentence
        else if (draggedFromSentence)
            setTileInSentence();

        // Dragged from word holder
        else if (draggedFromWordHolder)
            setTileInWordHolder();



		// Remove the placeholder from the sentence
		deactivatePlaceholder ();

		// Rays cast from the cursor onto the tile will intersect the tile, allowing the tile to be selected for dragging
		GetComponent<CanvasGroup> ().blocksRaycasts = true;

	}

	/// <summary>
	/// Flags this word tile for deletion.
	/// </summary>
	public void delete(){

		// Delete tile ASAP
		flaggedForDeletion = true;

		// Re-center the scroll view on the sentence
		sentence.GetComponent<Sentence> ().resetScroll ();
	}

	/// <summary>
	/// Returns the text contained within this word tile
	/// 
	/// </summary>
	/// <returns>The word tile text</returns>
	public string getText(){
		return transform.GetChild (0).GetComponent<Text> ().text;
	}

	/// <summary>
	/// Constructs a placeholder tile that will be used for rearranging tiles within the sentence.
	/// </summary>
	/// <returns>The place holder.</returns>
	private Transform buildPlaceHolder(){

		// Create the placeholder and set dimensions to dragged tile's dimensions
		GameObject placeholder = new GameObject ();
		Rect placeHolderDimension = placeholder.AddComponent<RectTransform> ().rect;
		Rect draggedTileDimension = ((RectTransform)this.transform).rect;
		placeHolderDimension.height = draggedTileDimension.height;
		placeHolderDimension.width = draggedTileDimension.width;

		// Don't make use of the placeholder yet
		placeholder.SetActive (false);

		return placeholder.transform;
	}

	/// <summary>
	/// Activates the placeholder so that it occupies sentence whitespace for tile rearrangement.
	/// </summary>
	/// <param name="index">Index.</param>
	private void activatePlaceholder(int index){

		// Add placeholder tile to parent
		placeholderTile.SetParent (sentence, false);

		// Set placeholder tile index to dragged tile's index
		placeholderTile.SetSiblingIndex (index);

		// Active the placeholder tile
		placeholderTile.gameObject.SetActive (true);

	}


	/// <summary>
	/// Deactivate the placeholder so it does not occupy the sentence and contribute whitespace.
	/// </summary>
	private void deactivatePlaceholder(){

		// Orphan placeholder tile from previous parent
		placeholderTile.transform.SetParent (null);

		// Deactivate the placeholder tile
		placeholderTile.gameObject.SetActive (false);
	}

	/// <summary>
	/// Sets this tile to be a child of the sentence.
	/// </summary>
	private void setTileInSentence(){

		// Add the tile to the sentence
		transform.SetParent (sentence);

		// Position the tile at the correct index in the sentence
		transform.SetSiblingIndex (placeholderTile.GetSiblingIndex ());
	}

    private void setTileInWordHolder()
    {

        // Add the tile to the sentence
        if (wordHolder.childCount < 2)
        {
            transform.SetParent(wordHolder);

            transform.GetComponent<RectTransform>().sizeDelta.Set(150,50);

            // Position the tile at the correct index in the sentence
            transform.SetSiblingIndex(placeholderTile.GetSiblingIndex());
        }
    }

}