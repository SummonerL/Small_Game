using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogueBoxScript : MonoBehaviour
{
    // reference to the object this dialogue box is above
    private GameObject _targetObject;

    // reference to child text label
    private TMP_Text textLabel;

    // reference to the animation tween ids
    private int _entryAnimationID = -1;
    private int _exitAnimationID = -1;

    private string _currentStoryLine;

    // keep a reference to the initial scale, so that we can 'reset' the box once deactivated/hidden
    private Vector3 _initialScale;

    // keep a reference to our progression dots
    public GameObject progressionDots;

    // keep a reference to the dialogue tail
    public GameObject dialogueBoxTail;

    // images used for internal dialogue
    public Sprite internalDialogueBoxSprite;
    public Sprite internalDialogueBoxProgressionDots;

    // images used for external dialogue
    public Sprite externalDialogueBoxSprite;
    public Sprite externalDialogueBoxProgressionDots;

    private void Awake() {
        if (transform.childCount > 0)
            textLabel = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    public void ShowDialogueBox(GameObject targetObject, string initialLine) { // passed from ui controller

        GameEventsScript.Instance.onActiveCameraChanged += PositionBox; // reposition the dialogue box if the camera angle changes
        GameEventsScript.Instance.onTypeWriterCompleted += StartDotAnimation; // trigger the progression dot animation
        GameEventsScript.Instance.onTypeWriterStarted += StopDotAnimation; // stop the dot animation

        _targetObject = targetObject;

        PositionBox(CameraManager.Instance.activeCamera);

        gameObject.SetActive(true);
    
        // set the current story line
        _currentStoryLine = initialLine;

        if (PrepareBoxSizeForText(_currentStoryLine)) {
            // dialogue box pops up
            StartEntryAnimation();
        }
    }

    // position the box based on the position of the target object
    public void PositionBox(Camera targetCamera) {
        
        Vector3 targetWorldPosition = Vector3.zero;

        // if this is the player, we'll just use his head bone instead of the collider
        if (_targetObject == PlayerSingleton.Instance.gameObject) {
            targetWorldPosition = PlayerSingleton.Instance.headBone.transform.position;
            targetWorldPosition.y += Constants.DIALOGUE_BOX_POSITION_VERTICAL_BUFFER_PLAYER;
        } else {
            // otherwise, we'll use the object's collider

            // get the generic collider - we don't care if it's capsule, box, etc.
            Collider targetObjectCollider = _targetObject.GetComponent<Collider>();
            targetWorldPosition = targetObjectCollider.bounds.center;
            targetWorldPosition.y = targetObjectCollider.bounds.max.y;
        }
        
        // using the distance of the camera to player's head, we can synchronize the position a bit more
        float cameraDistance = (targetCamera.transform.position - targetWorldPosition).magnitude;

        Vector3 targetScreenPosition = targetCamera.WorldToScreenPoint(targetWorldPosition);

        // increase y by a calculated amount - this should position the box above the player at (relatively) the same position, regardless of angle
        targetScreenPosition.y += (cameraDistance * Constants.DIALOGUE_BOX_POSITION_VERTICAL_BUFFER);

        transform.position = targetScreenPosition;
    }

    public void StartDotAnimation() {
        progressionDots.GetComponent<DialogueProgressionDotsScript>().StartWaitingAnimation();
    }

    public void StopDotAnimation() {
        progressionDots.GetComponent<DialogueProgressionDotsScript>().CancelTweens();
    }

    public void HideDialogueBox() {
        StopDotAnimation();
        StartEndAnimation();
    }

    // after start animation completes
    private void OnStartEntryComplete() {
        /* TODO: If we don't want words to move to the next line as they are being written, we can easily join the chunked
            string with an \n and pass that to WriteText(). Consider returning a string in PrepareBoxSizeForText() */
        WriteText(_currentStoryLine);
    }

    // after end animation completes
    private void OnEndComplete() {
        // clear the active object + deactivate
        _targetObject = null;
        gameObject.SetActive(false);
        
        // we no longer need to listen for events
        GameEventsScript.Instance.onActiveCameraChanged -= PositionBox;
        GameEventsScript.Instance.onTypeWriterCompleted -= StartDotAnimation;
        GameEventsScript.Instance.onTypeWriterStarted -= StopDotAnimation;

        // reset the box to ensure it is reusable
        Reset();
    }

    // scale from 0,0,0 to initial scale to 'pop up' the bubble
    public void StartEntryAnimation() {
        _initialScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
        _entryAnimationID = LeanTween.scale(gameObject, _initialScale, Constants.DIALOGUE_BOX_ENTRY_TIME).setOnComplete(OnStartEntryComplete).id;
    }

    // scale back to 0,0,0, then deactivate the object
    public void StartEndAnimation() {
        
        // cancel all active tweens
        LeanTween.cancel(gameObject);

        // ending the animation, reset the rotation
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        _exitAnimationID = LeanTween.scale(gameObject, new Vector3(0, 0, 0), Constants.DIALOGUE_BOX_EXIT_TIME).setOnComplete(OnEndComplete).id;
    }

    public bool IsAnimating() {
        if (_entryAnimationID > -1 && LeanTween.isTweening(_entryAnimationID)) {
            return true;
        }

        if (_exitAnimationID > -1 && LeanTween.isTweening(_exitAnimationID)) {
            return true;
        }

        return false;
    }

    /* Size the dialogue box appropriately, depending on the size of the string */ 
    private bool PrepareBoxSizeForText(string textForPreparation) {

        // split the text into chunks of similar size
        MatchCollection textCollection = Helpers.SplitToLines(textForPreparation, Constants.DIALOGUE_ROW_CHARACTER_LIMIT); 
        int rowCount = textCollection.Count;

        if (rowCount > 0 && rowCount <= Constants.DIALOGUE_MAX_ROW_COUNT_LIMIT) {
            
            // determine the maximum width based on each line of dialogue
            Vector2 preferredValues = Vector2.zero; // holds the width + height that would be required to fit text - should be based on the widest string
            float maxX = 0;
           
           for (int i = 0; i < rowCount; ++i) {
                string currentLine = textCollection[i].ToString();

                if (textLabel.GetPreferredValues(currentLine).x > maxX ) {
                    preferredValues = textLabel.GetPreferredValues(currentLine);
                    maxX = preferredValues.x;
                }
            }

            RectTransform dialogueBoxRectTransform = GetComponent<RectTransform>();
            RectTransform labelRectTransform = textLabel.gameObject.GetComponent<RectTransform>();

            // because our image has a buffer space to the left and right, let's determine that from the anchored label rect transform
            float totalHorizontalOffset = labelRectTransform.offsetMin.x * 2; // offset is equal
            float totalVerticalOffset = labelRectTransform.offsetMin.y * 2;

            float lineSpacing = textLabel.GetPreferredValues("T\nT").y - (preferredValues.y * 2); // use two lines to determine 'actual' line height

            // set the dialogue's width to the preferred width of the text + the offset
            dialogueBoxRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredValues.x + totalHorizontalOffset);

            // calculate the height based on the number of rows, preferred height, linespacing, and the label:container offset
            float dialogueBoxTotalHeight = totalVerticalOffset + ((preferredValues.y) * rowCount) + (lineSpacing * (rowCount - 1));
            dialogueBoxRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dialogueBoxTotalHeight);
            
            // Yay. The box should now PERFECTLY wrap the text :)

            return true;
        } else {
            return false; // the string is invalid
        }
    }

    public void PrepareAndWrite(string newStoryLine) {
        // set the current story line
        _currentStoryLine = newStoryLine;

        if (PrepareBoxSizeForText(_currentStoryLine)) {
            // write the text (without animation)
            WriteText(_currentStoryLine);
        }
    }

    // reset the dialogue box to it's initial state (pooled objects will be reused)
    public void Reset() {
        ClearText();
        transform.localScale = _initialScale;
    }

    // returns a bool indicating whether or not we can progress the dialogue.
    // will also invoke FastType() if the caller requests it.
    public bool ReadyForProgression(bool fastTypeIfEligible) {
        if (GetComponent<TypeWriterEffectScript>().IsRunning()) {
            if (fastTypeIfEligible) FastType();
            return false;
        } else if (IsAnimating()) {
            return false;
        } else {
            return true;
        }
    }

    // fast types the current story line, or returns false if not in an active coroutine
    public void FastType() {
        if (GetComponent<TypeWriterEffectScript>().IsRunning()) {
            // fast type
        }
    }

    // the 'external' box is used for offscreen dialogue, and differs slightly
    public void UseExternalBox() {
        // hide the tail
        dialogueBoxTail.SetActive(false);

        // use the 'external' sprites
        gameObject.GetComponent<Image>().sprite = externalDialogueBoxSprite;
        progressionDots.GetComponent<DialogueProgressionDotsScript>().UseExternalSprite(externalDialogueBoxProgressionDots);
    }

    // the 'external' text is just a change of color
    public void UseExternalText() {
        textLabel.color = Constants.EXTERNAL_FONT_COLOR;
    }

    public void UseInternalBox() {
        // unhide the tail
        dialogueBoxTail.SetActive(true);

        // use the 'internal' sprites
        gameObject.GetComponent<Image>().sprite = internalDialogueBoxSprite;
        progressionDots.GetComponent<DialogueProgressionDotsScript>().UseInternalSprite(internalDialogueBoxProgressionDots);
    }

    // the 'internal' text is just a change of color
    public void UseInternalText() {
        textLabel.color = Constants.INTERNAL_FONT_COLOR;
    }

    public void WriteText(string textToWrite) {
        GetComponent<TypeWriterEffectScript>().Run(textToWrite, textLabel);
    }

    public void ClearText() {
        textLabel.text = string.Empty;
    }
}
