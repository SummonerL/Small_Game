using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
*   Small uses an Event/Observer system. The purpose of this class
*   is to provide a list of Actions and their corresponding dispatch methods.
**/
public class GameEventsScript : MonoBehaviour
{

    // GameEventsScript Singleton Ref
    private static GameEventsScript _instance;
    public static GameEventsScript Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    /**
    *   Interactive Object Events
    **/

    // when player moves into proximity of interactive object
    public event Action<GameObject> onEligibleInteractiveObject; 
    public void EligibleInteractiveObject(GameObject eligibleObject) {
        if (onEligibleInteractiveObject != null) {
            onEligibleInteractiveObject(eligibleObject);
        }
    }

    // when player leaves proximity of interactive object
    public event Action<GameObject> onIneligibleInteractiveObject; 
    public void IneligibleInteractiveObject(GameObject ineligibleObject) {
        if (onIneligibleInteractiveObject != null) {
            onIneligibleInteractiveObject(ineligibleObject);
        }
    }

    // when the player 'selects' an eligible interactive object (initiated by the primary controller button)
    public event Action<GameObject> onSelectedInteractiveObject; 
    public void SelectedInteractiveObject(GameObject selectedObject) {
        if (onSelectedInteractiveObject != null) {
            onSelectedInteractiveObject(selectedObject);
        }
    }

    // when a player 'targets' an eligible object (this applies to multiple eligible objects, only one can be targeted at a time)
    public event Action<GameObject> onNewEligibleObjectTargeted;
    public void NewEligibleObjectTargeted(GameObject targetedObject) {
        if (onNewEligibleObjectTargeted != null) {
            onNewEligibleObjectTargeted(targetedObject);
        }
    }

    /**
    *  Story / Dialogue Events
    **/

    // when the typewriter has finished typing a block of text
    public event Action onTypeWriterCompleted; 
    public void TypeWriterCompleted() {
        if (onTypeWriterCompleted != null) {
            onTypeWriterCompleted();
        }
    }

    // when the typewriter has started typing a block of text
    public event Action onTypeWriterStarted; 
    public void TypeWriterStarted() {
        if (onTypeWriterStarted != null) {
            onTypeWriterStarted();
        }
    }

    // when the player wants to progress dialogue (usually by pressing the primary button / key)
    public event Action<GameObject> onProgressDialogueInput; 
    public void ProgressDialogueInput(GameObject activeObject) {
        if (onProgressDialogueInput != null) {
            onProgressDialogueInput(activeObject);
        }
    }

    // when the player has completed a story session
    public event Action<GameObject> onCompletedStorySession; 
    public void CompletedStorySession(GameObject activeObject) {
        if (onCompletedStorySession != null) {
            onCompletedStorySession(activeObject);
        }
    }

    /**
    *   Camera Events
    **/ 

    // when the active camera changes
    public event Action<Camera> onActiveCameraChanged;
    public void ActiveCameraChanged(Camera newCamera) {
        if (onActiveCameraChanged != null) {
            onActiveCameraChanged(newCamera);
        }
    }

    /**
    *   State Events
    **/

    // when a day state transition occurs
    public event Action<DayBaseState> onDayStateTransitioned;
    public void DayStateTransitioned(DayBaseState newState) {
        if (newState != null) {
            onDayStateTransitioned(newState);
        }
    }

    // when a player state transition occurs
    public event Action<PlayerBaseState> onPlayerStateTransitioned;
    public void PlayerStateTransitioned(PlayerBaseState newState) {
        if (newState != null) {
            // no active subscribers
            //onPlayerStateTransitioned(newState);
        }
    }

}
