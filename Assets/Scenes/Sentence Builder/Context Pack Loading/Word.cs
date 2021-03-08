using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Word
{
    /*
     * What are we keeping track of?
     * =============================
     * context pack id
     * part of speech id
     * the actual base word
     * the forms of the word
     */

    // Identifiers for our words
    public int contextPackId = -1;
    public int partOfSpeechId = -1;

    // The actual information about the word itself
    public string word;
    public List<string> forms;
}