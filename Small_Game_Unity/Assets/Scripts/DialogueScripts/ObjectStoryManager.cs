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
        DialogueBoxScript dialogBox = UIControllerScript.Instance.ShowDialogueBox(gameObject, _currentStoryLine);
    }
}
