using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/***
*   This class tracks player proximity to interactive objects.
*
***/
public class PlayerInteractionScript : MonoBehaviour
{

    private CharacterController playerCharacterController;
    private List<GameObject> interactionEligibleObjects;

    // PlayerInteractionScript Singleton Ref
    private static PlayerInteractionScript _instance;
    public static PlayerInteractionScript Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    // start is called before the first frame update
    void Start()
    {
        playerCharacterController = GetComponent<CharacterController>();
        interactionEligibleObjects = new List<GameObject>();

        GameEventsScript.Instance.onPlayerMoved += CheckInteractiveEligibleObjects;
    }

    // update is called once per frame
    void Update()
    {
        // check for input 
        // TODO: Consider having an input manager, or creating Events for player input
        if ( interactionEligibleObjects.Count > 0 &&
            (Input.GetButtonDown("Keyboard_Enter") || Input.GetButtonDown("Joystick_Button_Down")) ) { ObjectSelection(interactionEligibleObjects[0]); }
    }

    // determine and track proximity to interactive objects
    public void CheckInteractiveEligibleObjects() {
        // use Physics.OverlapSphere to determine proximity to all interactive objects
        Vector3 playerPosition = transform.position + playerCharacterController.center;

        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, Constants.PLAYER_PROXIMITY_RADIUS);

        // filter list to only contain interactive objects
        List<GameObject> filteredCollisionsList = new List<GameObject>();

        int minAngleIndex = 0;
        float minAngle = Mathf.Infinity;

        // find relevant collisions + determine object the player is looking towards (the most)
        foreach(Collider collider in hitColliders) {
            if (Array.IndexOf(InteractiveObjectsScript.InteractiveObjects, collider.gameObject) > -1) { // make sure the object is actually interactive
                filteredCollisionsList.Add(collider.gameObject);

                // determine the angle between the player's forward and the interactive object (ignoring vertical index y)
                Vector3 playerForwardYIgnored = new Vector3(transform.forward.x, 0f, transform.forward.z);
                Vector3 objectDistance = collider.bounds.center - transform.position;
                Vector3 objectYIgnored = new Vector3(objectDistance.x, 0f, objectDistance.z);

                float angle = Mathf.Abs(Vector3.Angle(playerForwardYIgnored, objectYIgnored));

                if (angle < minAngle) {
                    minAngle = angle;
                    minAngleIndex = filteredCollisionsList.Count - 1;
                }
            }
        }

        // ensure the object the player is (mostly) facing sits in front
        if (minAngleIndex > 0) {
            GameObject tempObject = filteredCollisionsList[0];
            filteredCollisionsList[0] = filteredCollisionsList[minAngleIndex];
            filteredCollisionsList[minAngleIndex] = tempObject;
        }
        
        // check our existing list to see if the targeted object has changed
        if (interactionEligibleObjects.Count > 0 && filteredCollisionsList.Count > 0 &&
                interactionEligibleObjects[0] != filteredCollisionsList[0]) {
            GameEventsScript.Instance.NewEligibleObjectTargeted(filteredCollisionsList[0]);
        }

        // find the difference between the old list and the new list
        List<GameObject> additions = filteredCollisionsList.Except(interactionEligibleObjects).ToList();
        List<GameObject> removals = interactionEligibleObjects.Except(filteredCollisionsList).ToList();

        // update the tracked list
        interactionEligibleObjects = filteredCollisionsList;

        foreach (GameObject addedObject in additions) {
            // publish an event indicating player is in proximity of interactive object
            GameEventsScript.Instance.EligibleInteractiveObject(addedObject);
        }

        foreach (GameObject removedObject in removals) {
            // publish an event indicating player is no longer in proximity of interactive object
            GameEventsScript.Instance.IneligibleInteractiveObject(removedObject);
        }
    }

    // the player has selected an eligible interactive object
    void ObjectSelection(GameObject selectedObject) {
        // publish an event indicating player has selected an object
        GameEventsScript.Instance.SelectedInteractiveObject(selectedObject);
    }

    // publicly accessible method to get the currently targeted object
    public GameObject GetTargetedObject() {
        return (interactionEligibleObjects.Count > 0) ? interactionEligibleObjects[0] : null;
    }

    // remove all objects from the interaction eligible list, and publish the appropriate event
    public void clearInteractionEligibleObjects() {
        foreach (GameObject interactiveObject in interactionEligibleObjects) {
            // publish an event indicating player is no longer able to interact with this object
            GameEventsScript.Instance.IneligibleInteractiveObject(interactiveObject);
        }

        // clear the list
        interactionEligibleObjects = new List<GameObject>();
    }

    void OnDrawGizmos() {
        // used for debugging
        if (playerCharacterController != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + playerCharacterController.center, Constants.PLAYER_PROXIMITY_RADIUS);
        }
    }

    
    // these methods are used in animations to attach/unattach objects to the players hand and are triggered from animation keyframes
    public void PickUpObject() {
        // attach the object to the hand
        GameFlowStateManager.Instance.targetInteractiveObject.transform.SetParent(PlayerSingleton.Instance.handBone.transform, true);
    }

    // triggered from animation keyframe
    public void PutDownObject() {
        // unattach the object to the hand and move it back to 'interactive objects'
        GameFlowStateManager.Instance.targetInteractiveObject.transform.SetParent(InteractiveObjectsScript.Instance.gameObject.transform, true);
    }

}
