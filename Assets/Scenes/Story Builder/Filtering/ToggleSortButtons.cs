using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSortButtons : MonoBehaviour
{
    //
    public Animator sortingPacksAnim;
    public Animator sortingAlphabetAnim;

    //
    public void Toggle()
    {
        //
        sortingPacksAnim.SetTrigger("Toggle");
        sortingAlphabetAnim.SetTrigger("Toggle");
    }
}
