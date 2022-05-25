using UnityEngine;

/**
*   Type: Constant State
*   Purpose: Game Flow state that occurs when the nothing is happening. We will often sit in this state 
*   until an appropriate target state is decided.
**/
public class GameFlowIdleState : GameFlowBaseState
{
    public override void EnterState(GameFlowStateManager gameFlow) {
        // since we're in a cutscene, there's no need to show interaction bubbles
        PlayerInteractionScript.Instance.clearInteractionEligibleObjects();
    
        // we also want to lock the script, given that there will be no new interactions
        PlayerInteractionScript.Instance.enabled = false;
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
        // reenable the interaction script
        PlayerInteractionScript.Instance.enabled = true;

        // and re-check objects in the proximity
        PlayerInteractionScript.Instance.CheckInteractiveEligibleObjects();
    }
}
