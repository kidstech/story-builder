using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AZDrawerMenu : MonoBehaviour
{
    GameObject sortButton;

    private void Start()
    {
        sortButton = GameObject.Find("SortButton");
    }

    public void OpenMenu()
    {
        if(sortButton != null)
        {
            Animator animator = sortButton.GetComponent<Animator>();
            if(animator != null)
            {
                bool isOpen = animator.GetBool("open");

                animator.SetBool("open", !isOpen);
            }
        }
    }
}
