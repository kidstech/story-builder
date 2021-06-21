using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using DatabaseEntry;
using System;

// originally based on this tutorial https://developer.mongodb.com/how-to/sending-requesting-data-mongodb-unity-game/

public class WordCountHandler : MonoBehaviour
{
    public static string dirPath;
    public static string filePath;
    public string sessionDate;

    void Start()
    {
        sessionDate = DateTime.Now.ToString();
        dirPath = Path.Combine(Application.dataPath + "/", "Saves/", "LearnerData/");
        filePath = Path.Combine(Application.dataPath + "/", "Saves/", "LearnerData/");
        // store name and object id in static LearnerData fields
        LearnerData.staticLearnerName = LearnerLogin.staticLearner.name;
        LearnerData.staticLearnerId = LearnerLogin.staticLearner._id;
        // add the start time of the new session
        LearnerData.staticSessionTimes.Add(sessionDate, "");
    }
    void OnApplicationQuit()
    {
        // when we the learner quits the game, store the time they ran the application for in the DateTime logged at the start of the session
        LearnerData.staticSessionTimes[sessionDate] = FormatSeconds();
        // update local logs
        WordCountHandler.StoreLearnerData();
        // send logs to server using non-coroutine method so it can actually finish
        ServerRequestHandler.BlockingPostLearnerDataToServer();
        // give the program time to talk to the server before closing
        Debug.Log("Quitting Storybuilder...");
    }
    ///<summary>
    /// returns a string formatted to display the time returned from Time.time in hour/min/sec format
    ///</summary>
    private string FormatSeconds()
    {
        string formattedTime = null;
        if (Time.time < TimeSpan.MaxValue.TotalSeconds)
        {
        TimeSpan time = TimeSpan.FromSeconds(Time.time);
        formattedTime = time.ToString(@"hh\:mm\:ss");
        }
        return formattedTime;
    }

    // definitely need to refactor this at some point...
    public static void StoreLearnerData()
    {
        // create learnerdata object for serialization later
        LearnerData learnerData = new LearnerData();
        string jsonLearnerData;
        if (LearnerData.staticWordCounts == null) LearnerData.staticWordCounts = new Dictionary<string, int>();
        if (LearnerData.staticSessionTimes == null) LearnerData.staticSessionTimes = new Dictionary<string, string>();
        // if there isn't already a filepath made for this learner...
        if (!FileExists())
        {
            ResetFilePath();
            filePath = Path.Combine(filePath, LearnerData.staticLearnerName + ".json"); // should this be object ID so we don't have to worry so much about file name syntax?
            jsonLearnerData = null;
        }
        // populate non-static serializable fields
        learnerData.learnerName = LearnerData.staticLearnerName;
        learnerData.learnerId = LearnerData.staticLearnerId;
        learnerData.wordCounts = LearnerData.staticWordCounts;
        learnerData.sessionTimes = LearnerData.staticSessionTimes;
        // convert Learnerdata to json
        jsonLearnerData = JsonConvert.SerializeObject(learnerData, Formatting.Indented);
        // make the file
        CreateJsonFile(jsonLearnerData);
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
            //Debug.Log("new word heard!");
            LearnerData.staticWordCounts.Add(word, 1); // create entry for newly heard word
        }
        else // we've heard the word again
        {
           //Debug.Log("repeat word heard.");
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
