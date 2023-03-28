using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextPackMenu: MonoBehaviour {

    [SerializeField]
    private Button closeMenu;
    
    [SerializeField]
    private Image speakSentence;


    void Start() {
        closeMenu.onClick.AddListener(()=> {
            this.gameObject.SetActive(false);
        });
    }
}