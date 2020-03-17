using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class SavedSentence
{
    // Story id;
    public Guid id;

    // Author
    public string user;

    // List of tiles
    public List<Word> words;

    //
    public SavedSentence(Guid id, string user, List<Word> words)
    {
        //
        this.id = id;
        this.user = user;
        this.words = words;
    }
}