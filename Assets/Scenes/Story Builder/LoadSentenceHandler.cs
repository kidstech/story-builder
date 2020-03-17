using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LoadSentencesHandler
{
    //
    public static string path = Path.Combine(Application.dataPath, "Saves", "Sentences");

    public static List<SavedSentence> LoadSavedSentences()
    {
        //
        string[] paths = Directory.GetFiles(@path, "*.sen");

        //
        int numOfFiles = paths.Length;

        //
        List<SavedSentence> sentences = new List<SavedSentence>();

        //
        if (numOfFiles > 0)
        {
            //
            for(int i = 0; i < numOfFiles; i++)
            {
                //
                BinaryFormatter bf = new BinaryFormatter();

                //
                FileStream file = File.OpenRead(paths[i]);

                //
                SavedSentence sentence = (SavedSentence)bf.Deserialize(file);

                //
                sentences.Add(sentence);

                //
                file.Close();

            }
        }

        //
        return sentences;
    }
}
