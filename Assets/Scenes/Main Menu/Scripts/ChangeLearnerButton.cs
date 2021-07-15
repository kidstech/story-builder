using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DatabaseEntry;

public class ChangeLearnerButton : MonoBehaviour
{
    public void GoToLearnerLoginScene()
    {
        StartCoroutine(LearnerSelect());
    }
    public IEnumerator LearnerSelect()
    {
        // changing learner means their session is over
        LearnerData.staticSessionTimes[LearnerDataHandler.sessionDate] = LearnerDataHandler.FormatSeconds();
        // update local logs
        LearnerDataHandler.StoreLearnerData();
        // send logs to server
        StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
        // change scene
        AsyncOperation sceneChange = SceneManager.LoadSceneAsync(3, LoadSceneMode.Single);
        while (!sceneChange.isDone)
        {
            yield return null;
        }
    }
}
