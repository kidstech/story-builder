using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Auth;

public class UserLogin : MonoBehaviour
{
    // Need to look up how to properly protect passwords eventually.
    private protected string email;
    private protected string password;
    Firebase.Auth.FirebaseAuth firebaseAuth;
    public static Firebase.Auth.FirebaseUser user = null;
    public GameObject emailGO;
    public GameObject passwordGO;
    System.Threading.Tasks.Task<FirebaseUser> getUserTask;
    private bool loginFailed = false;
    // text box used for showing login errors to user
    public GameObject loginErrorGO;
    private ColorBlock errorColors = new ColorBlock();
    private ColorBlock defaultColors = new ColorBlock();
    private string loginErrorMessage = "Incorrect username/password. Please ensure details are correct and try again.";


    // Start is called before the first frame update
    void Start()
    {
        InitializeFirebase();
        // change the physics update interval to once every 1 second to reduce number of update calls (if we end up using physics change this back)
        Time.fixedDeltaTime = 1f;
        // track original email/pass input field colors (assuming email field = pass field)
        defaultColors = emailGO.GetComponent<InputField>().colors;
    }

    void FixedUpdate()
    {
        // ideally we would just call this from login() when it fails, but seeing as that method takes place off the main thread, we can't use the Unity API
        // so instead we just store the login result in a bool and check each second to see if a login has failed and then do the usual function call
        if (loginFailed)
        {
            ShowErrorMessage();
        }
    }

    private void ShowErrorMessage()
    {
        loginErrorGO.SetActive(true);
        // store colorblock in error colors
        errorColors = emailGO.GetComponent<InputField>().colors;
        // modify normal color to red
        errorColors.normalColor = new Color(231f / 255f, 186f / 255f, 187f / 255f, 1f);
        // change input fields' normal color to red
        emailGO.GetComponent<InputField>().colors = errorColors;
        passwordGO.GetComponent<InputField>().colors = errorColors;
        // start timer to reset input field colors after a delay
        StartCoroutine(ResetInputFieldColors(5f));
        // pass the appropriate error message to our text LoginError game object
        loginErrorGO.GetComponent<Text>().text = loginErrorMessage;
        // prevent FixedUpdate from calling ShowErrorMessage() again
        loginFailed = false;
    }

    private IEnumerator ResetInputFieldColors(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        emailGO.GetComponent<InputField>().colors = defaultColors;
        passwordGO.GetComponent<InputField>().colors = defaultColors;
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
        email = emailGO.GetComponentInChildren<Text>().text;
        password = passwordGO.GetComponent<InputField>().text;
        if (email != null && password != null) return true;
        else
        {
            Debug.Log("Please enter your email and password.");
            return false;
        }
    }
    public async void Login()
    {
        if (userInputIsValid())
        {
            // waiting for this task to finish kinda kills the point of having an async function, but (as far as I know) a syncronous version doesn't exist
            // and this is just a login button anyway
            await firebaseAuth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(getUser =>
            {
                if (getUser.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    loginFailed = true;
                }
                if (getUser.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + getUser.Exception);
                    loginFailed = true;
                }
                if (getUser.IsCompleted)
                {
                    user = getUser.Result;
                    Debug.Log(user.Email + " has signed in.");
                }
            });
            StartCoroutine(GoToLearnerLoginScene());
        }
    }

    ///<summary>
    /// Stores the sentence builder scene in the variable of the same name after waiting a frame and allowing the scene to be loaded beforehand
    ///</summary>
    public IEnumerator GoToLearnerLoginScene()
    {
        Debug.Log("changing scenes");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(3, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
