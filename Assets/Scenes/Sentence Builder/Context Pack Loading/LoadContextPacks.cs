using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;

public class LoadContextPacks
{
    //
    public static List<Word> wordList = new List<Word>();

    // context pack list generated from loadcontextpacks
    public static List<ContextPack> contextPackList = new List<ContextPack>();
    // learner specific context packs grabbed from the server
    public static List<ServerContextPack> serverPacks = new List<ServerContextPack>();
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
            if (cp["enabled"])// == true && ServerPacksContainsContextPack(cp))
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
    // helper function that checks whether the jsonObject grabbed from one of the .json files in our packs directory
    public static bool ServerPacksContainsContextPack(JSONObject jSONObject)
    {
        // check through all the learners contextpacks...
        foreach (ServerContextPack contextPack in serverPacks)
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
        Debug.Log(dirPath);
        // clear list before loading to prevent pack duplication
        contextPackList.Clear();
        // Get all the json in the "packs" directory
        //string[] contextPacks = Directory.GetFiles(Application.dataPath + "/packs/", "*.json");
        string[] contextPacks = Directory.GetFiles(dirPath, "*.json");
        Debug.Log("number of files: " + contextPacks.Length);
        // For every .json we find in our context packs folder
        // (For every context pack)
        for (int contextPackId = 0; contextPackId < contextPacks.Length; contextPackId++)
        {
            string raw_json = File.ReadAllText(contextPacks[contextPackId]);
            Debug.Log(raw_json);
            JSONObject cp = new JSONObject(raw_json);

            //If the context pack is enabled
            if (cp["enabled"] == true)
            {
                Debug.Log("adding context pack...");
                //iterate through the context packs associated with the current learner
                foreach (ServerContextPack contextPack in serverPacks)
                {
                    // if the learner has this context pack in their list of enabled context packs
                    if (contextPack.name.Equals(cp["name"].str))
                    {
                        //Debug.Log(cp["name"].str + " added");
                        // Add this to our list of context packs
                        Debug.Log(cp["name"]);
                        AddContextPack(contextPackId, cp["name"].str, raw_json.Substring(0, raw_json.Length - 5));
                    }
                }
            }
        }



        // // For every .json we find in our context packs folder
        // // (For every context pack)
        // for (int contextPackId = 0; contextPackId < contextPacks.Length; contextPackId++)
        // {
        //     string raw_json = File.ReadAllText(contextPacks[contextPackId]);
        //     JSONObject cp = new JSONObject(raw_json);

        //     //If the context pack is enabled
        //     if (cp["enabled"] == true)
        //     {
        //         // iterate through the context packs associated with the current learner
        //         // foreach (ServerContextPack contextPack in serverPacks)
        //         // {
        //         //     // if the learner has this context pack in their list of enabled context packs
        //         //     if (contextPack.name.Equals(cp["name"].str))
        //         //     {
        //         //         //Debug.Log(cp["name"].str + " added");
        //         //         // Add this to our list of context packs
        //                  AddContextPack(contextPackId, cp["name"].str, contextPacks[contextPackId].Substring(0, contextPacks[contextPackId].Length - 5));
        //         //     }
        //         // }
        //     }
        // }

        //
        return contextPackList;
    }

    public static void StoreContextPacks()
    {
        Debug.Log("storing context packs in: " + dirPath);
        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
        string filePath = "";
        string jsonContextPack = "";
        foreach (ServerContextPack contextPack in serverPacks)
        {
            jsonContextPack = JsonConvert.SerializeObject(contextPack);
            filePath = Path.Combine(dirPath, contextPack.name) + ".json";
            Debug.Log("storing context pack: " + contextPack.name);
            File.WriteAllText(filePath, jsonContextPack);
        }
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