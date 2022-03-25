using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class ObjectStoryManager : MonoBehaviour
{

    // Ink story files
    public TextAsset inkJSON;
    public Story story;

    private string _currentStoryLine;
    private DialogueBoxScript _activeDialogueBox;

    // start is called before the first frame update
    void Start()
    {
        story = new Story(inkJSON.text);
    }

    // if available, feeds the next story line 
    string FeedStoryLine() {
        return story.Continue();
    }
    
    /**
    *   A story 'session' refers to a series of story lines written to the dialogue box. 
    *   The user will progress the story session with the primary controller button (indicated by the ... icon)
    **/
    public void StartStorySession() {

        _currentStoryLine = story.Continue();

        // create a dialogue box above this object. This will also write the initial line
        _activeDialogueBox = UIControllerScript.Instance.ShowDialogueBox(gameObject, _currentStoryLine);

        // start listening for a 'dialogue progression' input event (the user has pressed the primary button)
        GameEventsScript.Instance.onProgressDialogueInput += ProgressStorySession;
    }

    public void ProgressStorySession(GameObject activeObject) {
        // make sure that the event was actually published for this object
        if (activeObject == gameObject) {
            // progress the dialogue
            if (story.canContinue) {
                _currentStoryLine = story.Continue();
                _activeDialogueBox.PrepareAndWrite(_currentStoryLine);
            } else {
                // we can safely end the story session
                EndStorySession();
            }

        }
    }

    public void EndStorySession() {
        // stop listening for the dialogue progression event
        GameEventsScript.Instance.onProgressDialogueInput -= ProgressStorySession;

        // hide the dialogue box
        _activeDialogueBox.HideDialogueBox();

        // publish an event indicating that we've completed the story session
        GameEventsScript.Instance.CompletedStorySession(gameObject);
    }
}
