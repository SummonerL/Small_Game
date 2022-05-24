using UnityEngine;

/**
*   Type: Constant State
*   Purpose: Game Flow state that occurs when the nothing is happening. We will often sit in this state 
*   until an appropriate target state is decided.
**/
public class GameFlowIdleState : GameFlowBaseState
{
    public override void EnterState(GameFlowStateManager gameFlow) {
        
    }

    public override void UpdateState(GameFlowStateManager gameFlow) {
        // do nothing
    }

    public override void ExitState(GameFlowStateManager gameFlow) {

    }
}
