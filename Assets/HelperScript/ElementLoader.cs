using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HelperScript.ElementLoader
{
    public class ElementLoader : MonoBehaviour
    {

        //Creates a wordHolderScrollView from a given prefab on a given canvas
        public GameObject createWordHolderScrollView(GameObject scrollview, Canvas canvas)
        {
            GameObject whsv = Instantiate(scrollview) as GameObject;
            whsv.transform.SetParent(canvas.transform);
            return whsv;
        }

        //Creates a wordHolder from a given prefab on a given scrollview
        public GameObject createWordHolder(GameObject wordholder, Canvas canvas)
        {
            GameObject wh = Instantiate(wordholder) as GameObject;
            wh.transform.SetParent(canvas.transform);
            //wh.transform.x = Screen.width / 2;
            return wh;
        }

        public GameObject createConfirmButton(GameObject confirmbutton, Canvas canvas)
        {
            GameObject cb = Instantiate(confirmbutton) as GameObject;
            cb.transform.SetParent(canvas.transform);
            return cb;
        }

        public GameObject createWordChoices(GameObject wordchoices, Canvas canvas)
        {
            GameObject wc = Instantiate(wordchoices) as GameObject;
            wc.transform.SetParent(canvas.transform);
            return wc;
        }

        public GameObject createCloseWordChoices(GameObject closewordchoices, Canvas canvas)
        {
            GameObject cwc = Instantiate(closewordchoices) as GameObject;
            cwc.transform.SetParent(canvas.transform);
            return cwc;
        }

    }
}