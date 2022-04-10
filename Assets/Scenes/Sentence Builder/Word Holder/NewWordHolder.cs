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

    [SerializeField]
    private Transform baseWordT;

    [SerializeField]
    private GameObject baseWordGO;

    [SerializeField] 
    private GameObject wordHolderDrop; 

    private List<GameObject> buttons = new List<GameObject>();

    public static Word word2;

    // Start is called before the first frame update
    void Start()
    {
        closeForms.onClick.AddListener(()=> newFormsPopUp.SetActive(false));
         baseWordGO.GetComponentInChildren<Button>().onClick.AddListener(() => TaskOnClick2());
        instance = this;
    }

    void OnEnable()
    {
        baseWord.text = word2.baseWord;
    }

    void OnDisable() {
        foreach(GameObject go in buttons) {
            Destroy(go);
        }
    }

    public void setUpForms() {
        float offset = 40;
         for (int i = 1; i < word2.forms.Count; i++)
        {
                GameObject button = Instantiate(newFormButton);
                button.transform.SetParent(newFormsPopUp.GetComponentInChildren<Canvas>().transform, false); 
                button.GetComponentInChildren<TMP_Text>().text = word2.forms[i];
                string wordFormText = word2.forms[i];
                Debug.Log(i);
                button.transform.position = new Vector3(baseWordT.position.x, baseWordT.position.y - offset, baseWordT.position.z);
                button.GetComponent<Button>().onClick.AddListener(() => TaskOnClick(word2, wordFormText));
                offset += 40;
                buttons.Add(button);
            // float offset = 1;
            // button.GetComponent<RectTransform>().localPosition = new Vector2(baseWordT.position.x, baseWordT.position.y - offset);
            // button.GetComponentInChildren<TMP_Text>().text = word2.forms[i];
            // button.transform.SetParent(this.transform, false);
            // buttons.Add(button);
            }
        }

        void TaskOnClick(Word word2, string i) {
            //Debug.Log("You have been clicked");
            Debug.Log(i);
            wordHolderDrop.GetComponentInChildren<Text>().text = i;
        }

        void TaskOnClick2() {
             wordHolderDrop.GetComponentInChildren<Text>().text = baseWord.text;
        }
    }

