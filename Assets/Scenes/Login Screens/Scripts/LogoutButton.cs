using UnityEngine;
using UnityEngine.SceneManagement;
using DatabaseEntry;

public class LogoutButton : MonoBehaviour
{
    public void Logout()
    {
        // changing user means their session is over if a learner has logged in previously
        if (LearnerDataHandler.sessionDate != null)
        {
            LearnerData.staticSessionTimes[LearnerDataHandler.sessionDate] = LearnerDataHandler.FormatSeconds();
            // update local logs
            LearnerDataHandler.StoreLearnerData();
            // send logs to server
            StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
        }
        // swap to user login screen
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

}
