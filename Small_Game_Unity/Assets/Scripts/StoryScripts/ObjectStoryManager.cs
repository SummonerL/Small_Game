using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class ObjectStoryManager : MonoBehaviour
{

    // Ink story files
    public TextAsset inkJSON;
    public Story story;

    // used to control some animations triggered in our object's story
    private PlayerAnimationController playerAnimationController;

    private string _currentStoryLine;
    private AnimationMetadata _currentStoryAnimation;
    private DialogueBoxScript _activeDialogueBox;

    // start is called before the first frame update
    void Start()
    {
        story = new Story(inkJSON.text);

        playerAnimationController = PlayerSingleton.Instance.GetComponent<PlayerAnimationController>();

        GeneralStoryManager.Instance.BindEssentialFunctions(story);
    }
    
    /**
    *   A story 'session' refers to a series of story lines written to the dialogue box. 
    *   The user will progress the story session with the primary controller button (indicated by the ... icon)
    **/
    public void StartStorySession() {

        // given that the object is usually interacted with in some way, let's disable collision between the player and physical objects
        Physics.IgnoreLayerCollision(Constants.PHYSICAL_OBJECT_LAYER, Constants.PLAYER_LAYER, true);

        CheckNextStoryLine(true);
    }

    // determines how to proceed with the story session
    public void CheckNextStoryLine(bool firstInteraction = false) {

        if (story.canContinue) {
            _currentStoryLine = story.Continue();
            
            if (CheckStoryTags())
                return;
        }
        else {
            if (!firstInteraction) {
                // there's nothing else to say. End the story session
                EndStorySession();
                return;
            }

            _currentStoryLine = Constants.NO_STORY_DIALOGUE_DEFAULT_TEXT;
        }

        WriteStoryDialogue();

    }

    // returns a boolean indicating that we've received some story direction
    public bool CheckStoryTags() {
        // check for any story tags that might give us direction for this story session
        List<string> storyTags = story.currentTags;

        // we'll return 'digression', which indicates whether or not the tags have given us some story instruction.
        // generally, this will mean that we want the instruction to complete before continuing with the dialogue.
        bool digression = false;

        // we can add methods to a list (with params), and then invoke them sequentially. How cool!
        List<Action> digressionFunctions = new List<Action>();

        // WriteStoryDialogue will be the most common callback method for digressions
        StoryDirector.CallbackDelegate writeStoryCallback = WriteStoryDialogue;

        if (storyTags.Count > 0) {

            // check for animation
            string animationName = Helpers.GetTagValue("animation", storyTags);
            
            if (animationName.Length > 0) {
                _currentStoryAnimation = Constants.animationList[animationName];
                if (_currentStoryAnimation.movementFirst)
                    digressionFunctions.Add(StartStoriedMovement);
                else
                    digressionFunctions.Add(StartStoriedAnimation);
                
                digression = true;
            }

            // check for animation controls
            string animationControl = Helpers.GetTagValue("animationControl", storyTags);

            if (animationControl.Length > 0) {
                if (animationControl == Constants.ANIMATION_CONTROL_RESET_STATE) {
                    PlayerAnimationController animationControllerScript = PlayerSingleton.Instance.GetComponent<PlayerAnimationController>();
                    animationControllerScript.ResetAnimatorState();
                }
            }

            // check for time advancement
            string timeAdvance = Helpers.GetTagValue("advancetime", storyTags);

            if (timeAdvance.Length > 0) {
                // move forward X number of day states
                DayStateManager.Instance.AdvanceTime(Int32.Parse(timeAdvance));
            }

            // check for screen fades
            string screenFade = Helpers.GetTagValue("fade", storyTags);

            if (screenFade.Length > 0) {
                digressionFunctions.Add(() => {StoryDirector.StartStoriedFade(screenFade, writeStoryCallback); });
                digression = true;
            }

            // check for actor changes
            string actor = Helpers.GetTagValue("actor", storyTags);

            if (actor.Length > 0) {
                ChangeActor(actor); // doesn't require digression
            }

            // check for pauses
            string waitFor = Helpers.GetTagValue("dramaticpause", storyTags);

            if (waitFor.Length > 0) {
                // wait for X seconds
                int seconds = Int32.Parse(waitFor);

                digressionFunctions.Add(() => { StartCoroutine(StoryDirector.StartStoriedPause(seconds, writeStoryCallback)); });
                digression = true;
            }

            // finally, check to see if we want to end this session
            string endSession = Helpers.GetTagValue("endsession", storyTags);

            if (endSession.Length > 0) {
                digressionFunctions.Add(EndStorySession);
                digression = true;
            }

        }

        if (digression) {
            // clear the dialogue box, given that we'll be following some story digression
            ClearDialogueBox();

            // and execute all of our digression functions
            foreach (Action func in digressionFunctions)
                func();
        }

        return digression;
    }

    // write a line of dialogue
    public void WriteStoryDialogue() {

        if (_currentStoryLine.Length > 0 && !IsTagLine(_currentStoryLine)) {

            if (_activeDialogueBox == null) {
                // create a dialogue box above this object. This will also write the initial line
                _activeDialogueBox = UIControllerScript.Instance.ShowDialogueBox(gameObject, _currentStoryLine);
            } else {
                _activeDialogueBox.PrepareAndWrite(_currentStoryLine);
            }

            // start listening for a 'dialogue progression' input event (the user has pressed the primary button)
            GameEventsScript.Instance.onProgressDialogueInput += ProgressStorySession;

        } else {
            // this was most likely called on a tag-only line. Let's not write anything and just progress the story session.
            CheckNextStoryLine();
        }
    }

    public void ProgressStorySession(GameObject activeObject) {
        // make sure that the event was actually published for this object
        if (activeObject == gameObject) {

            // indicator that any text in-progress can be fast typed.
            bool fastType = true;

            if (_activeDialogueBox.ReadyForProgression(fastType)) { // make sure we're not actively typing, or animating

                // we can safely move forward with the story session. No need to listen for this event anymore.
                GameEventsScript.Instance.onProgressDialogueInput -= ProgressStorySession;

                CheckNextStoryLine();
            }
        }
    }

    // trigger some player movement that accompanies this object or story session.
    public void StartStoriedMovement() {
        PlayerStateManager.Instance.StartStoriedMovement(_currentStoryAnimation.startingPoint, _currentStoryAnimation.startingDirection);

        // once the player reaches the target position, they should perform the animation
        GameEventsScript.Instance.onPlayerReachedPosition += StoriedMovementComplete;
    }

    public void StoriedMovementComplete() {
        // we no longer need to listen for the event
        GameEventsScript.Instance.onPlayerReachedPosition -= StoriedMovementComplete;


        // an animation can either occur before movement, or after it. This flag allows for anim->movement->story or movement->anim->story
        if (_currentStoryAnimation.movementFirst)
            StartStoriedAnimation();
        else 
            WriteStoryDialogue();
    }

    public void StartStoriedAnimation() {
        // trigger the active animation
        playerAnimationController.SetAnimationParam<bool>(_currentStoryAnimation.animationParameter, _currentStoryAnimation.animationParameterValue);

        // listen for the animation completion event
        GameEventsScript.Instance.onAnimationCompleted += StoriedAnimationComplete;
    }

    public void StoriedAnimationComplete() {
        // we no longer need to listen for the event
        GameEventsScript.Instance.onAnimationCompleted -= StoriedAnimationComplete;

        if (_currentStoryAnimation.movementFirst)
            WriteStoryDialogue();
        else 
            StartStoriedMovement();
    }

    // the 'actor' is the person/entity that is currently speaking.
    public void ChangeActor(string actor) {
        // 'external' indicates an off-screen talker.
        if (actor == Constants.TAG_EXTERNAL_ACTOR) {
            if (_activeDialogueBox != null) {
                _activeDialogueBox.UseExternalBox();
                _activeDialogueBox.UseExternalText();
            }
            
            // the UIController will keep track of the status, in case we create a new dialogue box
            UIControllerScript.Instance.UseExternalActor();

        } else if (actor == Constants.TAG_PLAYER_ACTOR) {
            if (_activeDialogueBox != null) {
                _activeDialogueBox.UseInternalBox();
                _activeDialogueBox.UseInternalText();
            }

            UIControllerScript.Instance.UseInternalActor();

        }
    }

    public void EndStorySession() {

        // stop ignoring collision between the player and physical objects
        Physics.IgnoreLayerCollision(Constants.PHYSICAL_OBJECT_LAYER, Constants.PLAYER_LAYER, false);

        // stop listening for the dialogue progression event
        GameEventsScript.Instance.onProgressDialogueInput -= ProgressStorySession;

        // clear the dialogue box
        ClearDialogueBox();

        // publish an event indicating that we've completed the story session
        GameEventsScript.Instance.CompletedStorySession(gameObject);
    }

    public void ClearDialogueBox() {
        if (_activeDialogueBox != null) {
            _activeDialogueBox.HideDialogueBox();
            _activeDialogueBox = null;
        }
    }

    // checks if the provided story line is a 'tag-only' line (the actual text is ignored)
    public bool IsTagLine(string storyLine) {
        return (storyLine.Replace("\n", "") == Constants.IGNORED_TEXT);
    }
}
