﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buildWordBank : MonoBehaviour
{
    // Number of words stored vertically in a column
    public static readonly int WORDS_PER_COLUMN = 4;

    // Columns that are visible by default without any scrolling
    public static readonly int VISIBLE_COLUMNS_WITHOUT_SCROLL = 4;

    // Width of each word bank column
    public static readonly int COLUMN_WIDTH = 175;

    // Total number of words in word bank
    public static int totalWords = 0;

    // Total number of columns in word bank
    public static int totalColumns;

    // List of all word bank columns
    public static List<Transform> wordBankColumns = new List<Transform>();

    // Prefab for word bank column
    public Transform wordBankColumn;

    // Prefab for word tile to be stored in word bank columns
    public Transform wordTile;

    /// <summary>
    /// //////////////////////////////////
    /// </summary>
    /// 
    List<MasterWordList.Word> currentWordPool;
    MasterWordList w;

    // Start is called before the first frame update
    void Start()
    {
        // Load all the words into the master word list
        w = LoadContextPacks.loadContextPacks();

        // Get all the words
        currentWordPool = w.masterWordList;

        // Begin setting up the word bank
        setupWordBank();
    }

    private void setupWordBank()
    {
        // Count the total number of words in the master word list
        totalWords = currentWordPool.Count;

        // Calculate the total columns we'll need
        totalColumns = calculateTotalColumns();

        // Structure the columns and add them into the word bank
        buildWordBankColumns2();

        // Add tiles with names to the word bank
        populateWordTiles();
    }

    private int calculateTotalColumns()
    {
        // Find how many columns we'll need need
        int numColumns = totalWords / WORDS_PER_COLUMN;

        // Account for if the number of words aren't divisable by the number of words per column.
        if (totalWords % WORDS_PER_COLUMN != 0)
        {
            // Add extra column if that's the case
            numColumns++;
        }

        // Return the number of columns created
        return numColumns;
    }

    private void buildWordBankColumns2()
    {
        // Get the word bank's transform object
        RectTransform bank = (RectTransform)this.transform;

        // Reset Size of the wordbank
        bank.sizeDelta = new Vector2(COLUMN_WIDTH * totalColumns, bank.sizeDelta.y);

        for (int i = 0; i < totalColumns; i++)
        {
            // Create new column
            Transform newColumn = Instantiate(wordBankColumn);

            // Set it as a parent
            newColumn.SetParent(this.transform, false);

            // Set the name with offset
            newColumn.name = "WordBankColumn (" + (i) + ")";

            // Add it into the list of columns
            wordBankColumns.Add(newColumn);
        }
    }

    private void populateWordTiles()
    {
        // Pre-define colors for nouns, verbs, adj, and extra parts of speech
        Color[] colors = { new Color(0.357f, 0.608f, 0.835f), new Color(0.439f, 0.678f, 0.278f), new Color(0.929f, 0.49f, 0.192f), new Color(1f, 0.753f, 0f) };
        Color[] suplementary = { new Color(0.165f, 0.476f, 1.0f), new Color(0.354f, 0.547f, 0.0f), new Color(0.820f, 0.331f, 0.0f), new Color(0.82f, 0.618f, 0.0f) };

        // Which word we are on
        int currentWord = 0;

        // Which column are we on
        int currentColumn = -1;

        // Which position in the column we are on
        int currentWordSlotInColumn = -1;

        // For every word
        for(int word = 0; word < totalWords; word++)
        {
            // Make sure we are constantly updating which column and slot we are on as we move through
            if (currentWord % 4 == 0)
            {
                currentColumn++;
                currentWordSlotInColumn = -1;
            }

            // Get the slot this tile will go into
            Transform wordSlot = wordBankColumns[currentColumn].transform.GetChild(++currentWordSlotInColumn);

            // Add a new tile in
            Transform wordBankTile = Instantiate(this.wordTile);

            // Change its color based on what type of word it is.
            wordBankTile.Find("Background").GetComponent<Image>().color = colors[currentWordPool[word].partOfSpeechId];
            wordBankTile.Find("Highlight").GetComponent<Image>().color = suplementary[currentWordPool[word].partOfSpeechId];

            // Get the word we will be inserting into the tile
            string wordToBeInserted = currentWordPool[word].word;

            // Get the text component of the tile
            Text wordTileText = wordBankTile.Find("Text").transform.GetComponent<Text>();

            // Set it equal to the base case of the word
            wordTileText.text = wordToBeInserted;

            // Add in the rest of the forms
            populateWordForms(wordBankTile, currentWordPool[word].forms);

            // Set it as a child of the word slot
            wordBankTile.SetParent(wordSlot, false);

            // Move to the next word in the list
            currentWord++;
        }
    }

    private void populateWordForms(Transform wordBankTile, List<string> forms)
    {
        // Get the script component from the tile
        WordTile access = wordBankTile.GetComponent<WordTile>();

        // Assign it the forms we've given
        access.wordForms = forms;
    }

    public void rebuildWordBank(List<MasterWordList.Word> list)
    {
        foreach(Transform childTransform in this.transform)
        {
            Destroy(childTransform.gameObject);
        }

        wordBankColumns = new List<Transform>();

        currentWordPool = list;

        setupWordBank();
    }
}