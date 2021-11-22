using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveStoryHandler
{
    //
    public static string path = Path.Combine(Application.dataPath, "Saves", "Stories");

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
}
