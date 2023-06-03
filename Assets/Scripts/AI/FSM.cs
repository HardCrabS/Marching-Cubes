using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StateType
{
    // order defines priority
    Idle,
    Wander,
    Chase,
    Attack
}


public class FSM : MonoBehaviour
{
    StateType curStateType;

    Dictionary<StateType, State> stateTypeToComponent;

    bool isPlayerDead = false;
    
    private void Awake()
    {
        curStateType = StateType.Idle;

        stateTypeToComponent = new Dictionary<StateType, State>();
        stateTypeToComponent.Add(StateType.Wander, GetComponent<WanderState>());
        stateTypeToComponent.Add(StateType.Chase, GetComponent<ChaseState>());
        stateTypeToComponent.Add(StateType.Attack, GetComponent<AttackState>());
    }

    private void Start()
    {
        EventsDispatcher.Instance.onPlayerDead += () => { isPlayerDead = true; };

        var curStateComponent = StateTypeToComponent(curStateType);
        if (curStateComponent)
            curStateComponent.OnEnterState();
    }

    public State StateTypeToComponent(StateType stateType)
    {
        if (stateTypeToComponent.ContainsKey(stateType))
            return stateTypeToComponent[stateType];
        if (stateType == StateType.Idle)
            return null;

        Debug.LogError($"StateType {stateType} not found!");
        return null;
    }

    private void Update()
    {
        var curStateComponent = StateTypeToComponent(curStateType);
        if (curStateComponent)
            curStateComponent.Execute();

        StateType newStateType = GetTransitionState();

        // perform transition
        if (newStateType != curStateType)
        {
            if (curStateComponent)
                curStateComponent.OnExitState();
            var newStateComponent = StateTypeToComponent(newStateType);
            if (newStateComponent)
                newStateComponent.OnEnterState();
            curStateType = newStateType;
        }
    }

    StateType GetTransitionState()
    {
        if (isPlayerDead)
            return StateType.Wander;

        // check transition by finding the state with highest priority
        StateType newStateType = StateType.Idle;
        foreach (var stateComponent in stateTypeToComponent.Values)
        {
            var stateType = stateComponent.DecideTransition();
            if (stateType > newStateType)
            {
                newStateType = stateType;
            }
        }

        return newStateType;
    }
}
