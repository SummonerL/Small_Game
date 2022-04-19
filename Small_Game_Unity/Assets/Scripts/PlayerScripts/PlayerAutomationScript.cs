using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
*   This class manages 'automation' of the player. This is used when we want to control certain player actions, like movement, 
*   for the purpose of animation or cutscenes
***/
public class PlayerAutomationScript : MonoBehaviour
{
    private CharacterController playerCharacterController;
    private PlayerAnimationController playerAnimationController;

    private PlayerMovementScript movementScript;

    float turnSmoothVelocity; // holds smooth velocity from Mathf.SmoothDampAngle

    // start is called before the first frame update
    void Start()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerCharacterController = GetComponent<CharacterController>();
        movementScript = GetComponent<PlayerMovementScript>();
    }

    // used to position the player for animation
    public bool MoveTowardsPosition(Vector3 position, Vector3 direction) {
        // this will be called each frame, via the state's 'update' method

        bool reachedDestination = false;

        // MoveTowards will calculate the new position (ignore the vertical distance)     
        Vector3 playerYIgnored = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 targetYIgnored = new Vector3(position.x, 0f, position.z);

        Vector3 targetDirection = (targetYIgnored - playerYIgnored).normalized;
        float targetAngle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg; // target direction of model

        float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, movementScript.turnSmoothTime); // prevents sharp turns

        // update animation param (should trigger walk animation)
        playerAnimationController.SetAnimationParam<float>("player_speed", movementScript.walkingSpeed);

        // move the character based on direction
        playerCharacterController.Move(targetDirection * movementScript.walkingSpeed * Time.deltaTime);

        // rotate the character based on direction
        transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

        // determine if we're done moving
        float newDistance = Vector3.Distance(targetYIgnored, playerYIgnored);

        reachedDestination = (newDistance < (movementScript.walkingSpeed * Time.deltaTime));

        return reachedDestination;
    }

}
