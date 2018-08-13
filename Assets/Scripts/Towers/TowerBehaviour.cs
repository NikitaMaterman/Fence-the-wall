using UnityEngine;

public class TowerBehaviour : MonoBehaviour {
	private FSM fsm;
	private FSMState attackState;
	private FSMState idleState;
	private Attack attackAction;
	private Idle idleAction;

	void Start() {
		fsm = new FSM("Tower AI");

		// Create AttackState
		attackState = fsm.AddState("AttackState");
		attackAction = new Attack(gameObject, attackState);

		// Create IdleState
		idleState = fsm.AddState("IdleState");
		idleAction = new Idle(gameObject, idleState);

		// Add actions
		attackState.AddAction(attackAction);
		idleState.AddAction(idleAction);

		// Add events
		attackState.AddEvent("ToIdle", idleState);
		idleState.AddEvent("ToAttack", attackState);

		// Initialize actions
		attackAction.Start(finishEvent:"ToIdle");
		idleAction.Start(finishEvent:"ToAttack");

		// Start the fsm
		fsm.Start("IdleState");
	}

	void Update() {
		if (fsm != null) {
			fsm.Update();
		}
	}
}
