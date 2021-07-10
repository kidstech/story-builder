using UnityEngine;
using UnityEngine.SceneManagement;
using DatabaseEntry;

public class LogoutButton : MonoBehaviour
{
    public void Logout()
    {
        // changing learner means their session is over
        LearnerData.staticSessionTimes[WordCountHandler.sessionDate] = WordCountHandler.FormatSeconds();
        // update local logs
        WordCountHandler.StoreLearnerData();
        // send logs to server
        StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
        // swap to user login screen
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
    
}
