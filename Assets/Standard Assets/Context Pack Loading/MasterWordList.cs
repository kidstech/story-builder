using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MasterWordList
{
    public enum SORT
    {
        CONTEXT_PACK = 0,
        PART_OF_SPEECH = 1,
        ALPHABETICAL = 2
    }

    public enum FILTER
    {
        LETTER = 0,
        PACK = 1
    }

    public List<Word> masterWordList = new List<Word>();

    public List<ContextPack> masterContextPackList = new List<ContextPack>();

    public void addWordToList(int contextPackId, int wordPackId, int partOfSpeechId, string word, List<string> forms)
    {

        // Only add one instance of a word
        for(int i = 0; i < masterWordList.Count; i++)
        {
            if(masterWordList[i].word == word)
            {
                return;
            }
        }

        // Create a new word object
        Word w = new Word();

        // Populate the informaiton we need
        w.contextPackId = contextPackId;
        w.wordPackId = wordPackId;
        w.partOfSpeechId = partOfSpeechId;
        w.word = word;
        w.forms = forms;

        // Add the word into the word list
        masterWordList.Add(w);
    }

    public void addConextPackToList(int contextPackId, string contextPackName, string contextPackIconPath)
    {
        // Create a new context pack object
        ContextPack c = new ContextPack();

        // Populate it with the informaiton given
        c.contextPackId = contextPackId;
        c.contextPackName = contextPackName;
        c.contextPackIconPath = contextPackIconPath;

        // Add the context pack into the context pack list
        masterContextPackList.Add(c);
    }

    public class ContextPack
    {
        /*
         * What are we keeping track of?
         * =============================
         * context pack id
         * context pack name
         * context pack icon path
         */
        public int contextPackId = -1;
        public string contextPackName = "";
        public string contextPackIconPath = "";
    }

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
}