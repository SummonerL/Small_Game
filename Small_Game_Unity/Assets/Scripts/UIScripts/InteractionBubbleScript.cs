using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBubbleScript : MonoBehaviour
{

    // todo: break out constants?

    [SerializeField]
    private GameObject targetObject; // reference to the object this bubble is hovering above

    public void SetTargetObject(GameObject targetObject) {
        this.targetObject = targetObject;
    }

    public void ClearTargetObject() {
        this.targetObject = null;
    }

    public GameObject GetTargetObject() {
        return this.targetObject;
    }

    // scale from 0,0,0 to 1,1,1 to 'pop up' the bubble
    public void StartEntryAnimation() {
        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, currentScale, 0.2f).setOnComplete(() => { StartWiggleAnimation(Constants.DIRECTIONS.RIGHT, true); });
    }

    // scale back to 0,0,0, then deactivate the object
    public void StartEndAnimation() {
        
        // cancel all active tweens
        LeanTween.cancel(gameObject);

        // ending the animation, reset the rotation
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.1f).setOnComplete(() => { gameObject.SetActive(false); });
    }

    // a simple 'wiggle' animation that gives character to the icon.
    public void StartWiggleAnimation(Constants.DIRECTIONS direction, bool initialRotation = false) {
        
        // direction determines whether the Z is negative or positive
        int multiplier = 1;
        Constants.DIRECTIONS next;
        float timer = 1.0f;

        if (direction == Constants.DIRECTIONS.LEFT) {
            multiplier = -1;
            next = Constants.DIRECTIONS.RIGHT;
        } else {
            next = Constants.DIRECTIONS.LEFT;
        }

        if (initialRotation)
            timer /= 2.0f; // movement from middle to side should happen half as slow

        LeanTween.rotateLocal(gameObject, new Vector3(0, 0, 6.0f * multiplier), timer).setOnComplete(() => { StartWiggleAnimation(next); });
    }
}
