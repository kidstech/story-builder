using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MasterWordList
{
    public List<ContextPack> master = new List<ContextPack>();

    public List<Word> sort(int sortBy)
    {
        List<Word> list = new List<Word>();

        switch(sortBy)
        {
            // Return ever word available
            case (int)SORT.ALL:

                // For every context pack
                for(int i = 0; i < master.Count; i++)
                {
                    // For every word pack
                    for(int o = 0; o < master[i].wordPacks.Count; o++)
                    {
                        // For every part of speech (always 4)
                        for(int p = 0; p < 4; p++)
                        {
                            // For every word
                            for(int q = 0; q < master[i].wordPacks[o].partsOfSpeech[p].words.Count; q++)
                            {
                                Word word = new Word();

                                word.word = master[i].wordPacks[o].partsOfSpeech[p].words[q].word;

                                word.forms = master[i].wordPacks[o].partsOfSpeech[p].words[q].forms;

                                list.Add(word);
                            }
                        }
                    }
                }

                break;
        }

        return list;
    }

    public int numberOfWords = 0;

    public int[] numberOfPartOfSpeech = new int[4];

    // Method for filling in the details for just the context pack
    public void setUpContextPack(string name, string icon, int contextPackId, int numWordPacks)
    {
        master[contextPackId].name = name;
        master[contextPackId].icon = icon;
        master[contextPackId].wordPacks = new List<WordPack>(numWordPacks);
    }

    // Method for setting up the details about the parts of speech
    public void setUpPartOfSpeech(int contextPackId, int wordPackId, int partOfSpeechId, int numWords)
    {
        master[contextPackId].wordPacks[wordPackId].partsOfSpeech[partOfSpeechId].words = new List<Word>(numWords);
    }

    public void setUpWord(int contextPackId, int wordPackId, int partOfSpeechId, int wordId, string word, List<string> forms)
    {
        master[contextPackId].wordPacks[wordPackId].partsOfSpeech[partOfSpeechId].words[wordId].word = word;
        master[contextPackId].wordPacks[wordPackId].partsOfSpeech[partOfSpeechId].words[wordId].forms = forms;
    }

    // Context Pack
    public class ContextPack
    {
        // Some information about the context pack
        public string name;
        public string icon;
        
        // The list that will hold all our word packs
        public List<WordPack> wordPacks;
    }

    // Word Pack
    public class WordPack
    {
        // This will always be four, since we are only keeping track of nouns, verbs, adjectives, and miscs
        public List<PartOfSpeech> partsOfSpeech = new List<PartOfSpeech>(4);
    }

    // Part of Speech
    public class PartOfSpeech
    {
        // The list that will hold all our words
        public List<Word> words;
    }

    public class Word
    {
        // Some information about our words
        public string word;
        public List<string> forms;

        // Method for filling in the informaiton about our stored words
        public void setUpWord(string word, List<string> forms)
        {
            this.word = word;
            this.forms = forms;
        }
    }

    enum WORD
    {
        NOUN = 0,
        VERB = 1,
        ADJECTIVE = 2,
        MISC = 3
    }

    enum SORT
    {
        ALL = 0,
    }

    
}