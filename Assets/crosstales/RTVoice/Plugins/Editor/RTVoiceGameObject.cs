using UnityEngine;
using UnityEditor;

namespace Crosstales.RTVoice.EditorExt
{
    /// <summary>Editor component for the "Hierarchy"-menu.</summary>
	public class RTVoiceGameObject : MonoBehaviour
    {

        [MenuItem("GameObject/RTVoice/RTVoice", false, EditorHelper.GO_ID)]
        private static void AddRTVoice()
        {
            EditorHelper.InstantiatePrefab("RTVoice");
        }

        [MenuItem("GameObject/RTVoice/RTVoice", true)]
        private static bool AddRTVoiceValidator()
        {
            return !EditorHelper.isRTVoiceInScene;
        }

        [MenuItem("GameObject/RTVoice/SpeechText", false, EditorHelper.GO_ID + 1)]
        private static void AddSpeechText()
        {
            EditorHelper.InstantiatePrefab("SpeechText");
        }

        [MenuItem("GameObject/RTVoice/Sequencer", false, EditorHelper.GO_ID + 2)]
        private static void AddSequencer()
        {
            EditorHelper.InstantiatePrefab("Sequencer");
        }

        [MenuItem("GameObject/RTVoice/TextFileSpeaker", false, EditorHelper.GO_ID + 3)]
        private static void AddTextFileSpeaker()
        {
            EditorHelper.InstantiatePrefab("TextFileSpeaker");
        }

        [MenuItem("GameObject/RTVoice/Loudspeaker", false, EditorHelper.GO_ID + 4)]
        private static void AddLoudspeaker()
        {
            EditorHelper.InstantiatePrefab("Loudspeaker");
        }

        [MenuItem("GameObject/RTVoice/Proxy", false, EditorHelper.GO_ID + 5)]
        private static void AddProxy()
        {
            EditorHelper.InstantiatePrefab("Proxy");
        }

        [MenuItem("GameObject/RTVoice/Proxy", true)]
        private static bool AddProxyValidator()
        {
            return !EditorHelper.isProxyInScene;
        }
    }
}
// © 2017 crosstales LLC (https://www.crosstales.com)
