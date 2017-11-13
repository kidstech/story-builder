/// <summary>
/// Handles the text to speech system defined by RTVoice.
/// Can and should be extended upon in the future to account for different voices.
/// 
/// <author> antin006@morris.umn.edu </author>
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model.Event;

public class TextToSpeechHandler : MonoBehaviour {


	// Speech button that changes from play to stop
	public TextToSpeechButton textToSpeechButton;

	// Is a phrase or word currently being spoken?
	public static bool isSpeaking = false;

	// Are there TTS voices available?
	public static bool voicesAvailable = true;

	/// <summary>
	/// Start this instance.
	/// Checks for voices and initializes event hooks for RTVoice.
	/// </summary>
	void Start(){

		// Check if voices are available
		if (Speaker.Voices.Count <= 0) 
			voicesAvailable = false;
		
		// Initialize TTS event hooks
		Speaker.OnSpeakStart += speakStartMethod;
		Speaker.OnSpeakComplete += speakCompleteMethod;

	}

	/// <summary>
	/// Starts the speaking of the provided phrase.
	/// </summary>
	/// <param name="phrase">Phrase to speak.</param>
	public void startSpeaking(string phrase){

		// If we aren't currently speaking
		if (!isSpeaking && voicesAvailable) {

			// Then start speaking
			isSpeaking = true;
			Speaker.SpeakNative (phrase, Speaker.VoiceForCulture ("en"));
		}

	}
		
	/// <summary>
	/// Stops the speaking of any speech.
	/// </summary>
	public void stopSpeaking(){

		// Stop speaking
		Speaker.Silence ();
		isSpeaking = false;

		// Re-enable play option
		textToSpeechButton.showPlayOption ();
	}


	/// <summary>
	/// Event hook for the start of speaking.
	/// </summary>
	/// <param name="e">E.</param>
	private void speakStartMethod(SpeakEventArgs e){

		// Allow user to stop speech
		textToSpeechButton.showStopOption ();
	}

	/// <summary>
	/// Event hook for the completion of speaking.
	/// </summary>
	/// <param name="e">E.</param>
	private void speakCompleteMethod(SpeakEventArgs e){

		// No longer speaking
		isSpeaking = false;

		// Allow user to use TTS again
		textToSpeechButton.showPlayOption ();
	}

}

