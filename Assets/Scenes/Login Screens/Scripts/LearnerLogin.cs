using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ServerTypes;

public class LearnerLogin : MonoBehaviour
{
    // instantiated from the learnerselectpopup script
    public Learner selectedLearner;
    
    // static learner so we have a way to access the current learner without passing around the LearnerLogin object
    public static Learner staticLearner;

    ///<summary>
    /// Buttons aren't able to directly call IEnumerator functions, so this serves as an intermediary by starting the GoToSentenceBuilderScene coroutine.
    ///</summary>
    public void CallGoToSentenceBuilderScene()
    {
        StartCoroutine(GoToSentenceBuilderScene());
    }

    public IEnumerator GoToSentenceBuilderScene()
    {
        staticLearner = selectedLearner;
        // get and store learnerdata in static fields of LearnerDataHandler class
        StartCoroutine(ServerRequestHandler.GetLearnerDataFromServer());
        Debug.Log("Current learner: " + selectedLearner.name);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        // store the learnerdata we just finished getting from the server
        // this function call has to take place after the scene change has taken place because it is dependent on fields initialized in the start()
        // method of WordCountHandler, which is only called once the scene has switched
        LearnerDataHandler.StoreLearnerData();
    }
}
