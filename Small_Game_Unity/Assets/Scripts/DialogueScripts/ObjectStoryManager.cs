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
    }
    
    /**
    *   A story 'session' refers to a series of story lines written to the dialogue box. 
    *   The user will progress the story session with the primary controller button (indicated by the ... icon)
    **/
    public void StartStorySession() {

        // given that the object is usually interacted with in some way, let's disable collision between the player and physical objects
        Physics.IgnoreLayerCollision(Constants.PHYSICAL_OBJECT_LAYER, Constants.PLAYER_LAYER, true);

        if (story.canContinue) {
            _currentStoryLine = story.Continue();
            
            if (CheckStoryTags())
                return;
        }
        else
            _currentStoryLine = Constants.NO_STORY_DIALOGUE_DEFAULT_TEXT;

        StartStoryDialogue();

    }

    // returns a boolean indicating that we've received some story direction
    public bool CheckStoryTags() {
        // check for any story tags that might give us direction for this story session
        List<string> storyTags = story.currentTags;

        if (storyTags.Count > 0) {
            string animationName = Helpers.GetTagValue("animation", storyTags);
            if (animationName.Length > 0) {
                // hide the dialogue box, given that we'll be doing some animation + movement
                if (_activeDialogueBox != null)
                    _activeDialogueBox.HideDialogueBox();

                _currentStoryAnimation = Constants.animationList[animationName];
                if (_currentStoryAnimation.movementFirst)
                    StartStoriedMovement();
                else
                    StartStoriedAnimation();
                
                return true;
            }
        }

        return false;
    }

    public void StartStoryDialogue() {
        
        if (_currentStoryLine.Length > 0) {
            // create a dialogue box above this object. This will also write the initial line
            _activeDialogueBox = UIControllerScript.Instance.ShowDialogueBox(gameObject, _currentStoryLine);

            // start listening for a 'dialogue progression' input event (the user has pressed the primary button)
            GameEventsScript.Instance.onProgressDialogueInput += ProgressStorySession;
        } else {
            // this was most likely called on a tag-only line. Let's just progress the story session.
            ProgressStorySession(gameObject);
        }
    }

    public void ProgressStorySession(GameObject activeObject) {
        // make sure that the event was actually published for this object
        if (activeObject == gameObject) {

            // indicator that any text in-progress can be fast typed.
            bool fastType = true;

            if (_activeDialogueBox.ReadyForProgression(fastType)) { // make sure we're not actively typing, or animating
                // progress the dialogue
                if (story.canContinue) {
                    _currentStoryLine = story.Continue();

                    if (CheckStoryTags())
                        return;

                    _activeDialogueBox.PrepareAndWrite(_currentStoryLine);
                } else {
                    // we can safely end the story session
                    EndStorySession();
                }
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
            StartStoryDialogue();
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
            StartStoryDialogue();
        else 
            StartStoriedMovement();
    }

    public void EndStorySession() {

        // stop ignoring collision between the player and physical objects
        Physics.IgnoreLayerCollision(Constants.PHYSICAL_OBJECT_LAYER, Constants.PLAYER_LAYER, false);

        // stop listening for the dialogue progression event
        GameEventsScript.Instance.onProgressDialogueInput -= ProgressStorySession;

        // hide the dialogue box
        _activeDialogueBox.HideDialogueBox();

        // publish an event indicating that we've completed the story session
        GameEventsScript.Instance.CompletedStorySession(gameObject);
    }
}
