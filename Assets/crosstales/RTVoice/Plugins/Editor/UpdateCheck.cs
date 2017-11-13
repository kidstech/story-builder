using UnityEngine;
using UnityEditor;

namespace Crosstales.RTVoice.EditorExt
{
    /// <summary>Checks for updates of the asset.</summary>
    [InitializeOnLoad]
    public static class UpdateCheck
    {

        #region Variables

        public const string TEXT_NOT_CHECKED = "Not checked.";
        public const string TEXT_NO_UPDATE = "No update available - you are using the latest version.";

        private static char[] splitChar = new char[] { ';' };

        #endregion


        #region Constructor

        static UpdateCheck()
        {
            if (Util.Constants.UPDATE_CHECK)
            {
                if (Util.Constants.DEBUG)
                    Debug.Log("Updater enabled!");

                string lastDate = EditorPrefs.GetString(Util.Constants.KEY_UPDATE_DATE);
                string date = System.DateTime.Now.ToString("yyyyMMdd"); // every day
                //string date = DateTime.Now.ToString("yyyyMMddmm"); // every minute (for tests)

                if (!date.Equals(lastDate))
                {
                    if (Util.Constants.DEBUG)
                        Debug.Log("Checking for update...");

                    EditorPrefs.SetString(Util.Constants.KEY_UPDATE_DATE, date);

                    updateCheck();
                }
                else
                {
                    if (Util.Constants.DEBUG)
                        Debug.Log("No update check needed.");
                }

            }
            else
            {
                if (Util.Constants.DEBUG)
                    Debug.Log("Updater disabled!");
            }
        }

        #endregion


        #region Static methods

        public static void UpdateCheckForEditor(out string result)
        {
            string[] data = readData();

            if (isUpdateAvailable(data))
            {

                result = parseDataForEditor(data);
            }
            else
            {
                result = TEXT_NO_UPDATE;
            }
        }

        #endregion


        #region Private methods

        private static void updateCheck()
        {
            string[] data = readData();

            if (isUpdateAvailable(data))
            {

                Debug.LogWarning(parseData(data));

                if (Util.Constants.UPDATE_OPEN_UAS)
                {
                    Application.OpenURL(Util.Constants.ASSET_URL);
                }
            }
            else
            {
                if (Util.Constants.DEBUG)
                    Debug.Log("Asset is up-to-date.");
            }
        }

        private static string[] readData()
        {
            string[] data = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = Util.Helper.RemoteCertificateValidationCallback;

                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    using (System.IO.Stream stream = client.OpenRead(Util.Constants.ASSET_UPDATE_CHECK_URL))
                    {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                        {
                            string content = reader.ReadToEnd();

                            foreach (string line in Util.Helper.SplitStringToLines(content))
                            {
                                if (line.StartsWith(Util.Constants.ASSET_UID.ToString()))
                                {
                                    data = line.Split(splitChar, System.StringSplitOptions.RemoveEmptyEntries);

                                    if (data != null && data.Length >= 3) //valid record?
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        data = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Could not load update file: " + System.Environment.NewLine + ex);
            }

            return data;
        }

        private static bool isUpdateAvailable(string[] data)
        {
            if (data != null)
            {
                int buildNumber;

                if (int.TryParse(data[1], out buildNumber))
                {
                    if (buildNumber > Util.Constants.ASSET_BUILD)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static string parseData(string[] data)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (data != null)
            {
                sb.Append(Util.Constants.ASSET_NAME);
                sb.Append(" - update found!");
                sb.Append(System.Environment.NewLine);
                sb.Append(System.Environment.NewLine);
                sb.Append("Your version:\t");
                sb.Append(Util.Constants.ASSET_VERSION);
                sb.Append(System.Environment.NewLine);
                sb.Append("New version:\t");
                sb.Append(data[2]);
                sb.Append(System.Environment.NewLine);
                sb.Append(System.Environment.NewLine);
                sb.AppendLine("Please download the new version from the UAS:");
                sb.AppendLine(Util.Constants.ASSET_URL);
            }

            return sb.ToString();
        }

        private static string parseDataForEditor(string[] data)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (data != null)
            {
                sb.AppendLine("Update found!");
                sb.Append(System.Environment.NewLine);
                sb.Append("Your version:\t");
                sb.Append(Util.Constants.ASSET_VERSION);
                sb.Append(System.Environment.NewLine);
                sb.Append("New version:\t");
                sb.Append(data[2]);
                sb.Append(System.Environment.NewLine);
            }

            return sb.ToString();
        }

        #endregion
    }
}
// © 2016-2017 crosstales LLC (https://www.crosstales.com)