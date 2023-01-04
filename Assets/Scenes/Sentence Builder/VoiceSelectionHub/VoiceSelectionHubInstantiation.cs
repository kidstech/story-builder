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
    // currently just nabs first 3 en-US voices in system
    public void AssignVoices() {

        int numVoices = 0; // tracking index of next voice insertion point
        int numVoiceButtons = 3;

        // Get all available English voices
        List<Crosstales.RTVoice.Model.Voice> englishVoices = Speaker.Instance.VoicesForCulture("en");


        foreach(Crosstales.RTVoice.Model.Voice voice in englishVoices) {
            if(voice.Name == "Bad News" || voice.Name == "Karen" || voice.Name == "Nathan") {
                voiceButtons[numVoices].gameObject.GetComponentInChildren<Text>().text = voice.Name;
                numVoices++;
            }

            if(numVoices == numVoiceButtons) {
                break;
            }
        }

        if(numVoices < numVoiceButtons) {
            foreach(Crosstales.RTVoice.Model.Voice voice in englishVoices) {
                if(numVoices == numVoiceButtons) {
                    break;
                }
                
                else {
                    voiceButtons[numVoices].gameObject.GetComponentInChildren<Text>().text = voice.Name;
                    numVoices++;
                }
            }
        }

        // if we end up having less than 3 voices, delete any empty voice change buttons
        if (numVoices < numVoiceButtons) {
            for(int c = numVoiceButtons; c > numVoices; c--) {
                //Debug.Log("there are: " + voiceButtons.Count + "voice buttons");
                Destroy(voiceButtons[c-1].gameObject); // translate the length to an index value
            }
        }
    }
}
