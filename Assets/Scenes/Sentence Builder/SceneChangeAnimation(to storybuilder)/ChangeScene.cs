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
    private Vector3 newCameraPosition = new Vector3(0,0,-10); // initialized as default camera position
    private float sceneHeight = 0;
    private float transitionTime = 2f; // used to keep the wait function in line with animation time easily
    private string buttonText;
    private enum SceneType{
        SentenceBuilder,
        StoryBuilder,
    }
    private SceneType sceneState = SceneType.SentenceBuilder;

    public void ToggleScene()
    {
        if (sceneState == SceneType.SentenceBuilder) // sentencebuilder -> storybuilder
        {
            sceneHeight = storyBuilderCanvas.transform.GetComponent<RectTransform>().rect.height * .72f; // .72 multiplier is arbitrary. It just ended up being a proportion that kept most of the submitted sentence tile in view alongside the storybuilder view.
            storyBuilderCanvas.gameObject.SetActive(true); // activate story builder
            sentenceBuilderCanvas.gameObject.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace; // change sceen to worldspace so we can move the camera
            sentenceBuilderCanvas.gameObject.GetComponent<RectTransform>().position = new Vector3(0, 0, 90);
            sentenceBuilderCanvas.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1); // the scale kept changing upon scene change, so I've set it back to normal to counter it
            LeanTween.moveY(mainCamera, -sceneHeight, transitionTime);
            sceneState = SceneType.StoryBuilder;
            buttonText = "Build Sentence";
            StartCoroutine(WaitForTransition(transitionTime, buttonText));
            this.transform.SetParent(storyBuilderCanvas); // changescene button is moved over to the active canvas, so if/when we shut it down the previous canvas, it will remain
            this.transform.GetComponent<RectTransform>().localPosition = new Vector3(15, 315, 90); // move the changescene button down slightly so it doesn't overlap with the sentence tile
            


        }
        else if (sceneState == SceneType.StoryBuilder) // storybuilder -> sentencebuilder
        {
            sentenceBuilderCanvas.gameObject.SetActive(true); // activate sentence builder
            storyBuilderCanvas.gameObject.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace; // change sceen to worldspace so we can move the camera
            storyBuilderCanvas.gameObject.GetComponent<RectTransform>().position = new Vector3(0, -800, 90);
            storyBuilderCanvas.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1); // the scale kept changing upon scene change, so I've set it back to normal to counter it
            LeanTween.moveY(mainCamera, 0, 2f); 
            sceneState = SceneType.SentenceBuilder;
            buttonText = "Build Story";
            StartCoroutine(WaitForTransition(transitionTime, buttonText));
            this.transform.SetParent(sentenceBuilderCanvas);
            this.transform.GetComponent<RectTransform>().localPosition = new Vector3(0.298447847f,-352.858398f, 0);  // copy pasted position of change scene button in original render of scene
        }
    }

    private IEnumerator WaitForTransition(float waitTime, string text)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        this.GetComponentInChildren<Text>().text = buttonText; // change button text after changing to storybuilder view

        if (sceneState == SceneType.StoryBuilder)
        {
            //sentenceBuilderCanvas.gameObject.SetActive(false); // deactivate sentence builder
            storyBuilderCanvas.gameObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        }
        else
        {
            //storyBuilderCanvas.gameObject.SetActive(false); // deactivate story builder
            sentenceBuilderCanvas.gameObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        }

    }
}
