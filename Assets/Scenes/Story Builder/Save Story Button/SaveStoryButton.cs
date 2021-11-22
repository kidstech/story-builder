using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveStoryButton : MonoBehaviour
{
    [Header("Page Icon Containers")]
    public PageContainer pageContainer;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        //button.onClick.AddListener(SaveStory);
    }

    // no real story saving implemented just yet
    // private void SaveStory()
    // {
    //     List<StoryPage> toSave = pageContainer.GetAllSentencesInPages();

    //     SaveStoryHandler.SaveStory(toSave);

    //     // for(int i = 0; i < toSave.Count; i++)
    //     // {
    //     //     string blah = "";

    //     //     for (int o = 0; o < toSave[i].wordTiles.Count; o++)
    //     //     {
                

    //     //         blah += toSave[i].wordTiles[o].textToDisplay;
    //     //     }

    //     //     Debug.Log(blah);
    //     // }
    // }
}
