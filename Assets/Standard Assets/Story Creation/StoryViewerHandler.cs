using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryViewerHandler : MonoBehaviour
{
    //
    public Story[] stories;

    //
    public GameObject storyPrefab;
    public GameObject bigStoryPrefab;

    //
    private Transform storyHolder;

    private void Awake()
    {
        //
        storyHolder = transform.Find("Stories");
    }

    //
    private void Start()
    {
        //
        stories = SaveStorySystem.LoadStories();

        //
        BuildStories();
    }

    //
    public void BuildStories()
    {
        //
        for (int i = 0; i < stories.Length; i++)
        {
            //
            GameObject newStory = Instantiate(storyPrefab);

            //
            newStory.transform.SetParent(storyHolder);

            //
            newStory.GetComponentInChildren<Text>().text = stories[i].name;

            //
            newStory.GetComponent<StoryObject>().story = stories[i];
        }
    }
}
