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

    public string _cutscenePath = null;

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

        // unique functions
        story.BindExternalFunction("setCutscene", (string cutsceneName) => {
            SetCutscene(cutsceneName);
        });

        // we want to check for cutscenes on transition to the 'Decision' state
        GameEventsScript.Instance.onGameFlowStateTransitioned += HandleGameFlowTransition;
    }

    public void StartStorySession() {
        // use the General Story Manager to manage this active story
        GeneralStoryManager.Instance.StartStorySession(story);
    }

    // determine if a cutscene needs to be triggered at this moment in time
    public void CheckForCutscenes() {
        story.SwitchFlow("cutscene_check");
        story.ChoosePathString("cutscene_check");

        /* 
        *   this flow will run through the cutscene_check knot, and subsequently make 
        *   a call back to SetCutscene, if there is a cutscene.
        */

        story.Continue();

        if (_cutscenePath != null) {
            story.SwitchToDefaultFlow();
            story.RemoveFlow("cutscene_check");
            story.ChoosePathString(_cutscenePath);
            _cutscenePath = null;
            GameFlowStateManager.Instance.CutsceneReady();
        } else {
            story.SwitchToDefaultFlow();
            story.RemoveFlow("cutscene_check");
        }
    }

    public void SetCutscene(string cutsceneName) {
        _cutscenePath = cutsceneName;
    }

    public void HandleGameFlowTransition(GameFlowBaseState gameFlowState) {
        // when the game flow is idle, check for cutscenes
        if (gameFlowState.GetType() == typeof(GameFlowIdleState))
            CheckForCutscenes();
    }
}
