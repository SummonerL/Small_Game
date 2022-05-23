using UnityEngine;

/**
*   Type: Constant State
*   Purpose: Game Flow state that occurs when a cutscene occurs
**/
public class GameFlowCutsceneState : GameFlowBaseState
{
    public override void EnterState(GameFlowStateManager gameFlow) {
        // since we're in a cutscene, there's no need to show interaction bubbles
        PlayerInteractionScript.Instance.clearInteractionEligibleObjects();
    
        // we also want to lock the script, given that there will be no new interactions
        PlayerInteractionScript.Instance.enabled = false;
        
        // start a cutscene story 'session'
        CutsceneStoryManager.Instance.StartStorySession();

        // listen to the 'completed story session' event
        GameEventsScript.Instance.onCompletedStorySession += gameFlow.FinishedStorySession;
    }

    public override void UpdateState(GameFlowStateManager gameFlow) {
        // check for input - this will progress the cutscene's 'story'
        if (Input.GetButtonDown("Keyboard_Enter") || Input.GetButtonDown("Joystick_Button_Down"))
            GameEventsScript.Instance.ProgressDialogueInput(gameFlow.targetInteractiveObject);
    }

    public override void ExitState(GameFlowStateManager gameFlow) {
        // reenable the interaction script
        PlayerInteractionScript.Instance.enabled = true;

        // and re-check objects in the proximity
        PlayerInteractionScript.Instance.CheckInteractiveEligibleObjects();
    }
}
