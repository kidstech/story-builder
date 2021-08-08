using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ServerTypes;

public class LearnerIconStorageHandler : MonoBehaviour
{
    public static string dirPath;
    public static string filePath;
    // Start is called before the first frame update
    void Start()
    {
        dirPath = Path.Combine(Application.persistentDataPath + "/Resources/LearnerIcons/");
        filePath = "";
        CheckDirPath();
    }
    // store the learner icon byte array to a file named after the learnerId
    public static void StoreLearnerSprite(string learnerId, byte[] learnerIcon)
    {
        Debug.Log("storing sprite: " + learnerId + " at path = " + dirPath);
        filePath = Path.Combine(dirPath, learnerId + ".bytes");
        Debug.Log("filepath: " + filePath);
        File.WriteAllBytes(filePath, learnerIcon);
        filePath = "";
    }

    public void CheckDirPath()
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        else return;
    }
}
