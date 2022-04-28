using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{

    [SerializeField]
    public PlayerBaseState currentState;

    // concrete states
    PlayerStoppedState StoppedState = new PlayerStoppedState();
    PlayerActiveState ActiveState = new PlayerActiveState();
    PlayerAutoMovementState AutoMovementState = new PlayerAutoMovementState();

    // data-control between states
    public Vector3 autoMovementPosition;
    public Vector3 autoMovementDirection;

    // PlayerStateManager Singleton ref
    private static PlayerStateManager _instance;
    public static PlayerStateManager Instance { get { return _instance; } }

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
        currentState = ActiveState;

        currentState.EnterState(this);

        // subscribe to events
        GameEventsScript.Instance.onGameFlowStateTransitioned += HandleGameFlowTransition;
        GameEventsScript.Instance.onPlayerReachedPosition += AutoMovementComplete;
    }

    // update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(PlayerBaseState state) {
        currentState.ExitState(this); // leave the old state
        currentState = state;

        GameEventsScript.Instance.PlayerStateTransitioned(state); // publish event indicating a transition to the new player-state

        state.EnterState(this); // trigger the new state functionality
    }

    public void HandleGameFlowTransition(GameFlowBaseState gameFlowState) {
        if (gameFlowState.GetType() == typeof(GameFlowInteractionState))
            SwitchState(StoppedState); // a transition to the 'interaction' game flow state will stop the player

        if (gameFlowState.GetType() == typeof(GameFlowDecisionState))
            SwitchState(ActiveState); // the player can begin 'deciding' again and is now active
    }

    public void AutoMovementComplete() {
        // we're done with the auto-movement. Switch back to the relevant player state.
        HandleGameFlowTransition(GameFlowStateManager.Instance.currentState);
    }

    public void StartStoriedMovement(Vector3 autoMovementPosition, Vector3 autoMovementDirection) {
        this.autoMovementPosition = autoMovementPosition;
        this.autoMovementDirection = autoMovementDirection;
        SwitchState(AutoMovementState);
    }

    /**
    *   data-control Methods (passing data between states)
    **/
}
