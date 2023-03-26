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

    /*
    * Prevents the speak sentence button from being able to be pressed while the context pack menu is open to prevent potential misclicks
    */
    void OnEnable() {
        speakSentence.raycastTarget = false;
    }
    
    void OnDisable() {
        speakSentence.raycastTarget = true;
    }

}