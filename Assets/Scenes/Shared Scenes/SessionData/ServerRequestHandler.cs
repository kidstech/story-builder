using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using ServerTypes;
using System;
public class ServerRequestHandler
{
    // assumes user is logged in firebase
    public IEnumerator GetUserFromServer(Action<User> action) // action here allows the coroutine to call another function upon completion of the coroutine
    {
        // API call to locally hosted testing server, change in production
        string testURL = "http://localhost:4200/api/users/" + UserLogin.user.UserId;
        UnityWebRequest getLearners = UnityWebRequest.Get(testURL);
        yield return getLearners.SendWebRequest();
        Debug.Log("get request sent to server...");
        string response = getLearners.downloadHandler.text;
        User user = new User();
        // update static User
        user = JsonConvert.DeserializeObject<User>(response);
        Debug.Log("response from server: " + response);
        // run whatever function call we passed as a parameter to GetUserFromServer
        action(user);
    }
}