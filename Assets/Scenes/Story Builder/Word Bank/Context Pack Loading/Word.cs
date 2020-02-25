using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word
{
    /*
     * What are we keeping track of?
     * =============================
     * context pack id
     * word pack id
     * part of speech id
     * the actual base word
     * the forms of the word
     */

    // Identifiers for our words
    public int contextPackId = -1;
    public int wordPackId = -1;
    public int partOfSpeechId = -1;

    // The actual information about the word itself
    public string word;
    public List<string> forms;

    // Method for filling in the informaiton about our stored words
    public void setUpWord(string word, List<string> forms)
    {
        this.word = word;
        this.forms = forms;
    }
}