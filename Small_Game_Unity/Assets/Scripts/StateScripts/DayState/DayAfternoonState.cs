using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/**
*   Type: Constant State
*   Purpose: Day state that occurs when it is afternoon
**/
public class DayAfternoonState : DayBaseState
{
    public override void EnterState(DayStateManager dayState) {
        // use the 'noon' PostFX Profile
        PostFXSingleton.Instance.GetComponent<PostProcessVolume>().profile = dayState.NoonProfile;

        // set the time!
        AlarmClockScript.Instance.ChangeClockText(Constants.AFTERNOON_TIME);
    }

    public override void UpdateState(DayStateManager dayState) {

    }

    public override void ExitState(DayStateManager dayState) {
        // do something
    }
}
