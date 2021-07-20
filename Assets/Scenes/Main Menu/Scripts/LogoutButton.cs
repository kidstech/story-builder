using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutButton : MonoBehaviour
{
    public GameObject ContextPackFilter;
    public void Logout()
    {
        // deactivate the contextpack filter buttons so that they are only active after server calls have completed
        ContextPackFilter.SetActive(false);
        // swap to user login screen
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
    
}
