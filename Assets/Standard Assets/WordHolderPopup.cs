using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordHolderPopup : MonoBehaviour
{
    // For all the word forms
    public List<string> wordForms;

    // Prefab called "WordHolderPopupButton"
    public GameObject wordHolderPopupButton;

    // All the buttons we create
    private List<GameObject> buttons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the offset
        float offset = (2 * Mathf.PI) / wordForms.Count;

        // For every word
        for(int i = 0; i < wordForms.Count; i++)
        {
            // Create a new special button tile
            GameObject button = Instantiate(wordHolderPopupButton);

            // Add our button to our current list of buttons
            buttons.Add(button);

            // Set it as a child of the canvas
            button.transform.SetParent(GameObject.Find("Canvas").transform, false);

            // Change the title text of the button
            button.GetComponentInChildren<Text>().text = wordForms[i];

            // Change the X Y based on the offset
            button.GetComponent<RectTransform>().anchoredPosition = new Vector3(Mathf.Cos(offset * i) * 150, Mathf.Sin(offset * i) * 150, 0);
        }
    }

    // Set the current tile in the word holder equal to what we pass in
    public void setWordTile(string word)
    {
        // Get the child in the WordHolder (the word tile) and set its text to equal what we pass in
        GameObject.Find("WordHolder").transform.GetChild(0).GetComponentInChildren<Text>().text = word;

        // MAYBE CHANGE TO NOT DO THIS RIGHT AWAY!!
        closeMenu();
    }

    // Close the menu
    public void closeMenu()
    {
        // Destroy this menu
        Destroy(this.gameObject);

        // Destroy all the buttons we created
        foreach(GameObject button in buttons)
        {
            Destroy(button);
        }
    }

   
}
