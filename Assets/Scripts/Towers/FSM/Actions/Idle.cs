using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Idle : FSMAction {
	public string FinishEvent { get; set; }
	public GameObject Tower;
	public Tower TowerScript;

	public Idle(GameObject tower, FSMState owner) : base(owner) {
		Tower = tower;
		TowerScript = tower.GetComponent<Tower>();
	}

	public void Start(String finishEvent) {
		FinishEvent = finishEvent;
	}

	public override void OnUpdate() {
		List<GameObject> targetsInRange = GetTargetsInRange();

		if (targetsInRange.Count != 0) {
			Finish();
		}
	}

	private List<GameObject> GetTargetsInRange() {
		var result = new List<GameObject>();
		var enemiesOnMap = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (var enemy in enemiesOnMap) {
			var distanceToEnemy = (enemy.transform.position - Tower.transform.position).magnitude;
			if (distanceToEnemy <= TowerScript.Range) {
				result.Add(enemy);
			}
		}

		return result;
	}

	public override void OnExit() {

	}

	public void Finish() {
		if (!string.IsNullOrEmpty(FinishEvent)) {
			Owner.SendEvent(FinishEvent);
		}
	}
}
