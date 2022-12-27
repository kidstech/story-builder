using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;

public class InputManager : MonoBehaviour {

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

    void Start() {
        Input.multiTouchEnabled = false;
    }


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
}