using UnityEngine;

/**
*   Type: Constant State
*   Purpose: Player state that occurs when the game is moving the player to a set position / direction. Generally this happens prior to some cutscene or animation.
**/
public class PlayerAutoMovementState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerState) {
        // do something
    }

    public override void UpdateState(PlayerStateManager playerState) {
        // move player to target position + direction
        PlayerMovementScript movement = playerState.gameObject.GetComponent<PlayerMovementScript>();
        movement.MoveTowardsPosition(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
    }

    public override void ExitState(PlayerStateManager playerState) {
        // do something
    }
}
