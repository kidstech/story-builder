using UnityEngine;

[System.Serializable]
public class Story
{
    //
    public string name;
    public string story;
    public Sprite image;

    //
    private string version = "1.0.0";

    //
    public Story(string name, string story, Sprite image)
    {
        //
        this.name = name;
        this.story = story;
        this.image = image;
    }
}