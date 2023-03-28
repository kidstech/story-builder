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
    private int missingIcons = 0;

    [SerializeField]
    private GameObject contextPackList;

    public void SetUpPacks()
    {

        bool haveAllIcons = true;
        Debug.Log("Set up packs is being called");
        // clear old filter buttons if they exist
        if (filterByPacks != null)
        {
            filterByPacks.Clear();
            foreach (Transform child in contextPackList.transform)
            {
                Destroy(child.gameObject);
            }
        }
        // load the packs
        filterByPacks = ContextPackHandler.loadContextPacks();
        
        // Check if we have the pack icon
        foreach(ContextPack pack in filterByPacks) {
            if(!ContextPackHandler.checkContextPackIcon(pack)) {
                if(pack.icon != "" && pack.icon != null) {
                    haveAllIcons = false;
                    missingIcons++;
                    GetPackIconAndStoreLocally(pack);
                }
            }
        }

        if(haveAllIcons) {
             SetUpContextPackSortButtons();
        }
    }
    public void GetPackIconAndStoreLocally(ContextPack pack)
    {
        StartCoroutine(ServerRequestHandler.GetContextPackIconFromFirebase(pack, StorePackIconLocally));
    }


    /// <summary>This method stores icons grabbed from firebase. <br/>
    /// When the class' icon count reaches 0, it has finished all the server requests it started and will
    /// then setup the context pack sort buttons </summary>
    /// <param name="icon"> A byte array containing the data for a context pack icon </param>
    /// <param name="id"> The mongo object id of the context pack associated with the given icon byte array </param>
    private void StorePackIconLocally(byte[] icon, string id)
    {
        Debug.Log("storing context pack locally");
        ContextPackHandler.StoreContextPackIcon(id, icon);
        missingIcons--;
        if (missingIcons <= 0)
        {
            Debug.Log("firebase requests finished... setting up context pack buttons");
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
            if (filterByPacks[i]?.icon != "" )
            {
                Sprite packSprite = ContextPackHandler.GetContextPackIconFromStorage(filterByPacks[i]._id);
                AssignSprite(packSprite);
                sortButton.GetComponentInChildren<Text>().text = ""; // remove text if we have an icon for a pack
            }
            else{
                sortButton.GetComponentInChildren<Text>().text = filterByPacks[i].name;
            }
            
            //
            sortButton.AddComponent<PackFilterButton>().pack = filterByPacks[i];

            //
            sortButton.transform.SetParent(contextPackList.transform, false);
        }
    }

    /// <summary>
    /// Stores the given sprite in the sort button prefab image component <br/>
    /// </summary>
    /// <param name="sprite"> Unity sprite to be assigned to an image component </param>
    /// <return> void </return>
    private void AssignSprite(Sprite sprite)
    {
        // WIP for getting sprites with rounded edges 
        sortButton.GetComponent<Image>().sprite = sprite;
    }
}
