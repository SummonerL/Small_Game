using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using Ink.Runtime;


/**
*   This class provides handlers for story direction. Generally, these are invoked using story tags, and are considered 'digressions'
*   to the story. E.g. playing an animation, moving to a location, fading in / out, etc.
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

    public static void StartStoriedAnimation(AnimationMetadata animation, CallbackDelegate cb) {
        // trigger the active animation
        PlayerAnimationController animationController = PlayerSingleton.Instance.GetComponent<PlayerAnimationController>();

        animationController.SetAnimationParam<bool>(animation.animationParameter, animation.animationParameterValue);

        Action handler = null;
        handler = new Action(delegate() {
            GameEventsScript.Instance.onAnimationCompleted -= handler;

            cb();
        });

        // listen for the animation completion event
        GameEventsScript.Instance.onAnimationCompleted += handler;
    }

    // trigger some player movement that accompanies this object or story session.
    public static void StartStoriedMovement(AnimationMetadata animation, CallbackDelegate cb) {
        PlayerStateManager.Instance.StartStoriedMovement(animation.startingPoint, animation.startingDirection);

        Action handler = null;
        handler = new Action(delegate() {
            GameEventsScript.Instance.onPlayerReachedPosition -= handler;

            cb();
        });

        // once the player reaches the target position, they should perform the animation
        GameEventsScript.Instance.onPlayerReachedPosition += handler;
    }
}