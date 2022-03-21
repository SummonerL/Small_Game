using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

    private void Awake() {
        if (transform.childCount > 0)
            textLabel = transform.GetChild(0).GetComponent<TMP_Text>();
    }

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
    
        // start writing by grabbing the current story line from our Ink file
        _currentStoryLine = InkManagerScript.Instance.story.Continue();

        if (PrepareBoxSizeForText(_currentStoryLine)) {
            // dialogue box pops up
            StartEntryAnimation();
        }


    }

    public void HideDialogueBox() {
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
    }

    // scale from 0,0,0 to 1,1,1 to 'pop up' the bubble
    public void StartEntryAnimation() {
        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
        _entryAnimationID = LeanTween.scale(gameObject, currentScale, Constants.DIALOGUE_BOX_ENTRY_TIME).setOnComplete(OnStartEntryComplete).id;
    }

    // scale back to 0,0,0, then deactivate the object
    public void StartEndAnimation() {
        
        // cancel all active tweens
        LeanTween.cancel(gameObject);

        // ending the animation, reset the rotation
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        _exitAnimationID = LeanTween.scale(gameObject, new Vector3(0, 0, 0), Constants.DIALOGUE_BOX_EXIT_TIME).setOnComplete(OnEndComplete).id;
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

    private void WriteText(string textToWrite) {
        GetComponent<TypeWriterEffectScript>().Run(textToWrite, textLabel);
    }
}
