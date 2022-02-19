using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxScript : MonoBehaviour
{
    // reference to the object this dialogue box is above
    private GameObject _targetObject;

    public void ShowDialogueBox(GameObject targetObject) { // passed from ui controller
        _targetObject = targetObject;

        // get the generic collider - we don't care if it's capsule, box, etc.
        Collider targetObjectCollider = targetObject.GetComponent<Collider>();

        Vector3 targetWorldPosition = targetObjectCollider.bounds.center;
        targetWorldPosition.y = targetObjectCollider.bounds.max.y;
        
        // using the distance of the camera to player's head, we can synchronize the position a bit more
        float cameraDistance = (CameraManager.Instance.activeCamera.transform.position - targetWorldPosition).magnitude;

        Vector3 targetScreenPosition = CameraManager.Instance.activeCamera.WorldToScreenPoint(targetWorldPosition);

        // increase y by a calculated amount - this should position the box above the player at (relatively) the same position, regardless of angle
        targetScreenPosition.y += (cameraDistance * Constants.DIALOGUE_BOX_POSITION_VERTICAL_BUFFER);

        transform.position = targetScreenPosition;
        
        gameObject.SetActive(true);
    }

    public void HideDialogueBox() {
        gameObject.SetActive(false);
    }
}
