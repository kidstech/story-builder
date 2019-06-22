using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buildWordBank : MonoBehaviour
{
    public MasterWordList w = new MasterWordList();

    // Number of words stored vertically in a column
    public static readonly int WORDS_PER_COLUMN = 4;

    // Columns that are visible by default without any scrolling
    public static readonly int VISIBLE_COLUMNS_WITHOUT_SCROLL = 6;

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

    // Start is called before the first frame update
    void Start()
    {
        w = LoadContextPacks.loadContextPacks();

        setupWordBank();
    }

    private void setupWordBank()
    {
        totalWords = w.countTotalWords();

        totalColumns = calculateTotalColumns();

        buildWordBankColumns();

        populateWordTiles();
    }

    private int calculateTotalColumns()
    {
        int numColumns = 0;

        numColumns = totalWords / WORDS_PER_COLUMN;

        if(totalWords % WORDS_PER_COLUMN != 0)
        {
            numColumns++;
        }

        return numColumns;
    }

    private void buildWordBankColumns()
    {
        for(int i = 0; i < VISIBLE_COLUMNS_WITHOUT_SCROLL; ++i)
        {
            wordBankColumns.Add(this.transform.GetChild(i));
        }

        if(totalColumns > VISIBLE_COLUMNS_WITHOUT_SCROLL)
        {
            for(int i = 0; i < totalColumns - VISIBLE_COLUMNS_WITHOUT_SCROLL; ++i)
            {
                wordBankColumn = Instantiate(wordBankColumn);
                RectTransform bank = (RectTransform)this.transform;
                bank.sizeDelta = new Vector2(bank.sizeDelta.x + COLUMN_WIDTH, bank.sizeDelta.y);
                wordBankColumn.SetParent(this.transform, false);
                wordBankColumns.Add(wordBankColumn);
            }
        }
    }

    private void populateWordTiles()
    {
        Color[] colors = { new Color(0.357f, 0.608f, 0.835f), new Color(0.439f, 0.678f, 0.278f), new Color(0.929f, 0.49f, 0.192f), new Color(1f, 0.753f, 0f) };

        int currentWord = 0;

        int currentColumn = -1;

        int currentWordSlotInColumn = -1;

        for(int type = 0; type < 4; type++)
        {
            for(int word = 0; word < w.parseWordType(type).Count; word++)
            {
                if(currentWord % 4 == 0)
                {
                    currentColumn++;
                    currentWordSlotInColumn = -1;
                }

                Transform wordSlot = wordBankColumns[currentColumn].transform.GetChild(++currentWordSlotInColumn);

                Transform wordBankTile = Instantiate(this.wordTile);

                wordBankTile.GetComponent<Image>().color = colors[type % 4];

                Text wordTileText = wordBankTile.GetChild(0).transform.GetComponent<Text>();

                wordTileText.text = w.parseWordType(type)[word][0];

                wordBankTile.SetParent(wordSlot, false);

                currentWord++;
            }
        }
    }
}