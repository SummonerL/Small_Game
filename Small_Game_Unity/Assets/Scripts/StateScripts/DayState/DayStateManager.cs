using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayStateManager : MonoBehaviour
{

    [SerializeField]
    public DayBaseState currentState;

    // concrete states
    DayMorningState MorningState = new DayMorningState();

    DayAfternoonState AfternoonState = new DayAfternoonState();

    DayEveningState EveningState = new DayEveningState();

    // data-control between states
    [SerializeField]
    public Material glassMorningMaterial;

    [SerializeField]
    public Material glassNormalMaterial;

    [SerializeField] 
    public GameObject windowGlassObject;


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
        currentState = MorningState;

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

        GameEventsScript.Instance.DayStateTransitioned(state); // publish event indicating a transition to the new player-state

        state.EnterState(this); // trigger the new state functionality
    }

    /**
    *   data-control Methods (passing data between states)
    **/
}
