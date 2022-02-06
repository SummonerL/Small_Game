using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjectsScript : MonoBehaviour
{

    private GameObject[] interactiveObjects; // keep track of all child gameObjects 

    // start is called before the first frame update
    void Start()
    {
        interactiveObjects = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++) {
            interactiveObjects[i] = transform.GetChild(i).gameObject;
        }
    }

    // a new object has been deemed eligible.
    public void HandleEligibleObject(GameObject eligibleObject) {

    }

    // an eligible object has been deemed ineligible (no longer in proximity)
    public void HandleIneligibleObject(GameObject ineligibleObject) {

    }
    
    public GameObject[] GetInteractiveObjects() {
        return interactiveObjects;
    }
}
