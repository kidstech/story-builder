﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFilterButtons : MonoBehaviour
{
    //
    public Animator sortingPacksAnim;
    public Animator sortingAlphabetAnim;

    //
    private void Start()
    {
        //
        GetComponent<Button>().onClick.AddListener(Toggle);
    }

    //
    public void Toggle()
    {
        //
        sortingPacksAnim.SetTrigger("Toggle");
        sortingAlphabetAnim.SetTrigger("Toggle");
    }
}
