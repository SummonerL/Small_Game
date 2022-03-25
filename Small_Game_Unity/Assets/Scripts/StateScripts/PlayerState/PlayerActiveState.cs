using UnityEngine;

/**
*   Type: Constant State
*   Purpose: Player state that occurs when the player is actively doing something (usually walking around the room)
**/
public class PlayerActiveState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerState) {
        // do something
    }

    public override void UpdateState(PlayerStateManager playerState) {

    }

    public override void ExitState(PlayerStateManager playerState) {
        // do something
    }
}
