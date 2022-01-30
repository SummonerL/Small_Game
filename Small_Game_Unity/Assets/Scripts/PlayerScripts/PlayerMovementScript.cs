using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    public CharacterController playerCharacterController;
    public PlayerAnimationController playerAnimationController;

    // capsule collider representing the 'front' of the player
    public CapsuleCollider playerFrontCollider;

    public CameraManager cameraManager;
    public Transform activeCameraTransform; // move based on camera angle

    [SerializeField]
    public float walkingSpeed = .85f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity; // holds smooth velocity from Mathf.SmoothDampAngle

    // Start is called before the first frame update
    void Start()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerCharacterController = GetComponent<CharacterController>();
        playerFrontCollider = GetComponent<CapsuleCollider>();
        cameraManager = GameObject.FindWithTag("CameraManager").GetComponent<CameraManager>(); // CameraManager.cs
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (direction.magnitude >= 0.1f) { // player is wanting to move the character
            // get the active camera transform
            activeCameraTransform = cameraManager.activeCamera.transform;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + activeCameraTransform.eulerAngles.y; // target direction of model
            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            playerAnimationController.SetAnimationParam<float>("player_speed", walkingSpeed); // update animation param (should trigger walk animation)
            
            Vector3 calculatedMoveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerCharacterController.Move(calculatedMoveDirection.normalized * walkingSpeed * Time.deltaTime); // move the character based on direction
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f); // rotate the character based on direction
        } else {
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

}
