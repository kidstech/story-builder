﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupPackFilter : MonoBehaviour
{
    // The prefab button for filtering
    public GameObject packFilterButton;
    //
    private List<ContextPack> filterByPacks;
    private GameObject sortButton;

    public void SetUpPacks()
    {
        // clear old filter buttons if they exist
        if (filterByPacks != null)
        {
            filterByPacks.Clear();
            foreach (Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }
        }
        // load the packs
        filterByPacks = LoadContextPacks.loadContextPacks();
        // make the sorting buttons for the context packs
        for (int i = 0; i < filterByPacks.Count; i++)
        {
            //
            sortButton = Instantiate(packFilterButton);

            //
            sortButton.name = filterByPacks[i].name;

            //
            sortButton.GetComponentInChildren<Text>().text = filterByPacks[i].name;

            /*
             * Need to try to load an image from the path above ^ 
             */
            if (filterByPacks[i].icon != "")
            {
                GetPackIconAndStoreInGameObject(filterByPacks[i]);
            }

            //
            sortButton.AddComponent<PackFilterButton>().pack = filterByPacks[i];

            //
            sortButton.transform.SetParent(this.transform, false);
        }
    }
    public void GetPackIconAndStoreInGameObject(ContextPack pack)
    {
        StartCoroutine(ServerRequestHandler.GetContextPackIconFromFirebase(pack, AssignSprite));
    }
    private void AssignSprite(Sprite sprite)
    {
        sortButton.GetComponent<Image>().sprite = sprite;
    }
}
