using UnityEngine;

/**
*   Type: Constant State
*   Purpose: Day state that occurs when it is morning
**/
public class DayMorningState : DayBaseState
{
    public override void EnterState(DayStateManager dayState) {
        // use the 'morning' material on the window glass
        dayState.windowGlassObject.GetComponent<MeshRenderer>().material = dayState.glassMorningMaterial;

        AlarmClockScript.Instance.changeClockText("09:00");
    }

    public override void UpdateState(DayStateManager dayState) {

    }

    public override void ExitState(DayStateManager dayState) {
        // do something
    }
}
