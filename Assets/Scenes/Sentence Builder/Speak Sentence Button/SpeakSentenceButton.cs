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

public class SpeakSentenceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{


    // Blue speaker image
    [Header("Images")]
    public Image playAudioImage;

    // Red square image
    public Image stopAudioImage;

    // Red cross image
    public Image noVoicesImage;

    // Play/stop image must be changed accordingly
    private Image buttonImage;

    // Used to disable Raycast Target option on the submit sentence button to disable it while the sentence is being read
    public Image submitSentenceButttonImage;

    // Used to disable Raycast Target option to prevent the user from being able to click on the learner while the sentence is being read
    public Image currentLearnerSprite;
    
    // Used to disable the interactable option to prevent the user from being able to change the scene while the sentence is being read
    public Button changeSceneButton;

    // Used to prevent the wordBankHolder from being interactable while the sentence is being read
    public CanvasGroup wordBankHolder;


    // Used to disable interaction with the mostRecentSavedSentence while the sentence in the conveyor is being read
    public Image mostRecentSavedSentence;


    // Used

    //
    [Header("Objects in Scene")]
    public SentenceBar sentence;
    public TextToSpeechHandler tts;

    // Resize image on mouseover
    private Vector2 defaultSize, highlightSize;


    void Update() {
        if(TextToSpeechHandler.isSpeaking) {
            submitSentenceButttonImage.raycastTarget = false;
            currentLearnerSprite.raycastTarget = false;
            changeSceneButton.interactable = false;
            wordBankHolder.interactable = false;
            wordBankHolder.blocksRaycasts = false;
            mostRecentSavedSentence.raycastTarget = false;
        }
        else {
            submitSentenceButttonImage.raycastTarget = true;
            currentLearnerSprite.raycastTarget = true;
            changeSceneButton.interactable = true;
            wordBankHolder.interactable = true;
            wordBankHolder.blocksRaycasts = true;
            mostRecentSavedSentence.raycastTarget = true;
        }
    }

    //
    void Start()
    {
        // Get a reference to the button image to swap play/stop images
        buttonImage = this.GetComponent<Image>();

        // No voices available
        if (!TextToSpeechHandler.voicesAvailable)
        {
            buttonImage.sprite = noVoicesImage.sprite;
            buttonImage.rectTransform.sizeDelta = new Vector2(75, 75);
        }

        // Set resize image dimensions
        defaultSize = buttonImage.rectTransform.sizeDelta;
        highlightSize = new Vector2(defaultSize.x + 10, defaultSize.y + 10);
    }

    //
    public void OnPointerClick(PointerEventData eventData)
    {
        List<WordTile> words;
        words = sentence.GatherWordTiles();
        if (TextToSpeechHandler.isSpeaking == false)
        {

            // Slowly here meaning each word tile is processed individually rather than as an entire sentence.
            StartCoroutine(tts.startSpeakingSentenceSlowly(words, true));
            // track all the words in the sentence
            foreach (WordTile wordTile in words)
            {
                LearnerDataHandler.UpdateWordCount(wordTile.word.baseWord);
            }
            LearnerDataHandler.StoreLearnerData();
            StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
        }
        else
        {
            Debug.Log("TTS already reading sentence, please wait.");
        }
    }

    //
    public void OnPointerEnter(PointerEventData eventData)
    {
        //
        buttonImage.rectTransform.sizeDelta = highlightSize;
    }

    //
    public void OnPointerExit(PointerEventData eventData)
    {
        //
        buttonImage.rectTransform.sizeDelta = defaultSize;
    }
}
