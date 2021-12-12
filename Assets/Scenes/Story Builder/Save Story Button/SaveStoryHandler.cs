using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using ServerTypes;


public class SaveStoryHandler : MonoBehaviour
{
    // PageContainer game object that contains all the different pages a user has created for their story
    public GameObject PageContainer;
    public static string path;
    //

    public void Start()
    {
        path = Path.Combine(Application.dataPath, "Saves", "Stories");
    }
    

    //
    public static void SaveStory(ArrayList pages, string storyName)
    {
        
    }
    
    
    
    
    // public static void SaveStory(List<SavedSentence> sentences)
    // {
    //     // Make sure the path where we are saving exists (if directory doesn't exist, create it).
    //     CheckPath();

    //     // Create a GUID for each sentence, just as identifier.
    //     Guid id = Guid.NewGuid();

    //     // Create a new SavedSentence object
    //     SavedStory s = new SavedStory(id, "Test Client", sentences);

    //     // Fancy way to save in an unformated manner
    //     BinaryFormatter bf = new BinaryFormatter();

    //     // Create a new file at the desired location with the UUID being the name and the extension .story
    //     FileStream file = File.Create(Path.Combine(path, id.ToString() + ".story"));

    //     // Write the object to the file, this will allow us to load the SavedStory object back into Unity
    //     bf.Serialize(file, s);

    //     // Clean up
    //     file.Close();
    // }

    public static List<SavedStory> LoadJson()
    {
        //
        CheckPath();

        // Read in the current save file
        string jsonToLoad = File.ReadAllText(path);

        // Convert to List
        if (jsonToLoad == string.Empty || jsonToLoad == "" || jsonToLoad == null)
        {
            return new List<SavedStory>();
        }
        else
        {
            // Load it as an array using JsonHelper
            SavedStory[] tempLoadSaves = JsonHelper.FromJson<SavedStory>(jsonToLoad);

            // Convert to List
            List<SavedStory> loadedList = tempLoadSaves.OfType<SavedStory>().ToList();

            // Return our list
            return loadedList;
        }
    }

    //
    private static void CheckPath()
    {
        //
        if (!Directory.Exists(path))
        {
            //
            Directory.CreateDirectory(path);
        }
    }


    // save story to mongo database
    public void PutStoryInDatabase(string storyName)
    {
        List<StoryPage> storyPages = PageContainer.GetComponent<PageContainer>().GetPagesForStorySubmission();
        Story story = new Story(storyPages); // story with no name or font
        story.learnerId = LearnerLogin.staticLearner._id; // should make another constructor if these get made more often
        story.storyName = storyName;
        StartCoroutine(ServerRequestHandler.PostStory(story));
    }
}
