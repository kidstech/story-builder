using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Firebase.Auth;
using ServerTypes;
using UnityEngine.UI;

public class LearnerSelectPopup : MonoBehaviour
{
    public GameObject learnerButtonPrefab;
    // LearnerSelect game object this script is attached to
    public GameObject learnerSelect;
    private ServerRequestHandler serverRequestHandler;

    void Start()
    {
        serverRequestHandler = new ServerRequestHandler();
        // get user from the server and then create learner buttons to choose from
        StartCoroutine(serverRequestHandler.GetUserFromServer(SetUpLearnerButtons));
    }

    public void SetUpLearnerButtons(User user)
    {
        foreach (Learner learner in user.learners)
        {
            // make a button for that learner
            GameObject button = Instantiate(learnerButtonPrefab);
            // set object this script is attached to as its parent (LearnerSelect)
            // these buttons will be grouped simply by having LearnerSelect as their parent because of the layout group component attached to said parent
            button.transform.SetParent(learnerSelect.transform);
            // fix weird issue where z coord is instantiated at -300 or so
            Vector3 correctedZPosition = button.transform.GetComponent<RectTransform>().localPosition;
            correctedZPosition.z = 0;
            button.transform.GetComponent<RectTransform>().localPosition = correctedZPosition; 
            // add learner name to their button
            button.GetComponentInChildren<Text>().text = learner.name;
            // store learner info in the learnerlogin script (in case we need id for database queries)
            button.GetComponent<LearnerLogin>().selectedLearner = learner;
        }
    }

}
