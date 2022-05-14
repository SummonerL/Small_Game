using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    private static PlayerSingleton _instance;

    public static PlayerSingleton Instance { get { return _instance; } }

    /**
    *   keep track of various bones on our character rig. This will allow us to move objects easily or determine target positions for UI elements
    **/

    // head bone used for UI elements (dialogue box)
    public GameObject headBone;

    // hand bone used for picking up objects (e.g. phone)
    public GameObject handBone;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }


    // triggered from animation keyframe
    public void PickUpObject() {
        Debug.Log("Picking up!");

        Debug.Log(GameFlowStateManager.Instance.targetInteractiveObject);

        // attach the object to the hand
        GameFlowStateManager.Instance.targetInteractiveObject.transform.SetParent(handBone.transform, true);
    }
}
