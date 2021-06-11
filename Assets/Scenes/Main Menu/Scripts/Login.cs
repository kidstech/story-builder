using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Auth;

public class Login : MonoBehaviour
{
    public Scene sentenceBuilderScene;
    // Need to look up how to properly protect passwords eventually.
    public string email;
    public string password;
    Firebase.Auth.FirebaseAuth firebaseAuth;
    Firebase.Auth.FirebaseUser user;
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

    ///<summary>
    /// Stores the sentence builder scene in the variable of the same name after waiting a frame and allowing the scene to be loaded beforehand
    ///</summary>
    IEnumerator getSentenceBuilderScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        while(!asyncLoad.isDone)
        {
            yield return null;
        }
        Debug.Log("scene name: " + sentenceBuilderScene.name);
    }

    void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync();
        firebaseAuth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        firebaseAuth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        
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

    public void login()
    {
        if (userInputIsValid())
        {
            Debug.Log("email: " + email + " password: " + password);
            // send login request to firebase
            firebaseAuth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
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
                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.Log("User signed in successfully: email: " + newUser.Email + " userId: " + newUser.UserId);

                // login successful, swap scenes
                swapToSentenceBuilder();
            });
        }
    }

    public void swapToSentenceBuilder()
    {
        // loads the sentence builder scene while also unloading the current main menu scene
        Debug.Log("Loading sentence builder...");
        getSentenceBuilderScene();
        SceneManager.UnloadSceneAsync("MainMenu");
    }
}
