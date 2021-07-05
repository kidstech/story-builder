using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLearnerButton : MonoBehaviour
{
    public void GoToLearnerLoginScene()
    {
        StartCoroutine(LearnerSelect());
    }
    public IEnumerator LearnerSelect()
    {
        AsyncOperation sceneChange = SceneManager.LoadSceneAsync(3, LoadSceneMode.Single);
        while (!sceneChange.isDone)
        {
            yield return null;
        }
    }
}
