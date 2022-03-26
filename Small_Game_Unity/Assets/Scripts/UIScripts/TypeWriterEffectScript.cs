using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriterEffectScript : MonoBehaviour
{
    private bool _coroutineRunning = false; 

    public bool IsRunning() {
        return _coroutineRunning;
    }

    public void Run(string textToWrite, TMP_Text labelToWriteTo) {
        StartCoroutine(TypeText(textToWrite, labelToWriteTo));
    }

    private IEnumerator TypeText(string textToWrite, TMP_Text labelToWriteTo) {
        
        _coroutineRunning = true;

        float t = 0; // elapsed time since we began writing
        int charIndex = 0;
        
        labelToWriteTo.text = string.Empty;

        while (charIndex < textToWrite.Length) {
            t += Time.deltaTime * Constants.TYPEWRITER_SPEED_NORMAL;
            
            // get the current index based on the elapsed time
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToWrite.Length);

            labelToWriteTo.text = textToWrite.Substring(0, charIndex);

            yield return null;
        }

        labelToWriteTo.text = textToWrite;

        _coroutineRunning = false;
    }
}
