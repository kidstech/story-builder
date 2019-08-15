using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    private Animator anim;
    private RectTransform image;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();

        image = GetComponentInChildren<RectTransform>();
    }

    public void ToggleTransition()
    {
        anim.SetBool("open", !anim.GetBool("open"));

        image.localScale = new Vector3(image.localScale.x, -1 * image.localScale.y, image.localScale.z);
    }
}
