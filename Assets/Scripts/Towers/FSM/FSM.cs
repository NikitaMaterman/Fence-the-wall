using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM {
	public String Name;
	private FSMState currentState;
	private readonly Dictionary<string, FSMState> states = new Dictionary<string, FSMState>();

	public FSM(String name)
	{
		Name = name;
	}

	// Use this for initialization
	public void Start (String stateName) {
		if (!states.ContainsKey(stateName)) {
			Debug.LogWarning("The FSM doesn't contain: " + stateName);
			return;
		}

		ChangeToState(states[stateName]);
	}

	private delegate void StateActionProcessor(FSMAction action);

	private void ProcessStateAction(FSMState state, StateActionProcessor actionProcessor) {
		FSMState currentStateOnInvoke = currentState;
		foreach (var action in state.Actions) {
			// Avoid processing the action when the state has changed
			if (currentState != currentStateOnInvoke) {
				break;
			}

			actionProcessor(action);
		}
	}
	
	public FSMState AddState(string stateName) {
		if (states.ContainsKey(stateName)) {
			return null;
		}

		FSMState newState = new FSMState(stateName, this);
		states[stateName] = newState;
		return newState;
	}

	public void Update() {
		if (currentState != null) {
			ProcessStateAction(currentState, action => {
				action.OnUpdate();
			});
		}
	}

	public void ChangeToState(FSMState state) {
		if (currentState != null) {
			ExitState(currentState);
		}

		currentState = state;
		EnterState(currentState);
	}

	public void EnterState(FSMState state) {
		ProcessStateAction(state, action => {
			action.OnEnter();
		});
	}

	public void ExitState(FSMState state) {
		ProcessStateAction(state, action => {
			action.OnExit();
		});
	}

	public void SendEvent(string eventName) {
		FSMState eventState = currentState.GetEvent(eventName);
		if (eventState != null) {
			ChangeToState(eventState);
		}
	}
}
