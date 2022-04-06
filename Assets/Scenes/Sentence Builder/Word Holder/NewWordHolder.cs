using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewWordHolder : MonoBehaviour
{
    public static NewWordHolder instance;

    [SerializeField]
    private Button closeForms;
    [SerializeField]
    private GameObject newFormsPopUp;

    [SerializeField]
    private TMP_Text baseWord; 

    [SerializeField]
     private GameObject newFormButton;
    // Start is called before the first frame update

    [SerializeField]
    private Transform baseWordT;

    public List<GameObject> buttons = new List<GameObject>();

    public static Word word2;
    void Start()
    {
        closeForms.onClick.AddListener(()=> newFormsPopUp.SetActive(false));
        instance = this;
    }

    void OnEnable()
    {
        baseWord.text = word2.baseWord;
    }

    public void setUpForms() {
         for (int i = 0; i < word2.forms.Count; i++)
        {
            Debug.Log("Hello");
            if(word2.forms.Count != 1) {
            GameObject button = Instantiate(newFormButton);
            float offset = 1;
            button.GetComponent<RectTransform>().localPosition = new Vector2(baseWordT.position.x, baseWordT.position.y - offset);
            button.GetComponentInChildren<TMP_Text>().text = word2.forms[i];
            button.transform.SetParent(this.transform, false);
            buttons.Add(button);
            }
        }
    }
}
