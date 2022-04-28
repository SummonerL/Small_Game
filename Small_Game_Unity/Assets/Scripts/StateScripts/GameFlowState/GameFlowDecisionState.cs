using UnityEngine;

/**
*   Type: Constant State
*   Purpose: Game Flow state that occurs when the player is deciding what to (what to interact with)
**/
public class GameFlowDecisionState : GameFlowBaseState
{
    public override void EnterState(GameFlowStateManager gameFlow) {
        GameEventsScript.Instance.onSelectedInteractiveObject += gameFlow.SelectedInteractiveObject;
    }

    public override void UpdateState(GameFlowStateManager gameFlow) {

    }

    public override void ExitState(GameFlowStateManager gameFlow) {
        // we know longer need to observe this event, given that this state is not 'alive'
        GameEventsScript.Instance.onSelectedInteractiveObject -= gameFlow.SelectedInteractiveObject;
    }
}
