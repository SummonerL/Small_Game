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
    }

    public override void UpdateState(DayStateManager day) {

    }

    public override void ExitState(DayStateManager day) {

    }
}
