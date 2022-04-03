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
    void CheckInteractiveEligibleObjects() {
        // use Physics.OverlapSphere to determine proximity to all interactive objects
        Vector3 playerPosition = transform.position + playerCharacterController.center;

        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, Constants.PLAYER_PROXIMITY_RADIUS);

        // filter list to only contain interactive objects
        List<GameObject> filteredCollisionsList = new List<GameObject>();

        int minDistanceIndex = 0;
        float minDistance = Mathf.Infinity;

        // find relevant collisions + determine closest object
        foreach(Collider collider in hitColliders) {
            if (Array.IndexOf(InteractiveObjectsScript.InteractiveObjects, collider.gameObject) > -1) { // make sure the object is actually interactive
                filteredCollisionsList.Add(collider.gameObject);

                // get the distance between the player and the interactive object (ignoring vertical index Y)
                Vector3 playerYIgnored = new Vector3(transform.position.x, 0f, transform.position.z);
                Vector3 objectYIgnored = new Vector3(collider.bounds.center.x, 0f, collider.bounds.center.z);

                float distance = Vector3.Distance(playerYIgnored, objectYIgnored);

                if (distance < minDistance) {
                    minDistance = distance;
                    minDistanceIndex = filteredCollisionsList.Count - 1;
                }
            }
        }

        // ensure the closest object sits in front
        if (minDistanceIndex > 0) {
            GameObject tempObject = filteredCollisionsList[0];
            filteredCollisionsList[0] = filteredCollisionsList[minDistanceIndex];
            filteredCollisionsList[minDistanceIndex] = tempObject;
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

    void OnDrawGizmos() {
        // used for debugging
        if (playerCharacterController != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + playerCharacterController.center, Constants.PLAYER_PROXIMITY_RADIUS);
        }
    }
}
