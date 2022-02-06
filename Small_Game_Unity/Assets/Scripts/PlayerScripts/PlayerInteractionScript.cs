using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{

    private CharacterController playerCharacterController;
    private InteractiveObjectsScript interactiveObjectController;
    
    [SerializeField]
    private List<GameObject> interactionEligibleObjects;

    [SerializeField]
    private GameObject interactiveObjectsObject;

    // start is called before the first frame update
    void Start()
    {
        playerCharacterController = GetComponent<CharacterController>();
        interactiveObjectController = interactiveObjectsObject.GetComponent<InteractiveObjectsScript>();
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
            if (Array.IndexOf(interactiveObjectController.GetInteractiveObjects(), collider.gameObject) > -1) { // make sure the object is actually interactive
                filteredCollisionsList.Add(collider.gameObject);
            }
        }
        
        // find the difference between the old list and the new list
        List<GameObject> additions = filteredCollisionsList.Except(interactionEligibleObjects).ToList();
        List<GameObject> removals = interactionEligibleObjects.Except(filteredCollisionsList).ToList();

        foreach (GameObject addedObject in additions) {
            Debug.Log(addedObject.name + " added.");
        }

        foreach (GameObject removedObject in removals) {
            Debug.Log(removedObject.name + " removed.");
        }

        interactionEligibleObjects = filteredCollisionsList;
    }

    void OnDrawGizmos() {
        // Debugging
        if (playerCharacterController != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + playerCharacterController.center, Constants.PLAYER_PROXIMITY_RADIUS);
        }
    }
}
