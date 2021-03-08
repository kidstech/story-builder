using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LoadContextPacks
{
    //
    public static List<Word> wordList = new List<Word>();

    //
    public static List<ContextPack> contextPackList = new List<ContextPack>();

    //
    public static List<Word> loadWords()
    {
        // Get all the json in the "packs" directory
        string[] contextPacks = Directory.GetFiles(Path.Combine(Application.dataPath, "packs"), "*.json");

        // What are the categories of words we know will be in the JSON? (nouns, verbs, adjectives, misc)
        List<string> wordTypes = new List<string>() {"nouns", "verbs", "adjectives", "misc"};

        // For every .json we find in our context packs folder
        // (For every context pack)
        for (int contextPackId = 0; contextPackId < contextPacks.Length; contextPackId++)
        {
            string raw_json = File.ReadAllText(contextPacks[contextPackId]);
            JSONObject cp = new JSONObject(raw_json);

            // If the context pack is enabled
            if (cp["enabled"] == true)
            {
                // Loop through each word pack
                foreach (JSONObject wordpack in cp["wordpacks"]) 
                {
                    //Check if word pack is enabled
                    if (wordpack["enabled"] == true)
                    {
                        int whichWordTypeIndex = 0;
                        foreach (string wordType in wordTypes) {
                            JSONObject words = wordpack[wordType];
                            foreach (var word in words) {
                                // Get the base word
                                string baseWord = word["word"].str;

                                //Put all the forms into a list of strings
                                List<string> forms = new List<string>();
                                foreach (JSONObject form in word["forms"]) {
                                    forms.Add(form.str);
                                }

                                // Add that word (and all its forms) into our list
                                AddWord(contextPackId, whichWordTypeIndex, baseWord, forms);
                            }
                            whichWordTypeIndex++;
                        }
                    }
                }
            }
        }

        return wordList;
    }

    //
    public static List<ContextPack> loadContextPacks()
    {
        // Get all the json in the "packs" directory
        string[] contextPacks = Directory.GetFiles(Application.dataPath + "/packs/", "*.json");

        // For every .json we find in our context packs folder
        // (For every context pack)
        for (int contextPackId = 0; contextPackId < contextPacks.Length; contextPackId++)
        {
            string raw_json = File.ReadAllText(contextPacks[contextPackId]);
            JSONObject cp = new JSONObject(raw_json);

            //If the context pack is enabled
            if (cp["enabled"] == true)
            {

                // Add this to our list of context packs
                AddContextPack(contextPackId, cp["name"].str, contextPacks[contextPackId].Substring(0, contextPacks[contextPackId].Length - 5));
            }
        }

        //
        return contextPackList;
    }

    //
    private static void AddContextPack(int contextPackId, string contextPackName, string contextPackIconPath)
    {
        // Create a new context pack object
        ContextPack c = new ContextPack();

        // Populate it with the information given
        c.contextPackId = contextPackId;
        c.contextPackName = contextPackName;
        c.contextPackIconPath = contextPackIconPath;

        // Add the context pack into the context pack list
        contextPackList.Add(c);
    }

    //
    private static void AddWord(int contextPackId, int partOfSpeechId, string word, List<string> forms)
    {
        // Only add one instance of a word
        for (int i = 0; i < wordList.Count; i++)
        {
            //
            if (wordList[i].word == word)
            {
                //
                return;
            }
        }

        // Create a new word object
        Word w = new Word();

        // Populate the information we need
        w.contextPackId = contextPackId;
        w.partOfSpeechId = partOfSpeechId;
        w.word = word;
        w.forms = forms;

        // Add the word into the word list
        wordList.Add(w);
    }
}