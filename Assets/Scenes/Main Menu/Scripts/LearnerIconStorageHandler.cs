﻿using System.Collections;
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

    public static Sprite GetLearnerSprite(string learner_Id)
    {
        string spritePath = Path.Combine(dirPath, learner_Id + ".bytes");
        Sprite learnerSprite;
        try
        {
            learnerSprite = GetSprite(File.ReadAllBytes(spritePath));
            return learnerSprite;
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }

    // credit to: https://www.programmersought.com/article/74693938105/
    // converts a byte array into a Unity Sprite
    public static Sprite GetSprite(byte[] bytes)
    {
        //First create a Texture2D object, which is used to convert the streaming data to Texture2D
        Texture2D texture = new Texture2D(10, 10);
        texture.LoadImage(bytes);//Streaming data is converted to Texture2D
        //Create a Sprite, based on Texture2D object
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        return sp;
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
