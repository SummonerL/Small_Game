using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionBubbleScript : MonoBehaviour
{

    private int _entryAnimationID = -1;
    private int _exitAnimationID = -1;
    private int _wiggleAnimationID = -1;
    private LTSeq _untargetedAnimationSequence;

    private GameObject _attachedObject; // ref to the object this bubble is hovering above

    // start is called before the first frame update
    void Start(){
        // subscribe to game events
        GameEventsScript.Instance.onActiveCameraChanged += RepositionBubble; // reposition and rescale the bubble when the active camera changes
        GameEventsScript.Instance.onNewEligibleObjectTargeted += DetermineAnimation; // if a new object has been targeted, we might need to 'untarget' this one
    }

    public void ShowInteractionBubble(GameObject targetObject) {

        // prior to showing the bubble, set the alpha accordingly
        if (PlayerInteractionScript.Instance.GetTargetedObject() == targetObject)
            SetAlpha(1.0f);
        else 
            SetAlpha(Constants.INTERACTION_BUBBLE_UNTARGETED_FADE_OPACITY);

        _attachedObject = targetObject;

        gameObject.SetActive(true);

        // posistioned above the target object
        RepositionBubble(CameraManager.Instance.activeCamera);

        // bubble pops up
        StartEntryAnimation();
    }

    public void HideInteractionBubble(GameObject targetObject) {
        if (targetObject == _attachedObject) { // ensure the bubble matches the passed in object
            // StartEndAnimation will also call SetActive(false) on completion
            StartEndAnimation();
        }
    }

    // reposition the interaction bubble (generally upon creation or camera change)
    public void RepositionBubble(Camera activeCamera) {
        if (gameObject.activeInHierarchy) {
            // cancel entry/exit tweens (prevents weird scaling issues)
            CancelEntryExitTweens();
        }

        // bubbles that were previously exit-tweening will no longer be active
        if (gameObject.activeInHierarchy) {
            // position
            BoxCollider targetObjectCollider = _attachedObject.GetComponent<BoxCollider>();
            Vector3 targetPosition = _attachedObject.transform.TransformPoint(targetObjectCollider.center); // get the world position of the collider
            
            targetPosition.y = targetObjectCollider.bounds.max.y; // bubble should sit above the top of the collider
            targetPosition.y += Constants.BUBBLE_POSITION_VERTICAL_BUFFER;

            transform.position = targetPosition; // update the position

            // scale
            float size = (activeCamera.transform.position - transform.position).magnitude;
            transform.localScale = new Vector3(size,size,size) * Constants.WORLD_SPACE_CANVAS_SCALE; // maintain scale across all elements
        }
    }

    private void SetAlpha(float alpha) {
        Image bubbleImage = GetComponent<Image>();
        Color tempColor = bubbleImage.color;
        tempColor.a = alpha;
        bubbleImage.color = tempColor;
    }

    // after start animation completes
    private void OnStartEntryComplete() {
        // start either the wiggle or untargeted animations
        DetermineAnimation(PlayerInteractionScript.Instance.GetTargetedObject());
    }

    // after end animation completes
    private void OnEndComplete() {
        // clear the active object + deactivate
        _attachedObject = null;
        gameObject.SetActive(false); 
    }

    // starts either the 'targeted' or 'untargeted' animations, depending on the current target
    public void DetermineAnimation(GameObject newTarget) {
        if (gameObject.activeInHierarchy) {
            if (newTarget != _attachedObject) {
                ShowUntargeted();
            } else {
                // this object is being targeted
                SetAlpha(1.0f);
                StartWiggleAnimation(Constants.DIRECTIONS.RIGHT, true);
            }
        }
    }

    // an 'untargeted' bubble will appear opaque, with no animation.
    public void ShowUntargeted() {

        // cancel all active tweens
        LeanTween.cancel(gameObject);

        RectTransform bubbleRectTransform = GetComponent<RectTransform>();
        _untargetedAnimationSequence = LeanTween.sequence();
        
        // start fading out asynchronously
        _untargetedAnimationSequence.append(() => {
            LeanTween.alpha(bubbleRectTransform, Constants.INTERACTION_BUBBLE_UNTARGETED_FADE_OPACITY, Constants.INTERACTION_BUBBLE_UNTARGETED_FADE_TIME);
        });

        // rotate back to the default position
        _untargetedAnimationSequence.append(LeanTween.rotateLocal(gameObject, new Vector3(0, 0, 0), Constants.INTERACTION_BUBBLE_UNTARGETED_ROTATE_BACK_TIME));
    }

    // scale from 0,0,0 to 1,1,1 to 'pop up' the bubble
    public void StartEntryAnimation() {
        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
        _entryAnimationID = LeanTween.scale(gameObject, currentScale, Constants.INTERACTION_BUBBLE_ENTRY_TIME).setOnComplete(OnStartEntryComplete).id;
    }

    // scale back to 0,0,0, then deactivate the object
    public void StartEndAnimation() {
        
        // cancel all active tweens
        LeanTween.cancel(gameObject);

        // ending the animation, reset the rotation
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        _exitAnimationID = LeanTween.scale(gameObject, new Vector3(0, 0, 0), Constants.INTERACTION_BUBBLE_EXIT_TIME).setOnComplete(OnEndComplete).id;
    }

    // when we reposition the camera, we want to cancel these tweens, to prevent scaling issues - we will still need to call their onCompletes
    public void CancelEntryExitTweens() {
        if (_entryAnimationID > -1 && LeanTween.isTweening(_entryAnimationID)) {
            LeanTween.cancel(_entryAnimationID);
            OnStartEntryComplete();
        }

        if (_exitAnimationID > -1 && LeanTween.isTweening(_exitAnimationID)) {
            LeanTween.cancel(_exitAnimationID);
            OnEndComplete();
        }
    }

    // a simple 'wiggle' animation that gives character to the icon.
    public void StartWiggleAnimation(Constants.DIRECTIONS direction, bool initialRotation = false) {

        // cancel all active tweens
        LeanTween.cancel(gameObject);

        // direction determines whether the Z is negative or positive
        int multiplier = 1;
        Constants.DIRECTIONS next;
        float timer = Constants.INTERACTION_BUBBLE_WIGGLE_TIME;

        if (direction == Constants.DIRECTIONS.LEFT) {
            multiplier = -1;
            next = Constants.DIRECTIONS.RIGHT;
        } else {
            next = Constants.DIRECTIONS.LEFT;
        }

        if (initialRotation)
            timer /= 2.0f; // movement from middle to side should happen half as slow

        _wiggleAnimationID = LeanTween.rotateLocal(gameObject, new Vector3(0, 0, Constants.INTERACTION_BUBBLE_WIGGLE_ROTATION_DEGREES * multiplier), timer)
            .setOnComplete(() => { StartWiggleAnimation(next); }).id;
    }
}
