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

    // memory bank holds memories across all ink Story files. 
    public static List<string> memoryBank;

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
        memoryBank = new List<string>();
    }

    // called during story instantiation to bind the 'essential' functions to the story
    public void BindEssentialFunctions(Story inkStory) {
        inkStory.BindExternalFunction("addMemory", (string memoryName) => {
            AddMemory(memoryName);
        });

        inkStory.BindExternalFunction("checkMemory", (string memoryName) => {
            return CheckMemory(memoryName);
        });
    }

    // accessed as an 'external' function from our Ink stories
    public void AddMemory(string memoryName) {
        memoryBank.Add(memoryName);
    }

    // ink 'external' function
    public bool CheckMemory(string memoryName) {
        return memoryBank.Contains(memoryName);
    }
}
