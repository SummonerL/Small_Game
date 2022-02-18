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

    // when the active camera changes
    public event Action<Camera> onActiveCameraChanged;
    public void ActiveCameraChanged(Camera newCamera) {
        if (onActiveCameraChanged != null) {
            onActiveCameraChanged(newCamera);
        }
    }
}
