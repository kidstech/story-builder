using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryObject : MonoBehaviour
{
    //
    public Story story;

    //
    private GameObject bigStory;

    //
    public void DisplayStory()
    {
        //
        bigStory = Instantiate(transform.parent.parent.GetComponent<StoryViewerHandler>().bigStoryPrefab);

        //
        bigStory.transform.SetParent(transform.parent.parent, false);

        //
        bigStory.GetComponent<StoryReader>().StartBuildingStory(story);

    }

    public void CloseStory()
    {
        GameObject smallStory = GameObject.Find("BigStoryPrefab(Clone)");

        if(smallStory != null)
        {
            Destroy(smallStory.gameObject);
        }
    }
}
