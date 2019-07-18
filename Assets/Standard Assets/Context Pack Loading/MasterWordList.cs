using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MasterWordList
{
    public List<Word> masterWordList = new List<Word>();

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

    public class Word
    {
        /*
         * What are we keeping track of?
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