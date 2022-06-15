using System.Collections;
using System.Collections.Generic;
using Crosstales.RTVoice;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;

public class SaveStoryButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Sprite upLever;
    public Sprite downLever;

    private Image currentImage;

    public TextToSpeechHandler tts;
    private Vector2 defaultSize, highlightSize;

    private Button button;
    public GameObject saveStoryHandler;
    public GameObject StoryNamePrompt;
    public GameObject StorySubmissionStatus;

    [SerializeField]
    private GameObject storyNameInputField;
    [SerializeField]
    private SavedSentenceBank sentenceBank;
    [SerializeField]
    private AudioSource successNoise;
    


    public void Update() {
        if(StoryNamePrompt.activeInHierarchy == true) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                CloseStoryNameMenu();
                sentenceBank.speakStory();
                StartCoroutine(removeSentenceBackground()); 
        }
    }
    }

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
       StartCoroutine(pullLever());
       OpenStoryNameMenu();
       //sentenceBank.speakStory();
       //StartCoroutine(removeSentenceBackground()); 
       
       //CloseStoryNameMenu();
    }

     private IEnumerator pullLever()
    {
        currentImage.sprite = downLever;
        yield return new WaitForSecondsRealtime(2);
        currentImage.sprite = upLever;
    }

    private IEnumerator removeSentenceBackground()
    {
        for (int i=0; i<sentenceBank.transform.childCount; i++){
            //Debug.Log("We are at sentence: " + i);
            string tileText = sentenceBank.transform.GetChild(i).GetComponentInChildren<Text>().text;
            float timeToSpeak = Speaker.Instance.ApproximateSpeechLength(tileText) / tts.getVoiceRate();
            yield return new WaitForSecondsRealtime(timeToSpeak - 0.1f);
            Destroy(sentenceBank.transform.GetChild(i).GetComponent<Image>());
        }    

        DisplaySubmissionStatus();
    }

    public void OpenStoryNameMenu()
    {
        storyNameInputField.transform.Find("Placeholder").GetComponent<Text>().text = sentenceBank.getSentencesInBank()[0];
        StoryNamePrompt.SetActive(true);
    }
    public void CloseStoryNameMenu()
    {
        // string storyName = StoryNamePrompt.GetComponentInChildren<Text>().text;
         StoryNamePrompt.SetActive(false);
        saveStoryHandler.GetComponent<SaveStoryHandler>().PutStoryInDatabase();
        // if story submitted successfully... (should check this eventually)
        //DisplaySubmissionStatus();
    }
    private void DisplaySubmissionStatus()
    {
        // might be nice to have the success message contain the name of the submitted story
        StorySubmissionStatus.SetActive(true);
        successNoise.Play();
    }
    public void CloseSubmissionStatus()
    {
        StorySubmissionStatus.SetActive(false);
        //sentenceBank.GetComponent<SavedSentenceBank>().destroySentences();
        clearSentences();
    }

    public void clearSentences(){
        Debug.Log("Clearing sentences");
        for (int i=0; i<sentenceBank.transform.childCount; i++){
            Destroy(sentenceBank.transform.GetChild(i).gameObject);
        }
    }

}
