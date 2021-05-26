using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

// based heavily off of this tutorial https://developer.mongodb.com/how-to/sending-requesting-data-mongodb-unity-game/
// tldr => public fields are database fields
namespace DatabaseEntry
{
    public class UserData
    {

        // Database fields
        public string userName = "Test";
        public static Dictionary<string, int> wordCounts = new Dictionary<string, int>();


        // UserData Methods
        public static string Stringify() // very much WIP
        {
            UserData userData = new UserData();
            Debug.Log(JsonConvert.SerializeObject(wordCounts, Formatting.Indented));
            return JsonConvert.SerializeObject(wordCounts, Formatting.Indented);
        }

        public static UserData Parse(string json) // WIP
        {
            return JsonUtility.FromJson<UserData>(json); // convert a JSON string into UserData form
        }
        public static void UpdateWordCount(string word)
        {
            // if the word isn't in the dictionary... (haven't heard it yet)
            if (!wordCounts.ContainsKey(word))
            {
                wordCounts.Add(word, 1); // create entry for newly heard word
            }
            else // we've heard the word again
            {
                wordCounts[word]++; // increment word counter
            }

            // remove below in production
            foreach (var entry in wordCounts)
            {
                Debug.Log($"Word {entry.Key}: TimesWordHeard={entry.Value}"); // print all key value pairs to console
            }
            // remove above in production
        }
    }
}