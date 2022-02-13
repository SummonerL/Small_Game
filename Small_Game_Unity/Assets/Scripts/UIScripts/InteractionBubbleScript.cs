using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBubbleScript : MonoBehaviour
{

    // todo: break out constants?

    private int entryAnimationID = -1;
    private int exitAnimationID = -1;
    private int wiggleAnimationID = -1;

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

    // when we reposition the camera, we want to cancel these tweens, to prevent scaling issues - we will still need to call their onCompletes
    public void CancelEntryExitTweens() {
        if (entryAnimationID > -1 && LeanTween.isTweening(entryAnimationID)) {
            LeanTween.cancel(entryAnimationID);
            OnStartEntryComplete();
        }

        if (exitAnimationID > -1 && LeanTween.isTweening(exitAnimationID)) {
            LeanTween.cancel(exitAnimationID);
            OnEndComplete();
        }
    }

    // after start animation completes
    private void OnStartEntryComplete() {
        // start the wiggle animation
        StartWiggleAnimation(Constants.DIRECTIONS.RIGHT, true);
    }

    // after end animation completes
    private void OnEndComplete() {
        // clear the active object + deactivate
        ClearTargetObject();
        gameObject.SetActive(false); 
    }

    // scale from 0,0,0 to 1,1,1 to 'pop up' the bubble
    public void StartEntryAnimation() {
        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
        entryAnimationID = LeanTween.scale(gameObject, currentScale, 0.2f).setOnComplete(OnStartEntryComplete).id;
    }

    // scale back to 0,0,0, then deactivate the object
    public void StartEndAnimation() {
        
        // cancel all active tweens
        LeanTween.cancel(gameObject);

        // ending the animation, reset the rotation
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        exitAnimationID = LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.1f).setOnComplete(OnEndComplete).id;
    }

    // a simple 'wiggle' animation that gives character to the icon.
    public void StartWiggleAnimation(Constants.DIRECTIONS direction, bool initialRotation = false) {
        
        // direction determines whether the Z is negative or positive
        int multiplier = 1;
        Constants.DIRECTIONS next;
        float timer = 1f;

        if (direction == Constants.DIRECTIONS.LEFT) {
            multiplier = -1;
            next = Constants.DIRECTIONS.RIGHT;
        } else {
            next = Constants.DIRECTIONS.LEFT;
        }

        if (initialRotation)
            timer /= 2.0f; // movement from middle to side should happen half as slow

        wiggleAnimationID = LeanTween.rotateLocal(gameObject, new Vector3(0, 0, 5.0f * multiplier), timer).setOnComplete(() => { StartWiggleAnimation(next); }).id;
    }
}
