using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

/***
*   This class will function as a controller / manager for the overall story. This class sits at the topmost layer, and will 
*   utilize the object stories, cutscene stories, and any exclusive story classes. 
***/
public class GeneralStoryManager : MonoBehaviour
{

    // keep a singleton reference to this script
    private static GeneralStoryManager _instance;

    public static GeneralStoryManager Instance { get { return _instance; } }

    // memory bank holds memories across all ink Story files. 
    // TODO: Maybe move this somewhere else? Somewhere more player-oriented?
    public static List<string> memoryBank;

    private string _currentStoryLine;
    private Story _currentStory;
    private DialogueBoxScript _activeDialogueBox;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    // start is called before the first frame update
    void Start()
    {
        memoryBank = new List<string>();
    }

    // called during story instantiation to bind the 'essential' functions to the story
    public void BindEssentialFunctions(Story inkStory) {
        inkStory.BindExternalFunction("addMemory", (string memoryName) => {
            AddMemory(memoryName);
        });

        inkStory.BindExternalFunction("removeMemory", (string memoryName) => {
            RemoveMemory(memoryName);
        });

        inkStory.BindExternalFunction("checkMemory", (string memoryName) => {
            return CheckMemory(memoryName);
        });

        inkStory.BindExternalFunction("getDate", () => {
            return DayStateManager.Instance.currentDay;
        });

        inkStory.BindExternalFunction("getTime", () => {
            return AlarmClockScript.Instance.GetClockText();
        });
    }

    // accessed as an 'external' function from our Ink stories
    public void AddMemory(string memoryName) {
        memoryBank.Add(memoryName);
    }

    public void RemoveMemory(string memoryName) {
        memoryBank.Remove(memoryName);
    }

    // ink 'external' function
    public bool CheckMemory(string memoryName) {
        return memoryBank.Contains(memoryName);
    }

    /****************************************
    *   The following methods create and manage an active 'story session'. 
    *   A story session is a set of story text that should be read as dialogue
    *   until an #endsession tag is reached.
    *****************************************/
    public void StartStorySession(Story inkStory) {
        // given that the object is usually interacted with in some way, let's disable collision between the player and physical objects
        Physics.IgnoreLayerCollision(Constants.PHYSICAL_OBJECT_LAYER, Constants.PLAYER_LAYER, true);

        _currentStory = inkStory;

        CheckNextStoryLine(true);
    }

    // determines how to proceed with the story session
    public void CheckNextStoryLine(bool firstInteraction = false) {

        if (_currentStory.canContinue) {
            _currentStoryLine = _currentStory.Continue();
            
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

    // write a line of dialogue
    public void WriteStoryDialogue() {

        if (_currentStoryLine.Length > 0 && !IsTagLine(_currentStoryLine)) {

            if (_activeDialogueBox == null) {
                // create a dialogue box. This will also write the initial line.
                // TODO: if we don't actually end up needing the target object, remove the parameter
                _activeDialogueBox = UIControllerScript.Instance.ShowDialogueBox(GameFlowStateManager.Instance.targetInteractiveObject, _currentStoryLine);
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

    // returns a boolean indicating that we've received some story direction
    public bool CheckStoryTags() {
        // check for any story tags that might give us direction for this story session
        List<string> storyTags = _currentStory.currentTags;

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
                AnimationMetadata animation = Constants.animationList[animationName];

                if (animation.movementFirst) {
                    // animate after moving
                    StoryDirector.CallbackDelegate cb = () => { StoryDirector.StartStoriedAnimation(animation, writeStoryCallback); };
                    digressionFunctions.Add(() => { StoryDirector.StartStoriedMovement(animation, cb); });
                }
                else {
                    // move after animating
                    StoryDirector.CallbackDelegate cb = () => { StoryDirector.StartStoriedMovement(animation, writeStoryCallback); };
                    digressionFunctions.Add(() => { StoryDirector.StartStoriedAnimation(animation, cb); });
                }
                
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

            // check for audio
            string audioStart = Helpers.GetTagValue("audiostart", storyTags);
            string audioStop = Helpers.GetTagValue("audiostop", storyTags);

            if (audioStart.Length > 0) {
                // for now, let's just assume we're starting the phone audio
                AudioManager.Instance.PhoneAudio();
            }

            if (audioStop.Length > 0) {
                // for now, let's just assume we're stopping the phone audio
                AudioManager.Instance.PhoneAudio(true);
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

    public void ProgressStorySession(GameObject activeObject) {
        // indicator that any text in-progress can be fast typed.
        bool fastType = true;

        if (_activeDialogueBox.ReadyForProgression(fastType)) { // make sure we're not actively typing, or animating

            // we can safely move forward with the story session. No need to listen for this event anymore.
            GameEventsScript.Instance.onProgressDialogueInput -= ProgressStorySession;

            CheckNextStoryLine();
        }
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
        // TODO: I really don't think it's necessary to pass the target object around so much.
        GameEventsScript.Instance.CompletedStorySession(GameFlowStateManager.Instance.targetInteractiveObject);
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