﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class SavedSentence
{
    public string mongoObjectId;
    public string sentenceId;
    public string sentenceText;
    public string timeSubmitted;
    public string learnerId;
    public List<Word> words;
    // need this extra list because we don't actually ever update the "word" using the wordholder (we update the text component instead) , so we need to capture that elsewhere
    public List<string> selectedWordForms;
    public string userId;
    public List<string> contextPackIds;
    public bool deleted;

    public SavedSentence(string sentenceId, string sentenceText, String timeSubmitted, string learnerId, List<Word> words, List<string> selectedWordForms, string userId)
    {
        this.sentenceId = sentenceId;
        this.sentenceText = sentenceText;
        this.timeSubmitted = timeSubmitted;
        this.learnerId = learnerId;
        this.words = words;
        this.selectedWordForms = selectedWordForms;
        this.userId = userId;
        this.deleted = false;
        GetContextPackIds();
    }
    private void GetContextPackIds()
    {
        List<string> packIds = new List<string>();
        foreach (ContextPack pack in ContextPackHandler.activeContextPacks)
        {
            foreach (Word word in this.words)
            {
                if (word.contextPackId == pack._id && !packIds.Contains(pack._id))
                {
                    packIds.Add(pack._id);
                }
            }
        }
        this.contextPackIds = packIds;
    }
}