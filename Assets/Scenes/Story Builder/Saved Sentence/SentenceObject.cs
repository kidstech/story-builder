using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceObject : MonoBehaviour
{
    //
    public SavedSentence savedSentence;

    //
    public string CompileSentence()
    {
        //
        string fullSentence = ""; 

        //
        for(int i = 0; i < savedSentence.words.Count; i++)
        {
            fullSentence = fullSentence + savedSentence.words[i].word + " ";
        }

        //
        fullSentence = fullSentence.Remove(fullSentence.Length - 1, 1);

        //
        return fullSentence;
    }
}
