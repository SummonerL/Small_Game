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
        // check for exclusive events before moving back to the linear scripts
        story.ChoosePathString("exclusive_events");
        // continue will run through the external functions and evaluate any necessary conditions.
        // the $. catches the continue and saves us a lot of pain down the road.
        story.Continue();

        // use the General Story Manager to manage this active story
        GeneralStoryManager.Instance.StartStorySession(story);
    }
}
