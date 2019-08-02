using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AZDrawerMenu : MonoBehaviour
{
    GameObject sortButton;

    private Color c_green = new Color(0, 255, 0, 1);
    private Color c_red = new Color(255, 0, 0, 1);

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

                GameObject extraButton = GameObject.Find("SortButtonPrefabBack");

                if (extraButton != null)
                {
                    if (isOpen)
                    {
                        extraButton.gameObject.GetComponent<Image>().color = c_red;
                        extraButton.gameObject.GetComponentInChildren<Text>().text = "<";
                    }
                    else
                    {
                        extraButton.gameObject.GetComponent<Image>().color = c_green;
                        extraButton.gameObject.GetComponentInChildren<Text>().text = ">";
                    }
                }

                animator.SetBool("open", !isOpen);
            }
        }
    }
}
