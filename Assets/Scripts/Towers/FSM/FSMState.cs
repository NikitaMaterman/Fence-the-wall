using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FSMState {
	public FSM Owner {
		get { return _owner; }
	}

	public String Name;
	public List<FSMAction> Actions = new List<FSMAction>();
	public Dictionary<string, FSMState> Events = new Dictionary<string, FSMState>();

	private readonly FSM _owner;

	public FSMState(String name, FSM owner) {
		Name = name;
		_owner = owner;
	}

	public void AddEvent(string eventName, FSMState destinationState) {
		if (!Events.ContainsKey(eventName)) {
			Events[eventName] = destinationState;
		}
	}
	
	public FSMState GetEvent(string eventName) {
		return Events.ContainsKey(eventName) ? Events[eventName] : null;
	}
	
	public void AddAction(FSMAction action) {
		if (!Actions.Contains(action) && action.Owner == this) {
			Actions.Add(action);
		}
	}
	
	public void SendEvent(string eventName) {
		Owner.SendEvent(eventName);
	}
}
