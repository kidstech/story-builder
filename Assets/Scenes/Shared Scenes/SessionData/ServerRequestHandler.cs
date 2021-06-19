using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using ServerTypes;
using System;
using DatabaseEntry;
public class ServerRequestHandler : MonoBehaviour
{
    // assumes user is logged in firebase
    public static IEnumerator GetUserFromServer(Action<User> action) // action here allows the coroutine to call another function upon completion of the coroutine
    {
        // API call to locally hosted testing server, change in production
        string testURL = "http://localhost:4200/api/users/" + UserLogin.user.UserId;
        UnityWebRequest getUser = UnityWebRequest.Get(testURL);
        yield return getUser.SendWebRequest();
        string response = getUser.downloadHandler.text;
        User user = new User();
        user = JsonConvert.DeserializeObject<User>(response);
        // run whatever function call we passed as a parameter to GetUserFromServer
        action(user);
    }

    public static IEnumerator GetLearnerDataFromServer()
    {
        string requestURL = "http://localhost:4200/api/learnerData/" + LearnerLogin.staticLearner._id;
        using (UnityWebRequest getRequest = UnityWebRequest.Get(requestURL))
        {
            yield return getRequest.SendWebRequest();
            // respond to different potential results from server request
            switch (getRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                Debug.LogError("unable to connect to server... Error: " + getRequest.error);
                break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("error processing data received from server... Error: " + getRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("Communication successful, but received HTTP Error: " + getRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    string response = getRequest.downloadHandler.text;
                    LearnerData learnerData = new LearnerData();
                    // throws errors if it fails to grab learnerdata from the server (like when we try to grab learnerdata that doesn't exist)
                    learnerData = JsonConvert.DeserializeObject<LearnerData>(response);
                    // if we've only just received the object id...
                    // update static fields with the learnerdata we just got from the server
                    LearnerData.static_id = learnerData._id;
                    LearnerData.staticLearnerName = learnerData.learnerName;
                    LearnerData.staticLearnerId = learnerData.learnerId;
                    LearnerData.staticWordCounts = learnerData.wordCounts;
                    break;
            }
        }
    }

    // upsert operation server side
    public static IEnumerator PostLearnerDataToServer()
    {
        string requestURL = "http://localhost:4200/api/learnerData/" + LearnerData.static_id;
        string jsonLearnerData;
        UnityWebRequest postLearnerData = new UnityWebRequest(requestURL, "POST");
        // make learnerData object
        LearnerData learnerData = new LearnerData();
        // populate non-static fields
        learnerData._id = LearnerData.static_id;
        learnerData.learnerId = LearnerData.staticLearnerId;
        learnerData.learnerName = LearnerData.staticLearnerName;
        learnerData.wordCounts = LearnerData.staticWordCounts;
        // convert learnerDataobject to json
        jsonLearnerData = JsonConvert.SerializeObject(learnerData);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonLearnerData);
        postLearnerData.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        postLearnerData.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        postLearnerData.SetRequestHeader("Content-Type", "application/json");
        // pass the learnerData json to the server
        yield return postLearnerData.SendWebRequest();
        string response = postLearnerData.downloadHandler.text;
    }
}