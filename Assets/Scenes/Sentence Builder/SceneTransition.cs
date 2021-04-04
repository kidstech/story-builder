using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    private Button button;

    [Header("Game Objects in Scene")]
    public Transform canvas;

    // private void Start()
    // {
    //     // This is supposed to be the Text component
    //     this.transform.GetChild(0).GetComponent<Text>().text = "";
        
    // }

    public void GoToStorybuilder()
    {
        SceneManager.LoadScene("StoryBuilder");
    }

    public void GoToSentenceBuilder()
    {
        SceneManager.LoadScene("SentenceBuilder");
    }
}
