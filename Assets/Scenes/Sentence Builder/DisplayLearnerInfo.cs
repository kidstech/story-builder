using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayLearnerInfo : MonoBehaviour
{
    public GameObject currentLearnerNameGO;
    public GameObject currentLearnerSpriteGO;
    public Sprite defaultSprite;
    public Sprite moose;
    public Sprite narwhal;
    public Sprite penguin;
    public Sprite pig;
    public Sprite duck;

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

        switch (LearnerLogin.staticLearner.icon) {
            case "/assets/penguin.png":
                        currentLearnerSpriteGO.GetComponent<Image>().sprite = penguin;
                        break;
                    case "/assets/moose.png":
                        currentLearnerSpriteGO.GetComponent<Image>().sprite = moose;
                        break;
                    case "/assets/pig.png":
                        currentLearnerSpriteGO.GetComponent<Image>().sprite = pig;
                        break;
                    case "/assets/duck.png":
                        currentLearnerSpriteGO.GetComponent<Image>().sprite = duck;
                        break;
            default: 
             Sprite currentSprite = LearnerIconStorageHandler.GetLearnerSprite(LearnerLogin.staticLearner._id);

        if (currentSprite != null)
        {
            currentLearnerSpriteGO.GetComponent<Image>().sprite = currentSprite;
        }
        else 
        {
            currentLearnerSpriteGO.GetComponent<Image>().sprite = defaultSprite; // using default sprite here because it looks empty otherwise
        }
        break;

        }
    }

}
