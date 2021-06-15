using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using ServerTypes;
using System;
using DatabaseEntry;
public class ServerRequestHandler
{
    // assumes user is logged in firebase
    public IEnumerator GetUserFromServer(Action<User> action) // action here allows the coroutine to call another function upon completion of the coroutine
    {
        // API call to locally hosted testing server, change in production
        string testURL = "http://localhost:4200/api/users/" + UserLogin.user.UserId;
        UnityWebRequest getUser = UnityWebRequest.Get(testURL);
        yield return getUser.SendWebRequest();
        Debug.Log("get request sent to server...");
        string response = getUser.downloadHandler.text;
        User user = new User();
        // update static User
        user = JsonConvert.DeserializeObject<User>(response);
        Debug.Log("response from server: " + response);
        // run whatever function call we passed as a parameter to GetUserFromServer
        action(user);
    }

    public IEnumerator GetLearnerDataFromServer()
    {
        string requestURL = "http://localhost:4200/api/learnerData/" + LearnerLogin.staticLearner._id;
        UnityWebRequest getLearnerData = UnityWebRequest.Get(requestURL);
        yield return getLearnerData.SendWebRequest();
        Debug.Log("get learnerdata request sent to server with learnerId = " + LearnerLogin.staticLearner._id);
        string response = getLearnerData.downloadHandler.text;
        Debug.Log("Respone: " + response);
        LearnerData learnerData = new LearnerData();
        learnerData = JsonConvert.DeserializeObject<LearnerData>(response);
        Debug.Log("learner associated with learnerData gottem from server: " + learnerData.learnerName);
    }
}