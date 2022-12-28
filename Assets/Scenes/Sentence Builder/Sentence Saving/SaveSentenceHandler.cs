using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class SaveSentenceHandler
{
    public static string path = Path.Combine(Application.persistentDataPath, "Resources", "Sentences");
    // static reference to the most recent saved sentence for posting the sentence to the server
    public static SavedSentence mostRecentSentence;
    // static reference to the list of words that make up the most recent sentence
    // this is needed to preserve the contextPackId of the submitted
    public static List<Word> words;
    public static List<SavedSentence> sentences;

    public static void SaveSentence(List<WordTile> tiles)
    {
        string sentenceText = "";
        words = new List<Word>();
        List<string> selectedWords = new List<string>();
        foreach (WordTile tile in tiles)
        {
            // only add the word if we don't already have it
            if (!words.Contains(tile.word))
            {
                words.Add(tile.word);
            }
            selectedWords.Add(tile.textToDisplay);
            sentenceText = sentenceText + tile.textToDisplay + " ";
        }
        // trim off any extra white space at the end
        sentenceText = sentenceText.TrimEnd();
        // Make sure the path where we are saving exists (if directory doesn't exist, create it).
        CheckPath();

        // Create a GUID for each sentence, just as identifier.
        Guid id = Guid.NewGuid();

        // create new SavedSentence object
        SavedSentence sentence = new SavedSentence(id.ToString(), sentenceText, DateTime.Now.ToString(), LearnerLogin.staticLearner._id, words, selectedWords, LearnerSelectPopup.currentUser._id);
        //Debug.Log("saving sentence: ... at " + Path.Combine(path, id.ToString() + ".json"));
        // store the sentence in static variable for server call (can't call coroutine from static method)
        mostRecentSentence = sentence;
        string jsonSentence = JsonConvert.SerializeObject(sentence);
        // Write the object to the file, this will allow us to load the SavedSentence object back into Unity
        //File.WriteAllText(Path.Combine(path, id.ToString() + ".json"), jsonSentence);
    }

    public static void StoreSentences(List<SavedSentence> sentences2)
    {
        sentences = sentences2;
        // Debug.Log("Am i being called");
        // foreach (SavedSentence sentence in sentences)
        // {
        //     string jsonSentence = JsonConvert.SerializeObject(sentence);
        //     // make sure we don't overwrite any existing files
        //     if (!File.Exists(Path.Combine(path, sentence.sentenceId)))
        //     {
        //         Debug.Log("I am writing a sentence");
        //         // Write the object to the file, this will allow us to load the SavedSentence object back into Unity
        //         //Refine query so that we only store the sentences that the learner had from the time they last saved
        //         File.WriteAllText(Path.Combine(path, sentence.sentenceId.ToString() + ".json"), jsonSentence);
        //     }
        // }
    }

    public static List<SavedSentence> returnSentences() {
        return sentences;
    }

    // currently unused
    public static List<SavedSentence> LoadJson()
    {
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
            // Return our list
            return loadedList;
        }
    }

    public static void CheckPath()
    {
        if (!Directory.Exists(path))
        {
            Debug.Log(path + " directory has been created");
            Directory.CreateDirectory(path);
        }
        Debug.Log("directory already exists " + path);
    }
}
