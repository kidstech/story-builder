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

        // How many indexes we have to offset
        int offset = 3;

        // Get all the json in the "packs" directory
        string[] contextPacks = Directory.GetFiles(Application.dataPath + "/packs/", "*.json");

        // For every .json we find in our context packs folder
        // (For every context pack)
        for (int contextPackId = 0; contextPackId < contextPacks.Length; contextPackId++)
        {
            string raw_json = File.ReadAllText(contextPacks[contextPackId]);
            JSONObject cp = new JSONObject(raw_json);

            //If the context pack is enabled
            if (cp.list[2] == true)
            {

                // Add this to our list of context packs
                w.addConextPackToList(contextPackId, cp.list[0].str, cp.list[1].str);

                //Get the number of word packs
                int numWordPacks = cp.list.Count - offset;

                //Loop through each word pack
                for (int wordPackId = 0; wordPackId < numWordPacks; wordPackId++)
                {
                    //Check if word pack is enabled
                    if (cp.list[offset + wordPackId].list[0] == true)
                    {
                        int numNouns = cp.list[offset + wordPackId].list[1].Count;
                        int numVerbs = cp.list[offset + wordPackId].list[2].Count;
                        int numAdjectives = cp.list[offset + wordPackId].list[3].Count;
                        int numMisc = cp.list[offset + wordPackId].list[4].Count;

                        int[] numEach = new int[4] { numNouns, numVerbs, numAdjectives, numMisc };

                        //For all the nouns, verbs, adjectives, miscs
                        for (int partOfSpeechId = 0; partOfSpeechId < 4; partOfSpeechId++)
                        {
                            // For every word in current 'focus' (which type we are moving through: nouns, verbs, adj, etc)
                            for (int k = 0; k < numEach[partOfSpeechId]; k++)
                            {
                                // Get the base word
                                string baseWord = cp.list[offset + wordPackId].list[partOfSpeechId + 1].list[k].list[0].str;

                                //Get the number of forms a word has
                                int numForms = cp.list[offset + wordPackId].list[partOfSpeechId + 1].list[k].list[1].Count;

                                List<string> forms = new List<string>();

                                //Put all the forms into a string
                                for (int p = 0; p < numForms; p++)
                                {
                                    forms.Add(cp.list[offset + wordPackId].list[partOfSpeechId + 1].list[k].list[1].list[p].str);
                                }

                                // Add that into our list
                                w.addWordToList(contextPackId, wordPackId, partOfSpeechId, baseWord, forms);
                            }
                        }
                    }
                }
            }
        }

        // Sort the list
        w.basicSort();

        // Return the word list
        return w;
    }
}
