using System.Collections.Generic;
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
        public Dictionary<string, float> sessionTimes = new Dictionary<string, float>();

        // static fields (not serializable)
        public static string static_id;
        public static string staticLearnerId; // mongo object Id of the learner
        public static string staticLearnerName;
        public static Dictionary<string, int> staticWordCounts = new Dictionary<string, int>();
        public static Dictionary<string, float> staticSessionTimes = new Dictionary<string, float>();

        ///<summary>
        /// convert LearnerData object to json string
        ///</summary>
        public string serializeLearnerData()
        {
            return JsonConvert.SerializeObject(this);
        }
        ///<summary>
        /// convert json string to LearnerData object
        ///</summary>
        public LearnerData deserializeLearnerData(string jsonLearnerData)
        {
            return JsonConvert.DeserializeObject<LearnerData>(jsonLearnerData);
        }
    }
}