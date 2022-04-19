using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    private CharacterController playerCharacterController;
    private PlayerAnimationController playerAnimationController;

    // capsule collider representing the 'front' of the player
    private CapsuleCollider playerFrontCollider;

    private CameraManager cameraManager;
    private Transform activeCameraTransform; // move based on camera angle

    [SerializeField]
    public float walkingSpeed = .85f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity; // holds smooth velocity from Mathf.SmoothDampAngle

    // start is called before the first frame update
    void Start()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerCharacterController = GetComponent<CharacterController>();
        playerFrontCollider = GetComponent<CapsuleCollider>();
        cameraManager = GameObject.FindWithTag("CameraManager").GetComponent<CameraManager>(); // CameraManager.cs
    }

    // update is called once per frame
    void Update()
    {
        // input from keys / controller
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // get the direction based on the input
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // player is wanting to move the character
        if (direction.magnitude >= 0.1f) {
        
            // get the active camera transform
            activeCameraTransform = cameraManager.activeCamera.transform;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + activeCameraTransform.eulerAngles.y; // target direction of model
            Vector3 calculatedMoveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // vector3 from target

            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // prevents sharp turns

            // update animation param (should trigger walk animation)
            playerAnimationController.SetAnimationParam<float>("player_speed", walkingSpeed); 
            
            
            
            // move the character based on direction
            playerCharacterController.Move(calculatedMoveDirection.normalized * walkingSpeed * Time.deltaTime);

            // rotate the character based on direction
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f); 

            // publish an event, indicating that the player moved
            GameEventsScript.Instance.PlayerMoved();
        } else {
            // should trigger idle animation
            playerAnimationController.SetAnimationParam<float>("player_speed", 0f);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        
        // determine if the 'front' collider intersects with the target collider
        if (playerFrontCollider.bounds.Intersects(hit.collider.bounds)) {
            
            // we don't really want the walk cycle animation to play right now.
            playerAnimationController.SetAnimationParam<float>("player_speed", 0f);
        }
    }

    public void StopMovement() {
        playerAnimationController.SetAnimationParam<float>("player_speed", 0f);
    }
}
