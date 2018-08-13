using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtTarget : MonoBehaviour {

	[SerializeField]
	public GameObject Projectile;

	[SerializeField]
	public GameObject Target;

	[SerializeField]
	public float SpawnInterval;

	[SerializeField]
	public Vector3 SpawnPosition;

	public int Damage;

	Vector3 calculatedSpawnPosition { get { return transform.position + SpawnPosition; } }

	// Use this for initialization
	void Start() {

	}

	// Start was called before all parameters were passed
	public void StartFiring() {
		InvokeRepeating("ShootTarget", 0f, SpawnInterval);
	}

	void ShootTarget() {
		ShootTarget script = gameObject.AddComponent<ShootTarget>();
		script.Target = Target;
		script.DealDamage(Damage);
		script.Material = (Material)Resources.Load("Green");
	}

	internal void Destroy() {
		Destroy(this);
	}

	void OnDrawGizmosSelected() {
		Gizmos.DrawWireSphere(calculatedSpawnPosition, 1f);
	}
}
