using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveStoryButton : MonoBehaviour
{
    private Button button;
    public GameObject saveStoryHandler;
    public GameObject StoryNamePrompt;
    public GameObject StorySubmissionStatus;

    private void Start()
    {
        button = GetComponent<Button>();
        //button.onClick.AddListener(SaveStory);
    }

    public void OpenStoryNameMenu()
    {
        StoryNamePrompt.SetActive(true);
    }
    public void CloseStoryNameMenu()
    {
        string storyName = StoryNamePrompt.GetComponentInChildren<Text>().text;
        StoryNamePrompt.SetActive(false);
        saveStoryHandler.GetComponent<SaveStoryHandler>().PutStoryInDatabase(storyName);
        // if story submitted successfully... (should check this eventually)
        DisplaySubmissionStatus();
    }
    private void DisplaySubmissionStatus()
    {
        // might be nice to have the success message contain the name of the submitted story
        StorySubmissionStatus.SetActive(true);
    }
    public void CloseSubmissionStatus()
    {
        StorySubmissionStatus.SetActive(false);
    }

}
