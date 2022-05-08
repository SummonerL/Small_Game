using System.Collections;
using System.Collections.Generic;
using System;
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

    public Constants.MONTHS currentMonth;
    public int currentDay;

    // material / Rendering references
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

        // set the current date
        currentMonth = Constants.MONTHS.October;
        currentDay = 11;

        UpdateCalendar();

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

        // check to see if it's a new day
        int dayAdvancements = Mathf.FloorToInt( (_stateFlowIndex + advance) / (float) _stateFlow.Count );

        int daysInMonth = DateTime.DaysInMonth(2022, (int)currentMonth);
        int monthsInYear = Enum.GetNames(typeof(Constants.MONTHS)).Length;

        currentDay += dayAdvancements;

        if (currentDay > daysInMonth) {
            currentDay -= daysInMonth;
            currentMonth += 1;

            if ((int)currentMonth > monthsInYear)
                currentMonth -= monthsInYear;
        }

        UpdateCalendar();

        _stateFlowIndex = newIndex;
        SwitchState(_stateFlow[newIndex]);
    }

    public void UpdateCalendar() {
        CalendarScript.Instance.ChangeMonthText(currentMonth.ToString());
        CalendarScript.Instance.ChangeDayText(currentDay);
    }

    /**
    *   data-control Methods (passing data between states)
    **/
}
