using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class CutsceneStoryManager : MonoBehaviour
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
}
