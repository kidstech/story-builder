using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LoadContextPacks
{
    public static MasterWordList loadContextPacks()
    {
        MasterWordList w = new MasterWordList();
        MasterWordList.ContextPack c = new MasterWordList.ContextPack();
        MasterWordList.ContextPack.WordPack wp = new MasterWordList.ContextPack.WordPack();
        MasterWordList.ContextPack.WordPack.PartOfSpeech pos = new MasterWordList.ContextPack.WordPack.PartOfSpeech();

        // How many indexes we have to offset
        int offset = 3;

        //Get all the json in the "packs" directory
        string[] contextPacks = Directory.GetFiles(Application.dataPath + "/packs/", "*.json");

        //For every .json we find in our context packs folder
        for (int i = 0; i < contextPacks.Length; i++)
        {
            string raw_json = File.ReadAllText(contextPacks[i]);
            JSONObject cp = new JSONObject(raw_json);

            //If the context pack is enabled
            if (cp.list[2] == true)
            {
                //Get the number of word packs
                int numWordPacks = cp.list.Count - offset;

                // We have everything we need to fill up the context pack
                c.setUpContextPack("Hi", cp.list[0].str, numWordPacks);

                //Loop through each word pack
                for (int o = 0; o < numWordPacks; o++)
                {
                    Debug.Log(cp.keys[3]);

                    //Check if word pack is enabled
                    if (cp.list[offset + o].list[0] == true)
                    {
                        int numNouns = cp.list[offset + o].list[1].Count;
                        int numVerbs = cp.list[offset + o].list[2].Count;
                        int numAdjectives = cp.list[offset + o].list[3].Count;
                        int numMisc = cp.list[offset + o].list[4].Count;

                        int[] numEach = new int[4] { numNouns, numVerbs, numAdjectives, numMisc };

                        //For all the nouns, verbs, adjectives, miscs
                        for (int round = 0; round < 4; round++)
                        {
                            // Set the size of the resulting list


                            // For every word in current 'focus' (which type we are moving through: nouns, verbs, adj, etc)
                            for (int k = 0; k < numEach[round]; k++)
                            {
                                // Get the base word
                                string baseWord = cp.list[offset + o].list[round + 1].list[k].list[0].str;

                                //Get the number of forms a word has
                                int numForms = cp.list[offset + o].list[round + 1].list[k].list[1].Count;

                                List<string> forms = new List<string>();

                                //Put all the forms into a string
                                for (int p = 0; p < numForms; p++)
                                {
                                    forms.Add(cp.list[offset + o].list[round + 1].list[k].list[1].list[p].str);
                                }
                            }
                        }
                    }
                }
            }
        }

        //Return the word list
        return w;
    }
}
