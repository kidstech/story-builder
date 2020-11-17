using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;



public class VoiceSelectionHubInstantiation : MonoBehaviour
{
    // our list of transforms will be all the tranforms this script is attached to
    public List<Transform> voiceButtons; 
    private Transform voiceSelectionHub;
    // rather than using static index here, instead attach a script to the voiceselectionhub that can then iterate through it's children to create the 3 voice buttons

    void Start()
    {
        voiceSelectionHub = GameObject.FindWithTag("VoiceList").transform;
        foreach (Transform child in voiceSelectionHub) {
            voiceButtons.Add(child);
        }
        assignVoices();
        
    }

    // method that assigns voices to the three different change voice buttons
    // currently just nabs first 3 voices in system

    // TODO: implement logic checks for when there aren't enough voices in the system
    // Grab only from english cultures
    public void assignVoices() {
        int i = 0;
        foreach(Transform child in voiceButtons) {
            // assign the voice name to the text box of the buttons
            child.GetComponentInChildren<Text>().text = Speaker.Voices[i].Name;
            i++;
        }
    }
}
