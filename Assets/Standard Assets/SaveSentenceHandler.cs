using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveSentenceHandler : MonoBehaviour
{
    public static string path = Application.dataPath + "/saves/savedSentences.json";

    public static void SaveJson(string Sentence)
    {
        // Construct the current list of saved sentences
        List<SavedSentence> saveData = new List<SavedSentence>();

        // Populate the list 
        saveData = LoadJson();

        // Create a new instance of a saved sentence
        SavedSentence newSentence = new SavedSentence();

        // Add in the username and the sentence
        newSentence.sentence = Sentence;
        newSentence.user = "Test";

        // Insert the new sentence into the 'master' list
        saveData.Add(newSentence);

        // Convert our json into raw text
        string jsonToSave = JsonHelper.ToJson(saveData.ToArray());

        // Open up the file for writing
        StreamWriter writer = new StreamWriter(path, false);

        // Actually do the writing
        writer.WriteLine(jsonToSave);

        // Close the file
        writer.Close();
    }

    public static List<SavedSentence> LoadJson()
    {
        // Read in the current save file
        string jsonToLoad = File.ReadAllText(path);

        // Convert to List
        if(jsonToLoad == string.Empty || jsonToLoad == "" || jsonToLoad == null)
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
}
