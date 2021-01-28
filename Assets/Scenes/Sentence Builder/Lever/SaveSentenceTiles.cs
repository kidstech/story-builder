using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SaveSentenceTiles : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject draggedObject;
    private Vector3 startPosition;
    public List<WordTile> savedSentence; // storage for the latest submitted word tiles in case the user wishes to modify their sentence
    // Start is called before the first frame update
    private bool dragging;

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedObject = gameObject;
        startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggedObject = null;
        transform.position = startPosition; //bounce back testing
    }


}
