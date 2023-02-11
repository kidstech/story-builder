using UnityEngine;
using UnityEngine.UI;

public class ContextPackMenuButton: MonoBehaviour {

    [SerializeField]
    private GameObject contextPackMenu;

    void Start() {
        this.gameObject.GetComponent<Button>().onClick.AddListener(()=> contextPackMenu.SetActive(true));
    }
}