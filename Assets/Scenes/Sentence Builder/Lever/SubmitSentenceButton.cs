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
    private Image currentImage;
    public Animator conveyorAnimator;
    public Animator pipesAnimator;

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        // get a reference to the current image so it can be swapped later
        currentImage = this.GetComponent<Image>();

        defaultSize = this.transform.GetComponent<Image>().rectTransform.sizeDelta;
        highlightSize = new Vector2(defaultSize.x + 10, defaultSize.y + 10);
    }

    /// <summary>
    /// Raises the pointer enter event.
    /// Increases the size of the submit sentence button image.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        //
        currentImage.rectTransform.sizeDelta = highlightSize;
    }

    /// <summary>
    /// Raises the pointer exit event.
    /// Decreases the size of the submit sentence button image.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        //
        currentImage.rectTransform.sizeDelta = defaultSize;
    }



    /// <summary>
    /// Raises the pointer click event.
    /// Submits the sentence to the completed sentences list.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (TextToSpeechHandler.speakingSentence == false)
        {
            // Pull the lever kronk!
            StartCoroutine(pullLever());

            // set position to right so that the previous built sentence is hidden
            completedSentences.position += new Vector3(870f, 0f, 0f); // sentence game object

            List<WordTile> tiles = sentence.GatherWordTiles();
            // If there are words in the sentence
            if (tiles.Count > 0)
            {
                string rawSentence = "";
                foreach (WordTile tile in tiles)
                {
                    // track the submitted word
                    LearnerDataHandler.UpdateWordCount(tile.textToDisplay);
                    rawSentence = rawSentence + tile.textToDisplay + " ";
                }
                LearnerDataHandler.StoreLearnerData();
                StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());

                rawSentence = rawSentence.Remove(rawSentence.Length - 1, 1);

                // Save to json. This is temporary and is taking the place of a database;
                SaveSentenceHandler.SaveSentence(tiles);
                StartCoroutine(ServerRequestHandler.PostSentence(SaveSentenceHandler.mostRecentSentence));
                float speakDuration = tts.getApproxSpeechTime(tiles);
                StartCoroutine(animateConveyorBelt(speakDuration));
                StartCoroutine(animatePipes(speakDuration));
                StartCoroutine(tts.startSpeakingSentenceSlowly(tiles, false));

                //
                //StartCoroutine(revealSentenceWordByWord(words));
                completedSentences.GetComponentInChildren<Text>().text = rawSentence; // place the raw text of the completed sentence into the most recent saved sentence game object
                                                                                      // animate the big block of sentence to the left for approximately how long it takes for the speaker to speak it
                StartCoroutine(revealSentenceAnimation(tiles));

                StartCoroutine(sentence.GetComponent<SentenceBar>().AnimateAndTransferTiles());

            }
        }
        else
        {
            // maybe the button should shake left an right here to show that it's unavailable while TTS is already going?
            Debug.Log("TTS already reading sentence, please wait.");
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

    // method to slowly reveal the already completed sentence
    // this will move from right to left, making it look like it's coming out of the pipe instead of just appearing.
    private IEnumerator revealSentenceAnimation(List<WordTile> wordTiles)
    {
        float approxSpeechTime;
        approxSpeechTime = tts.getApproxSpeechTime(wordTiles);
        yield return new WaitForSeconds(approxSpeechTime);
        // set position to right so animation actually moves from somewhere
        completedSentences.position += new Vector3(800f, 0f, 0f); // sentence game object
        LeanTween.moveLocalX(completedSentences.gameObject, -95f, tts.getApproxSpeechTime(wordTiles));
        yield return new WaitForSeconds(approxSpeechTime/2);
        tts.startSpeakingSentence(wordTiles, false);
    }

    public IEnumerator animateConveyorBelt(float duration)
    {
        conveyorAnimator.SetBool("SubmittingSentence", true);
        yield return new WaitForSeconds(duration);
        conveyorAnimator.SetBool("SubmittingSentence", false);
    }

    public IEnumerator animatePipes(float duration)
    {
        pipesAnimator.SetBool("ProcessingTile", true);
        yield return new WaitForSeconds(duration);
        pipesAnimator.SetBool("ProcessingTile", false);
    }

}
