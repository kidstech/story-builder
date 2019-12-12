using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveStoryHandler : MonoBehaviour
{
    // Name of the character slot save file (Append with 0, 1, 2, etc...)
    private static string storyDataPath;

    //
    private static BinaryFormatter formatter = new BinaryFormatter();

    //
    private void Awake()
    {
        //
        storyDataPath = Path.Combine(Application.persistentDataPath, "Stories");
    }

    //
    public void StoryCompiler()
    {
        //
        string storyName = transform.Find("SaveStoryButton").Find("StoryInputField").Find("Text").GetComponent<Text>().text;

        //
        string storyContent = "";

        //
        if(storyName.Length > 0 && storyName != null && storyName != string.Empty)
        {
            // Get all the other pages, excluding the last page
            for (int i = 0; i < (transform.parent.childCount - 1); i++)
            {
                //
                string storyFragment = "";

                //
                for(int o = 0; o < transform.parent.GetChild(i).Find("SentenceSlotScrollviewPrefab").Find("SentenceSlot").childCount; o++)
                {
                    //
                    storyFragment += transform.parent.GetChild(i).Find("SentenceSlotScrollviewPrefab").Find("SentenceSlot").GetChild(o).Find("Text").GetComponent<Text>().text;
                }

                //
                storyContent += storyFragment + ".";
            }

            //
            storyContent = CleanStory(storyContent);

            //
            Story story = new Story(storyName, storyContent, null);

            //
            SaveStorySystem.SaveStoryData(story);
        }
        else
        {
            Debug.Log("Enter a valid name.");
        }
    }

    private string CleanStory(string content)
    {
        //
        string cleanedStory = "";

        //
        string[] fragments = content.Split(' ');

        for(int i = 0; i < fragments.Length; i++)
        {
            string fragment = fragments[i];

            if (fragment == string.Empty || fragment.Equals(' '))
            {
                // Do nothing
            }
            else
            {
                //
                if(fragment == ".")
                {
                    if(i == fragments.Length - 1)
                    {
                        cleanedStory = cleanedStory.Substring(0, cleanedStory.Length - 1) + ".";
                    }
                    else
                    {
                        cleanedStory = cleanedStory.Substring(0, cleanedStory.Length - 1) + ". ";
                    }
                }
                else
                {
                    cleanedStory += fragment + " ";
                }
            } 
        }

        //
        return cleanedStory;
    }
}
