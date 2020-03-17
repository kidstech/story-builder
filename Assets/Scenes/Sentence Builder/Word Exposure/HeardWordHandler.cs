using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HeardWordHandler : MonoBehaviour
{
    // Threshhold of when to switch algorithms
    public int threshhold = 10;

    // Our current words heard
    public Dictionary<string, int> wordsThisSession = new Dictionary<string, int>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Saving..");
            SaveHeardWords();
        }
    }

    public void HeardWord(string word)
    {
        // If our dictionary is small, it is likely that we any word we try to use will not be in the dictionary
        // So we will use a more efficient search in the beginning, and switch to a faster one laster.
        if(wordsThisSession.Count < threshhold)
        {
            int value;

            // Check to see if the key exists
            if(wordsThisSession.TryGetValue(word, out value))
            {
                // If so, just increment the value at that position
                wordsThisSession[word]++;
            }
            else
            {
                // Otherwise add it in
                wordsThisSession.Add(word, 1);
            }
        }
        else
        {
            // Check to see if the key exists
            if (wordsThisSession.ContainsKey(word))
            {
                // If so, just increment the value at that position
                wordsThisSession[word]++;
            }
            else
            {
                // Otherwise add it in
                wordsThisSession.Add(word, 1);
            }
        }
    }

    public void SaveHeardWords()
    {
        // Set up the path
        string path = Application.dataPath + "/saves/heardWords.json";

        // TODO:
        // Introduce something to auto-generate empty json if heardWords.json
        // Isn't found

        // Read in our current list of heard words and their values
        string previousHeardWords = File.ReadAllText(path);

        // Construct a json object that will let us add our words
        // From this session into the json
        JSONObject pw = new JSONObject(previousHeardWords);

        // Loop through every element to see if we find a matching entry
        foreach (KeyValuePair<string, int> entry in wordsThisSession)
        {
            bool foundMatch = false;

            // Loop through the dictionary
            for (int i = 0; i < pw.Count; i++)
            {
                // If we find a match, we add the value from our entry to the saved json
                if(pw.keys[i] == entry.Key)
                {
                    // Increment the saved number
                    pw[i].i += entry.Value;

                    // We found a match
                    foundMatch = true;

                    // Exit the foreach
                    break;
                }
            }

            // If we didn't find a match, add it in to the saved json
            if(!foundMatch)
            {
                pw.AddField(entry.Key, entry.Value);
            }
        }

        Debug.Log(pw.ToString());

        // Once we are all done, save it!
        File.WriteAllText(@path, pw.ToString());
    }
}
