using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateWordBank : MonoBehaviour
{
    public GameObject wordBank;

    
    //public GameObject packFilters;
    // pull any changes to words/contextpacks from wordriver
    public void UpdateWords()
    {
        // send get request to server for updated learnerdata and then set up the wordbank
        wordBank.GetComponent<BuildWorldBankNew>().UpdateWordBank();
    }
}
