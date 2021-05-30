using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

// originally based on this tutorial https://developer.mongodb.com/how-to/sending-requesting-data-mongodb-unity-game/
namespace DatabaseEntry
{
    public class UserData
    {

        // Database fields
        public string username;
        public Dictionary<string, int> wordCounts = new Dictionary<string, int>();

        // static fields
        public static string staticUsername;
        public static Dictionary<string, int> staticWordCounts = new Dictionary<string, int>();
        // there are two variables with the same path because the filePath will be changed around as users log in and out
        // dirPath exists to check that the directory still exists even if filePath changes
        private static string dirPath = Path.Combine(Application.dataPath + "/", "Saves/", "UserData/");
        private static string filePath = Path.Combine(Application.dataPath + "/", "Saves/", "UserData/");


        // UserData Methods
        public static void StoreUserData(string user)
        {
            string jsonUserData;
            UserData userData = new UserData();
            // Remove this Debug in production
            Debug.Log("Current user: " + user);
            // if there isn't already a filepath made for this user...
            if (!FileExistsIsTrue())
            {
                resetFilePath();
                staticUsername = user; // update userName
                filePath = Path.Combine(filePath, user + ".json");
                // and clear the dictionary
                staticWordCounts.Clear();
                jsonUserData = null;

            }
            // otherwise if we are logging into another user from a previous one
            else if (!string.Equals(staticUsername, user)) // userName would be previous user until overwritten
            {
                resetFilePath();
                // clear dictionary for new user
                staticWordCounts.Clear();
                // switch to new user
                staticUsername = user;
                // update the file path
                filePath = Path.Combine(filePath, user + ".json");
                // if they have info already...
                if (File.Exists(filePath))
                {
                    // load up their info
                    staticWordCounts = LoadUserData().wordCounts;
                }

            }
            // populate temporary instance of userData (JsonConvert.SerializeObject doesn't work on static fields)
            userData.wordCounts = staticWordCounts;
            userData.username = staticUsername;
            // convert UserData to Json for file creation
            jsonUserData = JsonConvert.SerializeObject(userData);
            Debug.Log(jsonUserData);
            
            // make the file
            CreateJsonFile(jsonUserData, user);
        }

        ///<summary>
        /// Returns a dictionary of type string, int. Assumes that a user has already been selected
        ///</summary>
        public static UserData LoadUserData()
        {
            return JsonConvert.DeserializeObject<UserData>(File.ReadAllText(filePath)); // convert a JSON string into Dictionary form
        }

        public static void UpdateWordCount(string word)
        {
            // if the word isn't in the dictionary... (haven't heard it yet)
            if (!staticWordCounts.ContainsKey(word))
            {
                staticWordCounts.Add(word, 1); // create entry for newly heard word
            }
            else // we've heard the word again
            {
                staticWordCounts[word]++; // increment word counter
            }
        }
        public static void CreateJsonFile(string jsonUserData, string user)
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

        ///<summary> 
        /// Simple clarifying helper function that returns true if a file already exists for an inputted user
        ///</summary>
        public static bool FileExistsIsTrue()
        {
            if (File.Exists(filePath))
            {
                return true;
            }
            else return false;
        }

        ///<summary>
        /// resets the static filePath variable to its original instatiation. (.../Saves/UserData/)
        ///</summary>
        public static void resetFilePath()// please update function summary if original filePath is changed
        {
            filePath = Path.Combine(Application.dataPath + "/", "Saves/", "UserData/");
        }

    }
}