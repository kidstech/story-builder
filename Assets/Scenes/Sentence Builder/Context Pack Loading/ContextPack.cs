using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class ContextPack
{
    /*
     * What are we keeping track of?
     * =============================
     * context pack id
     * context pack name
     * context pack icon path
     */
    public string _id;

    public string schema = "https://raw.githubusercontent.com/kidstech/story-builder/master/Assets/packs/schema/pack.schema.json";
    public string name;
    public string icon;
    public bool enabled;
    public List<WordList> wordlists;
    [NonSerialized]
    public Sprite image;

    public ContextPack(string id, string schema, string name, string icon, bool enabled, List<WordList> cWordLists)
    {
        this._id = id;
        this.schema = schema;
        this.name = name;
        this.icon = icon;
        this.enabled = enabled;
        this.wordlists = cWordLists;
    }
}