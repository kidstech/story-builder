﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;

public class LoadContextPacks
{
    //
    public static List<Word> wordList = new List<Word>();

    // context pack list from server
    public static List<ContextPack> contextPackList = new List<ContextPack>();
    public static List<ContextPack> activeContextPacks = new List<ContextPack>();
    // learner specific context packs grabbed from the server
    //public static List<ServerContextPack> serverPacks = new List<ServerContextPack>();
    public static string dirPath = Path.Combine(Application.persistentDataPath, "Resources", "Packs");

    //
    public static List<Word> loadWords()
    {
        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
        // clear any existing words from previous learners
        wordList.Clear();
        // Get all the json in the "Packs" directory
        string[] contextPacks = Directory.GetFiles(dirPath, "*.json");

        // What are the categories of words we know will be in the JSON? (nouns, verbs, adjectives, misc)
        List<string> wordTypes = new List<string>() { "nouns", "verbs", "adjectives", "misc" };

        // For every .json we find in our context packs folder
        // (For every context pack)
        for (int contextPackId = 0; contextPackId < contextPacks.Length; contextPackId++)
        {
            string raw_json = File.ReadAllText(contextPacks[contextPackId]);
            JSONObject cp = new JSONObject(raw_json);
            // If the context pack is enabled and in the current learner's context packs...
            if (cp["enabled"] == true && LearnerPacksContainThisContextPack(cp))
            {
                // Loop through each word pack
                foreach (JSONObject wordpack in cp["wordlists"])
                {
                    //Check if word pack is enabled
                    if (wordpack["enabled"] == true)
                    {
                        int whichWordTypeIndex = 0;
                        foreach (string wordType in wordTypes)
                        {
                            JSONObject words = wordpack[wordType];
                            foreach (var word in words)
                            {
                                // Get the base word
                                string baseWord = word["word"].str;

                                //Put all the forms into a list of strings
                                List<string> forms = new List<string>();
                                foreach (JSONObject form in word["forms"])
                                {
                                    forms.Add(form.str);
                                }
                                // Add that word (and all its forms) into our list
                                AddWord(JsonConvert.DeserializeObject<ContextPack>(raw_json)._id, whichWordTypeIndex, baseWord, forms);
                            }
                            whichWordTypeIndex++;
                        }
                    }
                }
            }
        }
        return wordList;
    }

    public static bool LearnerPacksContainThisContextPack(JSONObject jSONObject)
    {
        // check through all the learner's contextpacks...
        foreach (ContextPack contextPack in contextPackList)
        {
            // if the "name" field of the json object matches the context pack name...
            if (contextPack.name.Equals(jSONObject["name"].str))
            {
                return true;
            }
        }
        return false;
    }

    //
    public static List<ContextPack> loadContextPacks()
    {
        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
        // clear list before loading to prevent pack duplication
        activeContextPacks.Clear();
        // Get all the json in the "packs" directory
        string[] contextPacks = Directory.GetFiles(dirPath, "*.json");
        // For every .json we find in our context packs folder
        // (For every context pack)
        for (int contextPackId = 0; contextPackId < contextPacks.Length; contextPackId++)
        {
            string raw_json = File.ReadAllText(contextPacks[contextPackId]);
            JSONObject cp = new JSONObject(raw_json);

            //If the context pack is enabled
            if (cp["enabled"] == true)
            {
                //iterate through the context packs associated with the current learner
                foreach (ContextPack contextPack in contextPackList)
                {
                    // if the learner has this context pack in their list of enabled context packs
                    if (contextPack.name.Equals(cp["name"].str))
                    {
                        // make a ContextPack from our local json file
                        ContextPack pack = new ContextPack(cp["_id"].str, cp["schema"].str, cp["name"].str, cp["icon"].str, cp["enabled"], JsonConvert.DeserializeObject<List<WordList>>(cp["wordlists"].ToString()), null);
                        activeContextPacks.Add(pack);
                    }
                }
            }
        }
        return contextPackList;
    }

    public static void StoreContextPacks()
    {
        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
        string filePath = "";
        string jsonContextPack = "";
        foreach (ContextPack contextPack in contextPackList)
        {
            jsonContextPack = JsonConvert.SerializeObject(contextPack);
            filePath = Path.Combine(dirPath, contextPack.name) + ".json";
            File.WriteAllText(filePath, jsonContextPack);
        }
    }

    private static void AddWord(string contextPackId, int partOfSpeechId, string word, List<string> forms)
    {
        // Only add one instance of a word
        for (int i = 0; i < wordList.Count; i++)
        {
            //
            if (wordList[i].baseWord == word)
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
        w.baseWord = word;
        w.forms = forms;

        // Add the word into the word list
        wordList.Add(w);
    }
}