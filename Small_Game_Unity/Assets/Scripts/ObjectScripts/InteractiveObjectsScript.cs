using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
*   This class maintains a list of interactive objects, and provides handlers for
*   objects entering/leaving the player's proximity
***/
public class InteractiveObjectsScript : MonoBehaviour
{

    private GameObject[] interactiveObjects; // keep track of all child gameObjects 
    private UIControllerScript uiController;

    [SerializeField]
    private GameObject uiControllerGameObject;

    // start is called before the first frame update
    void Start()
    {
        uiController = uiControllerGameObject.GetComponent<UIControllerScript>();
        interactiveObjects = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++) {
            interactiveObjects[i] = transform.GetChild(i).gameObject;
        }
    }

    // a new object is within player proximity.
    public void HandleEligibleObject(GameObject eligibleObject) {
        // show interaction bubble
        uiController.ShowInteractionBubble(eligibleObject);
    }

    // an object is no longer within player proximity
    public void HandleIneligibleObject(GameObject ineligibleObject) {
        uiController.HideInteractionBubble(ineligibleObject);
    }
    
    public GameObject[] GetInteractiveObjects() {
        return interactiveObjects;
    }
}
