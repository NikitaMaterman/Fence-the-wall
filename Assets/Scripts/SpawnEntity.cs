using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEntity : MonoBehaviour {

	[SerializeField] GameObject Entity;
	[SerializeField] float spawnInterval;
	[SerializeField] int spawnAmount = 1;
	[SerializeField] Vector3 spawnPosition;
	
	public List<GameObject> SpawnedEntities = new List<GameObject>();
	public Boolean SpawningDone { get { return SpawnedEntities.Count >= spawnAmount; } }

	Vector3 calculatedSpawnPosition { get { return transform.position + spawnPosition; } }

	// Use this for initialization
	void Start() {
		
	}

	public void StartSpawning() {
		try
		{
			InvokeRepeating("SpawnEntityAtPosition", 0f, spawnInterval);
		} catch (Exception ex) {
			// Catch exception on destroy to avoid crash
		}
	}

	void SpawnEntityAtPosition() {
		// Ensure spawning doesn't go on indefinitely
		if (!SpawningDone) {
			SpawnedEntities.Add(Instantiate(Entity, calculatedSpawnPosition, transform.rotation));
		}
	}

	public void Destroy() {
		if (SpawnedEntities != null && SpawnedEntities.All(e => e == null)) {
			Destroy(this);
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.DrawWireSphere(calculatedSpawnPosition, 1f);
	}
}
