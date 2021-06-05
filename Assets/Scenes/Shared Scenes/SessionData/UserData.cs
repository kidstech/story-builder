using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Collections;
using UnityEngine.Networking;
using ServerTypes;

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
        public static string jsonUserData;
        // there are two variables with the same path because the filePath will be changed around as users log in and out
        // dirPath exists to check that the directory still exists even if filePath changes
        private static string dirPath = Path.Combine(Application.dataPath + "/", "Saves/", "UserData/");
        private static string filePath = Path.Combine(Application.dataPath + "/", "Saves/", "UserData/");


        // UserData Methods
        public static void StoreUserData(string user)
        {
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
            SerializeUserData();
            // make the file
            CreateJsonFile(jsonUserData, user);
        }

        public static void SerializeUserData()
        {
            UserData userData = new UserData();
            // populate temporary instance of userData (JsonConvert.SerializeObject doesn't work on static fields)
            userData.wordCounts = staticWordCounts;
            userData.username = staticUsername;
            // convert UserData to Json for file creation
            jsonUserData = JsonConvert.SerializeObject(userData);
        }

        ///<summary>
        /// Returns UserData from a json object. Assumes that a user has already been selected
        ///</summary>
        public static UserData LoadUserData()
        {
            return JsonConvert.DeserializeObject<UserData>(File.ReadAllText(filePath)); // convert a JSON string into UserData
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

        public static IEnumerator Upload(string userData, System.Action<bool> callback = null)
        {
            using (UnityWebRequest request = new UnityWebRequest("https://webhooks.mongodb-realm.com/api/client/v2.0/app/storybuilder-exhtp/service/StoryBuilderRealm/incoming_webhook/addUserData", "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(userData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.Log(request.error);
                    if (callback != null)
                    {
                        callback.Invoke(false);
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callback.Invoke(request.downloadHandler.text != "{}");
                    }
                }
            }
        }

        public static IEnumerator Replace(string userData, System.Action<bool> callback = null)
        {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(userData);
                UnityWebRequest uwr = UnityWebRequest.Put("https://webhooks.mongodb-realm.com/api/client/v2.0/app/storybuilder-exhtp/service/StoryBuilderRealm/incoming_webhook/updateUserData?username=" + staticUsername, bodyRaw);
                uwr.SetRequestHeader("Content-Type", "application/json");
                yield return uwr.SendWebRequest();
        }

        public static IEnumerator Download(System.Action<UserData> callback = null)
        {
            using (UnityWebRequest request = UnityWebRequest.Get("https://webhooks.mongodb-realm.com/api/client/v2.0/app/storybuilder-exhtp/service/StoryBuilderRealm/incoming_webhook/getUsers"))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.Log(request.error);
                    if (callback != null)
                    {
                        callback.Invoke(null);
                    }
                }
                else
                {
                    jsonUserData = request.downloadHandler.text;
                    if (callback != null)
                    {
                        callback.Invoke(JsonConvert.DeserializeObject<UserData>(request.downloadHandler.text));
                    }
                    
                }
            }
        }

        // initial server get request testing for our actual server setup rather than a test DB
        public static IEnumerator getUserFromServer()
        {
            // authID for user: Biruk
            string testAuthID = "BqswmmJd2VXBrVqEs9lQxVIZNmj2";
            // server get request (test) API
            string testUrl = "http://localhost:4200/api/users/" + testAuthID;
            UnityWebRequest getRequest = UnityWebRequest.Get(testUrl);
            // send result of server call to debug console in unity
            yield return getRequest.SendWebRequest();
            // assuming success here, but errors should definitely eventually be caught...
            string response = getRequest.downloadHandler.text;
            User user = JsonConvert.DeserializeObject<User>(response);
            Debug.Log(response);
            Debug.Log(user.name);
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