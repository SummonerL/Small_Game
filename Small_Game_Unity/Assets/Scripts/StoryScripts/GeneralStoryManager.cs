using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

/***
*   This class will function as a controller / manager for the overall story. This class sits at the topmost layer, and will 
*   utilize the object stories, cutscene stories, and any exclusive story classes. 
***/
public class GeneralStoryManager : MonoBehaviour
{

    // keep a singleton reference to this script
    private static GeneralStoryManager _instance;

    public static GeneralStoryManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    // find the value of a given story variable
    public void GetStoryVariable(string variableName) {
        foreach (GameObject interactiveObject in InteractiveObjectsScript.InteractiveObjects) {
            Story objectStory = interactiveObject.GetComponent<ObjectStoryManager>().story;

            Debug.Log(objectStory.variablesState[variableName]);
        }
    }
}
