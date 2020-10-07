using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;
using System.Linq;
using UnityEngine.UI;

public class SentenceBar : MonoBehaviour
{
    //
    private readonly int tileSize = 125;

    //
    private Vector2 originalSize;

    //
    private RectTransform r;

    int animcount = 1;

    //
    private void Start()
    {
        //
        r = GetComponent<RectTransform>();

        //
        originalSize = r.sizeDelta;
    }

    //
    public List<WordTile> GatherWordTiles()
    {
        //
        List<WordTile> results = new List<WordTile>();

        //
        for(int i = 0; i < transform.childCount; i++)
        {
            //
            results.Add(transform.GetChild(i).GetComponent<WordTile>());
        }

        //
        return results;
    }

    //
    public void ResizeSentence(int amountOfTileSpaceToAdd)
    {
        //
        if(GatherWordTiles().Count * tileSize < originalSize.x)
        {
            //
            return;
        }

        //
        float futureWidth = r.sizeDelta.x + (amountOfTileSpaceToAdd * tileSize);

        //
        if(futureWidth > originalSize.x)
        {
            //
            r.sizeDelta = new Vector2(futureWidth, originalSize.y);
        }
        else
        {
            //
            r.sizeDelta = originalSize;
        }
    }

    // ClearTiles() now also animates the tiles it is clearing before actually deleting them.
    public IEnumerator ClearTiles()
    {
        // animation time will be used to tell each part of the animation how long it gets to play.
        // we need this in order to keep our animations in sync with TTS.
        float animation_time =  0;
        int animation_segments = 3;
        int length = this.transform.childCount;
        int updatingLength = length;
        // this cannot be a for each loop. It turns out that destroying the game objects that the for each loop uses to
        // determine where it is in the loop causes very bad things to happen.
        for(int i = 0; i < length; i++)
        {
            
            // estimate the time it will take to speak a tile
            // so we can run this in parallel with text to speech
            string tileText = this.transform.GetChild(0).GetComponentInChildren<Text>().text;
            float timeToSpeak = Speaker.ApproximateSpeechLength(tileText);
            // logic here is that if we have x animations, we want each of their times to add up to timeToSpeak, so each animation gets timeToSpeak/x time to animate
            animation_time = timeToSpeak/animation_segments;

            // this initial wait time is here so we can see the word highlight and speak before it starts moving all over the place.
            yield return new WaitForSeconds(animation_time);

            StartCoroutine(animateTileUp(this.transform.GetChild(0), animation_time));
            yield return new WaitForSeconds(animation_time);

            // if we have multiple objects left in the list
            if(updatingLength > 1){
                // animate all but the first object in the list to the left
                for(int a = 1; a < updatingLength; a++){
                    StartCoroutine(animateTileLeft(this.transform.GetChild(a), animation_time));
                }
            }
            yield return new WaitForSeconds(animation_time);
            yield return new WaitForEndOfFrame();

            // By destroying the first child, we change the indices of the child array
            // So if we were to use i here, we would destroy every other game object and eventually find ourselves outside of the array.
            Destroy(this.transform.GetChild(0).gameObject);

            // we need the sentence bar length to match our changed number of game objects
            ResizeSentence(-1);
            updatingLength--;
            
            // the re-size of our hierarchy of gameObjects needs a frame to properly update
            yield return null;
        }

    }
    // useful info about tile dimensions
    // width of a tile is 125 units
    // and height is 50 units
    // word tiles seem to have ~10 units worth of padding around them


    // takes in a child of the sentence transform and moves it upward smoothly
    public IEnumerator animateTileUp(Transform child, float animation_time)
    {
            // You can't actually directly edit the position of the game object so we have to copy the
            // position info to a variable, change it, and then set the position equal to the value of that changed variable.
            Vector3 tilePosition = child.gameObject.transform.position;

            // number of frames our animation will take
            float frame_count = 30;

            // the time each frame will take
            float yield_time = animation_time/frame_count;
            float tile_height = 60; 

            // the distance the tile will move each frame
            float move_per_frame = tile_height/frame_count;

            for(int i = 0; i < frame_count; i++){
            tilePosition.y += move_per_frame;
            child.gameObject.transform.position = tilePosition;
            yield return new WaitForSeconds(yield_time);
            }
    }

        public IEnumerator animateTileLeft(Transform child, float animation_time)
    {
            // You can't actually directly edit the position of the game object so we have to copy the
            // position info to a variable, change it, and then set the position equal to the value of that changed variable.
            Vector3 tilePosition = child.gameObject.transform.position;

            // number of frames our animation will take
            float frame_count = 30;

            // the time each frame will take
            float yield_time = animation_time/frame_count;
            float tile_width = 136.7f; // we seem to also need to account for the padding on the tiles. 136.7 is not exactly the distance it needs to travel, but it's very close

            // the distance the tile will move each frame
            float move_per_frame = tile_width/frame_count;
            // height of a tile is 50 units, and it completely disappears into the pipe after 60 units
            for(int i = 0; i < frame_count; i++){
            tilePosition.x -= move_per_frame;
            child.gameObject.transform.position = tilePosition;
            yield return new WaitForSeconds(yield_time);
            }
    }
}
