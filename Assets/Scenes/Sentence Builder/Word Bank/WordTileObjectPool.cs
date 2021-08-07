using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTileObjectPool : MonoBehaviour
{
    public static WordTileObjectPool SharedInstance;
    public List<GameObject> pooledWordTiles;
    // wordTile prefab
    public GameObject wordTile;
    public int numPoolTiles;
    void Awake()
    {
        SharedInstance = this;
        // not sure how many word tiles we expect to have active to start... on average
        numPoolTiles = 200;
    }

    void Start()
    {
        pooledWordTiles = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < numPoolTiles; i++)
        {
            tmp = Instantiate(wordTile);
            tmp.SetActive(false);
            pooledWordTiles.Add(tmp);
        }
    }

    // get first inactive word tile from pool
    public GameObject GetPooledTile()
    {
        for (int i = 0; i < numPoolTiles; i++)
        {
            if (!pooledWordTiles[i].activeInHierarchy)
            {
                return pooledWordTiles[i];
            }
        }
        // if there aren't any inactive tiles available
        // make a new one and return it
        GameObject extraTile = Instantiate(wordTile);
        pooledWordTiles.Add(extraTile);
        extraTile.SetActive(false);
        return pooledWordTiles[pooledWordTiles.Count - 1];
    }

}
