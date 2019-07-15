using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MasterWordList
{
    public List<ContextPack> master = new List<ContextPack>();

    // Context Pack
    public class ContextPack
    {
        // Some information about the context pack
        public string name;
        public string icon;
        
        // Method for willing in the details for just the context pack
        public void setUpContextPack(string name, string icon, int numWordPacks)
        {
            this.name = name;
            this.icon = icon;
            wordPacks = new List<WordPack>(numWordPacks);
        }

        // The list that will hold all our word packs
        public List<WordPack> wordPacks;

        // Word Pack
        public class WordPack
        {
            // This will always be four, since we are only keeping track of nouns, verbs, adjectives, and miscs
            public List<PartOfSpeech> partsOfSpeech = new List<PartOfSpeech>(4);

            // Part of Speech
            public class PartOfSpeech
            {
                public int type;

                public string word;

                public List<string> forms;
            }
        }
    }

    enum WORD
    {
        NOUN = 0,
        VERB = 1,
        ADJECTIVE = 2,
        MISC = 3
    }

    
}