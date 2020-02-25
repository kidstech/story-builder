/// <summary>
/// Trashcan that can be used to wipe the current sentence and delete word tiles.
/// 
/// <author> antin006@morris.umn.edu </author>
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Trashcan : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
	

	// Resize image on mouseover
	private Vector2 defaultSize,
					highlightSize;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {

		// Resize image on mouseover
		defaultSize = this.transform.GetComponent<Image>().rectTransform.sizeDelta;
		highlightSize = new Vector2 (defaultSize.x + 15, defaultSize.y + 15);
	}

	/// <summary>
	/// Raises the pointer exit event.
	/// May decrease the image size
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerExit (PointerEventData eventData) {

		GameObject wordTile = eventData.pointerDrag;

		// If wordtile came from word bank don't hint with resizing that tile can be deleted
		if (wordTile != null && wordTile.GetComponent<WordTile> ().draggedFromWordBank)
			return;

		// Decrease trash can image size
		transform.GetComponent<Image> ().rectTransform.sizeDelta = defaultSize;
		
	}


	/// <summary>
	/// Raises the pointer enter event.
	/// May increase image size.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerEnter (PointerEventData eventData) {

		GameObject wordTile = eventData.pointerDrag;

		// If wordtile came from word bank don't hint with resizing that tile can be deleted
		if (wordTile != null && wordTile.GetComponent<WordTile> ().draggedFromWordBank)
			return;

		// Increase trash can image size
		transform.GetComponent<Image> ().rectTransform.sizeDelta = highlightSize;
		
	}
		
	/// <summary>
	/// Raises the drop event.
	/// If a word tile from the sentence or word holder is dropped onto trash can, it is deleted.
	/// A word tile from the word bank is not deleted by the trash can.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnDrop (PointerEventData eventData) {

		GameObject draggedObject = eventData.pointerDrag;

		if (draggedObject != null) {

			WordTile wordTile = draggedObject.GetComponent<WordTile> ();

			// If word tile was dragged from sentence and dropped on trash can then delete it
			if (wordTile != null && wordTile.draggedFromSentence) {
				wordTile.delete ();
				transform.GetComponent<Image> ().rectTransform.sizeDelta = defaultSize;
			}

            // If word tile was dragged from sentence and dropped on trash can, then it is deleted                  !!!!!!!!!!!!!!!!!!!!
            else if (wordTile != null && wordTile.draggedFromWordHolder)
            {
                wordTile.delete ();
                transform.GetComponent<Image>().rectTransform.sizeDelta = defaultSize;
            }
		}
	}

	/// <summary>
	/// If no word tile is dragged to the trash can then the sentence is cleared
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerClick (PointerEventData eventData) {
		Sentence sentence = GameObject.Find (Sentence.sentenceGameObjectName).GetComponent<Sentence> ();;

		sentence.clear ();
	}
}
