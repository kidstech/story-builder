using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class LoadSavedSentences
{
    //
    public static List<SavedSentence> LoadSentences()
    {
        //
        string[] savedSentences = Directory.GetFiles(Path.Combine(Application.dataPath, "Saves", "Sentences"), "*.sen");

        //
        List<SavedSentence> sentencesToReturn = new List<SavedSentence>();

        //
        for(int i = 0; i < savedSentences.Length; i++)
        {
            //
            FileStream file = new FileStream(savedSentences[i], FileMode.Open);
            
            //
            try
            {
                //
                BinaryFormatter bf = new BinaryFormatter();

                //
                SavedSentence newSentence = (SavedSentence)bf.Deserialize(file);

                //
                sentencesToReturn.Add(newSentence);
            }
            catch(SerializationException e)
            {
                //
                Debug.LogError("Cannot deserialize. " + e);
            }
            finally
            {
                // Clean up
                file.Close();
            }
        }

        //
        return sentencesToReturn;
    }

    
}
