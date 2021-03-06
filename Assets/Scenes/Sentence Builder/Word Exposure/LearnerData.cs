﻿using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

// originally based on this tutorial https://developer.mongodb.com/how-to/sending-requesting-data-mongodb-unity-game/
namespace DatabaseEntry
{
    ///<summary>
    /// Class that contains relevant leaner datafields for the LearnerData collection
    ///</summary>
    public class LearnerData
    {

        // Database fields (serializable)
        public string _id; // mongo object id
        public string learnerId; // mongo object Id of the learner
        public string learnerName;
        public Dictionary<string, int> wordCounts = new Dictionary<string, int>();
        public Dictionary<string, string> sessionTimes = new Dictionary<string, string>();

        // static fields (not serializable)
        public static string static_id;
        public static string staticLearnerId; // mongo object Id of the learner
        public static string staticLearnerName;
        public static Dictionary<string, int> staticWordCounts = new Dictionary<string, int>();
        public static Dictionary<string, string> staticSessionTimes = new Dictionary<string, string>();

        ///<summary>
        /// convert LearnerData object to json string
        ///</summary>
        public string SerializeLearnerData()
        {
            return JsonConvert.SerializeObject(this);
        }
        ///<summary>
        /// convert json string to LearnerData object
        ///</summary>
        public LearnerData DeserializeLearnerData(string jsonLearnerData)
        {
            return JsonConvert.DeserializeObject<LearnerData>(jsonLearnerData);
        }
    }
}