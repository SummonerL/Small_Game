using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueProgressionDotsScript : MonoBehaviour
{
    // keep a reference to all child objects (progression dots)
    private static Image[] _progressionDots; // keep track of all child gameObjects 

    // reference to the animation tween ids
    private LTSeq _waitingAnimationSequence;

    // Start is called before the first frame update
    void Start()
    {
        _progressionDots = new Image[transform.childCount];

        for (int i = 0; i < transform.childCount; i++) {
            _progressionDots[i] = transform.GetChild(i).gameObject.GetComponent<Image>();
            Color tempColor = _progressionDots[i].color;
            tempColor.a = 0f;
            _progressionDots[i].color = tempColor;
        } 
    }

    // dots pop up sequentially, then fade out sequentially.
    public void StartWaitingAnimation() {

        // start a LeanTween sequence
        _waitingAnimationSequence = LeanTween.sequence();

        for (int i = 0; i < _progressionDots.Length; ++i) {
            RectTransform dotTransform = _progressionDots[i].gameObject.transform as RectTransform;

            // show the current dot
            _waitingAnimationSequence.append(LeanTween.alpha(dotTransform, 1f , Constants.DIALOGUE_PROGRESSION_DOT_SHOW_TIME));
            _waitingAnimationSequence.append(Constants.DIALOGUE_PROGRESSION_DOT_DELAY_TIME);
        }

        for (int i = 0; i < _progressionDots.Length; ++i) {
            RectTransform dotTransform = _progressionDots[i].gameObject.transform as RectTransform;

            // show the current dot
            _waitingAnimationSequence.append(LeanTween.alpha(dotTransform, 0f , Constants.DIALOGUE_PROGRESSION_DOT_SHOW_TIME));
            _waitingAnimationSequence.append(Constants.DIALOGUE_PROGRESSION_DOT_DELAY_TIME);
        } 

        _waitingAnimationSequence.append(OnWaitingAnimationComplete);
    }

    public void OnWaitingAnimationComplete() {
        // loop the animation until further notice
        StartWaitingAnimation();
    }

    public void CancelTweens() {
        if (_waitingAnimationSequence != null) {
            LeanTween.cancel(_waitingAnimationSequence.id);

            // hide progression dots
            for (int i = 0; i < _progressionDots.Length; ++i) {
                Color tempColor = _progressionDots[i].color;
                tempColor.a = 0f;
                _progressionDots[i].color = tempColor;
            } 
        }
    }


}
