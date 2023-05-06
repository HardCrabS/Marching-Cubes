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
    
    private void Awake()
    {
        curStateType = StateType.Wander;

        stateTypeToComponent = new Dictionary<StateType, State>();
        stateTypeToComponent.Add(StateType.Wander, GetComponent<WanderState>());
        stateTypeToComponent.Add(StateType.Chase, GetComponent<ChaseState>());
        stateTypeToComponent.Add(StateType.Attack, GetComponent<AttackState>());
    }

    private void Start()
    {
        var curStateComponent = StateTypeToComponent(curStateType);
        curStateComponent.OnEnterState();
    }

    public State StateTypeToComponent(StateType stateType)
    {
        if (stateTypeToComponent.ContainsKey(stateType))
            return stateTypeToComponent[stateType];

        Debug.LogError($"StateType {stateType} not found!");
        return null;
    }

    private void Update()
    {
        var curStateComponent = StateTypeToComponent(curStateType);
        curStateComponent.Execute();

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

        // perform transition
        if (newStateType != curStateType)
        {
            curStateComponent.OnExitState();
            var newStateComponent = StateTypeToComponent(newStateType);
            newStateComponent.OnEnterState();
            curStateType = newStateType;
        }
    }
}
