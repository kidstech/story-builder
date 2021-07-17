using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class SavedSentence
{
    public string id;
    public string user;
    public List<Word> words;
    // need this extra list because we don't actually ever update the "word" using the wordholder (we update the text component instead) , so we need to capture that elsewhere
    public List<string> selectedWordForms;
    public string sentenceText;

    //
    public SavedSentence(string id, string user, List<Word> words, string sentenceText, List<string> selectedWordForms)
    {
        this.id = id;
        this.user = user;
        this.words = words;
        this.sentenceText = sentenceText;
        this.selectedWordForms = selectedWordForms;
    }
}