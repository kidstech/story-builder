using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using DatabaseEntry;

// originally based on this tutorial https://developer.mongodb.com/how-to/sending-requesting-data-mongodb-unity-game/

public class WordCountHandler : MonoBehaviour
{
    public static string dirPath;
    public static string filePath;

    void Start()
    {
        dirPath = Path.Combine(Application.dataPath + "/", "Saves/", "LearnerData/");
        filePath = Path.Combine(Application.dataPath + "/", "Saves/", "LearnerData/");
        // store name and object ic in static LearnerData fields
        LearnerData.staticLearnerName = LearnerLogin.staticLearner.name;
        LearnerData.staticLearnerObjectId = LearnerLogin.staticLearner._id;
        StoreLearnerData();

    }

    // definitely need to refactor this at some point...
    public static void StoreLearnerData()
    {
        // create learnerdata object for serialization later
        LearnerData learnerData = new LearnerData();
        string jsonWordCounts;
        // Remove this Debug in production
        Debug.Log("LearnerName: " + LearnerData.staticLearnerName);
        // if there isn't already a filepath made for this learner...
        if (!FileExists())
        {
            ResetFilePath();
            filePath = Path.Combine(filePath, LearnerData.staticLearnerName + ".json"); // should this be object ID so we don't have to worry so much about file name syntax?
            // and clear the dictionary
            LearnerData.staticWordCounts.Clear();
            jsonWordCounts = null;
        }
        // populate non-static serializable fields
        learnerData.learnerName = LearnerData.staticLearnerName;
        learnerData.learnerObjectId = LearnerData.staticLearnerObjectId;
        learnerData.wordCounts = LearnerData.staticWordCounts;
        // convert Learnerdata to json
        jsonWordCounts = JsonConvert.SerializeObject(learnerData, Formatting.Indented);
        // make the file
        CreateJsonFile(jsonWordCounts);
    }

    ///<summary>
    /// Returns a dictionary of type string, int. Assumes that a learner has already been selected
    ///</summary>
    public static Dictionary<string, int> LoadUserData()
    {
        return JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(filePath)); // convert a JSON string into Dictionary form
    }

    public static void UpdateWordCount(string word)
    {
        // if the word isn't in the dictionary... (haven't heard it yet)
        if (!LearnerData.staticWordCounts.ContainsKey(word))
        {
            Debug.Log("new word heard!");
            LearnerData.staticWordCounts.Add(word, 1); // create entry for newly heard word
        }
        else // we've heard the word again
        {
            Debug.Log("repeat word heard.");
            LearnerData.staticWordCounts[word]++; // increment word counter
        }
    }
    public static void CreateJsonFile(string jsonLearnerData)
    {
        CheckDirPath();
        Debug.Log("writing learnerdata to a local file...");
        File.WriteAllText(filePath, jsonLearnerData);
    }

    public static void CheckDirPath()
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        else return;
    }

    ///<summary> 
    /// Simple clarifying helper function that returns true if a file already exists for an inputted learner
    ///</summary>
    public static bool FileExists()
    {
        if (File.Exists(filePath))
        {
            return true;
        }
        else return false;
    }

    ///<summary>
    /// resets the static filePath variable to its original instatiation. (.../Saves/LearnerData/)
    ///</summary>
    public static void ResetFilePath()// please update function summary if original filePath is changed
    {
        filePath = Path.Combine(Application.dataPath + "/", "Saves/", "LearnerData/");
    }

}
