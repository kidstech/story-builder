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

public class CloseWordChoices : MonoBehaviour, IPointerClickHandler
{
    // Set-up for word choices image
    public bool isChoicesImgOn;
    public Image wordChoices;
    public Transform wordChoicesPrefab;

    public bool isCloseWordChoicesImgOn;
    public Image closeWordChoicesImage;
    public Transform closeWordChoicesPrefab;

    // Resize image on mouseover
    private Vector2 defaultSize,
                    highlightSize;

    void Start()
    {

        closeWordChoicesPrefab = GameObject.Find("close_word_choices").transform;
        closeWordChoicesImage = closeWordChoicesPrefab.GetComponent<Image>();

        wordChoices.enabled = false;
        isChoicesImgOn = false;
        closeWordChoicesImage.enabled = false;
        isCloseWordChoicesImgOn = false;

        // Resize on mouseover
        defaultSize = this.transform.GetComponent<Image>().rectTransform.sizeDelta;
        highlightSize = new Vector2(defaultSize.x + 15, defaultSize.y + 15);
    }

    /// <summary>
	/// Raises the pointer enter event.
	/// May increase image size.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerEnter(PointerEventData eventData)
    {

        // Increase image size
        transform.GetComponent<Image>().rectTransform.sizeDelta = highlightSize;

    }

    /// <summary>
	/// Raises the pointer exit event.
	/// May decrease the image size
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerExit(PointerEventData eventData)
    {

        // Decrease trash can image size
        transform.GetComponent<Image>().rectTransform.sizeDelta = defaultSize;

    }

    /// <summary>
    /// Raises the pointer click event.
    /// Submits the sentence to the completed sentences list.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        wordChoicesPrefab = GameObject.Find("word_choices").transform;
        wordChoices = wordChoicesPrefab.GetComponent<Image>();

        wordChoices.enabled = false;
        isChoicesImgOn = false;
        closeWordChoicesImage.enabled = false;
        isCloseWordChoicesImgOn = false;

    }
}