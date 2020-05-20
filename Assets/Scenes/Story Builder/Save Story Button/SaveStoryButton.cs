using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveStoryButton : MonoBehaviour
{
    [Header("Page Icon Containers")]
    public PageContainer pageContainer;
    private Button button;

    //
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SaveStory);
    }

    private void SaveStory()
    {
        List<SavedSentence> toSave = pageContainer.GetAllSentencesInPages();

        SaveStoryHandler.SaveStory(toSave);

        for(int i = 0; i < toSave.Count; i++)
        {
            string blah = "";

            for (int o = 0; o < toSave[i].words.Count; o++)
            {
                

                blah += toSave[i].words[o].word;
            }

            Debug.Log(blah);
        }
    }
}
