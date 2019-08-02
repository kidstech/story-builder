using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MasterWordList
{
    public List<Word> masterWordList = new List<Word>();

    public List<ContextPack> masterContextPackList = new List<ContextPack>();

    public void basicSort()
    {
        masterWordList.Sort((x, y) => x.partOfSpeechId.CompareTo(y.partOfSpeechId));
    }

    public List<Word> getLetter(List<string> letters)
    {
        List<Word> newList = new List<Word>();

        for(int i = 0; i < masterWordList.Count; i++)
        {
            string word = masterWordList[i].word;
            string firstChar = word.Substring(0, 1).ToLower();

            for(int o = 0; o < letters.Count; o++)
            {
                if(firstChar == letters[o].ToLower())
                {
                    newList.Add(masterWordList[i]);
                }
            }
        }

        if(letters.Count == 0)
        {
            newList = masterWordList;
        }

        return newList;
    }

    public List<Word> getPack(List<int> packs)
    {
        List<Word> newList = new List<Word>();

        for (int i = 0; i < masterWordList.Count; i++)
        {
            int wordConextPackId = masterWordList[i].contextPackId;

            for(int o = 0; o < packs.Count; o++)
            {
                if(wordConextPackId == packs[o])
                {
                    newList.Add(masterWordList[i]);
                }
            }  
        }

        if (packs.Count == 0)
        {
            newList = masterWordList;
        }

        return newList;
    }

    /*
     * This is functional, just not implimented
     * 
    // Used to get nouns, verbs, adjectives, and/or misc from a list
    public List<Word> getSpecific(int typeToGet)
    {
        List<Word> newList = new List<Word>();

        for(int i = 0; i < masterWordList.Count; i++)
        {
            if(masterWordList[i].partOfSpeechId == typeToGet)
            {
                newList.Add(masterWordList[i]);
            }
        }

        return newList;
    }
    */

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