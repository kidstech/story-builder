using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildWorldBankNew : MonoBehaviour
{
    //
    private readonly int TILE_HEIGHT = 75;
    private readonly int TILES_PER_ROW = 5;

    // Class that holds all of the words
    private List<Word> words;

    // Prefab for word tile to be stored in word bank columns
    public Transform wordTile;

    //
    private void Start()
    {
        // Load all words into our holder class
        words = LoadContextPacks.loadWords();

        // Begin setting up the word bank
        setupWordBank();
    }

    //
    private void setupWordBank()
    {
        // Get the word bank's transform object
        RectTransform bank = (RectTransform)this.transform;

        // Calculate the number of rows
        int numOfRows = Mathf.FloorToInt(words.Count / TILES_PER_ROW);

        // Reset Size of the wordbank
        bank.sizeDelta = new Vector2(bank.sizeDelta.x, TILE_HEIGHT * numOfRows);

        // Pre-define colors for nouns, verbs, adj, and extra parts of speech
        Color[] colors = { new Color(0.357f, 0.608f, 0.835f), new Color(0.439f, 0.678f, 0.278f), new Color(0.929f, 0.49f, 0.192f), new Color(1f, 0.753f, 0f) };
        // For every word
        for (int word = 0; word < words.Count; word++)
        {
            // Add a new tile in
            Transform wordBankTile = Instantiate(this.wordTile);

            // Change its color based on what type of word it is.
            wordBankTile.GetComponent<Image>().color = colors[words[word].partOfSpeechId];

            // Get the word we will be inserting into the tile
            wordBankTile.GetComponent<WordTile>().SetUpTile(words[word]);

            // Set it as a child of the word slot
            wordBankTile.SetParent(this.transform, false);
        }

        //
        GetComponent<WordBank>().SortWordBank();
    }
}
