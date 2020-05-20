using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class SavedStory
{
    // Story id;
    public Guid id;

    // Author
    public string user;

    // List of tiles
    public List<SavedSentence> sentences;

    //
    public SavedStory(Guid id, string user, List<SavedSentence> sentences)
    {
        //
        this.id = id;
        this.user = user;
        this.sentences = sentences;
    }
}