using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;

public class SavedSentenceBank : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject sentencePrefab;

    private Vector2 sentencePrefabSize;
    private string sentenceText;

    [SerializeField]
    private GameObject storyBuilderTouchBlock;

    //Have this because the user could still click on the change scene button while the story was reading
    [SerializeField]
    private GameObject sentenceBuilderTouchBlock;

    [SerializeField]
    private ScrollRect scrollBar;
    public List<SavedSentence> sentences = new List<SavedSentence>();

    public List<string> sentenceIds = new List<string>();

    [SerializeField]
    private GameObject storySubmissionStatus;

    [SerializeField]
    private GameObject stopSign;

    [SerializeField]
    private GameObject speakerButton;

    [SerializeField]
    private AudioSource successNoise;

    public bool stopSpeaking = false;

    // saved sentence bank disabled when not in use => change scene sentencebuilder -> storybuilder needs to activate the
    void OnEnable()
    {
        //Debug.Log(this.transform.name + " has been enabled");
        sentences = LoadSavedSentences.LoadSentences();

        sentencePrefabSize = sentencePrefab.GetComponent<RectTransform>().sizeDelta;
        GetComponent<RectTransform>().sizeDelta = new Vector2(sentencePrefabSize.x, sentencePrefabSize.y * sentences.Count);

        // create sentence game object for each sentence we have and populate its components
        Debug.Log(sentences.Count);
        for (int i = sentences.Count-1; i >= 0; i--)
        {
            if(!sentenceIds.Contains(sentences[i].sentenceId)) {
            GameObject newSentence = Instantiate(sentencePrefab);

            newSentence.GetComponent<SentenceObject>().savedSentence = sentences[i];
            newSentence.GetComponentInChildren<Text>().text = sentences[i].sentenceText;
            newSentence.transform.SetParent(this.transform, false);
            newSentence.GetComponent<SentenceTile>().textToDisplay = sentences[i].sentenceText; 
            } // update textToDisplay for SentenceTile script bc that's what it uses for tts
        }
    }
    void OnDisable()
    {
        //Debug.Log(this.transform.name + " has been disabled");
        // empty the bank each time so we don't create duplicates
        foreach (Transform child in this.transform)
        {
            Debug.Log("destroying: " + child.gameObject.name);
            GameObject.Destroy(child.gameObject);
        }
    }

    public static string CompileSentence(List<WordTile> tiles)
    {
        string compiledSentence = "";

        for(int i = 0; i < tiles.Count; i++)
        {
            // using textToDisplay here because that's what gets changed in the wordholder
            compiledSentence += tiles[i].textToDisplay + " ";
        }

        compiledSentence = compiledSentence.Remove(compiledSentence.Length - 1, 1);

        return compiledSentence;
    }

    public void ttsUpdateLearnerData() {
        //storyBuilderTouchBlock.SetActive(true);
        //sentenceBuilderTouchBlock.SetActive(true);
        StartCoroutine(speakStory());
        //foreach(SavedSentence sentence in sentences) {
            // iterate through all the words in a sentence
           // foreach(string selectedWord in sentence.selectedWordForms)
            //{
                // update the word counts for each word
              //  LearnerDataHandler.UpdateWordCount(selectedWord);
           // }
       // }
        // store updated wordcounts locally
        //LearnerDataHandler.StoreLearnerData();
        // update server with new word counts from speaking the page
        //StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
    }

     public IEnumerator speakAndSaveStory()
    {
        Debug.Log("I am being called");
        Canvas.ForceUpdateCanvases();
        if (transform.childCount == 0) yield return null;

        //
        //string fullPage = "";
        string textToRead = null;
        float speechDuration = 0;
        float children = this.gameObject.transform.childCount;
            //
            for (int o = 0; o < transform.childCount; o++)
            {
                Debug.Log("I am reading");
                if( o != 0 && scrollBar.verticalNormalizedPosition >= 0) {
                    Debug.Log(scrollBar.verticalNormalizedPosition);
                    scrollBar.verticalNormalizedPosition = scrollBar.verticalNormalizedPosition - (1/(children - 1));
                }
                // example:          PageContainer => PagePrefab => SentencePrefab
                textToRead = transform.GetChild(o).GetComponentInChildren<SentenceTile>().textToDisplay.ToLower();
                speechDuration = Speaker.Instance.ApproximateSpeechLength(textToRead) * (1 / TextToSpeechHandler.voiceRate);
                transform.GetChild(o).GetComponentInChildren<SentenceTile>().ReadSentence();
                yield return new WaitForSeconds(speechDuration + 0.5f);
                Destroy(transform.GetChild(o).GetComponentInChildren<Image>());
            }
            DisplaySubmissionStatus();
    }


    public IEnumerator speakStory()
    {
        Canvas.ForceUpdateCanvases();
        if (transform.childCount == 0) yield return null;

        //
        //string fullPage = "";
        string textToRead = null;
        float speechDuration = 0;
        float children = this.gameObject.transform.childCount;
        scrollBar.verticalNormalizedPosition = 1;
        speakerButton.SetActive(false);
        stopSign.SetActive(true); 
                  //
                  Debug.Log("The stopSpeaking bool is set to: " + stopSpeaking);
                  Debug.Log("Am I stopping here?");
            for (int o = 0; o < transform.childCount; o++)
            {
                if( o != 0 && scrollBar.verticalNormalizedPosition >= 0) {
                    scrollBar.verticalNormalizedPosition = scrollBar.verticalNormalizedPosition - (1/(children - 1));
                }
                // example:          PageContainer => PagePrefab => SentencePrefab
                textToRead = transform.GetChild(o).GetComponentInChildren<SentenceTile>().textToDisplay.ToLower();
                speechDuration = Speaker.Instance.ApproximateSpeechLength(textToRead) * (1 / TextToSpeechHandler.voiceRate);
                transform.GetChild(o).GetComponentInChildren<SentenceTile>().ReadSentence();
                foreach(string word in transform.GetChild(o).GetComponentInChildren<SentenceObject>().savedSentence.selectedWordForms) {
                     LearnerDataHandler.UpdateWordCount(word);
                     Debug.Log("What about here?");
                }
                yield return new WaitForSeconds(speechDuration + 0.5f);
                if(stopSpeakingSentences()) {
                    Debug.Log("I am stopping");
                    yield break;
                };
                }
                // store updated wordcounts locally
                LearnerDataHandler.StoreLearnerData();
                speakerButton.SetActive(true);
                stopSign.SetActive(false);
                // update server with new word counts from speaking the page
                StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
            }
                


    public void setStopSpeaking() {
        if (stopSpeaking == false){
            stopSpeaking = true;
        }
        Debug.Log("Stop speaking has been set to true and is: " + stopSpeaking);
    }
    
    public bool stopSpeakingSentences() {
        if(stopSpeaking == true) {
            stopSign.SetActive(false);
            speakerButton.SetActive(true);
            LearnerDataHandler.StoreLearnerData();
            // update server with new word counts from speaking the page
            StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
            stopSpeaking = false;
            storyBuilderTouchBlock.SetActive(false);
            sentenceBuilderTouchBlock.SetActive(false);
            return true;
        }
        else {
            return false;
        }
    }

    public List<string> getSentencesInBank() {
        List<string> sentencesInBank = new List<string>();
        foreach(Transform child in this.transform) {
            sentencesInBank.Add(child.GetComponent<SentenceTile>().textToDisplay);
        }
        return sentencesInBank;
    }


    public void clearSentences(){
        Debug.Log("Clearing sentences");
        GameObject sentenceBank = this.gameObject;
        for (int i=0; i<sentenceBank.transform.childCount; i++){
            sentenceIds.Add(sentenceBank.transform.GetChild(i).GetComponent<SentenceObject>().savedSentence.sentenceId);
            Destroy(sentenceBank.transform.GetChild(i).gameObject);
        }
    }

    private void DisplaySubmissionStatus()
    {
        // might be nice to have the success message contain the name of the submitted story
        storySubmissionStatus.SetActive(true);
        successNoise.Play();
        StartCoroutine(CloseSubmissionStatus());
    }
    public IEnumerator CloseSubmissionStatus()
    {
        yield return new WaitForSecondsRealtime(2); 
        storySubmissionStatus.SetActive(false);
        //sentenceBank.GetComponent<SavedSentenceBank>().destroySentences();
        clearSentences();
        storyBuilderTouchBlock.SetActive(false);
        sentenceBuilderTouchBlock.SetActive(false);
    }


}
