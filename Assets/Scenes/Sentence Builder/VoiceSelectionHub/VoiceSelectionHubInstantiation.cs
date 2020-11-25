using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;



public class VoiceSelectionHubInstantiation : MonoBehaviour
{
    public List<Transform> voiceButtons; 
    private Transform voiceSelectionHub;

    void Start()
    {
        // trying to save some processing power by not using gameobject.find() here and instead assuming that this script will only be attached to the VoiceSelectionHub game object
        voiceSelectionHub = this.gameObject.transform; 
        foreach (Transform child in voiceSelectionHub) {
            voiceButtons.Add(child);
        }
        AssignVoices();
        
    }

    // method that assigns voices to the three different change voice buttons
    // currently just nabs first 3 voices in system

    // TODO: implement logic checks for when there aren't enough voices in the system
    // Grab only from english cultures
    public void AssignVoices() {
        int i = 0;
        int numEnVoices = 0; // tracking index of next voice insertion point
        int numVoiceButtons = 3;
        int totalNumberOfVoices = Speaker.Voices.Count;
        // iterate through available voices to find english cultures
        for(i = 0; i < totalNumberOfVoices; i++) {

            // we only want english voices for pronunciation learning purposes
            if (Speaker.Voices[i].Culture == "en-US") {
                // assign the voice name to the text box of the buttons in VoiceSelectionHub
                voiceButtons[numEnVoices].gameObject.GetComponentInChildren<Text>().text = Speaker.Voices[i].Name;
                numEnVoices++; // track which voice button we will add text to next
            }

            // if we've found enough voices to fill all our buttons, we break from the loop (we only have three buttons max)
            if (numEnVoices == numVoiceButtons) {
                break;
            }
        }

        // if we end up having less than 3 english voices, delete any empty voice change buttons
        if (numEnVoices < numVoiceButtons) {
            for(int c = numVoiceButtons; c > numEnVoices; c--) {
                //Debug.Log("there are: " + voiceButtons.Count + "voice buttons");
                Destroy(voiceButtons[c-1].gameObject); // translate the length to an index value
            }
        }
    }
}
