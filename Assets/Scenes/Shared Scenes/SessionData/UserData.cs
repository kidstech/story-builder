using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

// based heavily off of this tutorial https://developer.mongodb.com/how-to/sending-requesting-data-mongodb-unity-game/
// tldr => public fields are database fields
namespace DatabaseEntry
{
    public class UserData
    {

        // Database fields
        public static string userName;
        public static Dictionary<string, int> wordCounts = new Dictionary<string, int>();
        public static string dirPath = Path.Combine(Application.dataPath + "/", "Saves/", "UserData/");
        public static string filePath = Path.Combine(Application.dataPath + "/", "Saves/", "UserData/");


        // UserData Methods
        public static string Stringify(string user) // very much WIP https://stackoverflow.com/a/43036691
        {
            string jsonWordCounts;
            UserData userData = new UserData();
            Debug.Log("Username: " + user);
            // if there isn't already a filepath made for this user...
            if (!File.Exists(filePath))
            {
                resetFilePath();
                userName = user; // update userName
                filePath = Path.Combine(filePath, user + ".json");
                // and clear the dictionary
                wordCounts.Clear();
                jsonWordCounts = null;
                
            }
            // otherwise if we are logging into another user from a previous one
            else if (!string.Equals(userName, user)) // userName would be previous user until overwritten
            {
                resetFilePath();
                // clear dictionary for new user
                wordCounts.Clear();
                // switch to new user
                userName = user;
                // update the file path
                filePath = Path.Combine(filePath, user + ".json");
                // if they have info already...
                if (File.Exists(filePath))
                {
                    // load up their info
                    wordCounts = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(filePath));
                }
                
            }
                // convert the dictionary to Json for file creation
                jsonWordCounts = JsonConvert.SerializeObject(wordCounts, Formatting.Indented); 

            userData.CreateJsonFile(jsonWordCounts, user);// testing file output
            return jsonWordCounts;
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
            // foreach (var entry in wordCounts)
            // {
            //     Debug.Log($"Word {entry.Key}: TimesWordHeard={entry.Value}"); // print all key value pairs to console
            // }
            // remove above in production
        }
        public void CreateJsonFile(string jsonUserData, string user)
        {
            CheckDirPath();
            File.WriteAllText(filePath, jsonUserData);
        }

        public static void CheckDirPath()
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            else return;
        }
        
        // not yet integrated
        public bool FileExistsIsTrue(string user)
        {
            if (File.Exists(filePath + user + ".json"))
            {
                return true;
            }
            else return false;
        }

        public static void resetFilePath()
        {
            filePath = Path.Combine(Application.dataPath + "/", "Saves/", "UserData/");
        }

    }
}