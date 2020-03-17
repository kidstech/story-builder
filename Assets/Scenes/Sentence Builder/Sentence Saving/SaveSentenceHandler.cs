using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSentenceHandler
{
    //
    public static string path = Path.Combine(Application.dataPath, "Saves", "Sentences");

    //
    public static void SaveSentence(List<Word> words)
    {
        // Make sure the path where we are saving exists (if directory doesn't exist, create it).
        CheckPath();

        // Create a GUID for each sentence, just as identifier.
        Guid id = Guid.NewGuid();

        // Create a new SavedSentence object
        SavedSentence s = new SavedSentence(id, "Test Client", words);

        // Fancy way to save in an unformated manner
        BinaryFormatter bf = new BinaryFormatter();

        // Create a new file at the desired location with the UUID being the name and the extension .sen
        FileStream file = File.Create(Path.Combine(path, id.ToString() + ".sen"));

        // Write the object to the file, this will allow us to load the SavedSentence object back into Unity
        bf.Serialize(file, s);

        // Clean up
        file.Close();
    }

    public static List<SavedSentence> LoadJson()
    {
        //
        CheckPath();

        // Read in the current save file
        string jsonToLoad = File.ReadAllText(path);

        // Convert to List
        if (jsonToLoad == string.Empty || jsonToLoad == "" || jsonToLoad == null)
        {
            return new List<SavedSentence>();
        }
        else
        {
            // Load it as an array using JsonHelper
            SavedSentence[] tempLoadSaves = JsonHelper.FromJson<SavedSentence>(jsonToLoad);

            // Convert to List
            List<SavedSentence> loadedList = tempLoadSaves.OfType<SavedSentence>().ToList();

            // TODO make loading new saved sentences appear at the top of the list

            // Return our list
            return loadedList;
        }
    }

    //
    private static void CheckPath()
    {
        //
        if(!Directory.Exists(path))
        {
            //
            Directory.CreateDirectory(path);
        }
    }
}
