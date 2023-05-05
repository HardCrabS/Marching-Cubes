using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StateType
{
    Unknown,
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
        StateType newStateType = curStateComponent.OnUpdate();

        if (newStateType != curStateType)
        {
            curStateComponent.OnExitState();
            var newStateComponent = StateTypeToComponent(newStateType);
            newStateComponent.OnEnterState();
            curStateType = newStateType;
        }
    }
}
