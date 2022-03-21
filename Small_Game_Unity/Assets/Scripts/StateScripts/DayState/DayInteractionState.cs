using UnityEngine;

/**
*   Type: Constant State
*   Purpose: Day state that occurs when the player decides to interact with a particular object
**/
public class DayInteractionState : DayBaseState
{
    public override void EnterState(DayStateManager day) {
        UIControllerScript.Instance.ShowDialogueBox(day.targetInteractiveObject);
    }

    public override void UpdateState(DayStateManager day) {

    }

    public override void ExitState(DayStateManager day) {

    }
}
