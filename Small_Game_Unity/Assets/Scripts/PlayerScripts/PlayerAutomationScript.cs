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

    public bool MoveAndRotate(Vector3 position, Vector3 direction) {
        bool complete = false;
        
        if (MoveTowardsPosition(position)) {
            complete = RotateTowardsDirection(direction);
        }

        return complete;
    }

    // used to position the player for animation
    public bool MoveTowardsPosition(Vector3 position) {
        // this will be called each frame, via the state's 'update' method

        bool reachedDestination = false;

        // MoveTowards will calculate the new position (ignore the vertical distance)     
        Vector3 playerYIgnored = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 targetYIgnored = new Vector3(position.x, 0f, position.z);

        // determine if we're done moving
        float newDistance = Vector3.Distance(targetYIgnored, playerYIgnored);

        if (newDistance < (movementScript.walkingSpeed * Time.deltaTime)) {
            reachedDestination = true;
            playerAnimationController.SetAnimationParam<float>("player_speed", 0f);

            // player is within the walkingSpeed distance, snap to the position.
            Vector3 targetPosition = new Vector3(position.x, transform.position.y, position.z);
            transform.position = targetPosition;
        } else {
            Vector3 targetDirection = (targetYIgnored - playerYIgnored).normalized;
            float targetAngle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg; // target direction of model

            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, movementScript.turnSmoothTime); // prevents sharp turns

            // update animation param (should trigger walk animation)
            playerAnimationController.SetAnimationParam<float>("player_speed", movementScript.walkingSpeed);

            // move the character based on direction
            playerCharacterController.Move(targetDirection * movementScript.walkingSpeed * Time.deltaTime);

            // rotate the character based on direction
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        }

        return reachedDestination;
    }

    public bool RotateTowardsDirection(Vector3 direction) {
        // this will be called once per frame, and will rotate the player towards a target direction
        bool complete = false;

        Vector3 targetDirection = direction.normalized;
        float targetAngle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg; // target direction of model
        float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, movementScript.turnSmoothTime); // prevents sharp turns

        // rotate the character based on direction
        transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

        if (turnSmoothVelocity < movementScript.turnSmoothTime) {
            complete = true;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }

        return complete;
    }

}
