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
    [Header("Images")]
	public Image playAudioImage;

	// Red square image
	public Image stopAudioImage;

	// Red cross image
	public Image noVoicesImage;

	// Play/stop image must be changed accordingly
	private Image buttonImage;

    //
    [Header("Objects in Scene")]
    public SentenceBar sentence;
    public TextToSpeechHandler tts;

    // Resize image on mouseover
    private Vector2 defaultSize, highlightSize;

    //
	void Start()
    {
		// Get a reference to the button image to swap play/stop images
		buttonImage = this.GetComponent<Image>();

		// No voices available
		if (!TextToSpeechHandler.voicesAvailable)
        {
			buttonImage.sprite = noVoicesImage.sprite;
			buttonImage.rectTransform.sizeDelta = new Vector2 (75, 75);
		}

		// Set resize image dimensions
		defaultSize = buttonImage.rectTransform.sizeDelta;
		highlightSize = new Vector2 (defaultSize.x + 10, defaultSize.y + 10);
	}

    //
    public void OnPointerClick(PointerEventData eventData)
    {
        List<WordTile> words;
        words = sentence.GatherWordTiles();
        // Slowly here meaning each word tile is processed individually rather than as an entire sentence.
        StartCoroutine(tts.startSpeakingSentenceSlowly(words, true));
        // track all the words in the sentence
        foreach(WordTile wordTile in words)
        {
            LearnerDataHandler.UpdateWordCount(wordTile.word.word);
        }
        LearnerDataHandler.StoreLearnerData();
        StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
        
    }

    //
    public void OnPointerEnter (PointerEventData eventData)
    {
        //
		buttonImage.rectTransform.sizeDelta = highlightSize;
	}
		
    //
	public void OnPointerExit (PointerEventData eventData)
    {
        //
        buttonImage.rectTransform.sizeDelta = defaultSize;
	}
}
