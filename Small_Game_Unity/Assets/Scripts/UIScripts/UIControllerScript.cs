using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
*   Handles all UI components in the game. There should only be a single child (canvas).
*
***/
public class UIControllerScript : MonoBehaviour
{

    // reference to the canvas
    private GameObject canvas;

    // reference to the camera manager
    [SerializeField]
    private CameraManager cameraManager;

    [SerializeField]
    // UI Elements
    private GameObject interactBubblePrefab;
    [SerializeField]
    private List<GameObject> interactBubblePool; // object pool to prevent runtime instantiation
    

    // start is called before the first frame update
    void Start()
    {
        canvas = transform.GetChild(0).gameObject;

        // instantiate interactBubble and add to pool
        interactBubblePool = new List<GameObject>();
        for (int i = 0; i < Constants.POOL_COUNT; i++) {
            GameObject tmpObject = Instantiate(interactBubblePrefab, canvas.transform);
            tmpObject.SetActive(false);
            interactBubblePool.Add(tmpObject);
        }

    }

    private GameObject GetPooledInteractionBubble() {
        for (int i = 0; i < Constants.POOL_COUNT; i++) {
            if (!interactBubblePool[i].activeInHierarchy) { // get first available int. bubble
                return interactBubblePool[i];
            }
        }

        return null;
    }

    // show interaction bubble above target object
    public void ShowInteractionBubble(GameObject targetObject) {
        // get a new interaction bubble
        GameObject newInteractionBubble = GetPooledInteractionBubble();

        if (newInteractionBubble != null) {
            Vector3 bubblePosition = cameraManager.activeCamera.WorldToScreenPoint(targetObject.transform.position); // determine screen pos from world pos
            newInteractionBubble.transform.position = bubblePosition;
            newInteractionBubble.SetActive(true);

            // set the reference to the target gameObject
            newInteractionBubble.GetComponent<InteractionBubbleScript>().SetTargetObject(targetObject);
        }
    }

    // hide the interaction bubble that is present above the target object
    public void HideInteractionBubble(GameObject targetObject) {
        // find the interaction bubble
        GameObject interactBubble;
        
        for (int i = 0; i < Constants.POOL_COUNT; i++) {
            if (interactBubblePool[i].activeInHierarchy) { 
                
                interactBubble = interactBubblePool[i];

                if (interactBubble.GetComponent<InteractionBubbleScript>().GetTargetObject() == targetObject) {
                    interactBubble.GetComponent<InteractionBubbleScript>().ClearTargetObject();
                    interactBubble.SetActive(false);
                }
            }
        }
    }
}
