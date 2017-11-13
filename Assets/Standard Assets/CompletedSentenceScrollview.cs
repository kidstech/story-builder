/// <summary>
/// Each time a sentence is completed by submitting with the green checkmark button a new completed sentence scroll view is created.
/// 
/// <author> antin006@morris.umn.edu </author>
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CompletedSentenceScrollview : MonoBehaviour, IPointerClickHandler {


	public TextToSpeechHandler textToSpeechHandler;

	public void OnPointerClick (PointerEventData eventData) {
		// Speak the completed sentence
		textToSpeechHandler.startSpeaking(eventData.pointerPress.transform.GetChild (0).GetComponent<Text> ().text);
	}
		
}
