using UnityEngine;
using ServerTypes;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

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
    // map associating a learner's id with their learnerButton's sprite grabbed from firebase
    public static Dictionary<string, Sprite> learnerSprites;
    void Start()
    {
        if (learnerSprites == null) learnerSprites = new Dictionary<string, Sprite>();
        Debug.Log("getting new user from server...");
        // get user from the server and then create learner buttons to choose from
        StartCoroutine(ServerRequestHandler.GetUserFromServer(SetUpLearnerButtons));
    }
    public void RefreshLearnerIcons()
    {
        // clear the old sprites
        LearnerSelectPopup.learnerSprites.Clear();
        // clear the old buttons
        foreach(Transform child in learnerSelect.transform)
        {
            Destroy(child.gameObject);
        }
        // and go through setup again
        SetUpLearnerButtons();
    }

    public void SetUpLearnerButtons()
    {
        Debug.Log("making buttons for user: " + currentUser?.name);
        Debug.Log("User doesn't have the appropriate sprites: " + DontHaveAppropriateLearnerSprites());
        // if we don't have the learner sprites we need already, go get them
        if (DontHaveAppropriateLearnerSprites())
        {
            foreach (Learner learner in currentUser.learners)
            {
                // for every learner that actually has an icon...
                if (learner.icon != null)
                {
                    Debug.Log("grabbing learner icon for: " + learner.name);
                    // grabs the firebase image URI => sends server request => updates image component field in the button
                    GetLearnerIconAndMakeButton(learner);
                }
                // for learners that don't have an icon, just make the button without trying to get an image from firebase
                else CreateLearnerButton(learner);
            }
        }
        // otherwise, if we already have the sprites, just make the buttons without talking to firebase
        else
        {
            foreach (Learner learner in currentUser.learners)
            {
                CreateLearnerButton(learner);
            }
        }

    }

    // currently calls firebase once for EACH learner... with limitations on server calls this should probably be refactored in the future.
    // seems likely that we'll have to create directories for a user's learners so we can just grab all the icons for a user in one call
    public void GetLearnerIconAndMakeButton(Learner learner)
    {
        // get the learner icon from firebase and THEN create the learner button
        StartCoroutine(ServerRequestHandler.GetLearnerIconFromFirebase(learner, CreateLearnerButton));

    }

    public void CreateLearnerButton(Learner learner)
    {
        // make a button for that learner using the LearnerButtonPrefab in the MainMenu folder
        GameObject button = Instantiate(learnerButtonPrefab);
        DontDestroyOnLoad(button);
        // set object this script is attached to as its parent (LearnerSelect)
        // these buttons will be grouped simply by having LearnerSelect as their parent because of the layout group component attached to said parent
        button.transform.SetParent(learnerSelect.transform);
        // fix weird issue where z coord is instantiated at -300 or so
        Vector3 correctedZPosition = button.transform.GetComponent<RectTransform>().localPosition;
        correctedZPosition.z = 0;
        // if we already have a profile sprite stored for this learner...
        if (learnerSprites.ContainsKey(learner._id) && learnerSprites[learner._id] != null)
        {
            // use that instead of making another get request to firebase
            button.transform.GetComponentInChildren<Image>().sprite = learnerSprites[learner._id];
        }
        // if we DO have learner icons and DON'T have any sprites already...
        else if (!learnerIconArrayIsEmpty && !learnerSprites.ContainsKey(learner._id))
        {
            // turn it into a sprite and use it for their button 
            button.transform.GetComponentInChildren<Image>().sprite = GetSprite(learnerIcon);
            // store the sprite to save on firebase image requests
            if (!learnerSprites.ContainsKey(learner._id))
            {
                learnerSprites.Add(learner._id, GetSprite(learnerIcon));
            }
            // clear the icon data so we don't duplicate it
            Array.Clear(learnerIcon, 0, learnerIcon.Length);
            learnerIconArrayIsEmpty = true;
        }
        // if we don't even have a learner icon, add the learner to the dictionary with a null sprite so we know it had the chance to get a sprite
        else if (!learnerSprites.ContainsKey(learner._id))
        {
            learnerSprites.Add(learner._id, null);
        }
        button.transform.GetComponent<RectTransform>().localPosition = correctedZPosition;
        // add learner name to their button
        button.GetComponentInChildren<Text>().text = learner.name;
        // store learner info in the learnerlogin script (in case we need id for database queries)
        button.GetComponent<LearnerLogin>().selectedLearner = learner;
    }

    // credit to: https://www.programmersought.com/article/74693938105/
    // converts a byte array into a Unity Sprite
    public Sprite GetSprite(Byte[] bytes)
    {
        //First create a Texture2D object, which is used to convert the streaming data to Texture2D
        Texture2D texture = new Texture2D(10, 10);
        texture.LoadImage(bytes);//Streaming data is converted to Texture2D
        //Create a Sprite, based on Texture2D object
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        return sp;
    }

    public bool DontHaveAppropriateLearnerSprites()
    {
        foreach (Learner learner in currentUser.learners)
        {
            if (!learnerSprites.ContainsKey(learner._id))
            {
                return true;
            }
        }
        return false;
    }
}
