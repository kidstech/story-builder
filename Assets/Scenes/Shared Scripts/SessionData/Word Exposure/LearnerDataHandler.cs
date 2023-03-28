using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using DatabaseEntry;
using System;

// originally based on this tutorial https://developer.mongodb.com/how-to/sending-requesting-data-mongodb-unity-game/

public class LearnerDataHandler : MonoBehaviour
{

    public static string sessionDate;

    void Start()
    {
        sessionDate = DateTime.Now.ToString();
    
        //store name and object id in static LearnerData fields
        LearnerData.staticLearnerName = LearnerLogin.staticLearner.name;
        LearnerData.staticLearnerId = LearnerLogin.staticLearner._id;
        if (LearnerData.staticWordCounts == null) LearnerData.staticWordCounts = new Dictionary<string, int>();
        if (LearnerData.staticSessionTimes == null) LearnerData.staticSessionTimes = new Dictionary<string, string>();
        // add the start time of the new session
        LearnerData.staticSessionTimes.Add(sessionDate, "");
    }
    void OnApplicationQuit()
    {
        // when we the learner quits the game, store the time they ran the application for in the DateTime logged at the start of the session
        LearnerData.staticSessionTimes[sessionDate] = FormatSeconds();
        // update local logs
        StoreLearnerData();
        // send logs to server using non-coroutine method so it can actually finish
        ServerRequestHandler.BlockingPostLearnerDataToServer();
        // give the program time to talk to the server before closing
        Debug.Log("Quitting Storybuilder...");
    }

    ///<summary>
    /// returns a string formatted to display the time returned from Time.time in hour/min/sec format
    ///</summary>
    public static string FormatSeconds()
    {
        string formattedTime = null;
        if (Time.time < TimeSpan.MaxValue.TotalSeconds)
        {
        TimeSpan time = TimeSpan.FromSeconds(Time.time);
        formattedTime = time.ToString(@"hh\:mm\:ss");
        }
        return formattedTime;
    }

    public static void StoreLearnerData()
    {
        // create learnerdata object for serialization later
        LearnerData learnerData = new LearnerData();
    
        // populate non-static serializable fields
        learnerData.learnerName = LearnerData.staticLearnerName;
        learnerData.learnerId = LearnerData.staticLearnerId;
        learnerData.wordCounts = LearnerData.staticWordCounts;
        learnerData.sessionTimes = LearnerData.staticSessionTimes;
    }

    // ///<summary>
    // /// Returns a dictionary of type string, int. Assumes that a learner has already been selected
    // ///</summary>
    // public static Dictionary<string, int> LoadUserData()
    // {
    //     return JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(filePath)); // convert a JSON string into Dictionary form
    // }

    public static void UpdateWordCount(string word)
    {
        // if the word isn't in the dictionary... (haven't heard it yet)
        if (!LearnerData.staticWordCounts.ContainsKey(word))
        {
            LearnerData.staticWordCounts.Add(word, 1); // create entry for newly heard word
        }
        else // we've heard the word again
        {
            LearnerData.staticWordCounts[word]++; // increment word counter
        }
    }
}
