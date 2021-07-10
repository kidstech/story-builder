using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using ServerTypes;
using System;
using DatabaseEntry;
public class ServerRequestHandler : MonoBehaviour
{
    private static readonly string serverIp = "http://localhost:4567"; // change localhost to ip of target machine if using separate device
    public static IEnumerator GetLearnerIconFromFirebase(Learner learner, Action<Learner> action)
    {
        UnityWebRequest getIcon = UnityWebRequest.Get(learner.icon);
        yield return getIcon.SendWebRequest();
        switch (getIcon.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                Debug.LogError("Unable to connect to server... Error: " + getIcon.error);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error processing data received from server... Error: " + getIcon.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("Communication successful, but received HTTP Error: " + getIcon.error);
                break;
            case UnityWebRequest.Result.Success:
                try
                {
                    LearnerSelectPopup.learnerIcon = getIcon.downloadHandler.data;
                    LearnerSelectPopup.learnerIconArrayIsEmpty = false;
                    Debug.Log("storing sprite locally...");
                    LearnerIconStorageHandler.StoreLearnerSprite(learner._id, LearnerSelectPopup.learnerIcon);
                }
                // make sure we account for images that may be larger than 1MB attempting to be written to our array of bytes
                catch (IndexOutOfRangeException e)
                {
                    Debug.Log("Error, tried to write outside the length of the array. (Image size greater than 1MB) Details: " + e);
                }                
                action(learner);
                break;
        }
    }
    // assumes user is logged in firebase
    public static IEnumerator GetUserFromServer(Action action) // action here allows the coroutine to call another function upon completion of the coroutine
    {
        // API call to locally hosted testing server, change in production
        string requestURL = serverIp + "/api/users/" + UserLogin.user.UserId;
        Debug.Log(requestURL);
        UnityWebRequest getUser = UnityWebRequest.Get(requestURL);
        yield return getUser.SendWebRequest();
        switch (getUser.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                Debug.LogError("Unable to connect to server... Error: " + getUser.error);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error processing data received from server... Error: " + getUser.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("Communication successful, but received HTTP Error: " + getUser.error);
                break;
            case UnityWebRequest.Result.Success:
                string response = getUser.downloadHandler.text;
                LearnerSelectPopup.currentUser = JsonConvert.DeserializeObject<User>(response);
                Debug.Log("user grabbed successfully! " + LearnerSelectPopup.currentUser.name);
                action();
                break;
        }
    }

    public static IEnumerator GetLearnerContextPacks(string learnerId, Action action)
    {
        string requestURL = serverIp + "/api/users/" + UserLogin.user.UserId + "/" + LearnerLogin.staticLearner._id + "/learnerPacks";
        UnityWebRequest getLearnerPacks = UnityWebRequest.Get(requestURL);
        yield return getLearnerPacks.SendWebRequest();
        switch (getLearnerPacks.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                Debug.LogError("Unable to connect to server... Error: " + getLearnerPacks.error);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error processing data received from server... Error: " + getLearnerPacks.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("Communication successful, but received HTTP Error: " + getLearnerPacks.error);
                break;
            case UnityWebRequest.Result.Success:
                string response = getLearnerPacks.downloadHandler.text;
                LoadContextPacks.serverPacks = JsonConvert.DeserializeObject<List<ServerContextPack>>(response);
                Debug.Log("learnercontextpacks grabbed successfully!");
                action();
                break;
        }
    }

    public static IEnumerator GetLearnerDataFromServer()
    {
        string requestURL = serverIp + "/api/learnerData/" + LearnerLogin.staticLearner._id;
        using (UnityWebRequest getRequest = UnityWebRequest.Get(requestURL))
        {
            yield return getRequest.SendWebRequest();
            // respond to different potential results from server request
            switch (getRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError("Unable to connect to server... Error: " + getRequest.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error processing data received from server... Error: " + getRequest.error);
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
                    LearnerData.staticSessionTimes = learnerData.sessionTimes;
                    break;
            }
        }
    }

    // upsert operation server side
    public static IEnumerator PostLearnerDataToServer()
    {
        string requestURL = serverIp + "/api/learnerData/" + LearnerData.static_id;
        string jsonLearnerData;
        // make learnerData object
        LearnerData learnerData = new LearnerData();
        // populate non-static fields
        learnerData._id = LearnerData.static_id;
        learnerData.learnerId = LearnerData.staticLearnerId;
        learnerData.learnerName = LearnerData.staticLearnerName;
        learnerData.wordCounts = LearnerData.staticWordCounts;
        learnerData.sessionTimes = LearnerData.staticSessionTimes;
        // convert learnerDataobject to json
        jsonLearnerData = JsonConvert.SerializeObject(learnerData);
        using (UnityWebRequest postRequest = UnityWebRequest.Post(requestURL, jsonLearnerData))
        {
            // prep json for sending
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonLearnerData);
            postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            postRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            postRequest.SetRequestHeader("Content-Type", "application/json");
            yield return postRequest.SendWebRequest();
            switch (postRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError("Unable to connect to server... Error: " + postRequest.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error processing data received from server... Error: " + postRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("Communication successful, but received HTTP Error: " + postRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("LearnerData successfully posted!");
                    break;
            }
        }
    }
    // a non-IEnumerator version of the post method for the OnApplicationExit() post request that logs learner session times
    public static void BlockingPostLearnerDataToServer()
    {
        string requestURL = serverIp + "/api/learnerData/" + LearnerData.static_id;
        string jsonLearnerData;
        // make learnerData object
        LearnerData learnerData = new LearnerData();
        // populate non-static fields
        learnerData._id = LearnerData.static_id;
        learnerData.learnerId = LearnerData.staticLearnerId;
        learnerData.learnerName = LearnerData.staticLearnerName;
        learnerData.wordCounts = LearnerData.staticWordCounts;
        learnerData.sessionTimes = LearnerData.staticSessionTimes;
        // convert learnerDataobject to json
        jsonLearnerData = JsonConvert.SerializeObject(learnerData);

        UnityWebRequest postRequest = UnityWebRequest.Post(requestURL, jsonLearnerData);
        {
            // prep json for sending
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonLearnerData);
            postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            postRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            postRequest.SetRequestHeader("Content-Type", "application/json");
            postRequest.SendWebRequest();
            // while the request is still being processed without error...
            while (!postRequest.isDone && !postRequest.isHttpError && !postRequest.isNetworkError)
            {
                // relax and wait for it to finish
            }
            switch (postRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError("Unable to connect to server... Error: " + postRequest.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error processing data received from server... Error: " + postRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("Communication successful, but received HTTP Error: " + postRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("LearnerData successfully posted!");
                    break;
            }
        }
    }
}