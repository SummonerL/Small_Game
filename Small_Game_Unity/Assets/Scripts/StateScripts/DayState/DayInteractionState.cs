using UnityEngine;

/**
*   Type: Constant State
*   Purpose: Day state that occurs when the player decides to interact with a particular object
**/
public class DayInteractionState : DayBaseState
{
    public override void EnterState(DayStateManager day) {
        ObjectStoryManager objectStory = day.targetInteractiveObject.GetComponent<ObjectStoryManager>();

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

    }
}
