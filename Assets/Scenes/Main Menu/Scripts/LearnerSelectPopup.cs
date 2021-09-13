using UnityEngine;
using ServerTypes;
using UnityEngine.UI;
using System;
using System.IO;

public class LearnerSelectPopup : MonoBehaviour
{
    public GameObject learnerButtonPrefab;
    // LearnerSelect game object this script is attached to
    public GameObject learnerSelect;
    const long maxImageSize = 1 * 1024 * 1024; // 1 MB of space for our image byte array
    public static byte[] learnerIcon = new byte[maxImageSize];
    // boolean indication of whether the array is empty to avoid iterating through the whole array every time and seeing if it's really empty
    public static bool learnerIconArrayIsEmpty = true;
    // current wordriver user logged in
    public static User currentUser;
    // array of files that contain previously stored learner icons
    private string[] filePaths;
    void Start()
    {
        filePaths = Directory.GetFiles(LearnerIconStorageHandler.dirPath);
        // get user from the server and then create learner buttons to choose from
        StartCoroutine(ServerRequestHandler.GetUserFromServer(SetUpLearnerButtons));
    }
    public void RefreshLearnerIcons()
    {
        Debug.Log("refreshing learner buttons");
        // clear the old buttons
        foreach (Transform child in learnerSelect.transform)
        {
            Destroy(child.gameObject);
        }
        // clear local file storage so we forcibly get updated sprites
        filePaths = null;
        // and go through setup again
        SetUpLearnerButtons();
    }

    public void SetUpLearnerButtons()
    {
        Debug.Log("making buttons for user: " + currentUser?.name);
        Debug.Log("User doesn't have the appropriate sprites: " + !AlreadyHaveAppropriateLearnerSprites());
        //if we don't have the learner sprites we need already, go get them
        if (!AlreadyHaveAppropriateLearnerSprites())
        {
            foreach (Learner learner in currentUser.learners)
            {
                //for every learner that actually has an icon...
                if (learner.icon != null)
                {
                    Debug.Log("grabbing learner icon for: " + learner.name);
                    //grabs the firebase image URI => sends server request => updates image component field in the button
                    GetLearnerIconAndMakeButton(learner);
                }
                //for learners that don't have an icon, just make the button without trying to get an image from firebase
                else CreateLearnerButton(learner);
            }
        }
        //otherwise, if we already have the sprites, just make the buttons without talking to firebase
        else
        {
            Debug.Log("making buttons with local files...");
            foreach (Learner learner in currentUser.learners)
            {
                CreateLearnerButton(learner);
            }
        }

    }
    public void GetLearnerIconAndMakeButton(Learner learner)
    {
        // get the learner icon from firebase and THEN create the learner button
        StartCoroutine(ServerRequestHandler.GetLearnerIconFromFirebase(learner, CreateLearnerButton));
    }

    public void CreateLearnerButton(Learner learner)
    {
        // make a button for that learner using the LearnerButtonPrefab in the MainMenu folder
        GameObject button = Instantiate(learnerButtonPrefab);
        // set object this script is attached to as its parent (LearnerSelect)
        // these buttons will be grouped simply by having LearnerSelect as their parent because of the layout group component attached to said parent
        button.transform.SetParent(learnerSelect.transform);
        // fix weird issue where z coord is instantiated at -300 or so
        Vector3 correctedZPosition = button.transform.GetComponent<RectTransform>().localPosition;
        correctedZPosition.z = 0;
        // make sure we grab any local icons that may have been added from most recent server call
        filePaths = Directory.GetFiles(LearnerIconStorageHandler.dirPath);
        //check and see if we have an image file for the learner
        foreach (string fileName in filePaths)
        {
            if (learner._id == Path.GetFileNameWithoutExtension(fileName))
            {
                Debug.Log("adding learner sprite to button from local file...");
                byte[] icon = File.ReadAllBytes(fileName);
                //LearnerImage component of learner button prefab
                button.transform.GetChild(1).GetComponent<Image>().sprite = GetSprite(icon);
            }
        }
        button.transform.GetComponent<RectTransform>().localPosition = correctedZPosition;
        // add learner name to their button
        button.GetComponentInChildren<Text>().text = learner.name;
        // store learner info in the learnerlogin script (in case we need id for database queries)
        button.GetComponent<LearnerLogin>().selectedLearner = learner;
    }

    // credit to: https://www.programmersought.com/article/74693938105/
    // converts a byte array into a Unity Sprite
    public static Sprite GetSprite(Byte[] bytes)
    {
        //First create a Texture2D object, which is used to convert the streaming data to Texture2D
        Texture2D texture = new Texture2D(10, 10);
        texture.LoadImage(bytes);//Streaming data is converted to Texture2D
        //Create a Sprite, based on Texture2D object
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        return sp;
    }

    // see if ANY learners have locally stored images
    public bool AlreadyHaveAppropriateLearnerSprites()
    {
        if (filePaths == null) 
        {
            Debug.Log("filepaths was null... User doesn't have appropriate learner sprites");
            return false;
        }
        foreach (Learner learner in currentUser.learners)
        {
            foreach(string fileName in filePaths)
            {
                if (Path.GetFileNameWithoutExtension(fileName) == learner._id)
                {
                    Debug.Log("matching file name found! We have the appropriate learner sprites!");
                    return true;
                }
            }
        }
        Debug.Log("no learnername matches locally stored image names...");
        return false;
    }
}
