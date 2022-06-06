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
    public List<SavedSentence> sentences = new List<SavedSentence>();

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
            GameObject newSentence = Instantiate(sentencePrefab);

            newSentence.GetComponent<SentenceObject>().savedSentence = sentences[i];
            newSentence.GetComponentInChildren<Text>().text = sentences[i].sentenceText;
            newSentence.transform.SetParent(this.transform, false);
            newSentence.GetComponent<SentenceTile>().textToDisplay = sentences[i].sentenceText; // update textToDisplay for SentenceTile script bc that's what it uses for tts
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

    public void speakStory() {
        StartCoroutine(ttsSpeakStory());
        foreach(SavedSentence sentence in sentences) {
            // iterate through all the words in a sentence
            foreach(string selectedWord in sentence.selectedWordForms)
            {
                // update the word counts for each word
                LearnerDataHandler.UpdateWordCount(selectedWord);
            }
        }
        // store updated wordcounts locally
        LearnerDataHandler.StoreLearnerData();
        // update server with new word counts from speaking the page
        StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
    }

     public IEnumerator ttsSpeakStory()
    {
        if (transform.childCount == 0) yield return null;

        //
        //string fullPage = "";
        string textToRead = null;
        float speechDuration = 0;
            //
            for (int o = 0; o < transform.childCount; o++)
            {
                // example:          PageContainer => PagePrefab => SentencePrefab
                textToRead = transform.GetChild(o).GetComponentInChildren<SentenceTile>().textToDisplay.ToLower();
                speechDuration = Speaker.Instance.ApproximateSpeechLength(textToRead) * (1 / TextToSpeechHandler.voiceRate);
                transform.GetChild(o).GetComponentInChildren<SentenceTile>().ReadSentence();
                yield return new WaitForSeconds(speechDuration);
            }
    }

    public List<string> getSentencesInBank() {
        List<string> sentencesInBank = new List<string>();
        foreach(Transform child in this.transform) {
            sentencesInBank.Add(child.GetComponent<SentenceTile>().textToDisplay);
        }
        return sentencesInBank;
    }


}
