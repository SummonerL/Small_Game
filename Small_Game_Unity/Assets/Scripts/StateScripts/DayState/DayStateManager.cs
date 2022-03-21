using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayStateManager : MonoBehaviour
{

    [SerializeField]
    public DayBaseState currentState;

    // concrete states
    DayDecisionState DecisionState = new DayDecisionState();
    DayInteractionState InteractionState = new DayInteractionState();

    // data-control between states
    public GameObject targetInteractiveObject;

    // DayStateManager Singleton ref
    private static DayStateManager _instance;
    public static DayStateManager Instance { get { return _instance; } }

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

    public void SwitchState(DayBaseState state) {
        currentState.ExitState(this); // leave the old state
        currentState = state;
        state.EnterState(this); // enter the new state
    }

    /**
    *   data-control Methods (passing data between states)
    **/
    public void SelectedInteractiveObject(GameObject selectedObject) {
        targetInteractiveObject = selectedObject;
        SwitchState(InteractionState);
    }
}
