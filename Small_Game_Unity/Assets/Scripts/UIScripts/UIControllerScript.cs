using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/***
*   Handles all UI components in the game. There should only be a World + Screen canvas child
*
***/
public class UIControllerScript : MonoBehaviour
{
    private GameObject _worldSpaceCanvas;
    private GameObject _screenSpaceCanvas;

    // various UI Elements
    [SerializeField]
    private GameObject _interactBubblePrefab;
    [SerializeField]
    private GameObject _dialogueBoxPrefab;

    [SerializeField]
    private GameObject _screenFader;

    // object pools to prevent runtime instantiation
    private List<GameObject> _interactBubblePool; 
    private List<GameObject> _dialogueBoxPool;

    // UI Controller Singleton Ref
    private static UIControllerScript _instance;
    public static UIControllerScript Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    // start is called before the first frame update
    void Start()
    {
        // initialize canvas references with singletons
        _worldSpaceCanvas = WorldSpaceCanvasSingleton.Instance.gameObject;
        _screenSpaceCanvas = ScreenSpaceCanvasSingleton.Instance.gameObject;

        // instantiate interactBubble and add to pool
        _interactBubblePool = new List<GameObject>();
        for (int i = 0; i < Constants.POOL_COUNT; i++) {
            GameObject tmpObject = Instantiate(_interactBubblePrefab, _worldSpaceCanvas.transform);
            tmpObject.SetActive(false);
            _interactBubblePool.Add(tmpObject);
        }

        // instantiate dialogueBox and add to the pool
        _dialogueBoxPool = new List<GameObject>();
        for (int i = 0; i < Constants.POOL_COUNT; i++) {
            GameObject tmpObject = Instantiate(_dialogueBoxPrefab, _screenSpaceCanvas.transform);
            tmpObject.SetActive(false);
            _dialogueBoxPool.Add(tmpObject);
        }

        // subscribe to game events
        GameEventsScript.Instance.onEligibleInteractiveObject += ShowInteractionBubble; // show interaction bubble when object becomes eligible
        GameEventsScript.Instance.onIneligibleInteractiveObject += HideInteractionBubble; // hide interaction bubble when object becomes ineligible
        GameEventsScript.Instance.onActiveCameraChanged += BillboardCanvas; // angle the canvas toward the new camera
    }

    // object pool methods
    private GameObject GetPooledInteractionBubble() {
        for (int i = 0; i < Constants.POOL_COUNT; i++) {
            if (!_interactBubblePool[i].activeInHierarchy) { // get first available int. bubble
                return _interactBubblePool[i];
            }
        }

        return null;
    }

    private GameObject GetPooledDialogueBox() {
        for (int i = 0; i < Constants.POOL_COUNT; i++) {
            if (!_dialogueBoxPool[i].activeInHierarchy) { // get first available dialogue box
                return _dialogueBoxPool[i];
            }
        }

        return null;
    }

    // show an interaction bubble above target object
    public void ShowInteractionBubble(GameObject targetObject) {
        // get a new interaction bubble
        GameObject newInteractionBubble = GetPooledInteractionBubble();

        if (newInteractionBubble != null) {
            // invoke the instance's ShowInteractionBubble
            newInteractionBubble.GetComponent<InteractionBubbleScript>().ShowInteractionBubble(targetObject);
        }
    }

    // hide the interaction bubble that is present above the target object
    public void HideInteractionBubble(GameObject targetObject) {
        // find the interaction bubble
        for (int i = 0; i < Constants.POOL_COUNT; i++) {
            if (_interactBubblePool[i].activeInHierarchy) { 
                GameObject interactBubble;
                interactBubble = _interactBubblePool[i];

                // invoke the instance's HideInteractionBubble
                interactBubble.GetComponent<InteractionBubbleScript>().HideInteractionBubble(targetObject);
            }
        }
    }

    // show the dialogue box above the current target.
    public DialogueBoxScript ShowDialogueBox(GameObject targetObject, string initialLine) {
        GameObject dialogueBox = GetPooledDialogueBox();
        DialogueBoxScript dbScript = dialogueBox.GetComponent<DialogueBoxScript>();
        dbScript.ShowDialogueBox(PlayerSingleton.Instance.gameObject, initialLine);

        return dbScript;
    }

    // make the world space canvas look at a given camera object
    public void BillboardCanvas(Camera targetCamera) {
        _worldSpaceCanvas.transform.LookAt(_worldSpaceCanvas.transform.position + targetCamera.transform.rotation * Vector3.back, targetCamera.transform.rotation * Vector3.up);
    }

    public void FadeOut() {
        // use the screen fader to fade to black
        Image screenFaderImage = _screenFader.GetComponent<Image>();
        LeanTween.value(_screenFader, 0f, 1f, Constants.SCREEN_FADE_OUT_TIME)
        .setOnUpdate((float val) =>
        {
            Color c = screenFaderImage.color;
            c.a = val;
            screenFaderImage.color = c;
        })
        .setOnComplete(() => {
            // publish an event, indicating that we've finished fading out
            GameEventsScript.Instance.ScreenFadedOut();
        });
    }

    public void FadeIn() {
        // use the screen fader to fade in from black
        Image screenFaderImage = _screenFader.GetComponent<Image>();
        LeanTween.value(_screenFader, 1f, 0f, Constants.SCREEN_FADE_IN_TIME)
        .setOnUpdate((float val) =>
        {
            Color c = screenFaderImage.color;
            c.a = val;
            screenFaderImage.color = c;
        })
        .setOnComplete(() => {
            // publish an event, indicating that we've finished fading in
            GameEventsScript.Instance.ScreenFadedIn();
        });
    }
}
