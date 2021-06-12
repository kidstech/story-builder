using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ServerTypes;

public class LearnerLogin : MonoBehaviour
{
    // instantiated from the learnerselectpopup script
    public Learner selectedLearner;

    ///<summary>
    /// Buttons aren't able to directly call IEnumerator functions, so this serves as an intermediary by starting the GoToSentenceBuilderScene coroutine.
    ///</summary>
    public void CallGoToSentenceBuilderScene()
    {
        StartCoroutine(GoToSentenceBuilderScene());
    }

    public IEnumerator GoToSentenceBuilderScene()
    {
        Debug.Log("Current learner: " + selectedLearner.name);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    
}
