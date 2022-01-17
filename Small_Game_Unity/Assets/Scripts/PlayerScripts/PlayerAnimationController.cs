using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    Animator playerAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set idle param when key is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            playerAnimator.SetBool("idle", !playerAnimator.GetBool("idle")); // opposite of current value
        }
    }
}
