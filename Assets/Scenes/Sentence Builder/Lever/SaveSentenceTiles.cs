using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SaveSentenceTiles : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public static GameObject draggedObject;
    int startSibIndex;
    private Vector3 startPosition;
    public List<WordTile> savedSentence; // storage for the latest submitted word tiles in case the user wishes to modify their sentence
    private bool dragging;
    public GameObject sentence;
    private bool drop = false;  
     public TextToSpeechHandler TTS;

public void OnPointerClick(PointerEventData eventData)
    { 
        if(savedSentence.Count != 0) {
            savedSentence.Clear();
        }
        // Speak the text on the tile using the correct voice
        Debug.Log(savedSentence.ToString());
        //savedSentence.Add(child.GetComponent<WordTile>());
        TTS = GetComponentInParent<TextToSpeechHandler>();
        //savedSentence.Clear();
        Transform sentenceInTiles = this.transform.GetChild(0);
        foreach(Transform child in sentenceInTiles) 
                {
                savedSentence.Add(child.GetComponent<WordTile>());
            }
        TTS.startSpeakingSentence(this.savedSentence, false);
        savedSentence.Clear();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedObject = this.gameObject;
        startPosition = this.transform.position;
        startSibIndex = this.transform.GetSiblingIndex();
        this.transform.SetAsLastSibling();
        this.transform.GetComponent<Image>().raycastTarget = false; // stop the object from blocking raycasts
        Debug.Log("Hello");
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        this.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // clear old sentence tiles
        savedSentence.Clear();
        // if we drag and release our sentence over the sentence bar, resubmit it
        if (eventData.hovered.Contains(sentence))
        {
            Transform sentenceInTiles = this.transform.GetChild(0);
            foreach(Transform child in sentenceInTiles) 
            {
                savedSentence.Add(child.GetComponent<WordTile>());
                child.position = new Vector3(child.position.x, child.position.y, 0); // resetting z value of word tiles to 0 to make sure they are actually in frame/not behind the canvas
            }
            resubmitSentence(savedSentence);
        }
        draggedObject = null;
        this.transform.position = startPosition; // reset the submitted sentence object back to its original position
        this.transform.SetSiblingIndex(startSibIndex); // reset parent so dragged object is no longer on top of everything
        this.transform.GetComponent<Image>().raycastTarget = true; // allow raycasts to hit the object again so we can drag it later
    }

    // note: with current implementation, the resubmitted word tiles cannot be placed in between word tiles in the sentence bar. They will always append themselves to the end.
    // Not sure if we want to be able to stuff the resubmitted sentence between words that have already been placed or not.
    public void resubmitSentence(List<WordTile> sentenceTiles)
    {
        foreach(WordTile wordtile in sentenceTiles)
        {
            // copy the word tile object and make it a child of SentenceInTiles 
            GameObject wordTileCopy = Instantiate(wordtile.gameObject, sentence.transform);
            // repopulate word.contextPackId field as it doesn't carry over from instantiation (the field isn't serializable)
            wordTileCopy.GetComponent<WordTile>().word.contextPackId = wordtile.word.contextPackId;
            // wordtile game objects are deactivated originally so we need to activate the copies after instantiation
            wordTileCopy.gameObject.SetActive(true);
            sentence.GetComponent<SentenceBar>().ResizeSentence(1);
        }
    }


}
