using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowStateManager : MonoBehaviour
{

    [SerializeField]
    public GameFlowBaseState currentState;

    // concrete states
    GameFlowDecisionState DecisionState = new GameFlowDecisionState();
    GameFlowInteractionState InteractionState = new GameFlowInteractionState();

    // data-control between states
    public GameObject targetInteractiveObject;

    // GameFlowStateManager Singleton ref
    private static GameFlowStateManager _instance;
    public static GameFlowStateManager Instance { get { return _instance; } }

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
        // default state for the state machine
        currentState = DecisionState;

        currentState.EnterState(this);
    }

    // update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(GameFlowBaseState state) {
        currentState.ExitState(this); // leave the old state
        currentState = state;

        GameEventsScript.Instance.GameFlowStateTransitioned(state); // publish event indicating a transition to the new game flow state

        state.EnterState(this); // trigger the enter state functionality
    }

    /**
    *   data-control Methods (passing data between states)
    **/
    public void SelectedInteractiveObject(GameObject selectedObject) {
        targetInteractiveObject = selectedObject;
        SwitchState(InteractionState);
    }

    public void FinishedStorySession(GameObject selectedObject) {
        targetInteractiveObject = null;
        SwitchState(DecisionState); // switch back to the decision state
    }
}
