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



public class ConfirmButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    // Resize image on mouseover
    private Vector2 defaultSize,
                    highlightSize;

    // Space holding dragged word
    public Transform wordHolder;

    // Set-up for confirm button image
    public bool isConfirmImgOn;
    public Image confirmButtonImage;

    // Set-up for word choices image
	public bool isChoicesImgOn;
    public Image wordChoicesImage;
	public Transform wordChoicesPrefab;

    //Set-up for word choice closer image
    public bool isCloseWordChoicesImgOn;
    public Image closeWordChoicesImage;
    public Transform closeWordChoicesPrefab;

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        // Resize image on mouseover
        defaultSize = this.transform.GetComponent<Image>().rectTransform.sizeDelta;
        highlightSize = new Vector2(defaultSize.x + 15, defaultSize.y + 15);

        // Sets reference for word holder from CreateMainScene.cs
        wordHolder = GameObject.Find("word_holder").transform;

        // Sets reference for word choices from CreateMainScene.cs
        wordChoicesPrefab = GameObject.Find("word_choices").transform;

        //https://forum.unity.com/threads/how-to-enable-and-disable-canvas-image.284242/
        //Allows to show/hide the image from wordChoicesPrefab - Link above is credit
        wordChoicesImage = wordChoicesPrefab.GetComponent<Image>();

        closeWordChoicesPrefab = GameObject.Find("close_word_choices").transform;
        closeWordChoicesImage = closeWordChoicesPrefab.GetComponent<Image>();
        


        //Images are set to false to begin with, so they won't show up right away
        confirmButtonImage.enabled = false;
		isConfirmImgOn = false;
		wordChoicesImage.enabled = false;
		isChoicesImgOn = false;
        closeWordChoicesImage.enabled = false;
        isCloseWordChoicesImgOn = false;

        }

	/// <summary>
	/// Update this instance.
	/// </summary>
    void Update()
    {
        if (wordHolder.childCount > 0)
        {
			confirmButtonImage.enabled = true; //When there is a tile in the word holder, the confirm button image will be visible
            isConfirmImgOn = true;
        }

        else
        {
            confirmButtonImage.enabled = false; 
            isConfirmImgOn = false;
        }
       
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
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isConfirmImgOn == true)
        {
            wordChoicesImage.enabled = true;
            isChoicesImgOn = true;

            closeWordChoicesImage.enabled = true;
            isCloseWordChoicesImgOn = true;
        }
        else
        {
            wordChoicesImage.enabled = false;
            isChoicesImgOn = false;
        }

       // Debug.Log(this.gameObject.name + " Was Clicked.");
    }

    /// <summary>
    /// Raises the pointer click event.
    /// Submits the sentence to the completed sentences list.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
          

            // Might be useful for looking at for the code that will populate the image below with related words, once that whole word system is a thing
            /*
            string sentenceText = " ";

            // Grab all text from sentence
            for (int i = 0; i < sentence.childCount; i++)
            {

                string word = sentence.GetChild(i).GetChild(0).GetComponent<Text>().text;

                sentenceText += word + " ";

                //System.IO.File.WriteAllText(@"C:\Users\gordo580\Documents\Sentences\WriteText.txt", sentenceText);
            }

            System.IO.File.AppendAllText(@"/Users/gordo580/Documents/Sentences/WriteText.txt", System.DateTime.Now + " Submitted: " + sentenceText + System.Environment.NewLine);
            */


            // Might be useful for looking at for the code that will be creating the image when this button is clicked...?
            /* // Create a new scroll view
             completedSentenceScrollView = Instantiate(completedSentenceScrollView);

             // Give it a new color
             completedSentenceScrollView.GetComponent<Image>().color = colors[currentColor++ % colors.Length];

             // Set the text of the scroll view
             completedSentenceScrollView.GetChild(0).GetComponent<Text>().text = sentenceText;

             // Add the scroll view to the completed sentences list
             completedSentenceScrollView.SetParent(completedSentences, false);

             // Clear the sentence to ready it for new sentences
             sentence.GetComponent<Sentence>().clear();
             */
        }
    }