using System.Collections;
using System.Collections.Generic;
using Crosstales.RTVoice;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class SaveStoryButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Sprite upLever;
    public Sprite downLever;
    private Image currentImage;
    private Vector2 defaultSize, highlightSize;
    public GameObject saveStoryHandler;
    public GameObject StoryNamePrompt;

    [SerializeField]
    private GameObject storyNameInputField;
    [SerializeField]
    private SavedSentenceBank sentenceBank;
    [SerializeField]
    private Button finalStorySubmit;

    [SerializeField]
    private Button cancelStorySubmit;

    [SerializeField]
    private GameObject touchBlock;

    [SerializeField]
    private GameObject touchBlock2;
    


    public void Update() {
        if(StoryNamePrompt.activeInHierarchy == true) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                CloseStoryNameMenu();
                StartCoroutine(sentenceBank.speakAndSaveStory());
        }
    }
    }

     void Start()
    {
        finalStorySubmit.onClick.AddListener(() => {
            CloseStoryNameMenu();
            StartCoroutine(sentenceBank.speakAndSaveStory());
        });

        cancelStorySubmit.onClick.AddListener(() => {
            StoryNamePrompt.SetActive(false);
            touchBlock.SetActive(false);
            touchBlock2.SetActive(false);
        });
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
    }

     private IEnumerator pullLever()
    {
        currentImage.sprite = downLever;
        yield return new WaitForSecondsRealtime(2);
        currentImage.sprite = upLever;
    }


    public void OpenStoryNameMenu()
    {
        storyNameInputField.transform.Find("Placeholder").GetComponent<Text>().text = sentenceBank.getSentencesInBank()[0];
        StoryNamePrompt.SetActive(true);
        touchBlock.SetActive(true);
        touchBlock2.SetActive(true);
    }
    public void CloseStoryNameMenu()
    {
        StoryNamePrompt.SetActive(false);
        saveStoryHandler.GetComponent<SaveStoryHandler>().PutStoryInDatabase();
    }
   

}
