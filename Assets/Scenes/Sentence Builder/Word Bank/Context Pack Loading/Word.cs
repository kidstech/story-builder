using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
    [JsonProperty("word")] // kept as word when serialized because that's the naming convention used by wordriver
    public string baseWord;
    public List<string> forms;
}