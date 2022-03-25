using UnityEngine;

/**
*   This class serves as an abstract interface for all concrete player states
*
**/
public abstract class PlayerBaseState
{
    // what happens when we transition to this state
    public abstract void EnterState(PlayerStateManager playerState);

    // what happens each frame of this state
    public abstract void UpdateState(PlayerStateManager playerState);

    // what happens when we leave this state
    public abstract void ExitState(PlayerStateManager playerState);


}
