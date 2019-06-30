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

        //Get all the json in the "packs" directory
        string[] contextPacks = Directory.GetFiles(Application.dataPath + "/packs/", "*.json");

        //For every .json we find in our context packs folder
        for (int i = 0; i < contextPacks.Length; i++)
        {
            string raw_json = File.ReadAllText(contextPacks[i]);
            JSONObject cp = new JSONObject(raw_json);

            //If the context pack is enabled
            if (cp.list[1] == true)
            {
                //Get the number of word packs
                int numWordPacks = cp.list.Count - 2;

                //Loop through each word pack
                for (int o = 0; o < numWordPacks; o++)
                {
                    //Check if word pack is enabled
                    if (cp.list[2 + o].list[0] == true)
                    {
                        int numNouns = cp.list[2 + o].list[1].Count;
                        int numVerbs = cp.list[2 + o].list[2].Count;
                        int numAdjectives = cp.list[2 + o].list[3].Count;
                        int numMisc = cp.list[2 + o].list[4].Count;

                        int[] numEach = new int[4] { numNouns, numVerbs, numAdjectives, numMisc };

                        //For all the nouns, verbs, adjectives, miscs
                        for (int round = 0; round < 4; round++)
                        {
                            // For every word in current 'focus' (which type we are moving through: nouns, verbs, adj, etc)
                            for (int k = 0; k < numEach[round]; k++)
                            {
                                //Get the number of forms a word has
                                int numForms = cp.list[2 + o].list[round + 1].list[k].list[1].Count;

                                List<string> forms = new List<string>();

                                //Put all the forms into a string
                                for (int p = 0; p < numForms; p++)
                                {
                                    forms.Add(cp.list[2 + o].list[round + 1].list[k].list[1].list[p].str);
                                }

                                //Add the string to the master word list
                                w.AddToList(round, forms);
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
