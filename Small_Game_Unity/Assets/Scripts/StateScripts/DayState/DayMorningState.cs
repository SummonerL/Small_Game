using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/**
*   Type: Constant State
*   Purpose: Day state that occurs when it is morning
**/
public class DayMorningState : DayBaseState
{
    public override void EnterState(DayStateManager dayState) {
        // use the 'morning' material on the window glass
        dayState.windowGlassObject.GetComponent<MeshRenderer>().material = dayState.glassMorningMaterial;

        // use the 'morning' PostFX Profile
        PostFXSingleton.Instance.GetComponent<PostProcessVolume>().profile = dayState.MorningProfile;

        // set the time!
        AlarmClockScript.Instance.ChangeClockText(Constants.MORNING_TIME);
    }

    public override void UpdateState(DayStateManager dayState) {

    }

    public override void ExitState(DayStateManager dayState) {
        // do something
    }
}
