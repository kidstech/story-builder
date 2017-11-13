/// <summary>
/// Word bank represents all words that can be used for building sentences. All words are loaded in from the master context pack file.
/// 
/// <author> antin006@morris.umn.edu </author>
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharpfirstpass;
using UnityEngine.UI;


public class WordBank : MonoBehaviour {

	// Number of words stored vertically in a column
	public static readonly int WORDS_PER_COLUMN = 4;

	// Columns that are visible by default without any scrolling
	public static readonly int VISIBLE_COLUMNS_WITHOUT_SCROLL = 6;

	// Width of each word bank column
	public static readonly int COLUMN_WIDTH = 125;

	// Should we rebuild from the master context pack JSON file?
	public static readonly bool REBUILD_MASTER_CONTEXT_PACK_JSON = false;

	// Total number of words in word bank
	public static int totalWords = 0;

	// Total number of columns in word bank
	public static int totalColumns;

	// All context packs shown in word bank
	public static ContextPack[] contextPacks;

	// List of all word bank columns
	public static List<Transform> wordBankColumns = new List<Transform>();

	// Prefab for word bank column
	public Transform wordBankColumn;

	// Prefab for word tile to be stored in word bank columns
	public Transform wordTile;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {

		// If the master context pack should be rebuilt
		if (REBUILD_MASTER_CONTEXT_PACK_JSON) {
			ContextPackFactory.buildContextPacks ();
		}
		// Initialize word bank with context packs from master context pack file
		setupWordBank ();
	}


	/// <summary>
	/// Sets up the word bank by loading in all context pack words.
	/// </summary>
	private void setupWordBank(){

		// Load in all context packs
		contextPacks = ContextPackFactory.loadContextPacks ();

		// Calculate total number of words
		for (int contextPack = 0; contextPack < contextPacks.Length; ++contextPack)			
			totalWords += contextPacks[contextPack].words.Length;		

		// Determine the number of word bank columns
		totalColumns = calculateTotalColumns ();

		// Retrieve and build word bank columns
		buildWordBankColumns();

		// Populate word tiles within word bank columns
		populateWordTiles ();
	}

	/// <summary>
	/// Calculates the total number of columns needed to construct the word bank.
	/// </summary>
	/// <returns>Total columns of word bank</returns>
	private int calculateTotalColumns(){

		// Calculate the total number of words
		int numberOfWords = 0;
		for (int i = 0; i < contextPacks.Length; ++i)
			numberOfWords += contextPacks [i].words.Length;

		// Calculate the total number of columns
		int numberOfColumns = numberOfWords / WORDS_PER_COLUMN;

		// Determine if an extra column is needed
		if (numberOfWords % WORDS_PER_COLUMN != 0)
			numberOfColumns++;

		return numberOfColumns;
	}

	/// <summary>
	/// Builds the word bank columns.
	/// Does not place word tiles within columns.
	/// </summary>
	private void buildWordBankColumns(){

		// Get references to default visible columns
		for (int i = 0; i < VISIBLE_COLUMNS_WITHOUT_SCROLL; ++i) 
			wordBankColumns.Add (this.transform.GetChild (i));

		// If extra columns are needed
		if (totalColumns > VISIBLE_COLUMNS_WITHOUT_SCROLL) {
			// Then build the extra columns
			for (int i = 0; i < totalColumns - VISIBLE_COLUMNS_WITHOUT_SCROLL; ++i) {
				wordBankColumn = Instantiate (wordBankColumn);
				RectTransform bank = (RectTransform)this.transform;
				bank.sizeDelta = new Vector2 (bank.sizeDelta.x + COLUMN_WIDTH, bank.sizeDelta.y);
				wordBankColumn.SetParent (this.transform, false);
				wordBankColumns.Add (wordBankColumn);
			}
		}
	}

	/// <summary>
	/// Populates the word tiles by filling each of the word bank columns.
	/// </summary>
	private void populateWordTiles(){

		// Some default colors for tiles
		Color[] colors = { new Color(0.357f,0.608f,0.835f), new Color(0.439f,0.678f,0.278f), new Color(0.929f,0.49f,0.192f), new Color(1f,0.753f,0f) };

		// Current word from all words from all context packs
		int currentWord = 0;

		// Current word bank column 
		int currentColumn = -1;

		// Current word tile within current word bank column
		int currentWordSlotInColumn = -1;

		// Iterate over all context packs
		for (int contextPack = 0; contextPack < contextPacks.Length; contextPack++) {

			// Iterate over all words within current context pack
			for (int word = 0; word < contextPacks [contextPack].words.Length; word++) {

				// If 4 words have been added to the current column
				if (currentWord % 4 == 0) {
					// The move onto the next column
					currentColumn++;
					currentWordSlotInColumn = -1;
				}

				// Grab the word slot within the current column
				Transform wordSlot = wordBankColumns [currentColumn].transform.GetChild (++currentWordSlotInColumn);

				// Create a new word tile to insert into the current column
				Transform wordBankTile = Instantiate (this.wordTile);

				// Set color of tile from the current context pack
				wordBankTile.GetComponent<Image> ().color = colors [contextPack % 4];

				// Grab the text component from the word tile
				Text wordTileText = wordBankTile.GetChild (0).transform.GetComponent<Text> ();

				// Set the word tile text to the current word from the context pack
				wordTileText.text = contextPacks [contextPack].words [word];

				// Set the parent of the word tile to the word slot
				wordBankTile.SetParent (wordSlot, false);

				// Move onto next word
				++currentWord;

			}
		}
	}

}