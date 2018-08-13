using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attack : FSMAction {
	public string FinishEvent { get; set; }
	public GameObject Tower;
	public GameObject Target;
	public Tower TowerScript;

	public Attack(GameObject tower, FSMState owner) : base(owner) {
		Tower = tower;
		TowerScript = tower.GetComponent<Tower>();
	}

	public void Start(String finishEvent) {
		FinishEvent = finishEvent;
	}

	public override void OnUpdate() {
		SortedList<float, GameObject> targetsInRange = GetTargetsInRange();
		if (targetsInRange.Count == 0) {
			Finish();
			return;
		}

		if (Target != null && targetsInRange.ContainsValue(Target)) {
			// Already shooting at Target, no need to change
			return;
		}

		// Remove existing projectile scripts
		var currentTargetScripts = Tower.GetComponents<ShootAtTarget>();
		foreach (var script in currentTargetScripts) {
			script.Destroy();
		}

		// Start attacking closest target
		foreach (var pair in targetsInRange) {
			var spawnProjectileScript = Tower.AddComponent(typeof(ShootAtTarget)) as ShootAtTarget;
			if (spawnProjectileScript == null) {
				// Retry adding in case of fail for debugging purposes
				continue;
			}

			Target = pair.Value;

			spawnProjectileScript.Target = Target;
			spawnProjectileScript.Damage = TowerScript.Damage;
			spawnProjectileScript.Projectile = TowerScript.Projectile;
			spawnProjectileScript.SpawnInterval = TowerScript.AttackSpeed;
			spawnProjectileScript.StartFiring();

			// Only attack a single target
			break;
		}
	}

	private SortedList<float, GameObject> GetTargetsInRange() {
		var result = new SortedList<float, GameObject>();
		var enemiesOnMap = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (var enemy in enemiesOnMap) {
			var distanceToEnemy = (enemy.transform.position - Tower.transform.position).magnitude;
			if (distanceToEnemy <= TowerScript.Range && !result.ContainsKey(distanceToEnemy)) {
				result.Add(distanceToEnemy, enemy);
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
