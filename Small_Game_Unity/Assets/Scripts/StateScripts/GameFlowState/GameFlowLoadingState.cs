using UnityEngine;

/**
*   Type: Constant State
*   Purpose: Game Flow state that occurs when the game is launched. We use this to allow all scripts to initialize before transitioning to the
*   appropriate state.
**/
public class GameFlowLoadingState : GameFlowBaseState
{
    public override void EnterState(GameFlowStateManager gameFlow) {
        
    }

    public override void UpdateState(GameFlowStateManager gameFlow) {
        gameFlow.FinishedLoading();
    }

    public override void ExitState(GameFlowStateManager gameFlow) {

    }
}
