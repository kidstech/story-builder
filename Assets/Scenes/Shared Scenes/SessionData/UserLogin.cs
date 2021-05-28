using System.Collections;
using UnityEngine;
using DatabaseEntry;
using System.IO;
using UnityEngine.UI;

public class UserLogin : MonoBehaviour
{
    public void Login()
    {
        string user;
        user = this.GetComponent<Text>().text;
        UserData.Stringify(user);
    }
}

