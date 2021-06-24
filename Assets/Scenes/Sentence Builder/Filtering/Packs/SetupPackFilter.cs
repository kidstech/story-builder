using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ServerTypes;
using Newtonsoft.Json;
using System.IO;

public class SetupPackFilter : MonoBehaviour
{
    // The prefab button for filtering
    public GameObject packFilterButton;
    //
    private List<ContextPack> filterByPacks;

    //
    private void Start()
    {
        SetUpPacks();
    }

    public void SetUpPacks()
    {
        // load the packs
        filterByPacks = LoadContextPacks.loadContextPacks();
        // make the sorting buttons for the context packs
        for (int i = 0; i < filterByPacks.Count; i++)
        {
            //
            GameObject sortButton = Instantiate(packFilterButton);

            //
            sortButton.name = filterByPacks[i].contextPackName;

            //
            sortButton.GetComponentInChildren<Text>().text = filterByPacks[i].contextPackName;

            /*
             * Need to try to load an image from the path above ^ 
             */

            //
            sortButton.AddComponent<PackFilterButton>().pack = filterByPacks[i];

            //
            sortButton.transform.SetParent(this.transform, false);
        }
    }
}
