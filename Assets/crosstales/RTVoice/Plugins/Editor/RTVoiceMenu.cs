using UnityEngine;
using UnityEditor;

namespace Crosstales.RTVoice.EditorExt
{
    /// <summary>Editor component for the "Tools"-menu.</summary>
    public class RTVoiceMenu
    {

        [MenuItem("Tools/RTVoice/Prefabs/RTVoice", false, EditorHelper.MENU_ID + 20)]
        private static void AddRTVoice()
        {
            EditorHelper.InstantiatePrefab("RTVoice");
        }

        [MenuItem("Tools/RTVoice/Prefabs/RTVoice", true)]
        private static bool AddRTVoiceValidator()
        {
            return !EditorHelper.isRTVoiceInScene;
        }

        [MenuItem("Tools/RTVoice/Prefabs/SpeechText", false, EditorHelper.MENU_ID + 40)]
        private static void AddSpeechText()
        {
            EditorHelper.InstantiatePrefab("SpeechText");
        }

        [MenuItem("Tools/RTVoice/Prefabs/Sequencer", false, EditorHelper.MENU_ID + 50)]
        private static void AddSequencer()
        {
            EditorHelper.InstantiatePrefab("Sequencer");
        }

        [MenuItem("Tools/RTVoice/Prefabs/TextFileSpeaker", false, EditorHelper.MENU_ID + 60)]
        private static void AddTextFileSpeaker()
        {
            EditorHelper.InstantiatePrefab("TextFileSpeaker");
        }

        [MenuItem("Tools/RTVoice/Prefabs/Loudspeaker", false, EditorHelper.MENU_ID + 80)]
        private static void AddLoudspeaker()
        {
            EditorHelper.InstantiatePrefab("Loudspeaker");
        }

        [MenuItem("Tools/RTVoice/Prefabs/Proxy", false, EditorHelper.MENU_ID + 100)]
        private static void AddProxy()
        {
            EditorHelper.InstantiatePrefab("Proxy");
        }

        [MenuItem("Tools/RTVoice/Prefabs/Proxy", true)]
        private static bool AddProxyValidator()
        {
            return !EditorHelper.isProxyInScene;
        }

        [MenuItem("Tools/RTVoice/Help/Manual", false, EditorHelper.MENU_ID + 600)]
        private static void ShowManual()
        {
            Application.OpenURL(Util.Constants.ASSET_MANUAL_URL);
        }

        [MenuItem("Tools/RTVoice/Help/API", false, EditorHelper.MENU_ID + 610)]
        private static void ShowAPI()
        {
            Application.OpenURL(Util.Constants.ASSET_API_URL);
        }

        [MenuItem("Tools/RTVoice/Help/Forum", false, EditorHelper.MENU_ID + 620)]
        private static void ShowForum()
        {
            Application.OpenURL(Util.Constants.ASSET_FORUM_URL);
        }

        [MenuItem("Tools/RTVoice/Help/Product", false, EditorHelper.MENU_ID + 630)]
        private static void ShowProduct()
        {
            Application.OpenURL(Util.Constants.ASSET_WEB_URL);
        }

        [MenuItem("Tools/RTVoice/Help/3rd Party Assets", false, EditorHelper.MENU_ID + 650)]
        private static void Show3rdPartyAV()
        {
            Application.OpenURL(Util.Constants.ASSET_3P_URL);
        }

        [MenuItem("Tools/RTVoice/About/Unity AssetStore", false, EditorHelper.MENU_ID + 800)]
        private static void ShowUAS()
        {
            Application.OpenURL(Util.Constants.ASSET_URL);
        }

        [MenuItem("Tools/RTVoice/About/" + Util.Constants.ASSET_AUTHOR, false, EditorHelper.MENU_ID + 820)]
        private static void ShowCT()
        {
            Application.OpenURL(Util.Constants.ASSET_AUTHOR_URL);
        }

        [MenuItem("Tools/RTVoice/About/Info", false, EditorHelper.MENU_ID + 840)]
        private static void ShowInfo()
        {
            EditorUtility.DisplayDialog(Util.Constants.ASSET_NAME,
               "Version: " + Util.Constants.ASSET_VERSION +
               System.Environment.NewLine +
               System.Environment.NewLine +
               "© 2015-2017 by " + Util.Constants.ASSET_AUTHOR +
               System.Environment.NewLine +
               System.Environment.NewLine +
               Util.Constants.ASSET_AUTHOR_URL +
               System.Environment.NewLine, "Ok");
        }
    }
}
// © 2015-2017 crosstales LLC (https://www.crosstales.com)