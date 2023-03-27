using UnityEngine;
using UnityEngine.UI;

public class ContextPackMenuButton: MonoBehaviour {

    [SerializeField]
    private GameObject contextPackMenu;

    void Start() {
        this.gameObject.GetComponent<Button>().onClick.AddListener(()=> {
            if(contextPackMenu.activeSelf == false) {
                contextPackMenu.SetActive(true);
            }

            else {
                contextPackMenu.SetActive(false);
            }
        });
    }
}