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


    // start is called before the first frame update
    void Start()
    {
        playerCharacterController = GetComponent<CharacterController>();
        interactionEligibleObjects = new List<GameObject>();
    }

    // update is called once per frame
    void Update()
    {
        // use Physics.OverlapSphere to determine proximity to all interactive objects
        Vector3 playerPosition = transform.position + playerCharacterController.center;

        Collider[] hitColliders = Physics.OverlapSphere(playerPosition, Constants.PLAYER_PROXIMITY_RADIUS);

        // filter list to only contain interactive objects
        List<GameObject> filteredCollisionsList = new List<GameObject>();

        foreach(Collider collider in hitColliders) {
            if (Array.IndexOf(InteractiveObjectsScript.InteractiveObjects, collider.gameObject) > -1) { // make sure the object is actually interactive
                filteredCollisionsList.Add(collider.gameObject);
            }
        }
        
        // find the difference between the old list and the new list
        List<GameObject> additions = filteredCollisionsList.Except(interactionEligibleObjects).ToList();
        List<GameObject> removals = interactionEligibleObjects.Except(filteredCollisionsList).ToList();

        foreach (GameObject addedObject in additions) {
            // publish an event indicating player is in proximity of interactive object
            GameEventsScript.Instance.EligibleInteractiveObject(addedObject);
        }

        foreach (GameObject removedObject in removals) {
            // publish an event indicating player is no longer in proximity of interactive object
            GameEventsScript.Instance.IneligibleInteractiveObject(removedObject);
        }

        interactionEligibleObjects = filteredCollisionsList;
    }

    void OnDrawGizmos() {
        // used for debugging
        if (playerCharacterController != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + playerCharacterController.center, Constants.PLAYER_PROXIMITY_RADIUS);
        }
    }
}
