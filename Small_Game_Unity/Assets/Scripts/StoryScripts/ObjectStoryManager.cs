using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class ObjectStoryManager : MonoBehaviour
{

    // Ink story files
    public TextAsset inkJSON;
    public Story story;

    // start is called before the first frame update
    void Start()
    {
        story = new Story(inkJSON.text);

        GeneralStoryManager.Instance.BindEssentialFunctions(story);
    }
    
    /**
    *   A story 'session' refers to a series of story lines written to the dialogue box. 
    *   The user will progress the story session with the primary controller button (indicated by the ... icon)
    **/
    public void StartStorySession() {
        // use the General Story Manager to manage this active story
        GeneralStoryManager.Instance.StartStorySession(story);
    }
}
