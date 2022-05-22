using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using Ink.Runtime;


/**
*   This class contains generic utilities + story flow management utilized by all Story scripts.
**/
public static class StoryDirector {

    public delegate void CallbackDelegate();

    // dramatic pause for seconds, then execute callback
    public static IEnumerator StartStoriedPause(int seconds, CallbackDelegate cb) {
        yield return new WaitForSeconds(seconds);

        cb();
    }

    // fade in a certain direction, then execute callback
    public static void StartStoriedFade(string fadeDirection, CallbackDelegate cb) {
        if (fadeDirection == Constants.TAG_FADE_OUT) {

            Action handler = null;
            handler = new Action(delegate() {
                GameEventsScript.Instance.onScreenFadedOut -= handler;

                cb();
            });

            // perform the fade out
            UIControllerScript.Instance.FadeOut();
            
            // start listening for event
            GameEventsScript.Instance.onScreenFadedOut += handler;
        } else {
            Action handler = null;
            handler = new Action(delegate() {
                GameEventsScript.Instance.onScreenFadedIn -= handler;

                cb();
            });

            // perform the fade in
            UIControllerScript.Instance.FadeIn();
            
            // start listening for event
            GameEventsScript.Instance.onScreenFadedIn += handler;
        }
    }
}