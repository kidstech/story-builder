﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;
using System.Linq;
using UnityEngine.UI;

public class SentenceBar : MonoBehaviour
{
    //
    private float tileSize = 130f;

    //
    private Vector2 originalSize;

    // 
    private RectTransform r;

    int animcount = 1;

    public TextToSpeechHandler tts;

    float futureWidth = 0;

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

        // if the word tiles haven't taken up more space than can be displayed...
        if(GatherWordTiles().Count * tileSize < originalSize.x)
        {
            // do nothing
            return;
        }

        futureWidth = r.sizeDelta.x + (amountOfTileSpaceToAdd * tileSize);
        
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

    // leantween version
    public IEnumerator ClearTiles()
    {
        // animation time will be used to tell each part of the animation how long it gets to play.
        // we need this in order to keep our animations in sync with TTS.
        float animation_time =  0;
        int animation_segments = 3;
        int length = this.transform.childCount;
        int updatingLength = length;
        float left_move_distance = 0;
        // finding the distance between any two tiles
        if(length > 1){
        left_move_distance = this.transform.GetChild(1).transform.position.x - this.transform.GetChild(0).transform.position.x;
        }

        // this cannot be a for each loop. It turns out that destroying the game objects that the for each loop uses to
        // determine where it is in the loop causes very bad things to happen.
        for(int i = 0; i < length; i++)
        {
            Transform child = this.transform.GetChild(0);
            
            // estimate the time it will take to speak a tile
            // so we can run this in parallel with text to speech
            
            string tileText = child.GetComponentInChildren<Text>().text; // we destroy a tile every loop and that moves our indices by one, so using a constant here works
            float timeToSpeak = Speaker.ApproximateSpeechLength(tileText) / tts.getVoiceRate(); // voice rate is a float that acts as a percentage so 1 = 100% and 1.5 = 150%
            // logic here is that if we have x animations, we want each of their times to add up to timeToSpeak, so each animation gets timeToSpeak/x time to animate
            animation_time = timeToSpeak/animation_segments;

            // this initial wait time is here so we can see the word highlight and speak before it starts moving all over the place.
            yield return new WaitForSeconds(animation_time);

            LeanTween.moveY(child.gameObject, child.position.y + 105, animation_time); // move tile up 105 units
            yield return new WaitForSeconds(animation_time);

            // if we have multiple objects left in the list
            if(updatingLength > 1){
                // animate all but the first object in the list to the left
                for(int a = 1; a < updatingLength; a++){
                    Transform childA = this.transform.GetChild(a);
                    LeanTween.moveX(childA.gameObject, childA.position.x - left_move_distance , animation_time);
                }
            }
            yield return new WaitForSeconds(animation_time);

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
            float tile_height = 105;
            //float tile_height = child.gameObject.GetComponent<RectTransform>().rect.height + 50; 

            // the distance the tile will move each frame
            float move_per_frame = tile_height/frame_count;

            for(int i = 0; i < frame_count; i++){
            tilePosition.y += move_per_frame;
            child.gameObject.transform.position = tilePosition;
            yield return new WaitForSeconds(yield_time);
            }
    }

        public IEnumerator animateTileLeft(Transform child, float animation_time, float left_move_distance)
    {
            // You can't actually directly edit the position of the game object so we have to copy the
            // position info to a variable, change it, and then set the position equal to the value of that changed variable.
            Vector3 tilePosition = child.gameObject.transform.position;

            // number of frames our animation will take
            float frame_count = 30;

            // the time each frame will take
            float yield_time = animation_time/frame_count;
            float tile_width = left_move_distance; // we seem to also need to account for the padding on the tiles. 136.7 is not exactly the distance it needs to travel, but it's very close

            // the distance the tile will move each frame
            float move_per_frame = tile_width/frame_count;
            
            for(int i = 0; i < frame_count; i++){
            tilePosition.x -= move_per_frame;
            child.gameObject.transform.position = tilePosition;
            yield return new WaitForSeconds(yield_time);
            }
    }
}
