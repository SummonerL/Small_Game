using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DayStateManager : MonoBehaviour
{

    [SerializeField]
    public DayBaseState currentState;

    // concrete states
    DayMorningState MorningState = new DayMorningState();

    DayAfternoonState AfternoonState = new DayAfternoonState();

    DayEveningState EveningState = new DayEveningState();

    private List<DayBaseState> _stateFlow;
    private int _stateFlowIndex = 0;

    // data-control between states
    public Material glassMorningMaterial;
    public Material glassNormalMaterial;
    public GameObject windowGlassObject;

    public PostProcessProfile MorningProfile;
    public PostProcessProfile NoonProfile;
    public PostProcessProfile EveningProfile;

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
        // initialize the state flow
        _stateFlow = new List<DayBaseState>() {
            MorningState,
            AfternoonState,
            EveningState
        };

        // default state for the state machine
        currentState = MorningState;

        currentState.EnterState(this);
    }

    // update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    private void SwitchState(DayBaseState state) {
        currentState.ExitState(this); // leave the old state
        currentState = state;

        GameEventsScript.Instance.DayStateTransitioned(state); // publish event indicating a transition to the new player-state

        state.EnterState(this); // trigger the new state functionality
    }

    public void AdvanceTime(int advance) {
        int newIndex = (_stateFlowIndex + advance) % _stateFlow.Count;

        _stateFlowIndex = newIndex;
        SwitchState(_stateFlow[newIndex]);
    } 

    /**
    *   data-control Methods (passing data between states)
    **/
}
