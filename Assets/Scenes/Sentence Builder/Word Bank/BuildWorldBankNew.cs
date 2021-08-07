using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildWorldBankNew : MonoBehaviour
{
    //
    private readonly int TILE_HEIGHT = 50;
    private readonly int TILES_PER_ROW = 6;

    // Class that holds all of the words
    private List<Word> words;

    // Prefab for word tile to be stored in word bank columns
    public GameObject wordTile;
    // game object storing the SetupPackFilter script
    public GameObject ContextPackFilter;

    //
    private void Start()
    {
        // get the user from the server and then set up the packs
        StartCoroutine(ServerRequestHandler.GetLearnerContextPacks(LearnerLogin.staticLearner._id, setupWordBankAndPackFilterButtons));
        setupWordBankAndPackFilterButtons();
    }
    // same as Start, but callable elsewhere
    public void UpdateWordBank()
    {
        StartCoroutine(ServerRequestHandler.GetLearnerContextPacks(LearnerLogin.staticLearner._id, setupWordBankAndPackFilterButtons));
    }

    private void setupWordBankAndPackFilterButtons()
    {
        // clear old words tiles (game objects)
        if (words != null)
        {
            words.Clear();
            foreach (Transform child in this.transform)
            {
                // return objects to pool
                child.gameObject.SetActive(false);
            }
        }
        LoadContextPacks.StoreContextPacks();
        words = LoadContextPacks.loadWords();
        // activate the contextpackfilter game object now that we've downloaded the learnercontextpacks
        //ContextPackFilter.SetActive(true);
        // Get the word bank's transform object
        RectTransform bank = (RectTransform)this.transform;

        // Calculate the number of rows
        // words.Count needs to be stored as a float or else the following numOfRows computation is processed as an integer before it's ever rounded (effectively rounding down)
        // this is why the scrollbar didn't reach the last row of tiles
        float numTiles = words.Count;
        int numOfRows = Mathf.CeilToInt(numTiles / TILES_PER_ROW);

        // Reset Size of the wordbank
        bank.sizeDelta = new Vector2(bank.sizeDelta.x, TILE_HEIGHT * numOfRows);

        // Pre-define colors for nouns, verbs, adj, and extra parts of speech
        Color[] colors = { new Color(0.357f, 0.608f, 0.835f), new Color(0.439f, 0.678f, 0.278f), new Color(0.929f, 0.49f, 0.192f), new Color(1f, 0.753f, 0f) };
        // For every word
        for (int word = 0; word < words.Count; word++)
        {
            // Add a new tile in
            GameObject wordBankTile = WordTileObjectPool.SharedInstance.GetPooledTile();
            wordBankTile.SetActive(true);

            // Change its color based on what type of word it is.
            wordBankTile.GetComponent<Image>().color = colors[words[word].partOfSpeechId];

            // Get the word we will be inserting into the tile
            wordBankTile.GetComponent<WordTile>().SetUpTile(words[word]);

            // Set it as a child of the word slot
            wordBankTile.transform.SetParent(this.transform, false);
        }

        //
        GetComponent<WordBank>().SortWordBank();
        // set up pack filter buttons now that words and context packs have been grabbed from the server
        ContextPackFilter.GetComponent<SetupPackFilter>().SetUpPacks();
    }
}
