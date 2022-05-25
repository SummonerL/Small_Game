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
        // determine which state to move to next
        if (CutsceneStoryManager.Instance.CheckForCutscenes()) {
            gameFlow.MoveToCutscene();
        } else {
            // move to the decision state
            gameFlow.MoveToDecision();
        }
    }

    public override void ExitState(GameFlowStateManager gameFlow) {

    }
}
