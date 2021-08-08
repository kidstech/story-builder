using System;
using System.Collections.Generic;

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
    [NonSerialized]
    public string contextPackId;
    [NonSerialized]
    public int partOfSpeechId;

    // The actual information about the word itself
    public string baseWord;
    public List<string> forms;
}