using UnityEngine;

/**
*   This class serves as an abstract interface for all concrete game flow states
*
**/
public abstract class GameFlowBaseState
{
    // what happens when we transition to this state
    public abstract void EnterState(GameFlowStateManager gameFlow);

    // what happens each frame of this state
    public abstract void UpdateState(GameFlowStateManager gameFlow);

    // what happens when we leave this state
    public abstract void ExitState(GameFlowStateManager gameFlow);


}
