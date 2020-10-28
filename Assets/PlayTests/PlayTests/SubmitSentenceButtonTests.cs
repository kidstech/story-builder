using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NSubstitute;

namespace Tests
{
    public class LeverTestScript
    {
        
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator LeverAnimatesWhenClicked()
        {
            // grab the different sprites for the lever
            Sprite leverUp = Resources.Load<Sprite>("Lever up");
            Sprite leverDown = Resources.Load<Sprite>("Lever down");
            // make our lever game object
            GameObject lever = new GameObject(name:"Lever");
            // attach the submitSentenceButton script to that game object
            SubmitSentenceButton submitSentenceButton = lever.AddComponent<SubmitSentenceButton>();
            lever.AddComponent<Image>();
            lever.GetComponent<Image>().sprite = leverUp;
            Debug.Log(lever.GetComponent<Image>());
            // lever should start in the up position
            yield return new WaitForSeconds(10f);
            Assert.AreEqual(lever.GetComponent<Image>().sprite, leverUp);
            // make a fake click on our game object
            //ExecuteEvents.Execute<IPointerClickHandler> (lever, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);

            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
