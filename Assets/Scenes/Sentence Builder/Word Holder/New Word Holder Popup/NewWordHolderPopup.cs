using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;

public class NewWordHolderPopup : MonoBehaviour
{
    public static NewWordHolderPopup instance;

    [SerializeField]
    private GameObject newFormsPopUp;

    [SerializeField]
    private RectTransform popUpBackground;

    [SerializeField]
    private TMP_Text baseWord; 

    [SerializeField]
     private GameObject newFormButton;

    [SerializeField]
    private Transform baseWordT;

    [SerializeField]
    private GameObject baseWordGO;

    [SerializeField]
    private TextToSpeechHandler TTS;

    [SerializeField]
    private Transform wordHolder;

    [SerializeField] 
    private GameObject wordHolderDrop; 

    [SerializeField]
    private Vector2 defaultHeightWidth;

    private List<GameObject> buttons = new List<GameObject>();

    public static Word word;

    private int wordHolderSiblingIndex;
    
    private int wordHolderDropZoneSiblingIndex;

    private int wordFormPopupSiblingIndex;

    
    /*
        * Setup button for the base word
        * Get indexes (where the GO is in the hieararchy) for the word holder, word holder dropzone, and the word holder popup
        * The indexes are used to change where the word holder, word holder dropzone, and the word holder popup are positioned in the hierarchy
    */
    void Awake()
    {
        baseWordGO.GetComponentInChildren<Button>().onClick.AddListener(() => baseWordClick(baseWordGO.GetComponent<Image>()));
        instance = this;
        wordHolderSiblingIndex = wordHolder.GetSiblingIndex();
        wordHolderDropZoneSiblingIndex = wordHolderDrop.transform.GetSiblingIndex();
        wordFormPopupSiblingIndex = this.transform.GetSiblingIndex();
    }

    /*
        * When the new word holder popup is enabled, set up the base word text and change the positions of the word holder and the word holder dropzone
        * The positions are changed so that they are in the "light" area and are not blocked by the tinted screen 
    */
    void OnEnable()
    {
        baseWord.text = word.baseWord;
        wordHolder.SetSiblingIndex(wordFormPopupSiblingIndex + 1);
        wordHolderDrop.transform.SetSiblingIndex(wordFormPopupSiblingIndex + 2);
    }

    /*
        * When the new word holder popup is closed, destroy all of the word forms in the popup, reset the size of the popup, and reset the indexes of the word holder and word holder dropzone
    */
    void OnDisable() {
        foreach(GameObject go in buttons) {
            Destroy(go);
        }
        
        popUpBackground.sizeDelta = defaultHeightWidth;
        wordHolder.SetSiblingIndex(wordHolderSiblingIndex);
        wordHolderDrop.transform.SetSiblingIndex(wordHolderDropZoneSiblingIndex);
    }

    /*
        * Instantiates buttons for the word forms when the new word holder popup is opened
        * offset controls how much vertical space is between the word form buttons
    */
    public void setUpForms() {
        float offset = 50;
         for (int i = 1; i < word.forms.Count; i++)
        {
                GameObject button = Instantiate(newFormButton);
                button.transform.SetParent(newFormsPopUp.GetComponentInChildren<Canvas>().transform, false); 
                button.GetComponentInChildren<TMP_Text>().text = word.forms[i];
                string wordFormText = word.forms[i];
                
                // Calculate the position of the button in the popup
                button.transform.position = new Vector3(baseWordT.position.x, baseWordT.position.y - offset, baseWordT.position.z);

                button.GetComponent<Button>().onClick.AddListener(() => formClick(word, wordFormText, button.GetComponent<Image>()));
                offset += 50;
                buttons.Add(button);
            }

            // The word form popup can handle about 5 buttons. Any more forms than that and the word holder popup has to be resized to accommodate the extra word forms
            if(word.forms.Count > 5) {
                float popUpWidth = popUpBackground.sizeDelta.x;
                float popUpHeight = 10 + (word.forms.Count * 28);
                popUpBackground.sizeDelta = new Vector2(popUpWidth, popUpHeight);
            }
            
        }

        /*
            * Update learner data when a word form is clicked
        */
        void formClick(Word word, string wordFormText, Image buttonImage) {
             // update word counts for the learner
            LearnerDataHandler.UpdateWordCount(wordFormText);
            // store it locally
            LearnerDataHandler.StoreLearnerData();
            // update the server's copy
            StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
            StartCoroutine(HighlightButton(Speaker.Instance.ApproximateSpeechLength(wordFormText), buttonImage));
            TTS.startSpeakingWordTile(wordFormText);
            wordHolderDrop.GetComponentInChildren<Text>().text = wordFormText;
            wordHolderDrop.GetComponentInChildren<WordTile>().textToDisplay = wordFormText;
        }

        /*
            * Update learner data when the base word is clicked
        */
        void baseWordClick(Image buttonImage) {
             // update word counts for the learner
            LearnerDataHandler.UpdateWordCount(baseWord.text);
            // store it locally
            LearnerDataHandler.StoreLearnerData();
            // update the server's copy
            StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
            StartCoroutine(HighlightButton(Speaker.Instance.ApproximateSpeechLength(baseWord.text), buttonImage));
            TTS.startSpeakingWordTile(baseWord.text);
             wordHolderDrop.GetComponentInChildren<Text>().text = baseWord.text;
             wordHolderDrop.GetComponentInChildren<WordTile>().textToDisplay = baseWord.text;
        }

        private IEnumerator HighlightButton(float seconds, Image buttonImage) {
            buttonImage.color = Color.yellow;
            yield return new WaitForSeconds(seconds);
            buttonImage.color = Color.white;
        }
    }

