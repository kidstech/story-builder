using System.Collections;
using System.Collections.Generic;
using Crosstales.RTVoice;
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

     [SerializeField]
     private GameObject sentenceInTiles; 
      private string sentenceText;
      private Image image = null;
      private Color originalColor;

private void Start() {
     image = GetComponent<Image>();
     originalColor = image.color;
}
public void OnPointerClick(PointerEventData eventData)
    { 
        if(savedSentence.Count != 0) {
            savedSentence.Clear();
        }

        image.color = originalColor;
        TTS = GetComponentInParent<TextToSpeechHandler>();
        string sentenceText = sentenceInTiles.GetComponent<Text>().text;
        StartCoroutine(HighlightCoroutine(Speaker.Instance.ApproximateSpeechLength(sentenceText)));

        foreach(Transform child in sentenceInTiles.transform) 
            {
                WordTile word = child.gameObject.GetComponent<WordTile>();
                savedSentence.Add(word);
                LearnerDataHandler.UpdateWordCount(word.textToDisplay);
                LearnerDataHandler.StoreLearnerData();
                StartCoroutine(ServerRequestHandler.PostLearnerDataToServer());
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
        sentence.GetComponent<SentenceBar>().clearTiles();
        foreach(WordTile wordtile in sentenceTiles)
        {
            // copy the word tile object and make it a child of SentenceInTiles 
            GameObject wordTileCopy = Instantiate(wordtile.gameObject, sentence.transform);
            // repopulate word.contextPackId field as it doesn't carry over from instantiation (the field isn't serializable)
            wordTileCopy.GetComponent<WordTile>().word.contextPackId = wordtile.word.contextPackId;
            // wordtile game objects are deactivated originally so we need to activate the copies after instantiation
            wordTileCopy.gameObject.SetActive(true);
            //sentence.GetComponent<SentenceBar>().ResizeSentence(1);
        }
    }

     public IEnumerator HighlightCoroutine(float seconds)
    {
        Image image = GetComponent<Image>();
        Color previous = image.color;
        image.color = Color.yellow;
        yield return new WaitForSeconds(seconds);
        image.color = originalColor;
    }

}
