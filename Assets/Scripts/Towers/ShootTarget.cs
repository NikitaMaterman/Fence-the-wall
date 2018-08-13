using System;
using UnityEngine;

public class ShootTarget : MonoBehaviour {
	public GameObject Target;
	public Material Material;
	public GameObject LineRendererObject;
	private LineRenderer lineRenderer;
	private Vector3 spawnPosition;

	// Use this for initialization
	void Start() {
		spawnPosition = transform.position;
		spawnPosition.y += 20;
		LineRendererObject = Instantiate(GameObject.Find("LineRenderer"), spawnPosition, Quaternion.identity, transform);
		lineRenderer = LineRendererObject.GetComponent<LineRenderer>();
	}

	public void DealDamage(int damage) {
		// Calculate damage
		if (Target != null) {
			var enemy = Target.GetComponent<Enemy>();
			if (enemy != null) {
				enemy.TakeDamage(damage);
			}
		}
	}

	void Update() {
		// If Target's empty, don't continue as there's nothing to draw to
		if (Target == null) {
			Destroy(LineRendererObject);
			Destroy(this);
			return;
		}

		if (lineRenderer != null) {
			var targetPosition = Target.transform.GetComponent<Collider>().bounds.center;
			lineRenderer.SetPositions(new[] { spawnPosition, targetPosition });
		}

		// Ensure laser disappears
		Destroy(LineRendererObject, 0.5f);
		Destroy(this, 2f);
	}
}
