using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HelperScript.ElementLoader;

public class CreateMainScene : MonoBehaviour {

    public Canvas canvasPrefab;
    public GameObject wordHolderPrefab;
    public GameObject confirmButtonPrefab;
    public GameObject wordChoicesPrefab;
    public GameObject closeWordChoicesPrefab;

    // Use this for initialization
    void Start () {
        ElementLoader loadElement = new ElementLoader();

        // Use the wordholder prefab that is already on the screen instead of creating one
        // This allows it to be centered on the screen regardless of resolution
        GameObject wordHolder = GameObject.Find("WordHolderPrefab 1");
        wordHolder.name = "word_holder"; // Used for easy reference (GameObject.Find()) in WordTile.cs 

        GameObject confirmButton = loadElement.createConfirmButton(confirmButtonPrefab, canvasPrefab);
        var whX = wordHolder.transform.position.x;
        var whWidth = wordHolder.GetComponent<RectTransform>().sizeDelta.x;
        var whY = wordHolder.transform.position.y;
        var buttonX = whX + whWidth / 2 + 20;
        confirmButton.transform.position = new Vector3(buttonX, whY, 0); // Place this button just to the right of the wordholder

        // Get the wordChoices from the screen
        GameObject wordChoices = GameObject.Find("WordChoices");
        wordChoices.name = "word_choices";

        GameObject closeWordChoices = GameObject.Find("CloseWordChoices");
        closeWordChoices.name = "close_word_choices";

        // moves it under the wordChoices
        confirmButton.transform.SetSiblingIndex(wordChoices.transform.GetSiblingIndex()-1);
    }

  
	
	// Update is called once per frame
	void Update () {
		
	}
}
