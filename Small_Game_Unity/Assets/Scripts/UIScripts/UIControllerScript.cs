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

    // reference to the active camera
    [SerializeField]
    private CameraManager cameraManager;
    private Transform activeCameraTransform; // scale ui elements based on camera angle (maintain size)

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
            // set the reference to the target gameObject
            newInteractionBubble.GetComponent<InteractionBubbleScript>().SetTargetObject(targetObject);
            
            // position the new bubble
            RepositionBubble(newInteractionBubble);

            // activate it
            newInteractionBubble.SetActive(true);
        }
    }

    // hide the interaction bubble that is present above the target object
    public void HideInteractionBubble(GameObject targetObject) {
        // find the interaction bubble
        for (int i = 0; i < Constants.POOL_COUNT; i++) {
            if (interactBubblePool[i].activeInHierarchy) { 
                GameObject interactBubble;
                interactBubble = interactBubblePool[i];

                InteractionBubbleScript bubbleScript = interactBubble.GetComponent<InteractionBubbleScript>();

                if (bubbleScript.GetTargetObject() == targetObject) {
                    bubbleScript.ClearTargetObject();
                    interactBubble.SetActive(false);
                }
            }
        }
    }

    // make the canvas look at a given camera object
    public void BillboardCanvas(Camera targetCamera) {
        canvas.transform.LookAt(canvas.transform.position + targetCamera.transform.rotation * Vector3.back, targetCamera.transform.rotation * Vector3.up);

        RepositionBubbles();
    }

    // reposition all interaction bubbles (generally after a camera angle change)
    public void RepositionBubbles() {
        for (int i = 0; i < Constants.POOL_COUNT; i++) {
            if (interactBubblePool[i].activeInHierarchy) { 
                GameObject interactBubble;
                interactBubble = interactBubblePool[i];
                InteractionBubbleScript bubbleScript = interactBubble.GetComponent<InteractionBubbleScript>();
                
                // position
                Vector3 targetPosition = bubbleScript.GetTargetObject().transform.position;
                targetPosition.y = bubbleScript.GetTargetObject().GetComponent<BoxCollider>().bounds.max.y;
                targetPosition.y += Constants.BUBBLE_POSITION_VERTICAL_BUFFER;

                interactBubble.transform.position = targetPosition; // update the position

                // scale
                activeCameraTransform = cameraManager.activeCamera.transform;
                float size = (activeCameraTransform.position - interactBubble.transform.position).magnitude;
                interactBubble.transform.localScale = new Vector3(size,size,size) * Constants.WORLD_SPACE_CANVAS_SCALE; // maintain scale across all elements
            }
        }
    }

    // reposition a single interaction bubble (generally upon creation)
    public void RepositionBubble(GameObject interactBubble) {
        InteractionBubbleScript bubbleScript = interactBubble.GetComponent<InteractionBubbleScript>();
        
        // position
        Vector3 targetPosition = bubbleScript.GetTargetObject().transform.position;
        targetPosition.y = bubbleScript.GetTargetObject().GetComponent<BoxCollider>().bounds.max.y;
        targetPosition.y += Constants.BUBBLE_POSITION_VERTICAL_BUFFER;

        interactBubble.transform.position = targetPosition; // update the position

        // scale
        activeCameraTransform = cameraManager.activeCamera.transform;
        float size = (activeCameraTransform.position - interactBubble.transform.position).magnitude;
        interactBubble.transform.localScale = new Vector3(size,size,size) * Constants.WORLD_SPACE_CANVAS_SCALE; // maintain scale across all elements

    }


}
