using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupPackFilter : MonoBehaviour
{
    // The prefab button for filtering
    public GameObject packFilterButton;
    //
    private List<ContextPack> filterByPacks;
    private GameObject sortButton;
    ///<summary>
    /// counter to track how many context pack icons we are waiting on requests for from firebase
    ///</summary>
    private int iconCount = 0;

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
        filterByPacks = ContextPackHandler.loadContextPacks();

        // if filterByPacks doesn't have a contextPackId that matches a stored context pack Icon file
        if (!ContextPackHandler.AlreadyHaveAppropriateContextPackIcons(filterByPacks))
        {
            foreach (ContextPack pack in filterByPacks)
            {
                if (pack.icon != "" && pack.icon != null)
                {
                    iconCount++;
                    // then we query firebase and store them
                    GetPackIconAndStoreLocally(pack);
                }
            }
        }
        else
        {
            // we already have the icons, just grab them locally
            SetUpContextPackSortButtons();
        }
    }
    public void GetPackIconAndStoreLocally(ContextPack pack)
    {
        StartCoroutine(ServerRequestHandler.GetContextPackIconFromFirebase(pack, StorePackIconLocally));
    }


    // This method stores icons grabbed from firebase.
    // When the class' icon count reaches 0, it has finished all the server requests it started and will
    // then setup the context pack sort buttons
    private void StorePackIconLocally(byte[] icon, string id)
    {
        ContextPackHandler.StoreContextPackIcon(id, icon);
        iconCount--;
        if (iconCount <= 0)
        {
            //Debug.Log("firebase requests finished... setting up context pack buttons");
            SetUpContextPackSortButtons();
        }
    }

    private void SetUpContextPackSortButtons()
    {
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
                Sprite packSprite = ContextPackHandler.GetContextPackIconFromStorage(filterByPacks[i]._id);
                AssignSprite(packSprite);
            }

            //
            sortButton.AddComponent<PackFilterButton>().pack = filterByPacks[i];

            //
            sortButton.transform.SetParent(this.transform, false);
        }
    }

    private void AssignSprite(Sprite sprite)
    {
        sortButton.GetComponent<Image>().sprite = sprite;
    }
}
