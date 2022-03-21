using UnityEngine;

/**
*   Type: Constant State
*   Purpose: Day state that occurs when the player is deciding what to (what to interact with)
**/
public class DayDecisionState : DayBaseState
{
    public override void EnterState(DayStateManager day) {
        GameEventsScript.Instance.onSelectedInteractiveObject += day.SelectedInteractiveObject;
    }

    public override void UpdateState(DayStateManager day) {

    }

    public override void ExitState(DayStateManager day) {
        // we know longer need to observe this event, given that this state is not 'alive'
        GameEventsScript.Instance.onSelectedInteractiveObject -= day.SelectedInteractiveObject;
    }
}
