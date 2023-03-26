using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;

public class NewWordHolder : MonoBehaviour
{
    public static NewWordHolder instance;

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

    public static Word word2;

    private int wordHolderSiblingIndex;
    
    private int wordHolderDropZoneSiblingIndex;

    private int wordFormPopupSiblingIndex;

    
    void Awake()
    {
        baseWordGO.GetComponentInChildren<Button>().onClick.AddListener(() => baseWordClick(baseWordGO.GetComponent<Image>()));
        instance = this;
        wordHolderSiblingIndex = wordHolder.GetSiblingIndex();
        wordHolderDropZoneSiblingIndex = wordHolderDrop.transform.GetSiblingIndex();
        wordFormPopupSiblingIndex = this.transform.GetSiblingIndex();
    }

    void OnEnable()
    {
        baseWord.text = word2.baseWord;
        wordHolder.SetSiblingIndex(wordFormPopupSiblingIndex + 1);
        wordHolderDrop.transform.SetSiblingIndex(wordFormPopupSiblingIndex + 2);
    }

    void OnDisable() {
        foreach(GameObject go in buttons) {
            Destroy(go);
        }
        
        popUpBackground.sizeDelta = defaultHeightWidth;
        wordHolder.SetSiblingIndex(wordHolderSiblingIndex);
        wordHolderDrop.transform.SetSiblingIndex(wordHolderDropZoneSiblingIndex);
    }

    public void setUpForms() {
        float offset = 50;
         for (int i = 1; i < word2.forms.Count; i++)
        {
                GameObject button = Instantiate(newFormButton);
                button.transform.SetParent(newFormsPopUp.GetComponentInChildren<Canvas>().transform, false); 
                button.GetComponentInChildren<TMP_Text>().text = word2.forms[i];
                string wordFormText = word2.forms[i];
                Debug.Log(i);
                button.transform.position = new Vector3(baseWordT.position.x, baseWordT.position.y - offset, baseWordT.position.z);
                button.GetComponent<Button>().onClick.AddListener(() => formClick(word2, wordFormText, button.GetComponent<Image>()));
                offset += 50;
                buttons.Add(button);
            // float offset = 1;
            // button.GetComponent<RectTransform>().localPosition = new Vector2(baseWordT.position.x, baseWordT.position.y - offset);
            // button.GetComponentInChildren<TMP_Text>().text = word2.forms[i];
            // button.transform.SetParent(this.transform, false);
            // buttons.Add(button);
            }

            if(word2.forms.Count > 5) {
                float popUpWidth = popUpBackground.sizeDelta.x;
                float popUpHeight = 10 + (word2.forms.Count * 28);
                popUpBackground.sizeDelta = new Vector2(popUpWidth, popUpHeight);
            }
            
        }

        void formClick(Word word2, string wordFormText, Image buttonImage) {
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

