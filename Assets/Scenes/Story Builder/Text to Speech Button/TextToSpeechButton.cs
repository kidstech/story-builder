/// <summary>
/// The text to speech button controls when to play the text within the sentence.
/// It also shows when any text is being spoken and can stop any spoken text on click.
/// 
/// <author> antin006@morris.umn.edu </author>
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model.Event;

public class TextToSpeechButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler  {
	

	// Blue speaker image
	public Image playAudioImage;

	// Red square image
	public Image stopAudioImage;

	// Red cross image
	public Image noVoicesImage;

	// Play/stop image must be changed accordingly
	private Image buttonImage;

	// Is the blue speaker image shown?
	private bool showingPlayOption = true;


	// Sentence containing phrase to be spoken
	public Sentence sentence;

	// Handles all text to speech operations
	public TextToSpeechHandler textToSpeechHandler;

	// Resize image on mouseover
	private Vector2 defaultSize,
					highlightSize;

	// Counter to keep track of number of listens
	public ListenCounter counter = new ListenCounter();


	/// <summary>
	/// Start this instance.
	/// If no voices are available a red x is displayed.
	/// </summary>
	void Start(){

		// Get a reference to the button image to swap play/stop images
		buttonImage = this.GetComponent<Image> ();

		// No voices available
		if (!TextToSpeechHandler.voicesAvailable) {
			buttonImage.sprite = noVoicesImage.sprite;
			buttonImage.rectTransform.sizeDelta = new Vector2 (75, 75);
		}

		// Set resize image dimensions
		defaultSize = this.transform.GetComponent<Image>().rectTransform.sizeDelta;
		highlightSize = new Vector2 (defaultSize.x + 10, defaultSize.y + 10);
	}


	/// <summary>
	/// Raises the pointer enter event.
	/// Increases size of TTS button.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerEnter (PointerEventData eventData) {
		transform.GetComponent<Image> ().rectTransform.sizeDelta = highlightSize;
	}
		
	/// <summary>
	/// Raises the pointer exit event.
	/// Decreases size of TTS button.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerExit (PointerEventData eventData) {
		transform.GetComponent<Image> ().rectTransform.sizeDelta = defaultSize;
	}
		
	/// <summary>
	/// Raises the pointer click event.
	/// Toggles the TTS button.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerClick (PointerEventData eventData) {

        // Start speaking
        if (showingPlayOption && sentence.transform.childCount > 0)
        {
            textToSpeechHandler.startSpeaking(sentence.getSentenceText(), TextToSpeechHandler.SoundType.SENTENCE, sentence.gameObject);
        }
        // Stop speaking
        else if (!showingPlayOption) { }
			//textToSpeechHandler.stopSpeaking ();

	}
		
	/// <summary>
	/// Shows the play option with the blue speaker image.
	/// </summary>
	public void showPlayOption(){
		
		// Show play option (Blue speaker image)
		buttonImage.sprite = playAudioImage.sprite;
		showingPlayOption = true;
	}

	/// <summary>
	/// Shows the stop option with the red square image.
	/// </summary>
	public void showStopOption(){

		// Show stop option (Red square image)
		buttonImage.sprite = stopAudioImage.sprite;
		showingPlayOption = false;
	}
		
}
