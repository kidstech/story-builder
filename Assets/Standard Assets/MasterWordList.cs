using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MasterWordList
{
    public List<List<string>> nouns = new List<List<string>>();
    public List<List<string>> verbs = new List<List<string>>();
    public List<List<string>> adjectives = new List<List<string>>();
    public List<List<string>> misc = new List<List<string>>();
    
    public void AddToList(int type, List<string> forms)
    {
        parseWordType(type).Add(forms);
    }

    public int countTotalWords()
    { 
        return nouns.Count + verbs.Count + adjectives.Count + misc.Count;
    }

    public List<List<string>> parseWordType(int type)
    {
        switch(type)
        {
            case 0:
                return nouns;

            case 1:
                return verbs;

            case 2:
                return adjectives;


            case 3:
                return misc;
        }

        return null;
    }
}