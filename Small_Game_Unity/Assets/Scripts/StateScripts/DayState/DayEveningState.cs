using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/**
*   Type: Constant State
*   Purpose: Day state that occurs when it is evening
**/
public class DayEveningState : DayBaseState
{
    public override void EnterState(DayStateManager dayState) {
        // use the 'standard' material on the window glass
        dayState.windowGlassObject.GetComponent<MeshRenderer>().material = dayState.glassNormalMaterial;

        // use the 'night' PostFX Profile
        PostFXSingleton.Instance.GetComponent<PostProcessVolume>().profile = dayState.EveningProfile;

        // set the time!
        AlarmClockScript.Instance.changeClockText(Constants.NIGHT_TIME);
    }

    public override void UpdateState(DayStateManager dayState) {

    }

    public override void ExitState(DayStateManager dayState) {
        // do something
    }
}
