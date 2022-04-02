using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayLearnerInfo : MonoBehaviour
{
    public GameObject currentLearnerNameGO;
    public GameObject currentLearnerSpriteGO;
    public Sprite defaultSprite;

    void Start()
    {
        PopulateLearnerInfo();
    }

    /// <summary> 
    /// Add current learner name and sprite to currentLearnerName/Sprite objects
    /// </summary>
    private void PopulateLearnerInfo()
    {
        currentLearnerNameGO.GetComponent<Text>().text = LearnerLogin.staticLearner.name;
        Sprite currentSprite = LearnerIconStorageHandler.GetLearnerSprite(LearnerLogin.staticLearner._id);

        if (currentSprite != null)
        {
            currentLearnerSpriteGO.GetComponent<Image>().sprite = currentSprite;
        }
        else 
        {
            currentLearnerSpriteGO.GetComponent<Image>().sprite = defaultSprite; // using default sprite here because it looks empty otherwise
        }
    }

}
