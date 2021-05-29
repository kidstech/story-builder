using System.Collections;
using UnityEngine;
using DatabaseEntry;
using System.IO;
using UnityEngine.UI;

public class UserLogin : MonoBehaviour
{
    ///<summary>
    /// user login function called from OnEndEdit call of the Input Field on the UserLogin game object
    /// creates a file named after the user and stores their dictionary of wordcounts in a json file
    /// TODO: authenticate users and create regex to prevent users from entering invalid file names
    ///</summary>
    public void Login()
    {
        string user;
        user = this.GetComponent<Text>().text;
        UserData.StoreUserData(user);
    }
}

