using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class CutsceneStoryManager : MonoBehaviour
{
    // Ink story files
    public TextAsset inkJSON;
    public Story story;

    // CutsceneStoryManager Singleton Ref
    private static CutsceneStoryManager _instance;
    public static CutsceneStoryManager Instance { get { return _instance; } }

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
        story = new Story(inkJSON.text);

        GeneralStoryManager.Instance.BindEssentialFunctions(story);
    }

    public void StartStorySession() {
        // use the General Story Manager to manage this active story
        GeneralStoryManager.Instance.StartStorySession(story);
    }
}
