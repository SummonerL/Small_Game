using UnityEngine;

/**
*   Type: Constant State
*   Purpose: Game Flow state that occurs when the player decides to interact with a particular object
**/
public class GameFlowInteractionState : GameFlowBaseState
{
    public override void EnterState(GameFlowStateManager gameFlow) {
        ObjectStoryManager objectStory = gameFlow.targetInteractiveObject.GetComponent<ObjectStoryManager>();

        // since we're interacting with the object, there's no need to show interaction bubbles
        PlayerInteractionScript.Instance.clearInteractionEligibleObjects();
    
        // we also want to lock the script, given that there will be no new interactions
        PlayerInteractionScript.Instance.enabled = false;
        
        // start a story 'session' with the target object
        objectStory.StartStorySession();

        // listen to the 'completed story session' event
        GameEventsScript.Instance.onCompletedStorySession += gameFlow.FinishedStorySession;
    }

    public override void UpdateState(GameFlowStateManager gameFlow) {
        // check for input - this will progress the 'story' of the current object
        if (Input.GetButtonDown("Keyboard_Enter") || Input.GetButtonDown("Joystick_Button_Down"))
            GameEventsScript.Instance.ProgressDialogueInput(gameFlow.targetInteractiveObject);
    }

    public override void ExitState(GameFlowStateManager gameFlow) {
        // reenable the interaction script
        PlayerInteractionScript.Instance.enabled = true;

        // and re-check objects in the proximity
        PlayerInteractionScript.Instance.CheckInteractiveEligibleObjects();
    }
}
