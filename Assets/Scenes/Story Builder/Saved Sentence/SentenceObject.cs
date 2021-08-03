using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceObject : MonoBehaviour
{
    public SavedSentence savedSentence;

    public string CompileSentence()
    {
        string fullSentence = savedSentence.sentenceText; 
        fullSentence = fullSentence.Remove(fullSentence.Length - 1, 1);

        return fullSentence;
    }
}
