using UnityEngine;
using UnityEngine.UI;

public class AlphaFilterButton : MonoBehaviour
{
    //
    private bool state = false;

    //
    private FilterController sortController;

    //
    private Image image;

    //
    private void Start()
    {
        //
        sortController = transform.parent.GetComponent<FilterController>();

        //
        image = GetComponent<Image>();
    }

    //
    public void UpdateLetter()
    {
        //
        sortController.UpdateLetterFilter(this.transform.GetComponentInChildren<Text>().text, state);

        //
        if(!state)
        {
            //
            image.color = Color.cyan;
        }
        else
        {
            //
            image.color = Color.white;
        }

        //
        state = !state;
    }
}
