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
    public Transform canvasttt;
    public Transform canvassss;

    [Header("Stuff")]
    public int i;

    private void Start()
    {
        // This is supposed to be the Text component
        this.transform.GetChild(0).GetComponent<Text>().text = "";
        
    }

    public void ChangeScreen()
    {
        SceneManager.LoadScene("StoryBuilder", LoadSceneMode.Single);
    }
}
