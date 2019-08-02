using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortPack : MonoBehaviour
{
    public GameObject sortConextPackObject;

    public GenerateSortPacks p;

    public int contextPackId;
    public string contextPackName;
    public string contextPackIconPath;

    void Start()
    {
        sortConextPackObject = GameObject.Find("SortPack");

        p = sortConextPackObject.GetComponent<GenerateSortPacks>();
    }

    public void togglePack()
    {
        p.updateSearchPack(this.gameObject);
    }

}
