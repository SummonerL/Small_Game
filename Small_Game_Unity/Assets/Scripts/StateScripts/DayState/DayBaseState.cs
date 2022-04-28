using UnityEngine;

/**
*   This class serves as an abstract interface for all concrete day states
*
**/
public abstract class DayBaseState
{
    // what happens when we transition to this state
    public abstract void EnterState(DayStateManager dayState);

    // what happens each frame of this state
    public abstract void UpdateState(DayStateManager dayState);

    // what happens when we leave this state
    public abstract void ExitState(DayStateManager dayState);


}
