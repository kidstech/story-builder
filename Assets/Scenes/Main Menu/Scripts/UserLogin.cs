﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Auth;

public class UserLogin : MonoBehaviour
{
    public Scene sentenceBuilderScene;
    // Need to look up how to properly protect passwords eventually.
    private protected string email;
    private protected string password;
    Firebase.Auth.FirebaseAuth firebaseAuth;
    public static Firebase.Auth.FirebaseUser user = null;
    GameObject emailGO;
    GameObject passwordGO;

    // Start is called before the first frame update
    void Start()
    {
        // grab email and password from the text boxes
        emailGO = GameObject.Find("EmailInput");
        passwordGO = GameObject.Find("PasswordInput");
        InitializeFirebase();

    }

    void InitializeFirebase()
    {
        Debug.Log("Initializing Firebase...");
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Initialization canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Initialization failed");
                return;
            }
            Debug.Log(task.Result);
            firebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            firebaseAuth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
        });
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (firebaseAuth.CurrentUser != user)
        {
            bool signedIn = user != firebaseAuth.CurrentUser && firebaseAuth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = firebaseAuth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    void OnDestroy()
    {
        firebaseAuth.StateChanged -= AuthStateChanged;
        firebaseAuth = null;
    }



    ///<summary>
    /// checks to see if the user entered in an email and password
    /// TODO: add regex to make sure there aren't empty spaces at the start or after the end or weird characters
    ///</summary>
    private bool userInputIsValid()
    {
        email = emailGO.GetComponent<Text>().text;
        password = passwordGO.GetComponent<Text>().text;
        if (email != null && password != null) return true;
        else
        {
            Debug.Log("Please enter your email and password.");
            return false;
        }
    }

    public async void login()
    {
        if (userInputIsValid())
        {
            // send login request to firebase and wait for response
            await firebaseAuth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }
                if (task.IsCompleted)
                {
                    user = task.Result;
                    Debug.Log("user after firebase login: " + user.Email);
                }
            });
            // change scenes after we've logged in
            StartCoroutine(GoToLearnerLoginScene());
        }
    }

    ///<summary>
    /// Stores the sentence builder scene in the variable of the same name after waiting a frame and allowing the scene to be loaded beforehand
    ///</summary>
    public IEnumerator GoToLearnerLoginScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(3, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
