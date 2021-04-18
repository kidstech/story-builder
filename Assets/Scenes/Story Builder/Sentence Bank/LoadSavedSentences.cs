using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class LoadSavedSentences
{
    public static string path = Path.Combine(Application.dataPath, "Saves", "Sentences");

    public static List<SavedSentence> LoadSentences()
    {
        // Create a DirectoryInfo of the directory of the files to enumerate.
        DirectoryInfo DirInfo = new DirectoryInfo(@path);
        // DateTime StartDate = new DateTime(2021, 02, 20);
        DateTime today = DateTime.Today;

        // LINQ query for all files created past a certain date.
        var files = from f in DirInfo.EnumerateFiles()
           // where f.CreationTimeUtc > StartDate
           where f.CreationTimeUtc > today
           where f.Name.EndsWith(".sen")
           orderby f.CreationTimeUtc descending
           select f;
           
        // Show results.
        List<SavedSentence> sentencesToReturn = new List<SavedSentence>();

        foreach (var file in files) {
            FileStream filestream = File.Open(Path.Combine(@path, file.Name), FileMode.Open);
            try {
                BinaryFormatter bf = new BinaryFormatter();
                SavedSentence thisSentence = (SavedSentence)bf.Deserialize(filestream);
                sentencesToReturn.Add(thisSentence);
            }
            catch(SerializationException e) {
                Debug.LogError("Cannot deserialize. " + e);
            }
            finally {
                // Clean up
                filestream.Close();
            }
        }
        
        return sentencesToReturn;
    }

    
}
