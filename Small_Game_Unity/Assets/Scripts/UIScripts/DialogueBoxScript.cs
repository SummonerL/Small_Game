using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBoxScript : MonoBehaviour
{
    // reference to the object this dialogue box is above
    private GameObject _targetObject;

    // Reference to child text label
    private TMP_Text textLabel;

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

        if (PrepareBoxSizeForText("Alright. Here is a super really cool dialogue test. Wowzers! I'm excited. Pizza!")) {
            WriteText("Alright. Here is a super really cool dialogue test. Wowzers! I'm excited. Pizza!");
        }
        
    }

    public void HideDialogueBox() {
        gameObject.SetActive(false);
    }

    /* Size the dialogue box appropriately, depending on the size of the string */ 
    private bool PrepareBoxSizeForText(string textForPreparation) {
        float contentLength = textForPreparation.Length; 
        int rowCount = Mathf.CeilToInt(contentLength / Constants.DIALOGUE_ROW_CHARACTER_LIMIT);

        if (rowCount > 0 && rowCount <= Constants.DIALOGUE_MAX_ROW_COUNT_LIMIT) {
            
            // determine the maximum width based on each line of dialogue

            string firstLine = (textForPreparation.Length <= Constants.DIALOGUE_ROW_CHARACTER_LIMIT) ? 
                textForPreparation : textForPreparation.Substring(0, Constants.DIALOGUE_ROW_CHARACTER_LIMIT);

            //Vector2 preferredValues = textLabel.GetPreferredValues(firstLine); 
            Vector2 preferredValues = Vector2.zero;// the width + height that would be required to fit text - should be based on the widest string
            
            float maxX = 0;
            for (int i = 0; i < rowCount; ++i) {
                string currentLine = textForPreparation.Substring(i * Constants.DIALOGUE_ROW_CHARACTER_LIMIT);
                if (currentLine.Length > Constants.DIALOGUE_ROW_CHARACTER_LIMIT)
                    currentLine = currentLine.Substring(0, Constants.DIALOGUE_ROW_CHARACTER_LIMIT);
                
                Debug.Log(currentLine);
                Debug.Log(textLabel.GetPreferredValues(currentLine).x);

                if (textLabel.GetPreferredValues(currentLine).x > maxX ) {
                    preferredValues = textLabel.GetPreferredValues(currentLine);
                    maxX = preferredValues.x;
                }
            }

            Debug.Log(preferredValues.ToString());

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

            return true;
        } else {
            return false; // the string is invalid
        }
    }

    private void WriteText(string textToWrite) {
        GetComponent<TypeWriterEffectScript>().Run(textToWrite, textLabel);
    }
}
