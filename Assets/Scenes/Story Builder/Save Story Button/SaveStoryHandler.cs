using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using ServerTypes;
using UnityEngine.UI;


public class SaveStoryHandler : MonoBehaviour
{
    public SavedSentenceBank sentenceBank;
    public GameObject storyNameInputField;
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
    public void PutStoryInDatabase()
    {
        List<string> storySentences = sentenceBank.getSentencesInBank();
        Story story = new Story(storySentences); // story with no name or font
        story.learnerId = LearnerLogin.staticLearner._id; // should make another constructor if these get made more often
        if (storyNameInputField.GetComponent<Text>().text != "") {
            story.storyName = storyNameInputField.GetComponent<Text>().text;
        }
        else {
            story.storyName = storySentences[0];
        }
        StartCoroutine(ServerRequestHandler.PostStory(story));
    }
}
