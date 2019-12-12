using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveStorySystem
{
    //
    private static BinaryFormatter formatter = new BinaryFormatter();

    //
    private static string storyDataPath = Path.Combine(Application.persistentDataPath, "Stories");

    //
    public static Story[] LoadStories()
    {
        Story[] results;

        // Check if this isn't the first time running
        if (Directory.Exists(storyDataPath))
        {
            //
            string[] storySavePaths = Directory.GetFiles(storyDataPath, "*.story");

            //
            results = new Story[storySavePaths.Length];

            //
            for (int i = 0; i < storySavePaths.Length; i++)
            {
                //
                FileStream stream = new FileStream(storySavePaths[i], FileMode.Open);

                //
                Story story = formatter.Deserialize(stream) as Story;

                //
                stream.Close();

                //
                results[i] = story;
            }

            return results;
        }
        else
        {
            // Directory does not exist, create it
            Debug.Log("Directory does not exist. Building...");

            //
            Directory.CreateDirectory(storyDataPath);

            //
            return null;
        }
    }

    //
    public static void SaveStoryData(Story data)
    {
        //
        string path = Path.Combine(Application.persistentDataPath, "Stories");

        //
        if (!Directory.Exists(path))
        {
            //
            Directory.CreateDirectory(path);
        }

        //
        path = Path.Combine(path, (data.name + ".story"));

        //
        FileStream stream = new FileStream(path, FileMode.Create);

        //
        formatter.Serialize(stream, data);

        //
        stream.Close();
    }
}
