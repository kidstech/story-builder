/// <summary>
/// Each time a sentence is completed by submitting with the green checkmark button a new completed sentence scroll view is created.
/// 
/// <author> antin006@morris.umn.edu </author>
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubmitSentenceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Images")]
    // Image is a lever which is pulled down while the sentence is being saved
    public Sprite upLever;
    public Sprite downLever;

    [Header("Text To Speech Handler")]
    // TTS Object
    public TextToSpeechHandler tts;

    [Header("Scene Objects")]
    // Completed sentences list at bottom of screen
    public Transform completedSentences;

    // Sentence at top of screen top pull sentence text from
    public SentenceBar sentence;

    // Resize image on mouseover
    private Vector2 defaultSize, highlightSize;

    //
    private Image currentImage;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
    {
        // get a reference to the current image so it can be swapped later
        currentImage = this.GetComponent<Image>();

		defaultSize = this.transform.GetComponent<Image>().rectTransform.sizeDelta;
		highlightSize = new Vector2 (defaultSize.x + 10, defaultSize.y + 10);
	}
		
	/// <summary>
	/// Raises the pointer enter event.
	/// Increases the size of the submit sentence button image.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerEnter (PointerEventData eventData)
    {
        //
        currentImage.rectTransform.sizeDelta = highlightSize;
	}

	/// <summary>
	/// Raises the pointer exit event.
	/// Decreases the size of the submit sentence button image.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerExit (PointerEventData eventData)
    {
        //
        currentImage.rectTransform.sizeDelta = defaultSize;
	}


		
	/// <summary>
	/// Raises the pointer click event.
	/// Submits the sentence to the completed sentences list.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerClick (PointerEventData eventData)
    {
        // Pull the lever kronk!
        StartCoroutine(pullLever());

        //
        List<WordTile> tiles = sentence.GatherWordTiles();

        // If there is words in sentence
        if (tiles.Count > 0)
        {
            //
            List<Word> words = new List<Word>();

            //
            string rawSentence = "";

            //
            foreach(WordTile tile in tiles)
            {
                //
                words.Add(tile.word);

                //
                rawSentence = rawSentence + tile.textToDisplay + " ";
            }

            //
            rawSentence = rawSentence.Remove(rawSentence.Length - 1, 1);

            // Save to json. This is temporary and is taking the place of a database;
            SaveSentenceHandler.SaveSentence(words);
            
            //
            StartCoroutine(tts.startSpeakingSentenceSlowly(tiles, false));

            //
            //StartCoroutine(revealSentenceWordByWord(words));
            completedSentences.GetComponentInChildren<Text>().text = rawSentence;
            // animate the big block of sentence to the left for approximately how long it takes for the speaker to speak it
            revealSentenceAnimation(tiles);
            

            StartCoroutine(sentence.GetComponent<SentenceBar>().ClearTiles());

        }
	}

    /// <summary>
    /// Changes the lever image to the down position for 2 seconds, then reset it
    /// </summary>
    private IEnumerator pullLever()
    {
        currentImage.sprite = downLever;
        yield return new WaitForSecondsRealtime(2);
        currentImage.sprite = upLever;
    }
    // currently just pops in one word at a time without an animation
    // to use this method, comment out the two lines below it:
    // completedSentences.GetComponentInChildren<Text>().text = rawSentence;
    // revealSentenceAnimation(rawSentence, words);
    // private IEnumerator revealSentenceWordByWord(List<Word> words) {
    //     // if a sentence is already in the box we need to clear it before showing a new one
    //     if (completedSentences.GetComponentInChildren<Text>().text != null){
    //         completedSentences.GetComponentInChildren<Text>().text = "";
    //     }
    //     foreach(Word word in words) {
    //     completedSentences.GetComponentInChildren<Text>().text += word.word + " ";
    //     yield return new WaitForSecondsRealtime(1);
    //     }
        
    // }
    
    // method to slowly reveal the already completed sentence
    // this will move from right to left, making it look like it's coming out of the pipe instead of just appearing.
    private void revealSentenceAnimation(List<WordTile> wordTiles){
        // set position to right so animation actually moves from somewhere
        completedSentences.GetComponentInChildren<Text>().rectTransform.position += new Vector3(350f,0f,0f);
        // moving the entire group of words to the center over a total time of 1 second per word (which should change to the approx of tts later)
        LeanTween.moveX(completedSentences.GetComponentInChildren<Text>().rectTransform, 0f, tts.getApproxSpeechTime(wordTiles));
        // make sure the animation starts at 350 pixels to the right
        completedSentences.GetComponentInChildren<Text>().rectTransform.position += new Vector3(350f,0f,0f);
    }
    
}
