using UnityEngine;

/**
*   Type: Constant State
*   Purpose: Player state that occurs when the player is inactive (usually standing still).
*   This is often aligned with the 'DayInteractionState', where the player is interacting with an object
**/
public class PlayerStoppedState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerState) {
        // disable player movement script
        PlayerMovementScript playerMovement = playerState.gameObject.GetComponent<PlayerMovementScript>();
        playerMovement.StopMovement();
        playerMovement.enabled = false;
    }

    public override void UpdateState(PlayerStateManager playerState) {

    }

    public override void ExitState(PlayerStateManager playerState) {
        // activate player movement script
        playerState.gameObject.GetComponent<PlayerMovementScript>().enabled = true;
    }
}
