using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonMouseOverEnlarger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   public void OnPointerEnter(PointerEventData eventData)
   {
       this.gameObject.transform.localScale = new Vector3(1.1f,1.1f,1f);
   }
   public void OnPointerExit(PointerEventData eventData)
   {
       this.gameObject.transform.localScale = new Vector3(1,1,1);
   }
}
