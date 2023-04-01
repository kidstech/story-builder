using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using Newtonsoft.Json;

public static class LoadSavedSentences
{
    public static string path = Path.Combine(Application.persistentDataPath, "Resources", "Sentences");

    public static List<SavedSentence> LoadSentences()
    {
        // Create a DirectoryInfo of the directory of the files to enumerate.
        DirectoryInfo DirInfo = new DirectoryInfo(@path);
        DateTime today = DateTime.Today;

        // LINQ query for all files created past a certain date.
        var files = from f in DirInfo.EnumerateFiles()
                        // where f.CreationTimeUtc > StartDate
                    where f.CreationTimeUtc > today
                    where f.Name.EndsWith(".json")
                    orderby f.CreationTimeUtc descending
                    select f;

        // Show results.
        List<SavedSentence> sentencesToReturn = new List<SavedSentence>();

        foreach (var file in files)
        {
            string filePath = file.FullName;
            string jsonFile = File.ReadAllText(filePath);
            SavedSentence thisSentence = JsonConvert.DeserializeObject<SavedSentence>(jsonFile);
            // only show sentences that the current learner made
            if (thisSentence.learnerId == LearnerLogin.staticLearner._id)
            {
                sentencesToReturn.Add(thisSentence);
            }
        }

        return sentencesToReturn;
    }


}
