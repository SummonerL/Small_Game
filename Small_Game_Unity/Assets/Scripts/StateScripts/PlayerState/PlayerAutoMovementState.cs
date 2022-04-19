using UnityEngine;

/**
*   Type: Constant State
*   Purpose: Player state that occurs when the game is moving the player to a set position / direction. Generally this happens prior to some cutscene or animation.
**/
public class PlayerAutoMovementState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerState) {
        // disable player movement/interaction scripts
        PlayerMovementScript playerMovement = playerState.gameObject.GetComponent<PlayerMovementScript>();
        PlayerInteractionScript playerInteraction = playerState.gameObject.GetComponent<PlayerInteractionScript>();
        playerMovement.StopMovement(); // revert to standing position
        playerMovement.enabled = false;
        playerInteraction.enabled = false;
    }

    public override void UpdateState(PlayerStateManager playerState) {
        // move player to target position + direction
        PlayerAutomationScript automate = playerState.gameObject.GetComponent<PlayerAutomationScript>();

        if (automate.MoveTowardsPosition(new Vector3(-1.3f, 0, 1.3f), new Vector3(0, 0, 0))) {
            
            // player is within the walkingSpeed distance, snap to the position.
            Vector3 targetPosition = new Vector3(-1.3f, playerState.transform.position.y, 1.3f);
            playerState.transform.position = targetPosition;

            // player reached the target position, publish an event
            GameEventsScript.Instance.PlayerReachedPosition(); 
        }
        
    }

    public override void ExitState(PlayerStateManager playerState) {
        // activate player movement/interaction scripts
        playerState.gameObject.GetComponent<PlayerMovementScript>().enabled = true;
        playerState.gameObject.GetComponent<PlayerInteractionScript>().enabled = true;
    }
}
