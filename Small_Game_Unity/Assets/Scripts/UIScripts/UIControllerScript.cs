using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
*   Handles all UI components in the game. There should only be a single child (canvas).
*
***/
public class UIControllerScript : MonoBehaviour
{

    // reference to the world space canvas
    [SerializeField]
    private GameObject worldSpaceCanvas;

    // reference to the screen space canvas
    [SerializeField]
    private GameObject screenSpaceCanvas;

    // reference to the active camera
    [SerializeField]
    private CameraManager cameraManager;
    private Transform activeCameraTransform; // scale ui elements based on camera angle (maintain size)

    [SerializeField]
    // UI Elements
    private GameObject interactBubblePrefab;
    [SerializeField]
    private GameObject dialogueBoxPrefab;

    [SerializeField]
    private List<GameObject> interactBubblePool; // object pool to prevent runtime instantiation
    

    // start is called before the first frame update
    void Start()
    {
        // instantiate interactBubble and add to pool
        interactBubblePool = new List<GameObject>();
        for (int i = 0; i < Constants.POOL_COUNT; i++) {
            GameObject tmpObject = Instantiate(interactBubblePrefab, worldSpaceCanvas.transform);
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

            // start the entry animation
            newInteractionBubble.GetComponent<InteractionBubbleScript>().StartEntryAnimation();
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
                    // StartEndAnimation will also call SetActive(false) on completion
                    bubbleScript.StartEndAnimation();
                }
            }
        }
    }

    // make the world space canvas look at a given camera object
    public void BillboardCanvas(Camera targetCamera) {
        worldSpaceCanvas.transform.LookAt(worldSpaceCanvas.transform.position + targetCamera.transform.rotation * Vector3.back, targetCamera.transform.rotation * Vector3.up);

        RepositionBubbles();
    }

    // reposition all interaction bubbles (generally after a camera angle change)
    public void RepositionBubbles() {
        for (int i = 0; i < Constants.POOL_COUNT; i++) {
            if (interactBubblePool[i].activeInHierarchy) { 
                GameObject interactBubble;
                interactBubble = interactBubblePool[i];
                InteractionBubbleScript bubbleScript = interactBubble.GetComponent<InteractionBubbleScript>();
                GameObject targetObject = bubbleScript.GetTargetObject();

                // cancel tweens (prevents weird scaling issues)
                bubbleScript.CancelEntryExitTweens();

                // position
                BoxCollider targetObjectCollider = targetObject.GetComponent<BoxCollider>();
                Vector3 targetPosition = targetObject.transform.TransformPoint(targetObjectCollider.center); // get the world position of the collider

                targetPosition.y = targetObjectCollider.bounds.max.y; // bubble should sit above the top of the collider
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
        GameObject targetObject = bubbleScript.GetTargetObject();
        
        // cancel entry/exit tweens (prevents weird scaling issues)
        bubbleScript.CancelEntryExitTweens();

        // position
        BoxCollider targetObjectCollider = targetObject.GetComponent<BoxCollider>();
        Vector3 targetPosition = targetObject.transform.TransformPoint(targetObjectCollider.center); // get the world position of the collider
        
        targetPosition.y = targetObjectCollider.bounds.max.y; // bubble should sit above the top of the collider
        targetPosition.y += Constants.BUBBLE_POSITION_VERTICAL_BUFFER;

        interactBubble.transform.position = targetPosition; // update the position

        // scale
        activeCameraTransform = cameraManager.activeCamera.transform;
        float size = (activeCameraTransform.position - interactBubble.transform.position).magnitude;
        interactBubble.transform.localScale = new Vector3(size,size,size) * Constants.WORLD_SPACE_CANVAS_SCALE; // maintain scale across all elements
    }


}
