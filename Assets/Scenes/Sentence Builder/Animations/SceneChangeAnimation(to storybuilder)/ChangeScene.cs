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
    //public GameObject openMenuButton;
    public GameObject updateWordBankButton;
    public GameObject sentenceBank;
    public GameObject currentLearnerInfoSentenceBuilder; // name and profile image of current learner in the sentence builder canvas
    private Vector3 newCameraPosition = new Vector3(0,0,-10); // initialized as default camera position
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

    // ideally this would be attached to the SavedSentenceBank script, but that game object starts disabled which means the start function isn't actually triggered until scene change (which is too late)
    void Start()
    {
        // get the learner's previously submitted sentences from the server and store them as local json files
        // potential race condition? immediately (faster than humanly possible?) after reaching sentence builder scene => press change scenes before coroutine completes => simultaneous write/read error as sentences are being loaded and stored at the same time?
        //StartCoroutine(ServerRequestHandler.GetSentences(SaveSentenceHandler.StoreSentences));
    }

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

            // move the options menu button/overlay over to storybuilder canvas
            // openMenuButton.transform.SetParent(storyBuilderCanvas);
            // openMenuButton.transform.GetComponent<RectTransform>().localPosition = new Vector3(-450,326,0); // roughly top left of the storybuilder
             //currentLearnerInfo.GetComponentInChildren<OptionsMenuHandler>().optionsPanel.transform.localPosition = new Vector3(0, -721, 0);
             //currentLearnerInfo.GetComponentInChildren<OptionsMenuHandler>().closeMenuButton.transform.localPosition = new Vector3(475,-375,0);
            // don't need to update word bank in storybuilder scene
            updateWordBankButton.SetActive(false);

            this.transform.SetParent(storyBuilderCanvas); // changescene button is moved over to the active canvas, so if/when we shut it down the previous canvas, it will remain
            this.transform.SetSiblingIndex(9);
            //this.transform.GetComponent<RectTransform>().localPosition = new Vector3(15, 315, 90); // move the changescene button down slightly so it doesn't overlap with the sentence tile
            //currentLearnerInfo.transform.GetComponent<RectTransform>().localPosition = new Vector3(-230f,-1060f,0f); // move learner profile to new scene
            currentLearnerInfoSentenceBuilder.SetActive(false); // disable the current learner info in the sentence builder scene / unused scene
            // enable/refresh the sentence bank

            // Change the pitch and rate in the story builder scene to match the pitch and rate in the sentence builder scene
            storyVoicePitch.value = sentenceVoicePitch.value;
            storyVoiceRate.value = sentenceVoiceRate.value;

            sentenceBank.SetActive(true);
            
        }
        else if (sceneState == SceneType.StoryBuilder) // storybuilder -> sentencebuilder
        {
            //StartCoroutine(ServerRequestHandler.GetSentences(SaveSentenceHandler.StoreSentences));
            //sentenceBuilderCanvas.gameObject.SetActive(true); // activate sentence builder
            LeanTween.moveY(mainCamera, 0, 2f); 
            sceneState = SceneType.SentenceBuilder;
            buttonText = "Build Story";
            StartCoroutine(WaitForTransition(transitionTime, buttonText));

            // move options menu stuff back
            // openMenuButton.transform.SetParent(sentenceBuilderCanvas);
            // openMenuButton.transform.SetSiblingIndex(0); // needs to be higher up in hierarchy so it doesn't go over top of the actual options menu popup
            // openMenuButton.transform.GetComponent<RectTransform>().localPosition = new Vector3(-450f,335f,0f);
            //    currentLearnerInfo.GetComponentInChildren<OptionsMenuHandler>().optionsPanel.transform.localPosition = new Vector3(0,0,0);
            //    currentLearnerInfo.GetComponentInChildren<OptionsMenuHandler>().closeMenuButton.transform.localPosition = new Vector3(483,351,0);
            // enable word bank updating for sentencebuilder scene
            updateWordBankButton.SetActive(true);

            this.transform.SetParent(sentenceBuilderCanvas);
            this.transform.SetSiblingIndex(11);
            //this.transform.GetComponent<RectTransform>().localPosition = new Vector3(0.298447847f,-352.858398f, 0);  // copy pasted position of change scene button in original render of scene
            // currentLearnerInfo.transform.GetComponent<RectTransform>().localPosition = new Vector3(-230f,-335f,0f);// move learner profile to new scene
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

        // if (sceneState == SceneType.StoryBuilder)
        // {
        //     //sentenceBuilderCanvas.gameObject.SetActive(false); // deactivate sentence builder
        // }
        // else
        // {
        //     //storyBuilderCanvas.gameObject.SetActive(false); // deactivate story builder
        // }

    }
}
