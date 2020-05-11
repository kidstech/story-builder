using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakPageButton : MonoBehaviour
{
    [Header("Page Icon Containers")]
    public PageContainer pageContainer;
    private Button button;

    //
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SpeakPage);
    }

    private void SpeakPage()
    {
        pageContainer.SpeakPage();
    }
}
