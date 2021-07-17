using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public static class LoadSavedSentences
{
    private static SavedSentence savedSentence;
    public static List<SavedSentence> LoadSentences()
    {
        // make sure we actually have a directory made
        SaveSentenceHandler.CheckPath();
        // grab sentences from said directory
        string[] savedSentences = Directory.GetFiles(Path.Combine(Application.persistentDataPath, "Resources", "Sentences"), "*.json");
        List<SavedSentence> sentencesToReturn = new List<SavedSentence>();
        // deserialize all our json sentence files into SavedSentence objects
        for(int i = 0; i < savedSentences.Length; i++)
        {
                savedSentence = JsonConvert.DeserializeObject<SavedSentence>(File.ReadAllText(savedSentences[i]));
                sentencesToReturn.Add(savedSentence);
        }
        return sentencesToReturn;
    }

    
}
