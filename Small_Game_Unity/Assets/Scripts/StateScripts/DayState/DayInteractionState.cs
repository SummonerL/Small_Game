using UnityEngine;

/**
*   Type: Constant State
*   Purpose: Day state that occurs when the player decides to interact with a particular object
**/
public class DayInteractionState : DayBaseState
{
    public override void EnterState(DayStateManager day) {
        ObjectStoryManager objectStory = day.targetInteractiveObject.GetComponent<ObjectStoryManager>();

        // since we're interacting with the object, there's no need to show interaction bubbles
        PlayerInteractionScript.Instance.clearInteractionEligibleObjects();
    
        // we also want to lock the script, given that there will be no new interactions
        PlayerInteractionScript.Instance.enabled = false;
        
        // start a story 'session' with the target object
        objectStory.StartStorySession();

        // listen to the 'completed story session' event
        GameEventsScript.Instance.onCompletedStorySession += day.FinishedStorySession;
    }

    public override void UpdateState(DayStateManager day) {
        // check for input - this will progress the 'story' of the current object
        if (Input.GetButtonDown("Keyboard_Enter") || Input.GetButtonDown("Joystick_Button_Down"))
            GameEventsScript.Instance.ProgressDialogueInput(day.targetInteractiveObject);
    }

    public override void ExitState(DayStateManager day) {
        // reenable the interaction script
        PlayerInteractionScript.Instance.enabled = true;

        // and re-check objects in the proximity
        PlayerInteractionScript.Instance.CheckInteractiveEligibleObjects();
    }
}
