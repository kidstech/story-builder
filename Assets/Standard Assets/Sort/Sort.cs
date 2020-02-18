using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sort : MonoBehaviour
{
    //
    private bool state = false;

    //
    private SortController sortController;

    //
    private void Start()
    {
        //
        sortController = transform.parent.GetComponent<SortController>();
    }

    //
    public void UpdateLetter()
    {
        //
        sortController.UpdateLetterFilter(this.transform.GetComponentInChildren<Text>().text, state);

        //
        state = !state;
    }
}
