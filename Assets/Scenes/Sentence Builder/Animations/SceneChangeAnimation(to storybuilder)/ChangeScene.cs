using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    [Header("Game Objects in Scene")]
    public Transform sentenceBuilderCanvas;
    public Transform storyBuilderCanvas;
    public GameObject mainCamera;

    public GameObject updateWordBankButton;
    public GameObject sentenceBank;
    public GameObject currentLearnerInfoSentenceBuilder; // name and profile image of current learner in the sentence builder canvas
    private float sceneHeight = 0;
    private float transitionTime = 2f; // used to keep the wait function in line with animation time
    private string buttonText;

    [SerializeField]
    private Slider sentenceVoicePitch;
    [SerializeField]
    private Slider sentenceVoiceRate;
    [SerializeField]
    private Slider storyVoicePitch;
    [SerializeField]
    private Slider storyVoiceRate;

    public enum SceneType{
        SentenceBuilder,
        StoryBuilder,
    }
    public static SceneType sceneState = SceneType.SentenceBuilder;

    public void ToggleScene()
    {
        if (sceneState == SceneType.SentenceBuilder) // sentencebuilder -> storybuilder
        {
            StartCoroutine(ServerRequestHandler.GetSentences(SaveSentenceHandler.StoreSentences));
            sceneHeight = storyBuilderCanvas.transform.GetComponent<RectTransform>().rect.height * .92f; // multiplier is arbitrary. It just ended up being a proportion that kept most of the submitted sentence tile in view alongside the storybuilder view.
            //storyBuilderCanvas.gameObject.SetActive(true); // activate story builder
            LeanTween.moveY(mainCamera, -sceneHeight, transitionTime);
            sceneState = SceneType.StoryBuilder;
            buttonText = "Build Sentence";
            StartCoroutine(WaitForTransition(transitionTime, buttonText));

            // don't need to update word bank in storybuilder scene
            updateWordBankButton.SetActive(false);

            this.transform.SetParent(storyBuilderCanvas); // changescene button is moved over to the active canvas, so if/when we shut it down the previous canvas, it will remain
            this.transform.SetSiblingIndex(9);
            currentLearnerInfoSentenceBuilder.SetActive(false); // disable the current learner info in the sentence builder scene / unused scene

            // Change the pitch and rate in the story builder scene to match the pitch and rate in the sentence builder scene
            storyVoicePitch.value = sentenceVoicePitch.value;
            storyVoiceRate.value = sentenceVoiceRate.value;

            sentenceBank.SetActive(true);
            
        }
        else if (sceneState == SceneType.StoryBuilder) // storybuilder -> sentencebuilder
        {
            LeanTween.moveY(mainCamera, 0, 2f); 
            sceneState = SceneType.SentenceBuilder;
            buttonText = "Build Story";
            StartCoroutine(WaitForTransition(transitionTime, buttonText));

            // enable word bank updating for sentencebuilder scene
            updateWordBankButton.SetActive(true);

            this.transform.SetParent(sentenceBuilderCanvas);
            this.transform.SetSiblingIndex(11);

            // deactivate sentence bank to ensure it gets refreshed again when changing scenes
            currentLearnerInfoSentenceBuilder.SetActive(true); // re-enable the current learner info in the sentence builder view

            // Change the pitch and rate in the sentence builder scene to match the pitch and rate in the story builder scene
            sentenceVoicePitch.value = storyVoicePitch.value;
            sentenceVoiceRate.value = storyVoiceRate.value;

            sentenceBank.SetActive(false);

        }
        Debug.Log(sceneState);
    }

    private IEnumerator WaitForTransition(float waitTime, string text)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        this.GetComponentInChildren<Text>().text = buttonText; // change button text after changing to storybuilder view

    }
}
