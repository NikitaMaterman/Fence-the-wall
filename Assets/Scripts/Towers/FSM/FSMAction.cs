public class FSMAction {
	public FSMState Owner {
		get { return _owner; }
	}

	private readonly FSMState _owner;

	public FSMAction(FSMState owner) {
		_owner = owner;
	}

	public virtual void OnEnter() {

	}
	
	public virtual void OnUpdate() {

	}
	
	public virtual void OnExit() {

	}
}
